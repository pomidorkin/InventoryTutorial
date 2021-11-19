using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
// Unity does not serialize Dictionaries, so we need to use a callback function
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    // An array of all the items that exist within the game
    public ItemObject[] Items;
    // It's a trade-off
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    // We can put code to fire before and after Unity serializes an object
    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            // Setting an id during serialization
            Items[i].Id = i;
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {

        GetItem = new Dictionary<int, ItemObject>();

    }
}
