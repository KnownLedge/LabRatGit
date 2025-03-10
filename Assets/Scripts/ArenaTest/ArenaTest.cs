using UnityEngine;

public class ArenaTest : MonoBehaviour
{
    [SerializeField] private Transform[] ratSpawnPositions;
    [SerializeField] private Transform[] cheeseSpawnPositions;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cheesePrefab;
    private GameObject currentCheese;
    private int spawnCount = 0;
    private int maxSpawns = 4;

    void Start()
    {
        SpawnRatAndCheese();
    }

    void Update()
    {
        OnCheeseCollected();
    }

    private void SpawnRatAndCheese()
    {
        if (spawnCount >= maxSpawns)
            return;

        int ratIndex = Random.Range(0, ratSpawnPositions.Length);
        int cheeseIndex = (ratIndex + 1) % cheeseSpawnPositions.Length;
        Debug.Log("Rat index: " + ratIndex);

        Ratmovement ratMove = player.gameObject.GetComponent<Ratmovement>();
        player.gameObject.transform.position = ratSpawnPositions[ratIndex].position;
        ratMove.backLeg.position = ratSpawnPositions[ratIndex].position;
        Debug.Log("Player position: " + player.transform.position);

        currentCheese = Instantiate(cheesePrefab, cheeseSpawnPositions[cheeseIndex].position, Quaternion.identity);
    }

    private void OnCheeseCollected()
    {
        if(currentCheese == null)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                if(spawnCount < maxSpawns)
                {
                    Debug.Log("Cheese collected");
                    spawnCount++;
                    SpawnRatAndCheese();
                }
                else
                {
                    
                }
            }

        }
        
    }
}
