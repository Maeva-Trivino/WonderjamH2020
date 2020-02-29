using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Gameplay.Delivery;
using UnityEngine;

public class DeliveryTruck : MonoBehaviour
{
    private Vector3 startingPosition;

    public Transform deliveryTarget;
    public Transform endPoint;

    private Vector3 deliveryTargetPosition;
    private Vector3 endPosition;

    public float speed = 100; 

    private bool outForDelivery = false;
    public bool IsOutForDelivery
    {
        get { return outForDelivery; }
        set { outForDelivery = value; }
    }

    private MissileItem itemToDeliver;
    public MissileItem ItemToDeliver
    {
        get { return itemToDeliver; }
        set { itemToDeliver = value; }
    }

    public void Start()
    {
        startingPosition = this.transform.position;
        deliveryTargetPosition = deliveryTarget.position;
        endPosition = endPoint.position;
    }

    public void Deliver(MissileItem item)
    {
        outForDelivery = true;
        itemToDeliver = item;
        StartCoroutine(MoveToTarget());
    }

    public IEnumerator MoveToTarget()
    {
        float distance = 0;
        while ((distance = Vector3.Distance(transform.position, deliveryTargetPosition)) > Mathf.Epsilon)
        {
            float timelineSpeed = Math.Max(Math.Min(-0.03f * distance * (distance - Vector3.Distance(startingPosition, deliveryTargetPosition)),15),1);
            float step = timelineSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, deliveryTargetPosition, step);
            yield return null;
        }

        PutBox();
    }

    public void PutBox()
    {
        //BOITE !!

        StartCoroutine(LeaveMap());
    }

    public IEnumerator LeaveMap()
    {
        float distance = 0;
        while ((distance = Vector3.Distance(transform.position, endPosition)) > Mathf.Epsilon)
        {
            float timelineSpeed = Math.Max(Math.Min(-0.04f * distance * (distance - Vector3.Distance(startingPosition, endPosition)), 15), 0.1f);
            float step = timelineSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPosition, step);
            yield return null;
        }

        this.transform.position = startingPosition;
        itemToDeliver = null;
        outForDelivery = false;
    }
}
