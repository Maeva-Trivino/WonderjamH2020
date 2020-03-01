using Interactive.Base;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : ChoicesSenderBehaviour
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


    public void OrderMissile(Player contextPlayer)
    {
        if (contextPlayer.CanAffordMissile(missilePrice))
        {
            contextPlayer.PayForMissile(missilePrice);
            deliverySystem.OrderItem(this,contextPlayer.PlayerId);
        }
    }

    public void RechargeMissile()
    {
        if(missile == null)
        {
            missile = Instantiate(missilePrefab, transform).GetComponent<Missile>();
            missile.transform.position = transform.position;
            missile.Initialize(opponentHouse,flightDuration,height,missileDamage,shakeAmplitude,shakePeriod,shakeDuration,
                                launchSound,impactSound);
        }
    }
    public void Fire()
    {
        if(missile != null)
        {
            missile.LaunchMissile();
            GetComponent<Animator>().SetTrigger("shoot");
        }
    }

    private void Upgrade()
    {
        Debug.Log("Upgrade");
    }

    public override List<GameAction> GetChoices(Player contextPlayer)
    {
        return new List<GameAction>() {
                new GameAction("Feu !", () => Fire(), () => true),
                new GameAction("Recharger", () => OrderMissile(contextPlayer), () => contextPlayer.CanAffordMissile(missilePrice)),
                new GameAction("Upgrade", () => Upgrade(), () => true)
            };
    }
}
