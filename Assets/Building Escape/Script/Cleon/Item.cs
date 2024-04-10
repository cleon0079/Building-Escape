using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public enum Index
    {
        None,
        One,
        Two,
        KeyDoor
    }

    // Item's ID
    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] Texture2D icon;
    [SerializeField] GameObject mesh;
    [SerializeField] Index type;

    public string Name { get { return name; } set { name = value; } }
    public string Description { get { return description; } set { description = value; } }
    public Texture2D Icon { get { return icon; } set { icon = value; } }
    public GameObject Mesh { get { return mesh; } set { mesh = value; } }
    public Index Type { get { return type; } set { type = value; } }

    public Item() { }

    public Item(Item _copyItem)
    {
        Name = _copyItem.Name;
        Description = _copyItem.Description;
        Icon = _copyItem.Icon;
        Mesh = _copyItem.Mesh;
        Type = _copyItem.Type;
    }
}