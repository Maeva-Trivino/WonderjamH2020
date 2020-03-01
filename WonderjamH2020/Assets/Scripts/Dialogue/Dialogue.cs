using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    [Header("Pour les dialogues de début 1 = Gauche et 2 = Droite")]
    [Header("Pour les dialogues de maison 1 = Owner et 2 = Enemy")]
    public string textSpeaker1;
    public string textSpeaker2;

    public float timeSpeaker1 = 1f;
    public float timeSpeaker2 = 1f;

    public BeginDialogueWith beginDialogueWith;
}
