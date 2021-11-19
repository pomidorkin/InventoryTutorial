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
    public void AddItem(Item _item, int _amoumt)
    {
        // This line of code makes items with buffs non-stackable.
        if (_item.buffs.Length > 0)
        {
            SetEmptySlot(_item, _amoumt);
            return;
        }

        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID == _item.id)
            {
                Container.Items[i].AddAmount(_amoumt);
                return; // We stop looping through the inventory
            }
        }
        // If we don't have such an item yet, we run this code
        SetEmptySlot(_item, _amoumt);

    }

    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID <= -1)
            {
                Container.Items[i].UpdateSlot(_item.id, _item, _amount);
                return Container.Items[i];
            }
        }
        // Here we need to implement logic when the inventary is full
        return null;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amout);
        item2.UpdateSlot(item1.ID, item1.item, item1.amout);
        item1.UpdateSlot(temp.ID, temp.item, temp.amout);
    }

    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if(Container.Items[i].item == _item)
            {
                Container.Items[i].UpdateSlot(-1, null, 0);
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
                Container.Items[i].UpdateSlot(newContainer.Items[i].ID, newContainer.Items[i].item, newContainer.Items[i].amout);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new Inventory();
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

}

// We made this class serializable so that when we add this class to an object
// within Unity, it will actually sesrialize and show up in the editor~~~
[System.Serializable]
public class InventorySlot
{
    public int ID = -1;
    public Item item;
    public int amout;
    // Conctructor setts some values when an inventory slot is created
    public InventorySlot(int _id, Item _item, int _amout)
    {
        ID = _id;
        item = _item;
        amout = _amout;
    }
    public InventorySlot()
    {
        ID = -1;
        item = null;
        amout = 0;
    }

    public void UpdateSlot(int _id, Item _item, int _amout)
    {
        ID = _id;
        item = _item;
        amout = _amout;
    }

    public void AddAmount(int value)
    {
        amout += value;
    }
}