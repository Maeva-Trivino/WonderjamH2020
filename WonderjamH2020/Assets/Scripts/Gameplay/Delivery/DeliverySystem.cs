using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Gameplay;
using Rewired;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DeliverySystem : MonoBehaviour
{
    private Rewired.Player inputManager;

    //TODO Remove (TEST)
    public Missile itemPrefab;

    private const int LEFT_SIDE_ID = 0;
    private const int RIGHT_SIDE_ID = 1;

    public Queue<Missile> rightWaitingOrders;
    public Queue<Missile> leftWaitingOrders;

    public DeliveryTruck rightDeliveryTruck;
    public DeliveryTruck leftDeliveryTruck;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = ReInput.players.GetPlayer(0);

        rightWaitingOrders = new Queue<Missile>();
        leftWaitingOrders = new Queue<Missile>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //TODO Remove (TEST)
        if (inputManager.GetButtonDown("TEST2"))
        {
            OrderItem(itemPrefab,0);
        }
        */
        
        if (leftWaitingOrders.Count > 0 && !leftDeliveryTruck.IsOutForDelivery)
        {
            leftDeliveryTruck.Deliver(leftWaitingOrders.Dequeue());
        }

        if (rightWaitingOrders.Count > 0 && !rightDeliveryTruck.IsOutForDelivery)
        {
            rightDeliveryTruck.Deliver(rightWaitingOrders.Dequeue());
        }
    }
    public void OrderItem(Missile item,int playerId)
    {
        StartCoroutine(WaitForDelivery(item, playerId));
    }

    IEnumerator WaitForDelivery(Missile order,int playerId)
    {
        yield return new WaitForSeconds(order.timeToDeliver);

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
