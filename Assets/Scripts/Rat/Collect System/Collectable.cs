using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
    public float interactionDistance = 3f;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryItem newItem = new InventoryItem(itemName, itemDescription, itemImage);
            InventoryManager.instance.AddItem(newItem);
            Destroy(gameObject); // Destroy the collectable item after adding it to the inventory
        }
    }
}
