using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
    public Sprite itemBackground;
    public float interactionDistance = 3f;
    private GameObject player;
    private ArenaTestEnter arenaTestEnterScript;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        arenaTestEnterScript = GameObject.Find("ArenaTestEnter").GetComponent<ArenaTestEnter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PopupManager.instance.ShowPopup(itemDescription, itemBackground);
            InventoryItem newItem = new InventoryItem(itemName, itemDescription, itemImage);
            InventoryManager.instance.AddItem(newItem);
            Destroy(gameObject);
            
            // if (itemName == arenaTestEnterScript.requiredCollectableName)
            // {
            //     arenaTestEnterScript.OnCollectableCollected();
            // }
        }
    }
}
