using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CannonReady : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] Color offColor;
    [SerializeField] Color onColor;
    private bool buttonOn = false;
    private bool isFlashing = false;
    [SerializeField] private float flashPeriod = 1f;


    private void Start()
    {
        text.color = offColor;
        GetComponent<Canvas>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }

    private void SwapColor()
    {
        if(buttonOn)
        {
            text.color = onColor;
        }
        else
        {
            text.color = offColor;
        }
    }

    private IEnumerator ToggleReadyCoroutine()
    {
        float timer = 0f;
        while (isFlashing)
        {
            timer += Time.deltaTime;
            if(timer > flashPeriod)
            {
                buttonOn = !buttonOn;
                SwapColor();
                timer = 0f;
            }
            yield return true;
        }
        buttonOn = false;
        SwapColor();
    }

    public void StartReady()
    {
        isFlashing = true;
        StartCoroutine(ToggleReadyCoroutine());
    }

    public void StopReady()
    {
        isFlashing = false;
    }
}
