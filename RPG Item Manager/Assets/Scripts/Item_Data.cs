using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item_Data
{
    //basic item Parameter
    public string ItemName;
    public string DescriptionItem;

    int TypeItem;

    //ammo type item Parameter
    public int ammoCount = 0;

    //item with charges type
    public int MaxCount = 0;

    //basic constructor
    public Item_Data(string n, string desc)
    {
        TypeItem = 0;

        ItemName = n;
        DescriptionItem = desc;
    }
    //ammo constructor
    public Item_Data(string n, string desc, int amount)
    {
        TypeItem = 1;

        ItemName = n;
        DescriptionItem = desc;
        ammoCount = amount;
    }
    //item with charges constructor
    public Item_Data(string n, string desc, int amount, int maxamount)
    {
        TypeItem = 2;

        ItemName = n;
        DescriptionItem = desc;
        ammoCount = amount;
        MaxCount = maxamount;
    }
}
