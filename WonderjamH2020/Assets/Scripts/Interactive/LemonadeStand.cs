using System.Collections;
using System.Collections.Generic;
using QTE;
using Rewired;
using UnityEngine;

public class LemonadeStand : MonoBehaviour, Interactive
{
    [SerializeField]
    private int lemonadePrice;
    public void Deselect()
    {
        
    }

    public UserAction GetAction(Player player)
    {
        return new ComboAction(player.inputManager, new List<string> { "←", "↑", "→", "↓" }, 1, () => SellLemonade(player), "Sell");
    }

    public void SellLemonade(Player player)
    {
        player.SellLemonade(lemonadePrice);
        Debug.Log("Lemonade Sold");
    }
    public void Select()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
