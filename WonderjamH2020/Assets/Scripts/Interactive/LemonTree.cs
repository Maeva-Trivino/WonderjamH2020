using System.Collections;
using System.Collections.Generic;
using QTE;
using UnityEngine;

public class LemonTree : MonoBehaviour, Interactive
{
    public Sprite spriteWithLemons;
    public Sprite spriteWithoutLemons;

    [SerializeField] private int lemonsPerBatchMin;
    [SerializeField] private int lemonsPerBatchMax;

    [SerializeField] private float refreshTimeMin;
    [SerializeField] private float refreshTimeMax;

    private bool hasLemons = true;
    public bool HasLemons
    {
        get { return hasLemons; }
        set
        {
            hasLemons = value;

            if(hasLemons)
            {
                this.GetComponent<SpriteRenderer>().sprite = spriteWithLemons;
            }
            else
            {
                this.GetComponent<SpriteRenderer>().sprite = spriteWithoutLemons;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        HasLemons = true;
    }

    public void Select()
    {
    }

    public void Deselect()
    {
    }

    public UserAction GetAction(Player player)
    {
        return new ComboAction(player.inputManager, new List<string> {"←", "→"}, 3,
            () => HarvestLemons(player), "Pick up");
    }

    public void HarvestLemons(Player player)
    {
        HasLemons = false;

        int lemonYield = Random.Range(lemonsPerBatchMin, lemonsPerBatchMax);

        player.HarvestLemons(lemonYield);

        StartCoroutine(this.refreshLemons());
        //JUST ANOTHER LEMON TREE
    }

    private IEnumerator refreshLemons()
    {
       
        float timeToWait = Random.Range(refreshTimeMin, refreshTimeMax);
        yield return new WaitForSeconds(timeToWait);
        HasLemons = true;
    }
}
