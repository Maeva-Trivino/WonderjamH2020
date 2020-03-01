using System.Collections.Generic;

namespace Interactive.Base
{
    public class ComboAction : UserAction
    {
        private Queue<string> comboBuffer;

        public List<string> expectedCombos;

        private int actualCombo;
        public int comboGoal;
        private Rewired.Player inputManager;


        public ComboAction(Rewired.Player inputManager, List<string> expectedCombos, int comboGoal, System.Action actionToDo,string name = DEFAULT_NAME) : base(actionToDo,name)
        {
            this.actualCombo = 0;
            this.comboGoal = comboGoal;
            this.expectedCombos = expectedCombos;
            this.inputManager = inputManager;
            comboBuffer = new Queue<string>();
            expectedCombos.ForEach(comboBuffer.Enqueue);
        }

        /* test stuff
        private void Start()
        {
            inputManager = ReInput.players.GetPlayer(0);
            comboBuffer.Enqueue(expectedCombos[0]);
        }
        */

        public override void Do()
        {
            if (inputManager.GetButtonDown(comboBuffer.Peek()))
            {
                comboBuffer.Dequeue();
            }
                

            if (comboBuffer.Count == 0)
            {
                expectedCombos.ForEach(comboBuffer.Enqueue);
                actualCombo++;
            }

            if (actualCombo == comboGoal)
            {
                DoAction();
                progression = 1f;
            }
            else
            {
                progression = ((float)actualCombo + (float)(expectedCombos.Count - comboBuffer.Count) / expectedCombos.Count) / comboGoal;
            }
        }
    }
}
