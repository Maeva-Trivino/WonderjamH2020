using UnityEngine;

namespace QTE
{
    public class HoldAction : UserAction
    {
        public float expectedDuration;
        public float currentDuration;

        public HoldAction(string name) : base(name) {}

        public HoldAction(float expectedDuration,string name = DEFAULT_NAME) : base(name)
        {
            this.currentDuration = 0;
            this.expectedDuration = expectedDuration;
        }

        public override void Do()
        {
            currentDuration += Time.deltaTime;
            if (currentDuration >= expectedDuration)
            {
                //Do action
                progression = 1f;
            }
            else
            {
                progression = currentDuration / expectedDuration;
            }
        }
    }
}
