using System.Collections;
using UnityEngine;

public class RPS_buttons : MonoBehaviour
{
    [SerializeField]
    private float T2R;
    private bool jumped;
    private IEnumerator coroutine;
    //when triggered the a function in RPS script called and the name of gameobject
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Player" && jumped == false)
        {
            RPS._instance.DO(name);
            jumped = true;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player" && jumped == true)
        {
            coroutine = Co(T2R);
            StartCoroutine(coroutine);
        }
    }

    IEnumerator Co(float time)
    {
        yield return new WaitForSeconds(time);
        jumped = false;
        RPS._instance.Reset();
    }
}
