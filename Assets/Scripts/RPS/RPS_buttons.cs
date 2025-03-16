using UnityEngine;

public class RPS_buttons : MonoBehaviour
{
    [SerializeField]
    private float T2R;
    //when triggered the a function in RPS script called and the name of gameobject
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Player")
        {
            RPS._instance.DO(name);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if(collider.tag == "Player")
        {
            RPS._instance.Invoke("Reset", T2R);
        }
    }

}
