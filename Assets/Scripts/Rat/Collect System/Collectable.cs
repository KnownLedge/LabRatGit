using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
    public Sprite itemBackground;
    public float interactionDistance = 3f;
    public bool Iscollected;
    private GameObject player;
    private ArenaTestEnter arenaTestEnterScript;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        GameObject arenaObject = GameObject.Find("ArenaTestEnter");
        if (arenaObject != null)
        {
            arenaTestEnterScript = arenaObject.GetComponent<ArenaTestEnter>();
        }
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                CollectItem();
            }
        }
    }

    private void CollectItem()
    {
       
            PopupManager.instance.ShowPopup(itemDescription, itemBackground);
            InventoryItem newItem = new InventoryItem(itemName, itemDescription, itemImage);
            InventoryManager.instance.AddItem(newItem);

            if (arenaTestEnterScript != null && !string.IsNullOrEmpty(itemName) && itemName == arenaTestEnterScript.requiredCollectableName)
            {
                arenaTestEnterScript.OnCollectableCollected();
            }

            Destroy(gameObject); 
        
    }


    public bool GetState()
    {
        return Iscollected;
    }

}
