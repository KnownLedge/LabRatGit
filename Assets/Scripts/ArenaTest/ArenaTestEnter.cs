using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaTestEnter : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float animationTime = 6f;
    [SerializeField] private FadeManager fadeManager;
    private bool isCollectableCollected = false;
    public string requiredCollectableName = "SpecialItem";

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        StartCoroutine(fadeManager.Fade(0));
    }

    void Update()
    {
        if (isCollectableCollected && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(EnterArena());
        }
    }

    private IEnumerator EnterArena()
    {

        // Ensure no other scripts are modifying position or rotation
        player.GetComponent<Rigidbody>().isKinematic = true;

        // Set position and rotation
        player.transform.localPosition = new Vector3(11.7725601f,0.657999992f,17.1599998f);
        player.transform.localRotation = Quaternion.Euler(0, 90, 0);
        
        Debug.Log("Player position before arena: " + player.transform.position);
        
        Debug.Log("Player position before arena: " + player.transform.position);
        
        //netAnimator.SetTrigger("GoingDown");
        
        yield return new WaitForSeconds(0.5f);
        //player.SetActive(false);

        fadeManager.FadeOutAndLoadScene("Arena_Test");
    }


    public void OnCollectableCollected()
    {
        isCollectableCollected = true;
        Debug.Log("Cheese Collected");
    }
}
