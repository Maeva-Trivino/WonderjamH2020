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
    [SerializeField] private AudioSource impactSound;
    [SerializeField] private AudioSource launchSound;
    [SerializeField] private DeliverySystem deliverySystem;

    public void Start()
    {
        GetComponent<Renderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }

    public void OrderMissileLauncher(Player contextPlayer)
    {
        if (deliverySystem.CanOrder)
        {
            deliverySystem.OrderItem(this,contextPlayer.PlayerId);
            Debug.Log(buildingCosts);
            contextPlayer.Pay(buildingCosts);
            deliverySystem.CanOrder = false;
        }
    }

    public void BuildMissileLauncher(Player contextPlayer)
    {
        GameObject missileLauncher = Instantiate(missileLauncherPrefab, transform.position, Quaternion.identity);
        MissileLauncher ml = missileLauncher.GetComponent<MissileLauncher>();
        ml.opponentHouse = opponentHouse;
        ml.impactSound = impactSound;
        ml.launchSound = launchSound;
        ml.deliverySystem = deliverySystem;
        contextPlayer.DestroyInteractive(this.gameObject);
        deliverySystem.CanOrder = true;
    }

    public override UserAction GetAction(Player contextPlayer)
    {
        if (!deliverySystem.CanOrder || !contextPlayer.CanAfford(buildingCosts))
        {
            return null;
        }
        else
        {
            return new ComboAction(contextPlayer.inputManager, new List<string> {"→"}, 5,
                () => OrderMissileLauncher(contextPlayer), "Build");
        }
    }

    public void Use(Player contextPlayer)
    {
        BuildMissileLauncher(contextPlayer);
    }
}
