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
        for(int i = 0; i < Container.Items.Count; i++)
        {
            if(Container.Items[i].item.id == _item.id)
            {
                Container.Items[i].AddAmount(_amoumt);
                return; // We stop looping through the inventory
            }
        }
        // If we don't have such an item yet, we run this code
        // Here when we add a new item to our inventory, we will use database's GetId
        // to pull the item id and populate it into our inventory slot.
            Container.Items.Add(new InventorySlot(_item.id, _item, _amoumt));
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
            Container = (Inventory)formatter.Deserialize(stream);
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
    public List<InventorySlot> Items = new List<InventorySlot>();

}

// We made this class serializable so that when we add this class to an object
// within Unity, it will actually sesrialize and show up in the editor~~~
[System.Serializable]
public class InventorySlot
{
    public int ID;
    public Item item;
    public int amout;
    // Conctructor setts some values when an inventory slot is created
    public InventorySlot(int _id, Item _item, int _amout)
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