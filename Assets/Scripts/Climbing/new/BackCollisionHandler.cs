using UnityEngine;

public class BackCollisionHandler : MonoBehaviour
{
    private Collider backCollider;

    void Start()
    {
        backCollider = GameObject.FindGameObjectWithTag("BackHalf").GetComponent<Collider>();
    }

    public void DisableBackCollisions()
    {
        if (backCollider != null)
        {
            backCollider.enabled = false;
        }
    }

    public void EnableBackCollisions()
    {
        if (backCollider != null)
        {
            backCollider.enabled = true;
        }
    }
}
