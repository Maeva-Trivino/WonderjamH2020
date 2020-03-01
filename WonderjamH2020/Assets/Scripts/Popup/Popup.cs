using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Popup
{
    public abstract class Popup : MonoBehaviour
    {
        public bool IsVisible => gameObject.activeInHierarchy;
        public virtual void Display() => gameObject.SetActive(true);
        public virtual void Hide()
        {
            gameObject.SetActive(false);
            onClose.Invoke();
        }
        public UnityEvent onClose = new UnityEvent();
    }
}