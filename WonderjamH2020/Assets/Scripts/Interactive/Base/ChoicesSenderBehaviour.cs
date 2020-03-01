using System.Collections.Generic;
using UnityEngine;

namespace Interactive.Base
{
    public abstract class ChoicesSenderBehaviour : MonoBehaviour, Interactable
    {
        public virtual void Deselect()
        {
            // TODO
            transform.localScale *= 1f / 1.01f;
        }

        public virtual void Select()
        {
            // TODO
            transform.localScale *= 1.01f;
        }

        public abstract List<GameAction> GetChoices(Player contextPlayer);

    }
}