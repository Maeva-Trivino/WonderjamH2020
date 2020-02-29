using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChoicePopup
{
    public abstract class ChoicesSenderBehaviourWithContext : MonoBehaviour
    {
        public abstract List<Choice> GetChoices(Player contextPlayer);
    }
}