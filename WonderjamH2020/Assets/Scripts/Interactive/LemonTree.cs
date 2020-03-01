using System.Collections;
using System.Collections.Generic;
using Interactive.Base;
using UnityEngine;

public class LemonTree : QTEBehaviour
{
    public Sprite spriteWithLemons;
    public Sprite spriteWithoutLemons;

    [SerializeField] private int lemonsPerBatchMin;
    [SerializeField] private int lemonsPerBatchMax;

    [SerializeField] private float refreshTimeMin;
    [SerializeField] private float refreshTimeMax;

    [SerializeField]
    private AudioSource pickupLemonAudio;

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

    public void HarvestLemons(Player player)
    {
        pickupLemonAudio.Play();
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

    public override UserAction GetAction(Player contextPlayer)
    {
        return HasLemons ? new ComboAction(contextPlayer.inputManager, new List<string> { "←", "→" }, 3,
            () => HarvestLemons(contextPlayer), "Pick up") : null;
    }
}
