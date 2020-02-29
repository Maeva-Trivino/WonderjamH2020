using System.Collections.Generic;
using UnityEngine;

namespace ChoicePopup
{
    public abstract class ChoicesSenderBehaviour : MonoBehaviour
    {
        public abstract List<Choice> GetChoices(Player contextPlayer);
    }
}