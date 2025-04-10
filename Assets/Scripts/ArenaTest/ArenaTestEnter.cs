using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaTestEnter : MonoBehaviour
{
    [SerializeField] private GameObject player;
    //[SerializeField] private GameObject net;
    //[SerializeField] private Animator netAnimator;
    [SerializeField] private float animationTime = 6f;
    [SerializeField] private FadeManager fadeManager;

    private bool isCollectableCollected = false;
    public string requiredCollectableName = "SpecialItem";

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.LoadPlayerState(player.transform);
        }
        else
        {
            Debug.LogError("❌ SaveManager is NULL!");
        }
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
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SavePlayerState(player.transform);
        }
        else
        {
            Debug.LogError("❌ SaveManager is NULL!");
        }

        player.GetComponent<Ratmovement>().enabled = false;
        player.gameObject.transform.position = new Vector3(-128.550003f, 78.1999969f, 15.96f);
        
        Debug.Log("Player position before arena: " + player.transform.position);
        
        Debug.Log("Player position before arena: " + player.transform.position);
        
        //netAnimator.SetTrigger("GoingDown");
        
        yield return new WaitForSeconds(3);
        //player.SetActive(false);

        fadeManager.FadeOutAndLoadScene("Arena_Test");
    }

    private IEnumerator EnableMovementAfterBlowOut()
    {
        player.GetComponent<Ratmovement>().enabled = false;
        // netAnimator.SetBool("isBlowing", true);
        Debug.Log("Blowing");

        yield return StartCoroutine(fadeManager.Fade(0));
        yield return new WaitForSeconds(animationTime);
        
        //netAnimator.SetBool("isBlowing", false);
        Debug.Log("Ready to play");
        player.GetComponent<Ratmovement>().enabled = true;
    }



    public void OnCollectableCollected()
    {
        isCollectableCollected = true;
        Debug.Log("Cheese Collected");
    }
}
