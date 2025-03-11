using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaTestEnter2 : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject pipe;
    [SerializeField] private Animator pipeAnimator;
    [SerializeField] private float animationTime = 6f;
    [SerializeField] private FadeManager fadeManager;
    public bool isReadyToPlay = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player"); 
        StartCoroutine(EnableMovementAfterBlowOut());
    }

    private IEnumerator EnableMovementAfterBlowOut()
    {
        pipeAnimator.SetBool("isBlowing", true);
        Debug.Log("Blowing");

        yield return StartCoroutine(fadeManager.Fade(0));
        yield return new WaitForSeconds(animationTime);
        
        pipeAnimator.SetBool("isBlowing", false);
        isReadyToPlay = true;
        Debug.Log("Ready to play");
        player.GetComponent<Ratmovement>().enabled = true;
    }
}
