using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    public TMPro.TextMeshProUGUI timerText;

    [SerializeField]
    private TMPro.TextMeshProUGUI timerStartGameText;

    [SerializeField]
    float initialTime;
    float remainingTime;
    bool timeIsTicking = false;

    public Rewired.Player inputManager1;
    public Rewired.Player inputManager2;


    // Start is called before the first frame update
    void Start()
    {
        inputManager1 = ReInput.players.GetPlayer(0);
        inputManager2 = ReInput.players.GetPlayer(1);

        StartCoroutine(ResetTimer());
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

    public IEnumerator ResetTimer()
    {
        remainingTime = initialTime;
        string secondes = ((int)remainingTime % 60).ToString();
        timerText.text = ((int)remainingTime / 60).ToString() + ":" + secondes;

        inputManager1.controllers.maps.SetAllMapsEnabled(false);
        inputManager2.controllers.maps.SetAllMapsEnabled(false);

        int compteur = 3;

        while(compteur != 1)
        {
            yield return new WaitForSeconds(1.0f);
            compteur--;
            timerStartGameText.text = compteur.ToString();
        }

        yield return new WaitForSeconds(1.0f);
        timerStartGameText.text = "GO !";
        yield return new WaitForSeconds(1.0f);
        timerStartGameText.text = null;

        inputManager1.controllers.maps.SetAllMapsEnabled(true);
        inputManager2.controllers.maps.SetAllMapsEnabled(true);
        timeIsTicking = true;
    }
}