using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default,
    Equipment,
    Food
}

public enum Attribute
{
    Agility,
    Intellect,
    Stamina,
    Strength
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
    public ItemBuff[] buffs;

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    public string name;
    public int id;
    public ItemBuff[] buffs;
    public Item(ItemObject item)
    {
        name = item.name;
        id = item.Id;
        buffs = new ItemBuff[item.buffs.Length];
        for(int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.buffs[i].min, item.buffs[i].max)
            {
                attribute = item.buffs[i].attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff
{
    public Attribute attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
        GenerateValue();
    }
    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}
