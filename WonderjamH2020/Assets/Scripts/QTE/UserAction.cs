namespace QTE
{
    public class UserAction
    {
        protected string name;
        protected float progression;

        protected const string DEFAULT_NAME = "default";

        public UserAction()
        {
            this.name = DEFAULT_NAME;
            this.progression = 0;
        }

        public UserAction(string name)
        {
            this.name = name;
            this.progression = 0;
        }

        public bool isDone()
        {
            return progression > 1f;
        }

        public virtual void Do()
        {
            if(isDone())
                return;

            progression = 1f;
        }
    }
}
