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

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                InventoryItem newItem = new InventoryItem(itemName, itemDescription, itemImage);
                InventoryManager.instance.AddItem(newItem);
                Destroy(gameObject);
            }
        }
    }
}
