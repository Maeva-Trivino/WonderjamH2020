using System.Collections.Generic;
using UnityEngine.UI;

namespace QTE
{
    public class ComboAction : UserAction
    {
        private Queue<Button> comboBuffer;
        private List<Button> expectedCombos;
        public int actualCombo;
        public int comboGoal;

        public ComboAction(string name) : base(name) { }

        public ComboAction(List<Button> expectedCombos, int comboGoal,string name = DEFAULT_NAME) : base(name)
        {
            this.actualCombo = 0;
            this.comboGoal = comboGoal;
            this.expectedCombos = expectedCombos;
            expectedCombos.ForEach(comboBuffer.Enqueue);
        }

        public override void Do()
        {
            //TODO GET INPUT
            if (true)
                comboBuffer.Dequeue();

            if (comboBuffer.Count == 0)
            {
                expectedCombos.ForEach(comboBuffer.Enqueue);
                actualCombo++;
            }

            if (actualCombo == comboGoal)
            {
                //TODO DO ACITON
                progression = 1f;
            }
            else
            {
                progression = ((float)actualCombo + (float)(expectedCombos.Count - comboBuffer.Count) / expectedCombos.Count) / comboGoal;
            }
        }
    }
}
