using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnim = null;

    private bool doorOpen = false;

    [SerializeField] private string openDoorAnimName = "DoorOpen";
    [SerializeField] private string closeDoorAnimName = "DoorClose";

    [SerializeField] private int waitTimer = 1;
    [SerializeField] private bool pauseInteraction = false;

    private IEnumerator PauseDoorInteraction()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(waitTimer);
        pauseInteraction = false;
    }

    public void PlayAnimation()
    {
        if (!doorOpen && !pauseInteraction)
        {
            doorAnim.Play(openDoorAnimName, 0, 0.0f);
            doorOpen = true;
            StartCoroutine(PauseDoorInteraction());
        }
        else if (doorOpen && !pauseInteraction)
        {
            doorAnim.Play(closeDoorAnimName, 0, 0.0f);
            doorOpen = false;
            StartCoroutine(PauseDoorInteraction());
        }
    }
}
