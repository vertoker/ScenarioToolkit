using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace VRF.Utils
{
    [RequireComponent(typeof(Button))]
    public class ButtonPointerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [FormerlySerializedAs("_button")] 
        [SerializeField] private Button button;
        private bool isPressedDown;
        private bool isPressedUp;
        private bool isClicked;
        private bool isDragging;
        
        public UnityEvent OnButtonPressedDown;
        public UnityEvent OnButtonPressedUp;
        public UnityEvent OnButtonClick;
        public UnityEvent OnButtonDrag;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log($"Pointer DOWN, is Up:{_isPressedUp} is Down {_isPressedDown}");
            isPressedUp = !isPressedUp;
            if (!isPressedDown)
            {
                OnButtonPressedDown?.Invoke();
                isPressedDown = true;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log($"Pointer UP, is Up:{isPressedUp} is Down {isPressedDown}");
            isPressedDown = !isPressedDown;
            if (!isPressedUp)
            {
                OnButtonPressedUp?.Invoke();
                isPressedUp = true;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(!isClicked)
                OnButtonClick?.Invoke();
            isClicked = !isClicked;
        }

    }
}