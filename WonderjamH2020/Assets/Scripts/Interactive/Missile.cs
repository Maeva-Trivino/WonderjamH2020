using System.Collections;
using System.Collections.Generic;
using Gameplay.Delivery;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private string name = "default";
    [SerializeField] private int price = 10;
    [Tooltip("Time to reach target")]
    [SerializeField] private float flightDuration = 5f;
    [Tooltip("How hight the missile will reach")]
    [SerializeField] private float height = 5f;
    [SerializeField] private int missileDamage = 5;
    [SerializeField] public House opponentHouse;
    [SerializeField] public int timeToDeliver;
    private bool launched = false;


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

    private void Update()
    {
        if(launched && Vector2.Distance(transform.position,opponentHouse.transform.position) < 0.1f)
        {
            Explode();
        }
    }

    public void Initialize(House _opponentHouse, float _flightDuration,float _height,int _missileDamage)
    {
        opponentHouse = _opponentHouse;
        flightDuration = _flightDuration;
        height = _height;
        missileDamage = _missileDamage;
    }

    public void LaunchMissile()
    {
        launched = true;
        Vector3[] bezier = {gameObject.transform.position, opponentHouse.transform.position + height * Vector3.up
                            ,gameObject.transform.position + height * Vector3.up, opponentHouse.transform.position};
        LeanTween.move(gameObject, bezier, flightDuration).setEaseInExpo();
        LeanTween.rotateZ(gameObject,180, flightDuration).setEaseInExpo();
    }

    private void Explode()
    {
        Debug.Log("B O O M");
        opponentHouse.DoDamage(missileDamage);
        Destroy(gameObject);
    }
}
