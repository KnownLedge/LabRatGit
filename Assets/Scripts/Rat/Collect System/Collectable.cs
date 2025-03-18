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
    private bool Iscollected;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PopupManager.instance.ShowPopup(itemDescription, itemBackground);
            InventoryItem newItem = new InventoryItem(itemName, itemDescription, itemImage);
            InventoryManager.instance.AddItem(newItem);
            Destroy(gameObject);

            if (arenaTestEnterScript != null && itemName != null && itemName == arenaTestEnterScript.requiredCollectableName)
            {
                arenaTestEnterScript.OnCollectableCollected();
            }
        }
    }

    public bool GetState()
    {
        return Iscollected;
    }
}
