using System;
using System.Collections;
using System.Collections.Generic;
using Interactive.Base;
using TMPro;
using UnityEngine;

public class ItemBoxContainer : QTEBehaviour
{
    private Queue<ItemBox> itemBoxes;
    private bool isDiplayed;
    [SerializeField] private TextMeshPro countText;
    [SerializeField] private GameObject displayContainer;
    
    private void Start()
    {
        itemBoxes = new Queue<ItemBox>();
        isDiplayed = true;
        var parent = countText.transform.parent;
        SpriteRenderer parentSR = parent.GetComponent<SpriteRenderer>();
        var renderer = countText.GetComponent<Renderer>();

        if (parentSR != null && renderer != null)
        {
            renderer.sortingLayerID = parentSR.sortingLayerID;
            renderer.sortingOrder = parentSR.sortingOrder;
        }

        UpdateDisplay();
    }

    public void Add(ItemBox newBox)
    {
        itemBoxes.Enqueue(newBox);

        UpdateDisplay();
    }

    public void UseTop(Player contextPlayer)
    {
        while (itemBoxes.Count > 0)
        {
            itemBoxes.Dequeue().item.Use(contextPlayer);
            UpdateDisplay();
        }
    }

    public void UpdateDisplay()
    {
        if (!isDiplayed && itemBoxes.Count > 0)
        {
            isDiplayed = true;
            displayContainer.SetActive(true);
            Debug.Log(GetComponent<BoxCollider2D>());
            GetComponent<BoxCollider2D>().enabled = true;
        }

        if (isDiplayed && itemBoxes.Count <= 0)
        {
            isDiplayed = false;
            displayContainer.SetActive(false);
            Debug.Log(GetComponent<BoxCollider2D>());
            GetComponent<BoxCollider2D>().enabled = false;
        }

        if (isDiplayed)
        {
            countText.text = string.Format("x{0}", itemBoxes.Count);
        }
    }

    public override UserAction GetAction(Player contextPlayer)
    {
        return new UserAction(()=>UseTop(contextPlayer),"Open packages");
    }
}
