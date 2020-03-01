using Interactive.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Popup
{
    public class QTEPopup : Popup
    {
        #region Variables
        #region Editor
        [SerializeField]
        private TMPro.TextMeshProUGUI combos;
        [SerializeField]
        private Slider slider;
        #endregion
        #region Public
        #endregion
        #region Private
        private UserAction qte;
        #endregion
        #endregion

        #region Methods
        #region Unity
        private void Update() => UpdateDisplay();
        #endregion
        #region Public
        public void SetAction(UserAction action)
        {
            qte = action;
            UpdateDisplay();
        }

        /*public override void Hide()
        {
            slider.transform.localScale = new Vector3(0, 0, 0);
            base.Hide();
        }*/
        public override void Hide()
        {
            base.Hide();
        }
        #endregion

        #region Private
        private void UpdateDisplay()
        {
            if (qte == null)
                return;

            // Checks if the QTE is done
            qte.Do();
            if (qte.IsDone())
            {
                Hide();
                return;
            }


            combos.text = "";

            if (qte is ComboAction)
            {
                ComboAction comboAction = (ComboAction)qte;
                foreach (string s in comboAction.expectedCombos)
                {
                    combos.text += s + " ";
                }

                combos.text = combos.text.Remove(combos.text.Length - 1);
            }

            slider.transform.localScale = new Vector3(1, 1, 1);
            slider.value = qte.progression;
        }
        #endregion
        #endregion
    }
}