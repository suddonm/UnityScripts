using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryController: MonoBehaviour
{
    public List<IItem> Items;
    public void Start()
    {
        Items = new List<IItem>();
    }

    public void AddItem(IItem item)
    {
        // Add item to inventory
        Items.Add(item);
    }

    public void RemoveItem(IItem item)
    {
        Items.Remove(item);
    }
}