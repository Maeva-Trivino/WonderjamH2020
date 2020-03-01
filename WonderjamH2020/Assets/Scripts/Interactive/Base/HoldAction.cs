using UnityEngine;

namespace Interactive.Base
{
    public class HoldAction : UserAction
    {
        public float expectedDuration;
        public float currentDuration;

        public HoldAction(System.Action actionToDo,float expectedDuration,string name = DEFAULT_NAME) : base(actionToDo,name)
        {
            this.currentDuration = 0;
            this.expectedDuration = expectedDuration;
        }

        public override void Do()
        {
            currentDuration += Time.deltaTime;
            if (currentDuration >= expectedDuration)
            {
                DoAction();
                progression = 1f;
            }
            else
            {
                progression = currentDuration / expectedDuration;
            }
        }
    }
}
