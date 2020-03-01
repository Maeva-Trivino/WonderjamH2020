using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class HealthBar : MonoBehaviour
{
    public Image bar;

    private void Start()
    {
        moveBar(1);
    }

    private void moveBar(float percentage)
    {
        bar.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        bar.GetComponent<RectTransform>().anchorMax = new Vector2(percentage, 1);
        bar.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        bar.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
    }


    public void UpdateBar(float newPercentage)
    {
        if (newPercentage <= 1.0f && newPercentage >= 0.0f)
        {
            moveBar(newPercentage);
        }
        else if (newPercentage > 1.0f)
        {
            moveBar(1);
        }
        else if (newPercentage < 0.0f)
        {
            moveBar(0);
        }
    }
}
