using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaTestEnter : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject pipe;
    [SerializeField] private Animator pipeAnimator;
    [SerializeField] private float animationTime = 6f;
    [SerializeField] private FadeManager fadeManager;

    private bool isCollectableCollected = false;
    public string requiredCollectableName = "SpecialItem";

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (isCollectableCollected && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(EnterArena());
        }
    }

    private IEnumerator EnterArena()
    {
        player.GetComponent<Ratmovement>().enabled = false;
        player.transform.position = new Vector3(-128.550003f,78.1999969f,15.96f);
        Debug.Log("Player position: " + player.transform.position);
        
        pipeAnimator.SetBool("isSucking", true);
        
        yield return new WaitForSeconds(3);
        player.SetActive(false);

        fadeManager.FadeOutAndLoadScene("Arena_Test");
    }


    public void OnCollectableCollected()
    {
        isCollectableCollected = true;
        Debug.Log("Cheese Collected");
    }
}
