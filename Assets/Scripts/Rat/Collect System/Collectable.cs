using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
    public Sprite itemBackground;
    public float interactionDistance = 3f;
    public bool Iscollected = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectItem();
        }
    }

    private void CollectItem()
    {
        PopupManager.instance.ShowPopup(itemDescription, itemBackground);
        InventoryItem newItem = new InventoryItem(itemName, itemDescription, itemImage);
        InventoryManager.instance.AddItem(newItem);

        CollectableOverlay overlay = FindObjectOfType<CollectableOverlay>();
        if (overlay != null)
        {
            int index = overlay.GetIndexOfItem(itemImage); // Find correct index
            overlay.MarkCollected(index); // Mark this as collected
        }

        //TB - TEST DELETE IF NEEDED
        Iscollected = true;
        GetComponent<CollectData>().Collected();

        Destroy(gameObject);
    }
}
