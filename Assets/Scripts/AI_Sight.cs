using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AI_Sight : MonoBehaviour
{
    public GameObject player;
    [Range(1f, 5f)]
    public float radius;
    [Range(1f, 10f)]
    public float rayDistance;
    [Range(1f, 15f)]
    public float ChaseRange;
    RaycastHit hit;
    List<Vector3> rays;
    public bool playerSpotted;
    public static AI_Sight instance;
    
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (playerSpotted == true)
        {
            if((player.transform.position - transform.position).magnitude >= ChaseRange)
            {
                playerSpotted = false;
            }
        }
    }
    void FixedUpdate()
    {
        Vector3 dir = transform.forward * rayDistance;
        Vector3 centerposition = new Vector3(0, 0, 0);
        //the amount of rays is determined by the radius of the circle
        for (int i = 0; i <= radius; i++)
        {
            for (int j = 0; j <= radius; j++)
            {
                float x = i;
                float y = j;
                float z = i;


                Vector3 currentVector = new Vector3(x, y );
                float Theta = transform.localRotation.eulerAngles.y;

                Vector3 Rota = new Vector3(Mathf.Sin(Theta * Mathf.Deg2Rad), 0, Mathf.Cos(Theta * Mathf.Deg2Rad));

                float dist = (centerposition - currentVector).magnitude;
                Gizmos.color = Color.green;

                if (dist <= radius)
                {
                    rays = new List<Vector3>()
                    {
                      new Vector3(x * Rota.z , y ,-z * Rota.x),
                      new Vector3(-x * Rota.z , y ,z * Rota.x),
                      new Vector3(x * Rota.z , -y ,-z * Rota.x),
                      new Vector3(-x * Rota.z , -y ,z * Rota.x)
                    };

                }
                if (playerSpotted == false)
                {

                    for (int k = 0; k < rays.Count; k++)
                    {
                        if (Physics.Raycast(transform.position, (centerposition + dir) - rays[k], out hit, rayDistance))
                        {
                            if (hit.collider.gameObject == player)
                            {
                                playerSpotted = true;
                            }

                        }
                    }
                
                }
            }
            
        }
    }

  



    void OnDrawGizmos()
    {

        Vector3 dir = transform.forward * rayDistance;
        Vector3 centerposition = new Vector3(0,0,0);
        
        //the amount of rays is determined by the radius of the circle
        for (int i = 0; i <= radius; i++)
        {
            for (int j = 0; j <= radius; j++ ) 
            {
                float x = i;
                float y = j;
                float z = i;


                Vector3 currentVector = new Vector3 (x, y);
                float Theta = transform.localRotation.eulerAngles.y;

                Vector3 Rota = new Vector3( Mathf.Sin(Theta * Mathf.Deg2Rad), 0, Mathf.Cos(Theta * Mathf.Deg2Rad));

                
                float dist = (centerposition - currentVector).magnitude;

                if (dist <= radius )
                {
                    rays = new List<Vector3>() {

                      new Vector3(x * Rota.z , y ,-z * Rota.x),
                      new Vector3(-x * Rota.z , y ,z * Rota.x),
                      new Vector3(x * Rota.z , -y ,-z * Rota.x),
                      new Vector3(-x * Rota.z , -y ,z * Rota.x),
                    };
                    if (playerSpotted == false)
                    {
                        Gizmos.color = Color.green;
                        for (int k = 0; k < rays.Count; k++)
                        {
                            Gizmos.DrawRay(transform.position, (centerposition + dir) - rays[k]);
                        }
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawRay(transform.position, player.transform.position - transform.position);
                    }
                }
                
            }

            

        }


        Handles.DrawWireDisc(dir + transform.position, transform.forward, radius);

     

    }

  
  
    
}
