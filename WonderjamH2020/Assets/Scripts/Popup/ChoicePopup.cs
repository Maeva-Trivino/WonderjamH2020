using Interactive.Base;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Popup
{
    public class ChoicePopup : Popup
    {
        #region Consts
        private readonly static Color COLOR_DISABLED = new Color(.6f, .4f, .4f);
        private readonly static Color COLOR_ENABLED = Color.white;
        private readonly static Vector3 SCALE_SELECTED = Vector3.one * 1.1f;
        private readonly static Vector3 SCALE_NORMAL = Vector3.one;
        #endregion

        #region Variables
        #region Editor
        [SerializeField]
        private GameObject choicePrefab;
        #endregion

        #region Public
        #endregion

        #region Private
        private List<GameAction> choices = new List<GameAction>();
        private int selection;
        private Rewired.Player inputManager;
        #endregion 
        #endregion

        #region Methods
        #region Unity
        private void Update()
        {
            // Check if an item should be enable during display
            foreach (GameAction choice in choices)
                SetButtonEnabled(choice.graphics, choice.IsEnabled);


            if (inputManager == null)
                return;

            if (inputManager.GetButtonDown("MenuLeft"))
            {
                GoLeft();
            }
            else if (inputManager.GetButtonDown("MenuRight"))
            {
                GoRight();
            }
            else if (inputManager.GetButtonDown("Interact"))
            {
                Validate();
            }
            else if (inputManager.GetButtonDown("Cancel"))
            {
                Hide();
            }
        }
        #endregion

        #region Public
        public void SetChoices(List<GameAction> choices)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            selection = 0;
            choices.Add(new GameAction("Fermer", () => Hide(), () => true));

            for (int i = 0; i < choices.Count; i++)
            {
                GameAction choice = choices[i];
                choice.graphics = Instantiate(choicePrefab);
                InitializeChoice(choice, i, choices.Count - 1);
            }

            SetButtonSelected(choices[selection].graphics, true);
            this.choices = choices;
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
            if (selection > choices.Count - 1) selection = choices.Count - 1;

            SetButtonSelected(choices[selection].graphics, true);
        }

        public void Validate()
        {
            if (choices[selection].IsEnabled)
                choices[selection].Invoke();
        }

        public void SetInputManager(Rewired.Player m) => inputManager = m;
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

        private void InitializeChoice(GameAction choice, int index, int maxIndex)
        {
            GameObject button = choice.graphics;
            button.transform.SetParent(transform);
            button.transform.localScale = SCALE_NORMAL;
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = choice.Description;

            RectTransform tr = (RectTransform)button.transform;
            float angle = (maxIndex - index) * Mathf.PI / maxIndex;
            tr.anchoredPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle) * .5f) * 100f;
        }
        #endregion
        #endregion
    }
}