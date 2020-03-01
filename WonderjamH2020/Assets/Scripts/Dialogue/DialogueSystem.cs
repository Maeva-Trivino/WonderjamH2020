using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private Player leftPlayer;
    [SerializeField] private Player rightPlayer;
    [SerializeField] private List<Dialogue> beginDialogues;
    [SerializeField] private List<Dialogue> destroyedDialogues;
    [SerializeField] private List<Dialogue> heavyDamageDialogues;
    [SerializeField] private List<Dialogue> lightDamageDialogues;
    /*
     * if(playerID == 0)
        {
            Speak("I'm the one selling Lemonade on Sunday !", 1f,0f) ;
        } else
        {
            Speak("You wish, old cow !", 1f, -1);
        }
     */

    private void Start()
    {
        DisplayBeginDialogue();
    }


    public void DisplayBeginDialogue()
    {
        DisplayDialogue(leftPlayer,rightPlayer,beginDialogues);
    }

    public void DisplayDialogue(Player speakerOne, Player speakerTwo, List<Dialogue> toUse)
    {
        if (toUse.Count > 0)
        {
            int dialogueIndex = Random.Range(0, toUse.Count - 1);

            Dialogue toPlay = toUse[dialogueIndex];

            switch (toPlay.beginDialogueWith)
            {
                case BeginDialogueWith.SpeakerOne:
                    speakerOne.Speak(toPlay.textSpeaker1, toPlay.timeSpeaker1, 0f);
                    speakerTwo.Speak(toPlay.textSpeaker2, toPlay.timeSpeaker2, toPlay.timeSpeaker1);
                    break;
                case BeginDialogueWith.SpeakerTwo:
                    speakerTwo.Speak(toPlay.textSpeaker2, toPlay.timeSpeaker2, 0);
                    speakerOne.Speak(toPlay.textSpeaker1, toPlay.timeSpeaker1, toPlay.timeSpeaker2);
                    break;
            }
        }
    }

    public void DisplayDamagedHouseDialogue(Player ownerPlayer, Player enemyPlayer,House.HouseState houseState)
    {
        List<Dialogue> toUse = null;
        switch (houseState)
        {
            case House.HouseState.Destroyed:
                toUse = destroyedDialogues;
                break;
            case House.HouseState.HeavilyDamaged:
                toUse = heavyDamageDialogues;
                break;
            case House.HouseState.LightlyDamaged:
                toUse = lightDamageDialogues;
                break;
        }

        DisplayDialogue(ownerPlayer,enemyPlayer,toUse);
    }


}
