using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Gameplay.Delivery;
using Rewired;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    private Rewired.Player inputManager;

    public MissileItem itemPrefab;

    private const int LEFT_SIDE_ID = 0;
    private const int RIGHT_SIDE_ID = 1;

    public Queue<MissileItem> rightWaitingOrders;
    public Queue<MissileItem> leftWaitingOrders;

    public DeliveryTruck rightDeliveryTruck;
    public DeliveryTruck leftDeliveryTruck;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = ReInput.players.GetPlayer(0);

        rightWaitingOrders = new Queue<MissileItem>();
        leftWaitingOrders = new Queue<MissileItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inputManager.GetButtonDown("TEST"))
        {
            OrderItem(itemPrefab,0);
        }

        if (leftWaitingOrders.Count > 0 && !leftDeliveryTruck.IsOutForDelivery)
        {
            leftDeliveryTruck.Deliver(leftWaitingOrders.Dequeue());
        }

        if (rightWaitingOrders.Count > 0 && !rightDeliveryTruck.IsOutForDelivery)
        {
            rightDeliveryTruck.Deliver(rightWaitingOrders.Dequeue());
        }
    }

    void OrderItem(MissileItem item,int playerId)
    {
        StartCoroutine(WaitForDelivery(item, playerId));
    }

    IEnumerator WaitForDelivery(MissileItem order,int playerId)
    {
        yield return new WaitForSeconds(1);

        //BREAK HERE
        switch (playerId)
        {
            case LEFT_SIDE_ID:
                leftWaitingOrders.Enqueue(order);
                break;
            case RIGHT_SIDE_ID:
                rightWaitingOrders.Enqueue(order);
                break;
            default:
                break;
        }
    }
}
