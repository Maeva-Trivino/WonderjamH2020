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
    [SerializeField] private float randomOffSetRange = 2;
    [SerializeField] public int timeToDeliver;
    private bool launched = false;
    [SerializeField] private Vector3 target;
    [SerializeField] private ParticleSystem fireTray;

    //ScreenShake
    [SerializeField] public float shakeAmplitude;
    [SerializeField] public float shakePeriod;
    [SerializeField] public float shakeDuration;

    //Audio
    [SerializeField] private AudioSource launchSound;
    [SerializeField] private AudioSource impactSound;

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
        this.impactSound = blueprint.impactSound;
        this.launchSound = blueprint.launchSound;
    }


    private void Update()
    {
        if(launched && Vector2.Distance(transform.position, target) < 0.1f)
        {
            Explode();
        }
    }

    public void Initialize(House _opponentHouse, float _flightDuration,float _height,int _missileDamage,
                            float _shakeAmplitude, float _shakePeriod, float _shakeDuration,
                            AudioSource _launchSound, AudioSource _impactSound)
    {
        opponentHouse = _opponentHouse;
        flightDuration = _flightDuration;
        height = _height;
        missileDamage = _missileDamage;
        shakeAmplitude = _shakeAmplitude;
        shakeDuration = _shakeDuration;
        shakePeriod = _shakePeriod;
        launchSound = _launchSound;
        impactSound = _impactSound;
    }

    public void LaunchMissile()
    {
        if(!launched)
        {
            fireTray.Play();
            launched = true;
            /*Vector3[] bezierCloche = {gameObject.transform.position, opponentHouse.transform.position + height * Vector3.up
                            ,gameObject.transform.position + height * Vector3.up, opponentHouse.transform.position};*/
            Vector3 randomOffSet = new Vector3(Random.Range(0, randomOffSetRange), Random.Range(0, randomOffSetRange));
            target = opponentHouse.transform.position + randomOffSet;
            Vector3 midle = new Vector3((gameObject.transform.position.x + target.x)/2,
                                         (gameObject.transform.position.y + target.y)/2,0);
            Vector3[] bezier = {gameObject.transform.position, midle + Vector3.up * height
                                ,Vector3.up * height, target};
            LeanTween.move(gameObject, bezier, flightDuration);//.setEaseInExpo();
            //LeanTween.rotateZ(gameObject, 180, flightDuration).setEaseInExpo();
            launchSound.Play();
        }
    }

    private void Explode()
    {
        Debug.Log("B O O M");
        opponentHouse.DoDamage(missileDamage);
        Camera.main.GetComponent<ScreenShaker>().ScreenShake(shakeAmplitude, shakePeriod,shakeDuration);
        fireTray.Stop();
        GetComponentInChildren<Animator>().SetTrigger("explode");
        DetachParticle();
        impactSound.Play();
        Destroy(gameObject);
    }

    private void DetachParticle()
    {
        transform.DetachChildren();
    }
}
