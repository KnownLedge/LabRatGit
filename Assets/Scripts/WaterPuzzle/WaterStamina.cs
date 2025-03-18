using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStamina : MonoBehaviour
{
    public StaminaController staminaRef;
    public Ratmovement ratRef;
    public float drainSpeed = 0.5f;
    public Scenemanager sceneControl;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
 //   if (ratRef.isJump && ratRef.moveState) { // Really dodgy method of checking if the rats swimming
        staminaRef.playerStamina -= drainSpeed;

        if(staminaRef.playerStamina <= 0){
            Scenemanager.sceneCheckPoint = 0;
            sceneControl.SceneTransition();
        }
   // }    
    }
}
