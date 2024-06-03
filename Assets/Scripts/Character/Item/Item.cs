using System;
using UnityEngine;

[Serializable]
public class Item
{
    [SerializeField]
    private string itemName;
    [SerializeField]
    private int price;

    public string ItemName => itemName;
    public int Price => price;
}
