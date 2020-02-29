using UnityEngine;

namespace QTE
{
    public class UserAction: MonoBehaviour
    {
        protected string name;
        public float progression;

        private System.Action actionToDo;

        protected const string DEFAULT_NAME = "default";

        public UserAction(System.Action actionToDo,string name = DEFAULT_NAME)
        {
            this.actionToDo = actionToDo;
            this.name = name;
            this.progression = 0;
        }

        public bool IsDone()
        {
            return progression > 1f;
        }

        public void DoAction()
        {
            actionToDo();
        }

        public virtual void Do()
        {
            if(IsDone())
                return;

            progression = 1f;
        }
    }
}
