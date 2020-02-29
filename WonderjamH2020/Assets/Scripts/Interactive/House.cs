using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField]
    protected int currentHealth = 50;
    protected int maxHealth = 100;

    public HealthBar healthBar;

    public int CurrentHealth
    {
        get { return currentHealth;}
        set
        {
            float newPercentage = (float) value / (float) maxHealth;
            UpdateHealthBar(newPercentage);
            currentHealth = value;
        }
    }

    [SerializeField]
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateHealthBar(float newPercentage)
    {
        if(healthBar != null)
        {
            healthBar.UpdateBar(newPercentage);
        }
    }

    public bool DoDamage(int damage)
    {
        CurrentHealth -= damage;
        if(CurrentHealth <= 0)
        {
            // %TODO% trigger end scene
            return true;
        }
        return false;

    }

    public void Repair(int repairPoint)
    {
        CurrentHealth += repairPoint;
        if ( CurrentHealth > maxHealth)
        {
            CurrentHealth = maxHealth;
        }
    }
}
