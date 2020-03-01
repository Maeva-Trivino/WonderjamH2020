using UnityEngine;

namespace Interactive.Base
{
    public abstract class QTEBehaviour : MonoBehaviour, Interactable
    {
        public virtual void Deselect()
        {
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

        public virtual string GetDecription(Player contextPlayer)
        {
            UserAction a = GetAction(contextPlayer);
            if (a == null)
                return string.Empty;
            return a.name;
        }

        public abstract UserAction GetAction(Player contextPlayer);

    }
}
