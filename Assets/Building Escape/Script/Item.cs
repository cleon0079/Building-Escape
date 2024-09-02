using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public enum Index
    {
        LevelOne,
        LevelTwo,
        LevelThree
    }

    // Item's ID
    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] Sprite image;
    [SerializeField] Index type;

    public string Name { get { return name; } set { name = value; } }
    public string Description { get { return description; } set { description = value; } }
    public Sprite Image { get { return image; } set { image = value; } }
    public Index Type { get { return type; } set { type = value; } }

    public Item() { }

    public Item(Item _copyItem)
    {
        Name = _copyItem.Name;
        Description = _copyItem.Description;
        Image = _copyItem.Image;
        Type = _copyItem.Type;
    }
}