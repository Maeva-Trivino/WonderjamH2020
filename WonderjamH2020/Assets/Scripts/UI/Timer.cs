using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    public TMPro.TextMeshProUGUI timerText;

    [SerializeField]
    float initialTime;
    float remainingTime;
    bool timeIsTicking = true;

    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeIsTicking)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                PauseTimer();
                // %TODO% Trigger endscene
            }
            string secondes = ((int)remainingTime % 60).ToString();
            if(secondes.Length == 1)
            {
                secondes = "0" + secondes;
            }
            timerText.text = ((int)remainingTime / 60).ToString() + ":" + secondes;
        }
    }

    public void PauseTimer()
    {
        timeIsTicking = false;
    }

    public void UnpauseTimer()
    {
        timeIsTicking = true;
    }

    public void ResetTimer()
    {
        remainingTime = initialTime;
        timeIsTicking = true;
    }
}
