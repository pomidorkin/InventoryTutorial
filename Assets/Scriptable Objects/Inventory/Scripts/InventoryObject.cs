using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    // We need to story a type of an item and the amount of items that is held
    // in that slot of our inventory. So we created an Inventory slot class
    // that holds InventorySlots insted of holding items. The slots contain
    // the item an the amount of items we have in that slot~~~
    public List<InventorySlot> Container = new List<InventorySlot>();
    // This function adds items to our inventory
    public void AddItem(ItemObject _item, int _amoumt)
    {
        // First we need to find out if we have that item within our inventory
        bool hasItem = false;
        for(int i = 0; i > Container.Count; i++)
        {
            if(Container[i].item == _item)
            {
                Container[i].AddAmount(_amoumt);
                hasItem = true;
                break; // We stop looping through the inventory
            }
        }
        if (!hasItem)
        {
            Container.Add(new InventorySlot(_item, _amoumt));
        }
    }
}

// We made this class serializable so that when we add this class to an object
// within Unity, it will actually sesrialize and show up in the editor~~~
[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amout;
    // Conctructor setts some values when an inventory slot is created
    public InventorySlot(ItemObject _item, int _amout)
    {
        item = _item;
        amout = _amout;
    }

    public void AddAmount(int value)
    {
        amout += value;
    }
}