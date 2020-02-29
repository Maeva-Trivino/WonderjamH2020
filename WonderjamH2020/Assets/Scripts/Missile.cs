using System.Collections;
using System.Collections.Generic;
using Gameplay.Delivery;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private string name = "default";
    [SerializeField] private int price = 10;
    [SerializeField] private float flightDuration = 5f;
    [SerializeField] private float height = 5f;
    [SerializeField] private int missileDamage = 5;
    [SerializeField] private House opponentHouse;
    [SerializeField] public int timeToDeliver;

    public Missile() : base() { }

    public Missile(MissileBlueprint blueprint)
    {
        this.name = blueprint.name;
        this.price = blueprint.price;
        this.flightDuration = blueprint.flightDuration;
        this.height = blueprint.height;
        this.missileDamage = blueprint.missileDamage;
        this.timeToDeliver = blueprint.timeToDeliver;
    }


    private void Start()
    {
        LaunchMissile();
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position,opponentHouse.transform.position) < 0.1f)
        {
            Explode();
        }
    }

    public void LaunchMissile()
    {
        Vector3[] bezier = {gameObject.transform.position, opponentHouse.transform.position + height * Vector3.up
                            ,gameObject.transform.position + height* Vector3.up, opponentHouse.transform.position};
        LeanTween.move(gameObject, bezier, flightDuration);
    }

    private void Explode()
    {
        Debug.Log("B O O M");
        opponentHouse.DoDamage(missileDamage);
        Destroy(gameObject);
    }
}
