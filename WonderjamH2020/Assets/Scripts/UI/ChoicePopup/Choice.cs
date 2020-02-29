using System;

namespace UI.ChoicePopup
{
    public class Choice
    {
        private Func<bool> delegateEnability;
        private Action delegateAction;
        public bool IsEnabled => delegateEnability();
        public string Description { get; private set; }

        public Choice(string description, Action action, Func<bool> enability)
        {
            delegateAction = action;
            delegateEnability = enability;
            Description = description;
        }

        public void Invoke() => delegateAction();
    }
}
