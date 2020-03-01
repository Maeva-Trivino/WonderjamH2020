using Interactive.Base;
using System.Collections.Generic;
using UnityEngine;


public class MissileLauncherSpot : QTEBehaviour
{
    [SerializeField]
    private GameObject missileLauncherPrefab;


    public void Start()
    {
        GetComponent<Renderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }

    public void BuildMissileLauncher(Player contextPlayer)
    {
        Instantiate(missileLauncherPrefab, transform.position, Quaternion.identity);
        contextPlayer.DestroyInteractive(this.gameObject);
    }

    public override UserAction GetAction(Player contextPlayer)
    {
        return new ComboAction(contextPlayer.inputManager, new List<string> { "→" }, 5, () => BuildMissileLauncher(contextPlayer), "Build");
    }
}
