using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
//using Cinemachine.Utility;

public class CameraFollow : MonoBehaviour
{
    public GameObject obj_path;
    public GameObject obj_doll;
    public Transform player;
    public Transform realCamera;
    void Start()
    {

    }

    void Update()
    {
        realCamera.transform.position = obj_doll.transform.position;
        realCamera.LookAt(player.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var path = obj_path.GetComponent<CinemachineSmoothPath>();
        var doll = obj_doll.GetComponent<CinemachineDollyCart>();
        doll.m_Position = path.FindClosestPoint(player.position, 1, -1, 20);
       
    }
}
