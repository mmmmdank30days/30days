using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // A dictionary for item name → quantity
    private Dictionary<string, int> items = new Dictionary<string, int>();

    public void AddItem(string itemName, int amount = 1)
    {
        if (items.ContainsKey(itemName))
            items[itemName] += amount;
        else
            items[itemName] = amount;

        Debug.Log($"📦 Added {amount} {itemName}(s). Total: {items[itemName]}");
    }

    public int GetItemCount(string itemName)
    {
        return items.ContainsKey(itemName) ? items[itemName] : 0;
    }

    public bool HasItem(string itemName)
    {
        return GetItemCount(itemName) > 0;
    }

    public void UseItem(string itemName, int amount = 1)
    {
        if (!HasItem(itemName)) return;

        items[itemName] -= amount;
        if (items[itemName] <= 0)
            items.Remove(itemName);

        Debug.Log($"🗑️ Used {amount} {itemName}(s). Left: {GetItemCount(itemName)}");
    }

    public Dictionary<string, int> GetAllItems()
    {
        return new Dictionary<string, int>(items);
    }
}
