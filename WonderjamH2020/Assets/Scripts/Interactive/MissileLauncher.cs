using Interactive.Base;
using System.Collections.Generic;
using Assets.Scripts.Gameplay.Delivery;
using UnityEngine;

public class MissileLauncher : ChoicesSenderBehaviour, OrderItem
{
    [SerializeField] private GameObject missilePrefab;
    private Missile missile;
    [SerializeField] public House opponentHouse;
    [SerializeField] private float flightDuration = 5f;
    [SerializeField] private float height = 5f;
    [SerializeField] private int missileDamage = 5;
    [SerializeField] public float shakeAmplitude = 0.2f;
    [SerializeField] public float shakePeriod = 0.1f;
    [SerializeField] public float shakeDuration = 2;
    //Audio
    [SerializeField] public AudioSource launchSound;
    [SerializeField] public AudioSource impactSound;

    [SerializeField] public DeliverySystem deliverySystem;
    [SerializeField] private int missilePrice = 100;
    [SerializeField] private Transform spawnPointOffSet;
    [SerializeField] private Transform firePoint;

    private void Start()
    {
        GetComponent<Renderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }

    public void OrderMissile(Player contextPlayer)
    {
        if (contextPlayer.CanAfford(missilePrice))
        {
            contextPlayer.Pay(missilePrice);
            deliverySystem.OrderItem(this,contextPlayer.PlayerId);
            deliverySystem.CanOrder = false;
        }
    }

    public void RechargeMissile()
    {
        if(missile == null)
        {
            missile = Instantiate(missilePrefab, transform).GetComponent<Missile>();
            missile.transform.position = spawnPointOffSet.position;
            missile.Initialize(opponentHouse,flightDuration,height,missileDamage,shakeAmplitude,shakePeriod,shakeDuration,
                                launchSound,impactSound);
        }
    }
    public void Fire()
    {
        if(missile != null)
        {
            missile.transform.position = firePoint.position;
            missile.LaunchMissile();
            GetComponent<Animator>().SetTrigger("shoot");
            missile = null;

            deliverySystem.CanOrder = true;
        }
    }

    public bool CanOrder(Player contextPlayer)
    {
        return deliverySystem.CanOrder && contextPlayer.CanAfford(missilePrice);
    }

    public override List<GameAction> GetChoices(Player contextPlayer)
    {
        return new List<GameAction>() {
                new GameAction("Feu !", () => {Fire(); contextPlayer.HideCurrentPopup(); }, () => missile != null),
                new GameAction("Recharger - $" + missilePrice, () => {OrderMissile(contextPlayer); contextPlayer.HideCurrentPopup(); }, () => CanOrder(contextPlayer))
            };
    }

    public void Use(Player contextPlayer)
    {
        RechargeMissile();
    }
}
