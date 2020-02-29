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

        LTDescr tweenDesc= LeanTween.move(this.gameObject, deliveryTargetPosition, 5.0f);
        if (tweenDesc != null)
        {
            tweenDesc.setOnComplete(PutBox);
        }
    }

    public void PutBox()
    {
        StartCoroutine(this.PutBoxCoroutine());
    }

    public IEnumerator PutBoxCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        ItemBox newItemBox = Instantiate(itemBoxPrefab, this.transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity);
        newItemBox.item = itemToDeliver;
        newItemBox.enabled = true;

        yield return new WaitForSeconds(0.5f);

        LTDescr tweenDesc = LeanTween.move(this.gameObject, endPosition, 2.0f);
        if (tweenDesc != null)
        {
            tweenDesc.setOnComplete(LeaveMap);
        }
    }



    public void LeaveMap()
    {
        this.transform.position = startingPosition;
        itemToDeliver = null;
        outForDelivery = false;
    }
}
