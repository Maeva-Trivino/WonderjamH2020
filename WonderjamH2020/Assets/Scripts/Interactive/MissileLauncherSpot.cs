using Interactive.Base;
using System.Collections.Generic;
using UnityEngine;


public class MissileLauncherSpot : QTEBehaviour
{
    [SerializeField]
    private GameObject missileLauncherPrefab;
    [SerializeField]
    private House opponentHouse;
    [SerializeField] private AudioSource impactSound;
    [SerializeField] private AudioSource launchSound;
    [SerializeField] private DeliverySystem deliverySystem;

    public void Start()
    {
        GetComponent<Renderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }

    public void BuildMissileLauncher(Player contextPlayer)
    {
        GameObject missileLauncher = Instantiate(missileLauncherPrefab, transform.position, Quaternion.identity);
        MissileLauncher ml = missileLauncher.GetComponent<MissileLauncher>();
        ml.opponentHouse = opponentHouse;
        ml.impactSound = impactSound;
        ml.launchSound = launchSound;
        ml.deliverySystem = deliverySystem;
        contextPlayer.DestroyInteractive(this.gameObject);
    }

    public override UserAction GetAction(Player contextPlayer)
    {
        return new ComboAction(contextPlayer.inputManager, new List<string> { "→" }, 5, () => BuildMissileLauncher(contextPlayer), "Build");
    }
}
