using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactive.Base;

public class Cat : QTEBehaviour
{
    [SerializeField]
    private AudioSource meowAudio;
    public override UserAction GetAction(Player contextPlayer)
    {
        return new ComboAction(contextPlayer.inputManager, new List<string> { "M", "E", "O", "W"}, 7, () => Meow(), "Pet");
    }
    private void Meow()
    {
        meowAudio.Play();
        Debug.Log("Meow");
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Renderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1 - 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
