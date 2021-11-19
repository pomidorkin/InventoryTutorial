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
    public int Id;
    public Sprite uiDisplay;
    // Store item type
    public ItemType type;
    [TextArea(15,20)]
    public string description;
}

[System.Serializable]
public class Item
{
    public string name;
    public int id;
    public Item(ItemObject item)
    {
        name = item.name;
        id = item.Id;
    }
}
