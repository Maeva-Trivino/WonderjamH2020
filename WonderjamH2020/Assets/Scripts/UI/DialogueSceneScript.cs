using System.Collections;
using System.Collections.Generic;
using Popup;
using Rewired;
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

    #region music
    [SerializeField]
    private AudioSource parolesGrognonAudio;

    [SerializeField]
    private AudioSource tremblementsAudio;
    #endregion

    //InputManager
    private Rewired.Player inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = ReInput.players.GetPlayer(0);
        memeImage = transform.GetComponent<Image>();
        RoutineSpeak();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.GetButtonDown("Interact"))
        {
            SceneManager.LoadScene("DirectivesMenu");
        }
    }

    private void RoutineSpeak()
    {
        if (isMemeBlue)
        {
            Speak("My lemonade company has always been \n the best !", 3.0f, 1.0f);
            StartCoroutine(ChangeSpriteMeme(5.0f, memeSemiEnervee));
            StartCoroutine(PlaySoundAngry(parolesGrognonAudio, 5.0f));
            Speak("Are you kidding ? You stole my recipe !", 3.0f, 5.0f);
            StartCoroutine(ChangeSpriteMeme(14.0f, memeEnervee));
            StartCoroutine(PlaySoundAngry(parolesGrognonAudio, 14.0f));
            Speak("AAHHH you're triggering me !\n I'll burn your lemon tree!", 3.0f, 14.0f);
            Speak("And you will meet mines!", 3.0f, 18.0f);
            Speak("FINE !", 2.0f, 22.0f);
            StartCoroutine(StartShakeAndGame(10f, 0.05f));
        }
        else
        {
            Speak("Of course, you stole \n my beautiful lemons !", 3.0f, 3.0f);
            StartCoroutine(ChangeSpriteMeme(7.0f, memeSemiEnervee));
            StartCoroutine(PlaySoundAngry(parolesGrognonAudio, 7.0f));
            Speak("What ? I would never use \n this cheap recipe of yours !", 3.0f, 7.0f);
            Speak("And you know that all children prefer \n MY lemonade !", 3.0f, 11.0f);
            StartCoroutine(ChangeSpriteMeme(16.0f, memeEnervee));
            StartCoroutine(PlaySoundAngry(parolesGrognonAudio, 16.0f));
            Speak("Is that so ?! Then you'll get \n to know my missiles launchers. :)", 3.0f, 16.0f);
            Speak("FINE !", 2.0f, 20.0f);
            StartCoroutine(StartShakeAndGame(10f, 0.05f));

        }

    }

    private IEnumerator ChangeSpriteMeme(float delay, Sprite spriteMeme)
    {
        yield return new WaitForSeconds(delay);
        memeImage.sprite = spriteMeme;
    }

    private IEnumerator PlaySoundAngry(AudioSource audioSourceToPlay, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSourceToPlay.Play();
    }
   
    private IEnumerator StartShakeAndGame(float amplitude, float periode)
    {
        yield return new WaitForSeconds(24.0f);
        if (isMemeBlue)
        {
            tremblementsAudio.Play();
        }
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
