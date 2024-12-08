using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    // List to hold items in the inventory
    public List<Item> items = new List<Item>();

    // Current amount of wood and food
    [SerializeField] TMP_Text woodCountText;
    [SerializeField] TMP_Text foodCountText;

    private void Awake()
    {
        // Ensure there is only one instance of Inventory
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Method to add items to the inventory
    public void AddItem(Item newItem)
    {
        // Check if the item already exists in the inventory
        Item foundItem = FindItem(newItem.Name);

        if (foundItem == null)
        {
            // If item doesn't exist, add it to the inventory
            items.Add(newItem);
        }
        else
        {
            // If item exists, just update the amount
            foundItem.Amount += newItem.Amount;
        }

        // Update UI after adding the item
        UpdateUI();
    }

    // Method to add wood to the inventory
    public void GetWood()
    {
        AddItem(new Item("Wood", 1));  // Add 1 wood to the inventory
    }

    // Method to add food to the inventory
    public void GetFood()
    {
        AddItem(new Item("Food", 3));  // Add 3 food to the inventory
    }

    // Helper method to find an item by its name
    public Item FindItem(string key)
    {
        foreach (Item item in items)
        {
            if (item.Name == key)
            {
                return item;
            }
        }
        return null;
    }

    // Method to update the UI text for wood and food counts
     public void UpdateUI()
    {
        // Update the wood count UI
        Item woodItem = FindItem("Wood");
        if (woodItem != null)
        {
            woodCountText.text = $"Wood: {woodItem.Amount}";
        }

        // Update the food count UI
        Item foodItem = FindItem("Food");
        if (foodItem != null)
        {
            foodCountText.text = $"Food: {foodItem.Amount}";
        }
    }
}

public class Item
{
    public string Name { get; set; }
    public int Amount { get; set; }
    public Sprite Icon { get; set; }

    public Item(string Name, int Amount = 0, Sprite Icon = null)
    {
        this.Name = Name;
        this.Amount = Amount;
        this.Icon = Icon;
    }
}
