using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [Tooltip("Time to reach target")]
    [SerializeField] private float flightDuration = 5f;
    [Tooltip("How hight the missile will reach")]
    [SerializeField] private float height = 5f;
    [SerializeField] private int missileDamage = 5;
    [SerializeField] public House opponentHouse;
    private bool launched = false;


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
