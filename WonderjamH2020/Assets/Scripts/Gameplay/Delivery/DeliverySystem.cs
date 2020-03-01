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

    private const int LEFT_SIDE_ID = 0;
    private const int RIGHT_SIDE_ID = 1;
    
    [SerializeField]
    private int timeForDelivery;

    private Queue<MissileLauncher> rightWaitingOrders;
    private Queue<MissileLauncher> leftWaitingOrders;

    [SerializeField]
    private DeliveryTruck rightDeliveryTruck;
    [SerializeField]
    private DeliveryTruck leftDeliveryTruck;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = ReInput.players.GetPlayer(0);

        rightWaitingOrders = new Queue<MissileLauncher>();
        leftWaitingOrders = new Queue<MissileLauncher>();
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
    public void OrderItem(MissileLauncher recipient,int playerId)
    {
        StartCoroutine(WaitForDelivery(recipient, playerId));
    }

    IEnumerator WaitForDelivery(MissileLauncher order,int playerId)
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
