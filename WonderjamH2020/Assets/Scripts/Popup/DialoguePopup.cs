using Interactive.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Popup
{
    public class DialoguePopup : Popup 
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI text;

        public void SetText(string message)
        {
            text.text = message;
        }

        public IEnumerator PopupDeactivation(float time)
        {
            yield return new WaitForSeconds(time);
            this.Hide();
        }
    }
}
