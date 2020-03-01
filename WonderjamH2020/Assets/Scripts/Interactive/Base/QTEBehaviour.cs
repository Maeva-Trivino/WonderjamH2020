using UnityEngine;

namespace Interactive.Base
{
    public abstract class QTEBehaviour : MonoBehaviour, Interactable
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

        public virtual string GetDecription(Player contextPlayer) => GetAction(contextPlayer).name;
        public abstract UserAction GetAction(Player contextPlayer);

    }
}
