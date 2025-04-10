using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTrigger : MonoBehaviour
{
    public AIRatMove targetRat;
    public List<Transform> targetPath;
    private bool isEnabled;
    void Start()
    {
        isEnabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && isEnabled)
        {
            targetRat.enabled = true;
            targetRat.wayPoints = targetPath;
            targetRat.restartPath();
        }
    }
}
