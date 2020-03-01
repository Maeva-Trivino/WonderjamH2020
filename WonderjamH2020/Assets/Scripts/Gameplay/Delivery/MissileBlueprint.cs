using UnityEngine;

namespace Gameplay.Delivery
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MissileBlueprint", order = 1)]
    public class MissileBlueprint : ScriptableObject
    {
        [SerializeField] public string name = "default";
        [SerializeField] public int price = 10;
        [SerializeField] public float flightDuration = 5f;
        [SerializeField] public float height = 5f;
        [SerializeField] public int missileDamage = 5;
        [SerializeField] public int timeToDeliver;
        [SerializeField] public float shakeAmplitude;
        [SerializeField] public float shakePeriod;
        [SerializeField] public float shakeDuration;
        [SerializeField] public AudioSource impactSound;
        [SerializeField] public AudioSource launchSound;

        public Missile GetMissile()
        {
            return new Missile(this);
        }
    }
}
