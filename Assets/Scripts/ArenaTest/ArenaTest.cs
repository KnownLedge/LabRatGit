using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ArenaTest : MonoBehaviour
{
    [SerializeField] private Transform[] ratSpawnPositions;
    [SerializeField] private Transform[] collectableSpawnPositions;
    [SerializeField] private Transform[] endFlagSpawnPositions;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject collectablePrefab;
    [SerializeField] private GameObject endFlagPrefab;
    [SerializeField] private FadeManager fadeManager;
    [SerializeField] private Text countdownText;
    [SerializeField] private StageEnd stageEnd;
    private GameObject currentCollectable;
    private bool endFlagSpawned = false;
    private int endFlagIndex = 0;
    private int spawnCount = 0;
    private int maxSpawns = 4;

    void Start()
    {
        StartCoroutine(InitializeArena());
    }

    void Update()
    {
        OnCollectableCollected();
    }

    private IEnumerator InitializeArena()
    {
        SpawnRatAndCheese();

        // Start fade and countdown at the same time
        Coroutine fade = StartCoroutine(fadeManager.Fade(0));
        yield return StartCoroutine(ShowCountdown(3)); // Countdown from 3

        yield return fade; // Wait for fade to complete if it’s longer than 3 seconds
        EnablePlayerControl();
    }


    private void SpawnRatAndCheese()
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Ratmovement>().enabled = false;

        int ratIndex = Random.Range(0, ratSpawnPositions.Length);
        int collectableIndex = ratIndex;
        endFlagIndex = ratIndex;

        Vector3 spawnPos = ratSpawnPositions[ratIndex].position;
        player.transform.position = spawnPos; 
        player.transform.rotation = Quaternion.Euler(0f, 90f, 0f);

        Vector3 cheesePos = collectableSpawnPositions[collectableIndex].position;
        currentCollectable = Instantiate(collectablePrefab, cheesePos, Quaternion.identity);

        Debug.Log($"[SPAWN] Selected Rat Index: {ratIndex}");
        Debug.Log($"[SPAWN] Selected Cheese Index: {collectableIndex}");
        Debug.Log($"[SPAWN] Rat Spawn Position: {spawnPos}");
        Debug.Log($"[SPAWN] Cheese Spawned at: {cheesePos}");
    }


    private void EnablePlayerControl()
    {
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<Ratmovement>().enabled = true;
    }

    private void OnCollectableCollected()
    {
        if (currentCollectable == null)
        {
            spawnCount++;
            if (spawnCount >= maxSpawns)
            {
                Debug.Log("All cheese collected, leaving arena...");
                LeaveArena();
            }
            else
            {
                Debug.Log("Cheese collected");
                StartCoroutine(RespawnSequence());
            }
        }
    }

    private IEnumerator RespawnSequence()
    {
        player.GetComponent<Ratmovement>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        
        fadeManager.fadeDuration = 0.5f;
        StartCoroutine(fadeManager.Fade(0));

        SpawnRatAndCheese();
        yield return new WaitForSeconds(0.5f);
        EnablePlayerControl();
    }

    private void LeaveArena()
    {
        fadeManager.fadeDuration = 3f;
        if(!endFlagSpawned)
        {
            Vector3 endFlasPos = endFlagSpawnPositions[endFlagIndex].position;
            GameObject endFlag = Instantiate(endFlagPrefab, endFlasPos, Quaternion.identity);
            endFlagSpawned = true;
            
            StageEnd stageEndComponent = endFlag.GetComponent<StageEnd>();
            if (stageEndComponent != null)
            {
                stageEndComponent.saveProgress = true;
                stageEndComponent.levelPosition = 5;
            }
            else
            {
                Debug.LogError("StageEnd component not found on endFlag GameObject.");
            }
        }
    }

    private IEnumerator ShowCountdown(float duration)
    {
        if (countdownText == null)
            yield break;

        countdownText.enabled = true;

        int seconds = Mathf.CeilToInt(duration);
        while (seconds > 0)
        {
            countdownText.text = $"{seconds}";
            yield return new WaitForSeconds(1f);
            seconds--;
        }

        countdownText.enabled = false;
    }

}
