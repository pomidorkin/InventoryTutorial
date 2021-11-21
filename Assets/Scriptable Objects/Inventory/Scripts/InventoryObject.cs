using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

// We will use the item id from the database to repopulate our inventory when we save/load.
// It will happen after we serialize an object
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;
    



    // This function adds items to our inventory
    public bool AddItem(Item _item, int _amoumt)
    {
        if (EmptySlotCount <= 0)
        {
            return false;
        }
        InventorySlot slot = FundItemOnInventory(_item);
        if (!database.GetItem[_item.id].stackable || slot == null)
        {
            SetEmptySlot(_item, _amoumt);
            return true;
        }
        slot.AddAmount(_amoumt);
        return true;
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < Container.Items.Length; i++)
            {
                if (Container.Items[i].item.id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public InventorySlot FundItemOnInventory(Item _item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item.id == _item.id)
            {
                return Container.Items[i];
            }
        }

        return null;
    }

    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item.id <= -1)
            {
                Container.Items[i].UpdateSlot(_item, _amount);
                return Container.Items[i];
            }
        }
        // Here we need to implement logic when the inventary is full
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amout);
            item2.UpdateSlot(item1.item, item1.amout);
            item1.UpdateSlot(temp.item, temp.amout);
        }
    }

    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if(Container.Items[i].item == _item)
            {
                Container.Items[i].UpdateSlot(null, 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        /*string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        // Application.persistentDataPath this function allows us to save file to a
        // persistent path across multiple devices
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();*/

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            /*BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();*/

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < Container.Items.Length; i++)
            {
                Container.Items[i].UpdateSlot(newContainer.Items[i].item, newContainer.Items[i].amout);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }

}

[System.Serializable]
public class Inventory
{
    // We need to store a type of an item and the amount of items that is held
    // in that slot of our inventory. So we created an Inventory slot class
    // that holds InventorySlots insted of holding items. The slots contain
    // the item an the amount of items we have in that slot~~~
    public InventorySlot[] Items = new InventorySlot[24]; // 24 is the initial size of our inventory

    public void Clear()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].RemoveItem();
        }
    }

}

// We made this class serializable so that when we add this class to an object
// within Unity, it will actually sesrialize and show up in the editor~~~
[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized]
    // "public UserInterface parent" links to the parent that this inventory slot
    // belongs to
    public UserInterface parent;
    public Item item;
    public int amout;

    public ItemObject ItemObject
    {
        get
        {
            if (item.id >= 0)
            {
                return parent.inventory.database.GetItem[item.id];
            }
            return null;
        }
    }

    // Conctructor setts some values when an inventory slot is created
    public InventorySlot(Item _item, int _amout)
    {
        item = _item;
        amout = _amout;
    }
    public InventorySlot()
    {
        item = new Item();
        amout = 0;
    }

    public void UpdateSlot(Item _item, int _amout)
    {
        item = _item;
        amout = _amout;
    }

    public void RemoveItem()
    {
        item = new Item();
        amout = 0;
    }

    public void AddAmount(int value)
    {
        amout += value;
    }

    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (AllowedItems.Length <= 0 || _itemObject == null || _itemObject.data.id < 0)
        {
            // if AllowedItems.Length is 0 or less, it means that any item can go in that
            // slot without ant restrictions
            return true;
        }

        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (_itemObject.type == AllowedItems[i])
            {
                return true;
            }
        }

        return false;
    }
}