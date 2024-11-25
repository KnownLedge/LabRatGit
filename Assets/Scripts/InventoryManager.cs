using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private List<InventoryItem> inventory = new List<InventoryItem>();
    private bool showInventory = false;
    private bool isPaused = false;
    private int currentIndex = 0; // notes the currently highlighted inv item
    private string selectedItemDescription = "";

    void Awake() // checks if the inventory is already open, prevents duplicate menus
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I)) // for closing and opening the inventory
        {
            showInventory = !showInventory;
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            if (!showInventory)
            {
                currentIndex = 0; // resets selected inventory item when the menu isnt enabled
                selectedItemDescription = ""; // wipes description 
            }
        }

        if (showInventory) // code for moving cursor up and down the inventory. Arrow keys could be swapped for W and S since the game is paused
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentIndex = (currentIndex > 0) ? currentIndex - 1 : inventory.Count - 1;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentIndex = (currentIndex < inventory.Count - 1) ? currentIndex + 1 : 0;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && inventory.Count > 0) // gets the description of the currently selected item
            {
                selectedItemDescription = inventory[currentIndex].description;
            }
        }
    }







    public void AddItem(InventoryItem item) // adds collected items to the inventory list and notes it in the console
    {
        inventory.Add(item);
        Debug.Log(item.name + " added to the inventory.");
    }





    void OnGUI()
    {
        GUIStyle customStyle = new GUIStyle(GUI.skin.label);
        customStyle.fontSize = 32; // changes the size of the text displayed, might need to be made bigger


        if (showInventory)  // code for drawing text on GUI, this will probably be heavily edited or changed completely at a later point so it's quite simple for now
        {
            if (inventory.Count > 0)
            {
                string inventoryDisplay = "Inventory:\n";
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (i == currentIndex)
                    {
                        inventoryDisplay += $"> {inventory[i].name}\n"; // highlight selected item
                    }
                    else
                    {
                        inventoryDisplay += $"- {inventory[i].name}\n";
                    }
                }
                GUI.Label(new Rect(10, 10, 200, 400), inventoryDisplay, customStyle);   // displays all inv items as a Gui label, not sure if this is the best way to do this but it works



                if (!string.IsNullOrEmpty(selectedItemDescription))  // displays the description of the selected item, but only if the player has actually collected something
                {
                    GUI.Label(new Rect(220, 10, 400, 400), $"Description:\n{selectedItemDescription}", customStyle);
                }
            }
            else // fallback in case the inventory is empty
            {
                GUI.Label(new Rect(10, 10, 200, 200), "Inventory is empty.", customStyle);
            }
        }
    }
}



    public class InventoryItem  // stores item name and description of a collectable
    {
        public string name;
        public string description;

        public InventoryItem(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }


