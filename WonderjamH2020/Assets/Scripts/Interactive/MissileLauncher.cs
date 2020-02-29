using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChoicePopup;
using System;

public class MissileLauncher : ChoicesSenderBehaviour
{
    [SerializeField] private GameObject missilePrefab;
    private Missile missile;
    [SerializeField] private House opponentHouse;
    [SerializeField] private float flightDuration = 5f;
    [SerializeField] private float height = 5f;
    [SerializeField] private int missileDamage = 5;
    [SerializeField] public float shakeAmplitude = 0.2f;
    [SerializeField] public float shakePeriod = 0.1f;
    [SerializeField] public float shakeDuration = 2;
    //Audio
    [SerializeField] private AudioSource launchSound;
    [SerializeField] private AudioSource impactSound;


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
        }
    }

    private void Upgrade()
    {
        Debug.Log("Upgrade");
    }

    public override List<Choice> GetChoices(Player contextPlayer)
    {
        return new List<Choice>() {
                new Choice("Feu !", () => Fire(), () => true),
                new Choice("Recharger", () => RechargeMissile(), () => true),
                new Choice("Upgrade", () => Upgrade(), () => true)
            };
    }
}
