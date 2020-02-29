using System.Collections.Generic;
using UnityEngine;
using ChoicePopup;
using Gameplay.Delivery;

public class OrderingStation : ChoicesSenderBehaviour
{
     private DeliverySystem deliverySystem;
    [SerializeField] private List<MissileBlueprint> missileBlueprints;

    public void Start()
    {
        deliverySystem = this.GetComponent<DeliverySystem>();
    }

    public override List<Choice> GetChoices(Player contextPlayer)
    {
        List<Choice> choices = new List<Choice>();

        missileBlueprints.ForEach((blueprint => choices.Add(new Choice(blueprint.name,() =>
        {
            //TODO Feedback
            if (contextPlayer.CanAffordMissile(blueprint))
            {
                ChooseMissile(blueprint, contextPlayer.PlayerId);
            }
        },() => contextPlayer.CanAffordMissile(blueprint)))));

        return choices;
    }

    public void ChooseMissile(MissileBlueprint blueprint,int playerId)
    {
        Missile orderedMissile = blueprint.GetMissile();
        deliverySystem.OrderItem(orderedMissile,playerId);
    }
}
