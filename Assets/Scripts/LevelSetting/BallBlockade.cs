using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBlockade : MonoBehaviour
{
    [SerializeField] private Animator blockadeAnimator;
    private int ballCount = 0;

    //When ball enters the trigger, the ball count will increase and when it reaches 3, 
    // the blockade animation will play
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            ballCount++;
            Debug.Log("Ball Count: " + ballCount);
            if (ballCount == 3)
            {
                blockadeAnimator.SetTrigger("PlayAnimation"); 
            }
        }
    }
}
