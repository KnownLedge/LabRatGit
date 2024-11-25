using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
//using Cinemachine.Utility;

public class DollyFollower : MonoBehaviour
{
    public GameObject obj_path;
    public GameObject obj_doll;
    public Transform player;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
                var path = obj_path.GetComponent<CinemachineSmoothPath>();
        var doll = obj_doll.GetComponent<CinemachineDollyCart>();
      doll.m_Position = path.FindClosestPoint(player.position, 1,-1, 15);
    }
}
