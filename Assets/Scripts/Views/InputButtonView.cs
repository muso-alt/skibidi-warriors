using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Skibidi.Views
{
    public class InputButtonView : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Action<bool> PointerDown = delegate { };
        public Action OnClicked = delegate { };
        
        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDown?.Invoke(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerDown?.Invoke(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke();
        }
    }
}