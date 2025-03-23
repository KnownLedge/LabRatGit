using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCork : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    private AudioSource audioSource;
    [SerializeField] private Animator corkAnimator;
    [SerializeField] private PipeTransport pipeTransport;

    private void Start()
    {
        pipeTransport.triggerA.enabled = false;
        pipeTransport.triggerB.enabled = false;
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    //When ball enters the trigger, the cork animation will play
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (collectSound != null)
            {
                audioSource.clip = collectSound;
                audioSource.Play();
            }
            corkAnimator.SetTrigger("PlayAnimation"); 
            pipeTransport.triggerA.enabled = true;
            pipeTransport.triggerB.enabled = true;
        }
    }
}
