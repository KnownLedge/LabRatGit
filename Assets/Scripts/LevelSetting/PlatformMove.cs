using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public float platformSpeed = 5f;
    public float TurnTime; // time till platforms turn
    private float timer = 0f;
    private bool moveup;


    // Start is called before the first frame update
    void Start()
    {
        moveup = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveup == true)
            transform.Translate(transform.up * platformSpeed * Time.deltaTime);

        if (moveup == false)
            transform.Translate(-transform.up * platformSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= TurnTime)
        {
            
            if (moveup == true) { moveup = false; } else { moveup = true; }
            timer = 0;
        }

            
    }
}
