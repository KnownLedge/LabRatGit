using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBlockade : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    private AudioSource audioSource;
    [SerializeField] private Animator blockadeAnimator;
    private int ballCount = 0;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    //When ball enters the trigger, the ball count will increase and when it reaches 3, 
    // the blockade animation will play
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            //Play sound when ball in hole
            if (collectSound != null)
            {
                audioSource.clip = collectSound;
                audioSource.Play();
            }
            
            ballCount++;
            Debug.Log("Ball Count: " + ballCount);

            if (ballCount == 3)
            {
                blockadeAnimator.SetTrigger("PlayAnimation"); 
            }
        }
    }
}
