using System;
using UnityEngine;

namespace ChoicePopup
{
    public class Choice
    {

        #region Variables
        #region Public
        public bool IsEnabled => delegateEnability();
        public string Description { get; private set; }
        public GameObject graphics;
        #endregion

        #region Private
        private Func<bool> delegateEnability;
        private Action delegateAction;
        #endregion
        #endregion

        #region Methods
        public Choice(string description, Action action, Func<bool> enability)
        {
            delegateAction = action;
            delegateEnability = enability;
            Description = description;
        }

        public void Invoke() => delegateAction();
        #endregion
    }
}
