using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

// We will use the item id from the database to repopulate our inventory when we save/load.
// It will happen after we serialize an object
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    private ItemDatabaseObject database;
    // We need to story a type of an item and the amount of items that is held
    // in that slot of our inventory. So we created an Inventory slot class
    // that holds InventorySlots insted of holding items. The slots contain
    // the item an the amount of items we have in that slot~~~
    public List<InventorySlot> Container = new List<InventorySlot>();

    private void OnEnable()
    {
#if UNITY_EDITOR
        // If we are in the editor
        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabaseObject));
#else
        // If the built the game
        database = Resources.Load<ItemDatabaseObject>("Database");
        
#endif
    }
    // This function adds items to our inventory
    public void AddItem(ItemObject _item, int _amoumt)
    {
        for(int i = 0; i > Container.Count; i++)
        {
            if(Container[i].item == _item)
            {
                Container[i].AddAmount(_amoumt);
                return; // We stop looping through the inventory
            }
        }
        // If we don't have such an item yet, we run this code
        // Here when we add a new item to our inventory, we will use database's GetId
        // to pull the item id and populate it into our inventory slot.
            Container.Add(new InventorySlot(database.GetId[_item], _item, _amoumt));
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        // Application.persistentDataPath this function allows us to save file to a
        // persistent path across multiple devices
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    // As soon as anything changes on our scriptable object, that causes Unity to serialize
    // that object, we are gonna go and look through all of the items in our Container and
    // repopulate the item slot to make sure it's the same item that matches with the item id.
    public void OnAfterDeserialize()
    {
        for(int i = 0; i < Container.Count; i++)
        {
            Container[i].item = database.GetItem[Container[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {
    }
}

// We made this class serializable so that when we add this class to an object
// within Unity, it will actually sesrialize and show up in the editor~~~
[System.Serializable]
public class InventorySlot
{
    public int ID;
    public ItemObject item;
    public int amout;
    // Conctructor setts some values when an inventory slot is created
    public InventorySlot(int _id, ItemObject _item, int _amout)
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