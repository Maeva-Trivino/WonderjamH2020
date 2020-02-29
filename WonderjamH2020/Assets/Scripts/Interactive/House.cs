using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField]
    protected int currentHealth = 50;

    [SerializeField]
    protected int maxHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool DoDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            // %TODO% trigger end scene
            return true;
        }
        return false;

    }
}
