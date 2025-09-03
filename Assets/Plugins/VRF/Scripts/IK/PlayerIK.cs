using KBCore.Refs;
using NaughtyAttributes;
using UnityEngine;
using VRF.Components.Players.Modules.IK;
using VRF.Utilities;

namespace VRF.IK
{
    /// <summary>
    /// Кастомное решение для IK персонажа, администрирует все основные параметры IK
    /// </summary>
    public class PlayerIK : ValidatedMonoBehaviour
    {
        // TODO работает только если pivot исходной модели находятся в ступнях, то есть y=0
        
        [Header("Core")]
        // Активность Ik игрока
        [SerializeField] private bool ikActive = true;
        // Высота головы относительно pivot'а модели
        [SerializeField] private float headOffset = 0.5f;
        
        // Использовать ограничение головы для роста (нужен для предотвращения эффекта Акиры)
        [SerializeField] private bool useHeadClamp = true;
        // Ограничение, на которое может идти голова
        [MinMaxSlider(-2, 4), EnableIf(nameof(useHeadClamp))]
        [SerializeField] private Vector2 headClamp = new(1, 1.7f);
        
        // Использовать ограничение на взгляд
        [SerializeField] private bool useHeadLookClamp = true;
        // Ограничение на вращение головой вверх-вниз
        [MinMaxSlider(-180, 180), EnableIf(nameof(useHeadLookClamp))]
        [SerializeField] private Vector2 headLookClamp = new(-60, 60);
        
        // Добавляет к финальному значению offset в виде градусов поворота вектора
        [SerializeField] private float headLookAngleOffset = 0f;
        // Максимальный угол наклона туловища игрока вместе с головой
        [SerializeField] private float maxInclination = 10f;
        // Скорость вращения туловища за головой
        [SerializeField] private float rotateSpeed = 5f;
        
        [Header("Legs")]
        // Активность ног впринципе (иначе отключает)
        [SerializeField] private bool ikFeetActive = true;
        // Контроллер шагов для ног
        [SerializeField] private PlayerFootsIK footsIK;
        // Ограничение минимальной высоты игрока относительно основания ног
        [SerializeField] private float minFeetHeight;
        
        // Точка ног относительно центра игрока (+ для правой, - для левой)
        [SerializeField] private Vector3 legCenterOffset = new(0.1f, 0, 0);
        // Точка ног относительно центра игрока (для обоих ног +)
        [SerializeField] private Vector3 legsOffset = new(0, 0, 0);
        // Высота ступней ноги (для каждой ноги отдельно)
        [SerializeField] private Vector2 heelsHeightOffset = new(0, 0);
        
        [Header("Raycast")]
        // Длина луча для определения высоты ног
        [SerializeField] private float raycastDistance = 100f;
        // Параметры этого луча
        [SerializeField] private LayerMask raycastMask;
        
        [Header("References")]
        // Исходный view (думаю надо его как-нибудь убрать)
        [SerializeField] private PlayerVRIKModule view;
        // Аниматор для указания IK
        [SerializeField] private Animator animator;
        [SerializeField] private AnimatorIKReceiver animatorIK;
        
        private LegRaycastResult leftLegRaycastResult;
        private LegRaycastResult rightLegRaycastResult;
        private Vector3 leftOriginLeg;
        private Vector3 rightOriginLeg;
        
        private bool initialized;
        private float currentFloorY;
        private float angle;
        
        public float HeadProgressUnClamped { get; private set; }
        /// <summary> Высота игрока относительно его локального минимума и максимума </summary>
        public float HeadProgressClamped => Mathf.Clamp01(HeadProgressUnClamped);
        
        public LegRaycastResult LeftLeg => leftLegRaycastResult;
        public LegRaycastResult RightLeg => rightLegRaycastResult;

        public Vector2 PlayerForward
        {
            get
            {
                var forward = view.Player.forward;
                return new Vector2(forward.x, forward.z);
            }
        }
        /// <summary> Проецирует позицию игрока на плоскость </summary>
        public Vector2 CurrentPosXZ
        {
            get
            {
                var pos = view.Player.position;
                return new Vector2(pos.x, pos.z);
            }
        }
        /// <summary> Угол поворота игрока </summary>
        public float CurrentAngle => view.Player.localEulerAngles.y;
        
        private void OnEnable() { if (animatorIK) animatorIK.OnAnimatorUpdateIK += OnAnimatorIK; }
        private void OnDisable() { if (animatorIK) animatorIK.OnAnimatorUpdateIK -= OnAnimatorIK; }
        
        /// <summary>
        /// Высчитывание особенного объекта для IK: корпуса игрока
        /// </summary>
        private void CalculatePlayerBodyIK()
        {
            var playerPos = view.Player.position;
            var playerRot = view.Player.rotation;
            var headPos = view.Head.position;
            var headRot = view.Head.rotation;
            
            var head = headPos.y;
            var min = headClamp.x + currentFloorY + headOffset;
            var max = headClamp.y + currentFloorY + headOffset;
            HeadProgressUnClamped = InverseLerpUnclamped(min, max, head);
            
            if (useHeadClamp)
                head = Mathf.Clamp(head, min, max);
            head -= headOffset;

            playerRot = Quaternion.Lerp(playerRot, headRot, Time.deltaTime * rotateSpeed);
            playerRot = ClampRotation(playerRot, maxInclination, maxInclination);
            
            view.Player.position = new Vector3(playerPos.x, head, playerPos.z);
            view.Player.rotation = playerRot;
        }
        private void CalculateHeadIK()
        {
            var delta = (view.HeadLook.position - view.Head.position).normalized;
            
            var angleXZ = Mathf.Atan2(delta.z, delta.x);
            // угол на 2-мерной плоскости по высоте, так как нулевой угол это forward,
            // то для расчёта нужен угол от x, который считается через cos(x)
            var angleY = Mathf.Asin(delta.y) * Mathf.Rad2Deg;
            
            if (useHeadLookClamp)
                angleY = Mathf.Clamp(angleY, headLookClamp.x, headLookClamp.y);
            angleY += headLookAngleOffset;

            delta = new Vector3
            {
                x = Mathf.Cos(angleXZ),
                y = Mathf.Sin(angleY * Mathf.Deg2Rad),
                z = Mathf.Sin(angleXZ)
            };
            
            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(view.Head.position + delta.normalized);
        }
        
        protected static Vector2 RotateVector2(Vector2 direction, float angle)
        {
            var rad = Mathf.Deg2Rad * -angle;
            var cos = Mathf.Cos(rad);
            var sin = Mathf.Sin(rad);
            return new Vector2(direction.x * cos - direction.y * sin, direction.x * sin + direction.y * cos);
        }
        
        public Vector2 RotateVector(Vector2 a, float offsetAngle)//метод вращения объекта
        {
            var power = Mathf.Sqrt(a.x * a.x + a.y * a.y);//коэффициент силы
            var atan2 = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg - 90f + offsetAngle;//угол из координат с offset'ом
            return Quaternion.Euler(0, 0, atan2) * Vector2.up * power;
            //построение вектора из изменённого угла с коэффициентом силы
        }
        
        private void SetIKPosition(AvatarIKGoal goal, Vector3 position)
        {
            animator.SetIKPositionWeight(goal, 1);
            animator.SetIKPosition(goal, position);
        }
        private void SetIKRotation(AvatarIKGoal goal, Quaternion rotation)
        {
            animator.SetIKRotationWeight(goal, 1);
            animator.SetIKRotation(goal, rotation);
        }
        private void SetIKHintPosition(AvatarIKHint hint, Vector3 position)
        {
            animator.SetIKHintPositionWeight(hint, 1);
            animator.SetIKHintPosition(hint, position);
        }
        
        private void DisableFeet()
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
            animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, 0);
            animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, 0);
        }
        private void DisableBody()
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);

            animator.SetLookAtWeight(0);
            
            DisableFeet();
        }

        /// <summary>
        /// Основной цикл расчёта IK
        /// </summary>
        private void OnAnimatorIK(int layerIndex)
        {
            if (ikActive)
            {
                // Расчёт лучей для ног и туловища
                CalculateLegsRaycasts();
                // Расчёт позиции туловиша
                CalculatePlayerBodyIK();
                
                // Установка точек для известных точек тела (голова, 2 контроллера)
                CalculateHeadIK();
                
                SetIKPosition(AvatarIKGoal.LeftHand, view.LeftController.position);
                SetIKRotation(AvatarIKGoal.LeftHand, view.LeftController.rotation);

                SetIKPosition(AvatarIKGoal.RightHand, view.RightController.position);
                SetIKRotation(AvatarIKGoal.RightHand, view.RightController.rotation);
                
                // Расчёт ног и шагов
                if (ikFeetActive)
                    UpdateLegsIK();
                else DisableFeet();
            }
            else DisableBody();
        }
        /// <summary>
        /// Построение IK для ног
        /// </summary>
        private void UpdateLegsIK()
        {
            // Получение данных для построения IK ног
            var leftFoot = leftLegRaycastResult.GetPoint();
            var rightFoot = rightLegRaycastResult.GetPoint();
            var leftLegLook = leftLegRaycastResult.LookFeet(view.Player);
            var rightLegLook = rightLegRaycastResult.LookFeet(view.Player);
            
            // Если есть контроллер для ходьбы
            if (footsIK)
            {
                footsIK.UpdateIK();
                // То идёт модификация высчитанных координат
                leftFoot = footsIK.OverrideLeftFoot(leftFoot);
                rightFoot = footsIK.OverrideRightFoot(rightFoot);
            }
            
            // Указание позиции ног
            SetIKPosition(AvatarIKGoal.LeftFoot, leftFoot);
            SetIKPosition(AvatarIKGoal.RightFoot, rightFoot);
            SetIKRotation(AvatarIKGoal.LeftFoot, leftLegLook);
            SetIKRotation(AvatarIKGoal.RightFoot, rightLegLook);
            
            // Получение данных для построения IK ног
            var leftKnee = CalculateKnee(leftOriginLeg, leftFoot);
            var rightKnee = CalculateKnee(rightOriginLeg, rightFoot);
            
            // Указание позиции колен
            SetIKHintPosition(AvatarIKHint.LeftKnee, leftKnee);
            SetIKHintPosition(AvatarIKHint.RightKnee, rightKnee);
        }
        /// <summary>
        /// Высчитывает raycast'ы для ног 
        /// </summary>
        private void CalculateLegsRaycasts()
        {
            // Подготовка переменных игрока
            var rootY = view.Head.position.y - headOffset;
            var playerPos = view.Player.position;
            var rootPos = new Vector3(playerPos.x, rootY, playerPos.z);
            // Подготовка для поворота точек ног
            angle = -view.Player.localEulerAngles.y;
            var legCenterOffsetRotated = legCenterOffset.RotateZDeg(angle);
            var legsOffsetRotated = legsOffset.RotateZDeg(angle);
            
            // Построение точек ног с учётом поворота игрока
            leftOriginLeg = rootPos - legCenterOffsetRotated + legsOffsetRotated;
            rightOriginLeg = rootPos + legCenterOffsetRotated + legsOffsetRotated;
            
            // Clamp относительно минимально возможной высоты
            if (leftOriginLeg.y < minFeetHeight) leftOriginLeg.y = minFeetHeight;
            if (rightOriginLeg.y < minFeetHeight) rightOriginLeg.y = minFeetHeight;
            
            // Результаты записываются в структуры результата
            leftLegRaycastResult = new LegRaycastResult(leftOriginLeg, raycastDistance, raycastMask);
            rightLegRaycastResult = new LegRaycastResult(rightOriginLeg, raycastDistance, raycastMask);

            CalculateFloor();
        }
        /// <summary>
        /// Нормализует значения пола для ног
        /// </summary>
        private void CalculateFloor()
        {
            // Temp переменные
            var currentLeftY = leftLegRaycastResult.PointY;
            var currentRightY = rightLegRaycastResult.PointY;
            
            // Clamp относительно минимально возможной высоты (да повторяется)
            if (currentLeftY < minFeetHeight) currentLeftY = minFeetHeight;
            if (currentRightY < minFeetHeight) currentRightY = minFeetHeight;
            
            // Средняя точка пола
            currentFloorY = (currentLeftY + currentRightY) / 2f;
            
            // Добавление высоты ступней
            currentLeftY += heelsHeightOffset.x;
            currentRightY += heelsHeightOffset.y;
            
            // UnTemp переменные
            leftLegRaycastResult.PointY = currentLeftY;
            rightLegRaycastResult.PointY = currentRightY;
        }
        /// <summary> Высчитывает колено относительно его начала и конца
        /// (то есть просто вперёд между ними). Основную работу выполняет Unity IK </summary>
        private Vector3 CalculateKnee(Vector3 startFoot, Vector3 endFoot)
        {
            var kneePos = (startFoot + endFoot) / 2f;
            var forward = Vector3.forward.RotateZDeg(angle);
            return kneePos + forward;
        }
        
        /// <summary>
        /// Вращает кватернион только по двум осям (в данном случае относительно оси y)
        /// </summary>
        private static Quaternion ClampRotation(Quaternion q, float x, float z)
        {
            // https://forum.unity.com/threads/how-do-i-clamp-a-quaternion.370041/
            
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;
 
            var angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, -x, x);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
 
            /*var angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
            angleY = Mathf.Clamp(angleY, -bounds.y, bounds.y);
            q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);*/
 
            var angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);
            angleZ = Mathf.Clamp(angleZ, -z, z);
            q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);
 
            return q;
        }

        /// <summary>
        /// Несуществующая формула в стандартной библиотеке, а именно обратный необрезанный lerp
        /// </summary>
        private static float InverseLerpUnclamped(float a, float b, float value)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return a != b ? (value - a) / (b - a) : 0;
        }
    }
}