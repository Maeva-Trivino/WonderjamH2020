using UnityEngine;

namespace Popup
{
    public class LabelPopup : Popup
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI text;

        public void SetText(string label)
        {
            text.text = label;
        }
    }
}