using System.Collections.Generic;
using UnityEngine;

namespace Interactive.Base
{
    public abstract class ChoicesSenderBehaviour : MonoBehaviour, Interactable
    {
        public virtual void Deselect()
        {
            // TODO
            // TODO
            transform.localScale *= 1f / 1.01f;
            GetComponent<SpriteRenderer>().material.SetInt("_OutlineEnabled", 0);
        }

        public virtual void Select()
        {
            // TODO
            transform.localScale *= 1.01f;
            GetComponent<SpriteRenderer>().material.SetInt("_OutlineEnabled", 1);
        }

        public virtual string GetDecription(Player contextPlayer) => "...";

        public abstract List<GameAction> GetChoices(Player contextPlayer);
    }
}