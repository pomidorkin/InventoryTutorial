using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
// Unity does not serialize Dictionaries, so we need to use a callback function
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    // An array of all the items that exist within the game
    public ItemObject[] Items;
    // Dictionaty is used to import an item and return it's id
    // We are putting an object into the dictionary and assigning it an id
    public Dictionary<ItemObject, int> GetId = new Dictionary<ItemObject, int>();
    // It's a trade-off
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    // We can put code to fire before and after Unity serializes an object
    public void OnAfterDeserialize()
    {
        // Clear our dictionary
        GetId = new Dictionary<ItemObject, int>();
        GetItem = new Dictionary<int, ItemObject>();
        for (int i = 0; i < Items.Length; i++)
        {
            // Adding an item to the dictionary and assigning an id, so each time Unity
            // serializes this object, it will populate the Dictionary with reference values
            // based of the Items array
            GetId.Add(Items[i], i);
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {

    }
}
