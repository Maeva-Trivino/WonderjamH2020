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

    //InputManagers
    public Rewired.Player inputManager1;
    public Rewired.Player inputManager2;

    [SerializeField] private House redHouse;
    [SerializeField] private House blueHouse;
    [SerializeField] private EndScreen endScreen;

    #region music
    [SerializeField]
    private AudioSource bipCompteurAudio;

    [SerializeField]
    private AudioSource bipStartGameAudio;

    [SerializeField]
    private AudioSource endGameAudio;

    [SerializeField]
    private AudioSource themeAudio;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        inputManager1 = ReInput.players.GetPlayer(0);
        inputManager2 = ReInput.players.GetPlayer(1);

        redHouse.RegisterTimer(this);
        blueHouse.RegisterTimer(this);

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

                EndGame();
            }
            string secondes = ((int)remainingTime % 60).ToString();
            if(secondes.Length == 1)
            {
                secondes = "0" + secondes;
            }
            timerText.text = ((int)remainingTime / 60).ToString() + ":" + secondes;
        }
    }
    public bool TimeIsStopped()
    {
        return !timeIsTicking;
    }

    public void PauseTimer()
    {
        timeIsTicking = false;
        themeAudio.Pause();
    }

    public void UnpauseTimer()
    {
        timeIsTicking = true;
        themeAudio.UnPause();
    }

    private void EndGame()
    {
        themeAudio.Stop();
        endGameAudio.Play();
        if (redHouse.CurrentHealth > blueHouse.CurrentHealth)
        {
            blueHouse.EndGame();
        }
        else if (redHouse.CurrentHealth < blueHouse.CurrentHealth)
        {
            redHouse.EndGame();
        }
        else
        {
            endScreen.ShowDraw();
        }
    }

    public IEnumerator ResetTimer()
    {
        remainingTime = initialTime;
        string secondes = ((int)remainingTime % 60).ToString();
        if (secondes.Length == 1)
        {
            secondes = "0" + secondes;
        }
        timerText.text = ((int)remainingTime / 60).ToString() + ":" + secondes;

        inputManager1.controllers.maps.SetAllMapsEnabled(false);
        inputManager2.controllers.maps.SetAllMapsEnabled(false);

        int compteur = 3;
        bipCompteurAudio.Play();

        while (compteur != 1)
        {
            yield return new WaitForSeconds(1.0f);
            bipCompteurAudio.Play();
            compteur--;
            timerStartGameText.text = compteur.ToString();
        }

        yield return new WaitForSeconds(1.0f);
        bipStartGameAudio.Play();
        timerStartGameText.text = "GO !";
        timeIsTicking = true;
        inputManager1.controllers.maps.SetAllMapsEnabled(true);
        inputManager2.controllers.maps.SetAllMapsEnabled(true);
        yield return new WaitForSeconds(1.0f);
        timerStartGameText.text = null;


        themeAudio.Play();

        
    }
}