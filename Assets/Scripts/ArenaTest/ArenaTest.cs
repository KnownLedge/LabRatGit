using UnityEngine;

public class ArenaTest : MonoBehaviour
{
    [SerializeField]private ArenaTestEnter2 arenaTestEnter2Script;
    [SerializeField] private Transform[] ratSpawnPositions;
    [SerializeField] private Transform[] collectibleSpawnPositions;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject collectiblePrefab;
    private GameObject currentCollectible;
    private int spawnCount = 0;
    private int maxSpawns = 4;

    void Start()
    {
        SpawnRat();
        if(arenaTestEnter2Script.isReadyToPlay)
        {
            SpawnRatAndCheese();
        }
    }

    void Update()
    {
        if(arenaTestEnter2Script.isReadyToPlay)
        {
            SpawnRatAndCheese();
            arenaTestEnter2Script.isReadyToPlay = false;
        }
        OnCheeseCollected();
    }
    

    private void SpawnRatAndCheese()
    {
        if (spawnCount >= maxSpawns)
            return;

        int ratIndex = Random.Range(0, ratSpawnPositions.Length);
        int cheeseIndex = (ratIndex + 1) % collectibleSpawnPositions.Length;
        Debug.Log("Rat index: " + ratIndex);

        Ratmovement ratMove = player.gameObject.GetComponent<Ratmovement>();
        if(arenaTestEnter2Script.isReadyToPlay == false)
        {
            player.gameObject.transform.position = ratSpawnPositions[ratIndex].position;
        }

        ratMove.backLeg.position = ratSpawnPositions[ratIndex].position;
        Debug.Log("Player position: " + player.transform.position);

        currentCollectible = Instantiate(collectiblePrefab, collectibleSpawnPositions[cheeseIndex].position, Quaternion.identity);
    }

    private void SpawnRat()
    {
        int ratIndex = Random.Range(0, ratSpawnPositions.Length);
        Debug.Log("Rat index: " + ratIndex);

        Ratmovement ratMove = player.gameObject.GetComponent<Ratmovement>();
        if(arenaTestEnter2Script.isReadyToPlay == false)
        {
            player.gameObject.transform.position = ratSpawnPositions[ratIndex].position;
        }
        ratMove.backLeg.position = ratSpawnPositions[ratIndex].position;
        Debug.Log("Player position: " + player.transform.position);
    }

    private void OnCheeseCollected()
    {
        if (/*currentCheese == null &&*/ Input.GetKeyDown(KeyCode.E))
        {
            if (spawnCount + 1 >= maxSpawns) // Check BEFORE increasing spawnCount
            {
                Debug.Log("All cheese collected, leaving arena...");
                SpawnRat();
                StartCoroutine(arenaTestEnter2Script.LeaveArena()); // Start coroutine immediately
            }
            else
            {
                spawnCount++;
                Debug.Log("Cheese collected");
                SpawnRatAndCheese();
                arenaTestEnter2Script.isReadyToPlay = false;
            }
        }
    }

}
