using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> items = new Dictionary<string, int>();
    
    public void AddItem(string itemName, int amount = 1)
    {
        if (items.ContainsKey(itemName))
        {
            items[itemName] += amount;
        }
        else
        {
            items[itemName] = amount;
        }
        Debug.Log($"Added {itemName}. New total: {items[itemName]}");
    }
} 