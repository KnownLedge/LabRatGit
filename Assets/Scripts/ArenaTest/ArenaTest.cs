using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ArenaTest : MonoBehaviour
{
    [SerializeField] private Transform[] ratSpawnPositions;
    [SerializeField] private Transform[] collectibleSpawnPositions;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject collectiblePrefab;
    [SerializeField] private FadeManager fadeManager;
    [SerializeField] private Text countdownText;
    private GameObject currentCollectible;
    private int spawnCount = 0;
    private int maxSpawns = 4;

    void Start()
    {
        StartCoroutine(InitializeArena());
    }

    void Update()
    {
        OnCollectibleCollected();
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

        Vector3 spawnPos = ratSpawnPositions[ratIndex].position;
        player.transform.position = spawnPos; 
        player.transform.rotation = Quaternion.Euler(0f, 90f, 0f);

        Vector3 cheesePos = collectibleSpawnPositions[collectableIndex].position;
        currentCollectible = Instantiate(collectiblePrefab, cheesePos, Quaternion.identity);

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

    private void OnCollectibleCollected()
    {
        if (currentCollectible == null && Input.GetKeyDown(KeyCode.E))
        {
            spawnCount++;
            if (spawnCount >= maxSpawns)
            {
                Debug.Log("All cheese collected, leaving arena...");
                StartCoroutine(LeaveArena());
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

        SpawnRatAndCheese();
        yield return new WaitForSeconds(0.5f);
        EnablePlayerControl();
    }

    private IEnumerator LeaveArena()
    {
        player.GetComponent<Ratmovement>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;

        yield return new WaitForSeconds(0.5f);
        fadeManager.FadeOutAndLoadScene("Lab1(For Arena_Test, made by Illia)");

    }

    private IEnumerator ShowCountdown(float duration)
    {
        if (countdownText == null)
            yield break;

        countdownText.enabled = true;

        int seconds = Mathf.CeilToInt(duration);
        while (seconds > 0)
        {
            countdownText.text = $"Ready to play in: {seconds}";
            yield return new WaitForSeconds(1f);
            seconds--;
        }

        countdownText.enabled = false;
    }

}
