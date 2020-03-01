using Interactive.Base;
using System.Collections.Generic;
using Assets.Scripts.Gameplay.Delivery;
using UnityEngine;


public class MissileLauncherSpot : QTEBehaviour, OrderItem
{
    [SerializeField]
    private GameObject missileLauncherPrefab;
    [SerializeField]
    private House opponentHouse;
    [SerializeField] private int buildingCosts;
    [SerializeField] private DeliverySystem deliverySystem;
    private bool canOrder;

    //Audio
    [SerializeField] private AudioSource impactSound;
    [SerializeField] private AudioSource launchSound;
    [SerializeField] private AudioSource constructionSound;

    

    public void Start()
    {
        GetComponent<Renderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
        canOrder = true;
    }

    public void OrderMissileLauncher(Player contextPlayer)
    {
        if (deliverySystem.CanOrder)
        {
            deliverySystem.OrderItem(this,contextPlayer.PlayerId);
            Debug.Log(buildingCosts);
            contextPlayer.Pay(buildingCosts);
            canOrder = false;
        }
    }

    public void BuildMissileLauncher(Player contextPlayer)
    {
        constructionSound.Play();
        GameObject missileLauncher = Instantiate(missileLauncherPrefab, transform.position, Quaternion.identity);
        MissileLauncher ml = missileLauncher.GetComponent<MissileLauncher>();
        ml.opponentHouse = opponentHouse;
        ml.impactSound = impactSound;
        ml.launchSound = launchSound;
        ml.deliverySystem = deliverySystem;
        contextPlayer.DestroyInteractive(this.gameObject);
    }

    public override UserAction GetAction(Player contextPlayer)
    {
        if (!canOrder || !contextPlayer.CanAfford(buildingCosts))
        {
            return null;
        }
        else
        {
            return new ComboAction(contextPlayer.inputManager, new List<string> { "←", "→", "→" }, 1,
                () => OrderMissileLauncher(contextPlayer), "Build - $" + buildingCosts);
        }
    }

    public void Use(Player contextPlayer)
    {
        BuildMissileLauncher(contextPlayer);
    }
}
