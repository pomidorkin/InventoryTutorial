using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default,
    Weapon,
    Helmet,
    Shield,
    Boots,
    Chest,
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
    public Sprite uiDisplay;
    public bool stackable;
    // Store item type
    public ItemType type;
    [TextArea(15,20)]
    public string description;
    public Item data = new Item();

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
    public int id = -1;
    public ItemBuff[] buffs;
    public Item()
    {
        name = "";
        id = -1;
    }
        public Item(ItemObject item)
    {
        name = item.name;
        id = item.data.id;
        buffs = new ItemBuff[item.data.buffs.Length];
        for(int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max)
            {
                attribute = item.data.buffs[i].attribute
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
