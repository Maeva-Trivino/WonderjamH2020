using System.Collections.Generic;
using UnityEngine;
using Gameplay.Delivery;
using Interactive.Base;

public class OrderingStation
{
    /*
     private DeliverySystem deliverySystem;
    [SerializeField] private List<MissileBlueprint> missileBlueprints;

    public void Start()
    {
        deliverySystem = this.GetComponent<DeliverySystem>();
    }

    public override List<GameAction> GetChoices(Player contextPlayer)
    {
        List<GameAction> choices = new List<GameAction>();

        missileBlueprints.ForEach((blueprint => choices.Add(new GameAction(blueprint.name,() =>
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
    */
}
