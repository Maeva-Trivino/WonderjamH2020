using ChoicePopup;
using System.Collections.Generic;
using QTE;
using UnityEngine;


public class House : ChoicesSenderBehaviour, Interactive
{
    [SerializeField]
    protected int currentHealth = 50;
    protected int maxHealth = 100;

    public HealthBar healthBar;
    
    [SerializeField]
    private int repairingAmount;

    [SerializeField] private EndScreen endScreen;
    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            float newPercentage = (float)value / (float)maxHealth;
            UpdateHealthBar(newPercentage);
            currentHealth = value;

            if (currentHealth <= 0)
            {
                endScreen.Show(enemyPlayer.name);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
        CurrentHealth = maxHealth;
    }

    private void UpdateHealthBar(float newPercentage)
    {
        if (healthBar != null)
        {
            healthBar.UpdateBar(newPercentage);
        }
    }

    public bool DoDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            // %TODO% trigger end scene
            return true;
        }
        return false;

    }

    public void Repair(int repairPoint)
    {
        CurrentHealth += repairPoint;
        if (CurrentHealth > maxHealth)
        {
            CurrentHealth = maxHealth;
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

    public UserAction GetAction(Player player)
    {
        return new ComboAction(player.inputManager ,new List<string> { "←", "→" }, 2, () => Repair(repairingAmount), "Repair");
    }

    public override List<Choice> GetChoices(Player contextPlayer)
    {
        // Test
        bool lol = true;
        return new List<Choice>() {
                new Choice("Toquer", () => Debug.Log("Knock! Knock!"), () => true),
                new Choice("Désactiver ce bouton", () => lol = false, () => lol),
            };
    }
}
