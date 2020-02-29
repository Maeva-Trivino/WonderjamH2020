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
    //ScreenShake
    [SerializeField] public float shakeAmplitude;
    [SerializeField] public float shakePeriod;
    [SerializeField] public float shakeDuration;
    private bool launched = false;
    private LTDescr missileTween;
    private LTDescr missileRotationTween;
    private ParticleSystem fireTray;

    public Missile() : base() { }

    public Missile(MissileBlueprint blueprint)
    {
        this.name = blueprint.name;
        this.price = blueprint.price;
        this.flightDuration = blueprint.flightDuration;
        this.height = blueprint.height;
        this.missileDamage = blueprint.missileDamage;
        this.timeToDeliver = blueprint.timeToDeliver;
        this.shakeAmplitude = blueprint.shakeAmplitude;
        this.shakePeriod = blueprint.shakePeriod;
        this.shakeDuration = blueprint.shakeDuration;
    }

    private void Start()
    {
        fireTray = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if(launched && Vector2.Distance(transform.position,opponentHouse.transform.position) < 0.1f)
        {
            Explode();
        }
    }

    public void Initialize(House _opponentHouse, float _flightDuration,float _height,int _missileDamage,
                            float _shakeAmplitude, float _shakePeriod, float _shakeDuration)
    {
        opponentHouse = _opponentHouse;
        flightDuration = _flightDuration;
        height = _height;
        missileDamage = _missileDamage;
        shakeAmplitude = _shakeAmplitude;
        shakeDuration = _shakeDuration;
        shakePeriod = _shakePeriod;
    }

    public void LaunchMissile()
    {
        fireTray.Play();
        launched = true;
        Vector3[] bezier = {gameObject.transform.position, opponentHouse.transform.position + height * Vector3.up
                            ,gameObject.transform.position + height * Vector3.up, opponentHouse.transform.position};
        missileTween = LeanTween.move(gameObject, bezier, flightDuration).setEaseInExpo();
        missileRotationTween = LeanTween.rotateZ(gameObject,180, flightDuration).setEaseInExpo();
    }

    private void Explode()
    {
        Debug.Log("B O O M");
        opponentHouse.DoDamage(missileDamage);
        Camera.main.GetComponent<ScreenShaker>().ScreenShake(shakeAmplitude, shakePeriod,shakeDuration);
        fireTray.Stop();
        DetachParticle();
        Destroy(gameObject);
    }

    private void DetachParticle()
    {
        transform.DetachChildren();
    }
}
