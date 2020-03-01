using Rewired;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Rewired.Player inputManager1;
    public Rewired.Player inputManager2;

    [SerializeField]
    Timer timer;

    [SerializeField]
    private Canvas Menu;

    [SerializeField]
    private TMP_Text Resume;

    [SerializeField]
    private TMP_Text Retry;

    [SerializeField]
    private TMP_Text Exit;
    
    
    private bool menuIsActive;


    private int selectedFontSize = 50;
    private int defaultFontSize = 36;

    private TMP_Text[] elements;
    private int selected;

    // Start is called before the first frame update
    void Start()
    {
        Menu.gameObject.SetActive(false);
        menuIsActive = false;
        inputManager1 = ReInput.players.GetPlayer(0);
        inputManager2 = ReInput.players.GetPlayer(1);
        elements = new TMP_Text[] { Resume, Retry, Exit};
        selected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!menuIsActive &&  inputManager1.GetButtonDown("Pause"))
        {
            menuIsActive = true;
            timer.PauseTimer();
            
            Menu.gameObject.SetActive(true);

            selected = 0;
            UpdateSelection();
        }
        else if (menuIsActive)
        {

            if (inputManager1.GetButtonDown("Pause"))
            {
                ResumePlaying();
            }
            else if (inputManager1.GetButtonDown("Up"))
            {
                selected = (--selected) % elements.Length;
                if (selected < 0)
                {
                    selected += elements.Length;
                }
                UpdateSelection();
            }
            else if (inputManager1.GetButtonDown("Down"))
            {
                selected = (++selected) % elements.Length;
                UpdateSelection();
            }
            else if (inputManager1.GetButtonDown("Validate"))
            {
                if( selected == 0)
                {
                    ResumePlaying();
                }
                else if( selected == 1)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
                } else if( selected == 2)
                {
                    Application.Quit();
                }
            }

        }
        }
    private void UpdateSelection()
    {
        foreach (TMP_Text element in elements)
        {
            if (element == elements[selected])
            {
                element.fontSize = selectedFontSize;
            }
            else
            {
                element.fontSize = defaultFontSize;
            }
        }
    }
    private void ResumePlaying()
    {
        menuIsActive = false;
        timer.UnpauseTimer();
        Menu.gameObject.SetActive(false);
    }
}
