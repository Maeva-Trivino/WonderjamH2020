using System.Collections;
using System.Collections.Generic;
using QTE;
using UnityEngine;

public class House : MonoBehaviour, Interactive
{
    [SerializeField]
    private int currentHealth = 50;

    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private int repairingAmount;

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

    public void Repair(int repairPoint)
    {
        currentHealth += repairPoint;
        if ( currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("Hp maison: " + currentHealth);
    }

    public void Select()
    {
        // Pas de surbrillance de la maison quand elle est a porté
    }

    public void Deselect()
    {
    }

    public UserAction GetAction(Rewired.Player inputManager)
    {
        return new ComboAction(inputManager ,new List<string> { "←", "→" }, 2, () => Repair(repairingAmount), "Repair");
    }
}
