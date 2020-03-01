using System.Collections;
using System.Collections.Generic;
using Popup;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSceneScript : MonoBehaviour
{
    #region sprites mémés
    [SerializeField]
    private Sprite memeNormale;

    [SerializeField]
    private Sprite memeSemiEnervee;

    [SerializeField]
    private Sprite memeEnervee;
    #endregion

    private Image memeImage;

    [SerializeField]
    private DialoguePopup DialoguePopup;

    [SerializeField]
    private bool isMemeBlue;

    // Start is called before the first frame update
    void Start()
    {
        memeImage = transform.GetComponent<Image>();
        RoutineSpeak();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RoutineSpeak()
    {
        if (isMemeBlue)
        {
            Speak("My lemonade company was the best \n in the city and still is !", 3.0f, 1.0f);
            StartCoroutine(ChangeSpriteMeme(5.0f, memeSemiEnervee));
            Speak("It's a joke, you stole my receipe !", 3.0f, 5.0f);
            StartCoroutine(ChangeSpriteMeme(14.0f, memeEnervee));
            Speak("AAHHH you make me mad !", 3.0f, 14.0f);
            Speak("SO ME TOO !", 3.0f, 18.0f);
            Speak("FINE !", 2.0f, 22.0f);
            StartCoroutine(StartShakeAndGame(10f, 0.05f));
        }
        else
        {
            Speak("So logical, you stole \n my beautiful lemons !", 3.0f, 3.0f);
            StartCoroutine(ChangeSpriteMeme(7.0f, memeSemiEnervee));
            Speak("What ? I told you I never did this !", 3.0f, 7.0f);
            Speak("And you know that all children love \n MY lemonade now because you're a liar !", 3.0f, 11.0f);
            StartCoroutine(ChangeSpriteMeme(16.0f, memeEnervee));
            Speak("WELL ! I'll order some cannons \n to destroy your house on Azamon !", 3.0f, 16.0f);
            Speak("FINE !", 2.0f, 20.0f);
            StartCoroutine(StartShakeAndGame(10f, 0.05f));

        }

    }

    private IEnumerator ChangeSpriteMeme(float delay, Sprite spriteMeme)
    {
        yield return new WaitForSeconds(delay);
        memeImage.sprite = spriteMeme;
    }
   
    private IEnumerator StartShakeAndGame(float amplitude, float periode)
    {
        yield return new WaitForSeconds(24.0f);
        transform.GetComponent<ScreenShaker>().ScreenShakeUI(amplitude, periode, 1f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("DirectivesMenu");
    }

    public void Speak(string message, float baseTime = 1.5f, float delay = 0f)
    {
        //DialoguePopup.SetText(message);
        if (delay == -1)
        {
            delay = baseTime;
        }
        StartCoroutine(DialoguePopup.PopupDeactivation(delay, baseTime, message));
    }
}
