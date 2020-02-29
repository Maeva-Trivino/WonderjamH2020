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

    [SerializeField]
    private Player player;
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
    }

    public void Select()
    {
        // Pas de surbrillance de la maison quand elle est a porté
    }

    public void Deselect()
    {
    }

    public UserAction GetAction()
    {
        return new ComboAction(player.inputManager ,new List<string> { "Left", "Right"}, 2, () => Repair(repairingAmount), "Repair");
    }
}
