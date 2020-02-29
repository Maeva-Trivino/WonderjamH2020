using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ChoicePopup
{
    public class ChoicePopup : MonoBehaviour
    {
        #region Variables
        #region Editor
        [SerializeField]
        private GameObject choicePrefab;
        #endregion

        #region Private
        #endregion 
        #endregion

        #region Methods
        #region Unity
        private void Update()
        {
            
        }
        #endregion

        #region Public
        public void SetChoices(List<Choice> choices)
        {


            for (int i = 0; i < choices.Count; i++)
            {
                Choice choice = (Choice)choices[i];
                Button button = Instantiate(choicePrefab).GetComponent<Button>();
                DisplayButton(button, i, choices.Count);
            }
        }

        /// <summary>
        /// Displays the popup at a specific world position
        /// </summary>
        /// <param name="position"></param>
        public void Display(Transform position)
        {

        }

        public void Hide()
        {

        }

        public void GoLeft()
        {

        }

        public void GoRight()
        {

        }

        public void Validate()
        {
            
        }
        #endregion

        #region Private
        private void SetButtonEnabled(Button button, bool isEnabled)
        {
            // todo
        }

        private void DisplayButton(Button button, int i, int count)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}