using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> items = new Dictionary<string, int>();

    [Header("UI")]
    public TMP_Text inventoryText;  // Assign in Inspector

    public void AddItem(string itemName, int amount = 1)
    {
        if (items.ContainsKey(itemName))
            items[itemName] += amount;
        else
            items[itemName] = amount;

        Debug.Log($"📦 Added {amount} {itemName}(s). Total: {items[itemName]}");
        UpdateUI();
    }

    public void RemoveItem(string itemName, int amount = 1)
    {
        if (!items.ContainsKey(itemName)) return;

        items[itemName] -= amount;
        if (items[itemName] <= 0)
            items.Remove(itemName);

        Debug.Log($"📦 Removed {amount} {itemName}(s). Total: {GetItemCount(itemName)}");
        UpdateUI();
    }

    public int GetItemCount(string itemName)
    {
        return items.ContainsKey(itemName) ? items[itemName] : 0;
    }

    public bool HasItem(string itemName, int amount = 1)
    {
        return GetItemCount(itemName) >= amount;
    }

    public void UseItem(string itemName, int amount = 1)
    {
        if (!HasItem(itemName, amount)) return;

        items[itemName] -= amount;
        if (items[itemName] <= 0)
            items.Remove(itemName);

        Debug.Log($"🗑️ Used {amount} {itemName}(s). Left: {GetItemCount(itemName)}");
        UpdateUI();
    }

    public Dictionary<string, int> GetAllItems()
    {
        return new Dictionary<string, int>(items);
    }

    private void UpdateUI()
    {
        if (inventoryText == null) return;

        if (items.Count == 0)
        {
            inventoryText.text = "Inventory: (empty)";
            return;
        }

        string content = "Inventory:\n";
        foreach (var pair in items)
        {
            content += $"• {pair.Key}: {pair.Value}\n";
        }

        inventoryText.text = content.TrimEnd();
    }
}
