using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyScript : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    public TMPro.TextMeshProUGUI moneyText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = player.money.ToString() + "$";   
    }
}
