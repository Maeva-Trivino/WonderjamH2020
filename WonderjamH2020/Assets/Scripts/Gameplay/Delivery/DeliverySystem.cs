using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts.Gameplay.Delivery;
using Gameplay;
using Rewired;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DeliverySystem : MonoBehaviour
{
    private Rewired.Player inputManager;

    private const int LEFT_SIDE_ID = 0;
    private const int RIGHT_SIDE_ID = 1;
    
    [SerializeField]
    private int timeForDelivery;

    private Queue<OrderItem> rightWaitingOrders;
    private Queue<OrderItem> leftWaitingOrders;

    [SerializeField]
    private DeliveryTruck rightDeliveryTruck;
    [SerializeField]
    private DeliveryTruck leftDeliveryTruck;

    public bool CanOrder;


    // Start is called before the first frame update
    void Start()
    {
        inputManager = ReInput.players.GetPlayer(0);
        CanOrder = true;
        rightWaitingOrders = new Queue<OrderItem>();
        leftWaitingOrders = new Queue<OrderItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (leftWaitingOrders.Count > 0 && !leftDeliveryTruck.IsOutForDelivery)
        {
            leftDeliveryTruck.Deliver(leftWaitingOrders.Dequeue());
        }

        if (rightWaitingOrders.Count > 0 && !rightDeliveryTruck.IsOutForDelivery)
        {
            rightDeliveryTruck.Deliver(rightWaitingOrders.Dequeue());
        }
    }

    public void OrderItem(OrderItem recipient,int playerId)
    {
        StartCoroutine(WaitForDelivery(recipient, playerId));
    }

    IEnumerator WaitForDelivery(OrderItem order,int playerId)
    {
        yield return new WaitForSeconds(timeForDelivery);

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
