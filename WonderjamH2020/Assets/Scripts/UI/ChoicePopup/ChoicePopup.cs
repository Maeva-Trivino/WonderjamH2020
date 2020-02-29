using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ChoicePopup
{
    public class ChoicePopup : MonoBehaviour
    {
        #region Consts
        private readonly static Color COLOR_DISABLED = new Color(.8f, .8f, .8f);
        private readonly static Color COLOR_ENABLED = Color.white;
        private readonly static Vector3 SCALE_SELECTED = Vector3.one * 1.1f;
        private readonly static Vector3 SCALE_NORMAL = Vector3.one;
        #endregion

        #region Variables
        #region Editor
        [SerializeField]
        private GameObject choicePrefab;
        #endregion

        #region Private
        private List<Choice> choices;
        private int selection;
        #endregion 
        #endregion

        #region Methods
        #region Unity
        private void Update()
        {
            // Chech if an item should be enable during display
            foreach (Choice choice in choices)
                SetButtonEnabled(choice.graphics, choice.IsEnabled);
        }
        #endregion

        #region Public
        public void SetChoices(List<Choice> choices)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            selection = 0;
            choices.Add(new Choice("Fermer", () => Hide(), () => true));

            for (int i = 0; i < choices.Count; i++)
            {
                Choice choice = choices[i];
                choice.graphics = Instantiate(choicePrefab);
                InitializeChoice(choice, i, choices.Count - 1);
            }

            SetButtonSelected(choices[selection].graphics, true);
            this.choices = choices;
        }

        /// <summary>
        /// Displays the popup at a specific world position
        /// </summary>
        /// <param name="position"></param>
        public void Display(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void GoLeft()
        {
            SetButtonSelected(choices[selection].graphics, false);

            --selection;
            if (selection < 0) selection = 0;

            SetButtonSelected(choices[selection].graphics, true);
        }

        public void GoRight()
        {
            SetButtonSelected(choices[selection].graphics, false);

            ++selection;
            if (selection >= choices.Count) selection = choices.Count;

            SetButtonSelected(choices[selection].graphics, true);
        }

        public void Validate()
        {
            if (choices[selection].IsEnabled)
                choices[selection].Invoke();
        }
        #endregion

        #region Private
        private void SetButtonEnabled(GameObject button, bool isEnabled)
        {
            button.GetComponent<Image>().color = isEnabled ? COLOR_ENABLED : COLOR_DISABLED;
        }
        private void SetButtonSelected(GameObject button, bool isSelected)
        {
            ((RectTransform)button.transform).localScale = isSelected ? SCALE_SELECTED : SCALE_NORMAL;
        }

        private void InitializeChoice(Choice choice, int index, int maxIndex)
        {
            GameObject button = choice.graphics;
            button.transform.SetParent(transform);
            button.transform.localScale = SCALE_NORMAL;
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = choice.Description;

            RectTransform tr = (RectTransform)button.transform;
            float angle = (maxIndex - index) * Mathf.PI / maxIndex;
            tr.anchoredPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle) * .4f) * 50f;
        }
        #endregion
        #endregion
    }
}