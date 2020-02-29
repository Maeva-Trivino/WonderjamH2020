using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float flightDuration = 5f;
    [SerializeField] private float height = 5f;
    [SerializeField] private int missileDamage = 5;
    [SerializeField] private House opponentHouse;


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
