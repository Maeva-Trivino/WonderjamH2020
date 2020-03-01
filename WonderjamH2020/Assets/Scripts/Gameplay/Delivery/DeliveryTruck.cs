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

    private MissileLauncher recipientToDeliver;
    public MissileLauncher RecipientToDeliver
    {
        get { return recipientToDeliver; }
        set { recipientToDeliver = value; }
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

    public void Deliver(MissileLauncher recipient)
    {
        outForDelivery = true;
        recipientToDeliver = recipient;

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

        ItemBox newItemBox = Instantiate(itemBoxPrefab, this.transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity);
        newItemBox.recipient = recipientToDeliver;
        newItemBox.enabled = true;

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
        recipientToDeliver = null;
        outForDelivery = false;
    }
}
