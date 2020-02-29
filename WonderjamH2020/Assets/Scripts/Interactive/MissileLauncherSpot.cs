using QTE;
using System.Collections.Generic;
using UnityEngine;


public class MissileLauncherSpot : MonoBehaviour, Interactive
{
    [SerializeField]
    private GameObject missileLauncherPrefab;

    public void Deselect()
    {
    }

    public UserAction GetAction(Player player)
    {
        return new ComboAction(player.inputManager, new List<string> {"→"}, 5, () => BuildMissileLauncher(), "Build");
    }

    public void BuildMissileLauncher()
    {
        Instantiate(missileLauncherPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject,0.2f);
    }
    public void Select()
    {
    }
    public void Start()
    {
        GetComponent<Renderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }
}
