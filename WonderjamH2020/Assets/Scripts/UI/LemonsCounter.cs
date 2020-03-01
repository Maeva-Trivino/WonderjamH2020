using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemonsCounter : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    public TMPro.TextMeshProUGUI lemonsText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lemonsText.text = player.lemons.ToString();
    }
}
