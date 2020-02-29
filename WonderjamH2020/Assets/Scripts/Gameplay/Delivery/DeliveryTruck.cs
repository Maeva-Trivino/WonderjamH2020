using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public class DeliveryTruck : MonoBehaviour
{
    private Vector3 startingPosition;

    public Transform deliveryTarget;
    public Transform endPoint;

    private Vector3 deliveryTargetPosition;
    private Vector3 endPosition;

    public float maxSpeed = 15;

    public ItemBox itemBoxPrefab;

    private bool outForDelivery = false;
    public bool IsOutForDelivery
    {
        get { return outForDelivery; }
        set { outForDelivery = value; }
    }

    private Missile itemToDeliver;
    public Missile ItemToDeliver
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

    public void Deliver(Missile item)
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
            float timelineSpeed = Math.Max(Math.Min(-0.03f * distance * (distance - Vector3.Distance(startingPosition, deliveryTargetPosition)), maxSpeed),1);
            float step = timelineSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, deliveryTargetPosition, step);
            yield return null;
        }

        PutBox();
    }

    public void PutBox()
    {
        ItemBox newItemBox = Instantiate(itemBoxPrefab, this.transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity);
        newItemBox.item = itemToDeliver;
        newItemBox.enabled = true;
        StartCoroutine(LeaveMap());
    }

    public IEnumerator LeaveMap()
    {
        float distance = 0;
        while ((distance = Vector3.Distance(transform.position, endPosition)) > Mathf.Epsilon)
        {
            float timelineSpeed = Math.Max(Math.Min(-0.04f * distance * (distance - Vector3.Distance(startingPosition, endPosition)), maxSpeed), 0.1f);
            float step = timelineSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPosition, step);
            yield return null;
        }

        this.transform.position = startingPosition;
        itemToDeliver = null;
        outForDelivery = false;
    }
}
