using NaughtyAttributes;
using UnityEngine;
using VRF.Components.Players.Views.PlayerIK;
using VRF.Players.Hands;

namespace VRF.Players.Models
{
    /// <summary> Все варианты отображения игрока </summary>
    public enum PlayerVisualMode
    {
        /// <summary> Виртуальный игрок, не имеет моделей </summary>
        NoGraphics,
        /// <summary> Летающие руки и голова </summary>
        ControllerModels,
        /// <summary> Полноценный IK скелет </summary>
        SkeletonIK
    }
    
    /// <summary>
    /// Конфигурация для хранения представления игрока.
    /// Применим как для локального игрока, так и для сетевого
    /// </summary>
    [System.Serializable]
    public class PlayerVisualConfiguration
    {
        [SerializeField] private PlayerVisualMode visualMode = PlayerVisualMode.ControllerModels;
        
        [AllowNesting] [ShowIf(nameof(UseControllerModels))]
        [SerializeField] private HandSkin handSkinLeft;
        [AllowNesting] [ShowIf(nameof(UseControllerModels))]
        [SerializeField] private HandSkin handSkinRight;
        
        [AllowNesting] [ShowIf(nameof(UseSkeletonIK))]
        // Для VR IK нужен объект, который и будет синхронизироваться с виртуальным игроком
        [SerializeField] private BasePlayerIKView skeletonIK;

        public PlayerVisualMode VisualMode => visualMode;
        public bool UseNoGraphics => visualMode == PlayerVisualMode.NoGraphics;
        public bool UseControllerModels => visualMode == PlayerVisualMode.ControllerModels;
        public bool UseSkeletonIK => visualMode == PlayerVisualMode.SkeletonIK;
        
        /// <summary> Скин для левой руки (проверьте сначала UseControllerModels) </summary>
        public HandSkin HandSkinLeft => handSkinLeft;
        /// <summary> Скин для правой руки (проверьте сначала UseControllerModels) </summary>
        public HandSkin HandSkinRight => handSkinRight;
        /// <summary> Исходный IK для игроков (проверьте сначала UseSkeletonIK) </summary>
        public BasePlayerIKView SkeletonIK => skeletonIK;
        
        // Конструкторы нужны для удобства создания внутри сериализованных классов
        public PlayerVisualConfiguration() { }
        public PlayerVisualConfiguration(PlayerVisualMode mode)
        {
            visualMode = mode;
        }
    }
}