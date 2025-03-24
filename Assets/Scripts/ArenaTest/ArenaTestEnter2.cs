using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaTestEnter2 : MonoBehaviour
{
    [SerializeField] private GameObject player;
    //[SerializeField] private GameObject net;
    //[SerializeField] private Animator netAnimator;
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
        player.GetComponent<Ratmovement>().enabled = false;
        //netAnimator.SetBool("isBlowing", true);
        Debug.Log("Blowing");

        yield return StartCoroutine(fadeManager.Fade(0));
        yield return new WaitForSeconds(animationTime);
        
        //netAnimator.SetBool("isBlowing", false);
        isReadyToPlay = true;
        Debug.Log("Ready to play");
        player.GetComponent<Ratmovement>().enabled = true;
    }

    public IEnumerator LeaveArena()
    {
        player.GetComponent<Ratmovement>().enabled = false;
        //player.transform.position = new Vector3(-128.550003f,78.1999969f,15.96f);
        Debug.Log("Player position: " + player.transform.position);
        
        //netAnimator.SetBool("isSucking", true);
        
        yield return new WaitForSeconds(3);
        player.SetActive(false);

        fadeManager.FadeOutAndLoadScene("Lab1");
    }
}
