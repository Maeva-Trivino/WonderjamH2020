using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimonadeQueue : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> characterPrefabs;
    [SerializeField]
    private Transform popZone;
    [SerializeField]
    private bool isLeftSide;

    public bool IsEmpty => waitingPeople.Count == 0;
    private List<GameObject> waitingPeople = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        TryPopCharacter();
    }

    public void TryPopCharacter()
    {
        if (waitingPeople.Count < 6)
        {
            GameObject character = Instantiate(characterPrefabs[UnityEngine.Random.Range(0, characterPrefabs.Count)], transform);
            character.transform.position = popZone.position;
            character.GetComponentInChildren<Renderer>().sortingOrder = Mathf.RoundToInt(character.transform.position.y * 100f) * -1;
            //character.transform.localScale = Vector3.one;
            Animator animator = character.GetComponentInChildren<Animator>();
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsLookingLeft", !isLeftSide);
            waitingPeople.Add(character);

            Vector3 dest = popZone.localPosition * (waitingPeople.Count * 1f / 6);
            float dist = (dest - popZone.position).magnitude;

            LeanTween.moveLocal(character, dest, dist * .1f)
                .setOnComplete(() => { animator.SetBool("IsWalking", false); });
        }

        Invoke("TryPopCharacter", UnityEngine.Random.Range(1f, 3f));
    }

    public void ServeFirst()
    {
        if (waitingPeople.Count == 0)
            return;

        GameObject person = waitingPeople[0];
        waitingPeople.RemoveAt(0);
        Animator animator = person.GetComponentInChildren<Animator>();
        animator.SetBool("IsLookingLeft", isLeftSide);
        animator.SetBool("IsWalking", true);

        LeanTween.cancel(person);
        LeanTween.moveLocalY(person, -.75f, .5f).setEaseOutSine().setOnUpdate((float val) =>
            person.GetComponentInChildren<Renderer>().sortingOrder = Mathf.RoundToInt(person.transform.position.y * 100f) * -1);
        LeanTween.moveLocalX(person, popZone.localPosition.x, Mathf.Abs(popZone.localPosition.x) * .3f)
            .setOnComplete(() => Destroy(person));


        for (int i = 0; i < waitingPeople.Count; i++)
        {
            StartCoroutine(AdvanceToPlace(waitingPeople[i], i));
        }
    }

    private IEnumerator AdvanceToPlace(GameObject person, float placeIndex)
    {
        yield return new WaitForSeconds(.4f * placeIndex);


        LeanTween.cancel(person);

        Animator animator = person.GetComponentInChildren<Animator>();
        animator.SetBool("IsWalking", true);

        Vector3 dest = popZone.localPosition * (placeIndex / 6);
        float dist = (dest - popZone.position).magnitude;

        LeanTween.moveLocal(person, dest, .75f)
            .setOnComplete(() => { animator.SetBool("IsWalking", false); });
    }
}
