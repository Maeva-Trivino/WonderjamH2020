using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Gameplay.Delivery;
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

    [SerializeField] private ItemBoxContainer itemBoxContainer;

    private bool outForDelivery = false;
    public bool IsOutForDelivery
    {
        get { return outForDelivery; }
        set { outForDelivery = value; }
    }

    private OrderItem itemToDeliver;
    public OrderItem ItemToDeliver
    {
        get { return itemToDeliver; }
        set { itemToDeliver = value; }
    }

    #region music
    [SerializeField]
    private AudioSource stopCarAudio;

    [SerializeField]
    private AudioSource klazonCarAudio;

    [SerializeField]
    private AudioSource startCarAudio;
    #endregion

    public void Start()
    {
        startingPosition = this.transform.position;
        deliveryTargetPosition = deliveryTarget.position;
        endPosition = endPoint.position;
    }

    public void Deliver(OrderItem item)
    {
        outForDelivery = true;
        itemToDeliver = item;

        StartCoroutine(PlayStopCar());
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

        ItemBox newItemBox = new ItemBox(this.itemToDeliver);
        itemBoxContainer.Add(newItemBox);

        klazonCarAudio.Play();

        yield return new WaitForSeconds(0.5f);

        startCarAudio.Play();

        LTDescr tweenDesc = LeanTween.move(this.gameObject, endPosition, 2.0f);
        if (tweenDesc != null)
        {
            tweenDesc.setOnComplete(LeaveMap);
        }
    }


    public IEnumerator PlayStopCar()
    {
        yield return new WaitForSeconds(4.5f);

        stopCarAudio.Play();
    }

    public void LeaveMap()
    {
        this.transform.position = startingPosition;
        itemToDeliver = null;
        outForDelivery = false;
    }
}
