using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default,
    Equipment,
    Food
}
// This class is abstract because it is the base class, other objects will extend this class
public abstract class ItemObject : ScriptableObject
{
    // The prefab is going to hold the displa for the item once we add it to the inventory
    public GameObject prefab;
    // Store item type
    public ItemType type;
    [TextArea(15,20)]
    public string description;
}
