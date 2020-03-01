using System.Collections;
using System.Collections.Generic;
using Popup;
using UnityEngine;

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

    [SerializeField]
    private DialoguePopup DialoguePopup;

    [SerializeField]
    private bool isMemeBlue;

    // Start is called before the first frame update
    void Start()
    {
        //DialoguePopup.Hide();
        //StartCoroutine(RoutineSpeak());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private IEnumerator RoutineSpeak()
    //{
    //    if (isMemeBlue)
    //    {
    //        //DialoguePopup.enabled = true;
    //        //Speak("Your dog has pee in my flower again !");
    //        //yield return new WaitForSeconds(1.0f);

    //    }
    //    else
    //    {
    //        //yield return new WaitForSeconds(2.0f);
            
    //        //Speak("He just make it more beautiful !");
    //        //DialoguePopup.enabled = true;
    //        //yield return new WaitForSeconds(1.0f);


    //    }
    //}

    public void Speak(string message, float delay = 0)
    {
        float baseTime = 1.5f;
        DialoguePopup.Display();
        DialoguePopup.SetText(message);
        if (delay == -1)
        {
            delay += baseTime;
        }
        StartCoroutine(DialoguePopup.PopupDeactivation(baseTime + delay));
    }
}
