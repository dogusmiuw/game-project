using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public ArrayList items = new ArrayList();

    [SerializeField]
    TMP_Text woodCountText;
    [SerializeField]
    TMP_Text foodCountText;

    private void AddItem(Item newItem)
    {
        Item foundItem = FindItem(newItem.Name);

        if (foundItem == null)
        {
            items.Add(newItem);
            return;
        }

        foundItem.Amount += newItem.Amount;
    }

    public void GetWood()
    {
        AddItem(new Item("Wood", 5));
        woodCountText.text = $"Wood: {FindItem("Wood").Amount}";
    }

    public void GetFood()
    {
        AddItem(new Item("Food", 3));
        foodCountText.text = $"Food: {FindItem("Food").Amount}";
    }

    private Item FindItem(string key)
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