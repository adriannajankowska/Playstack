using UnityEngine;
using System;

[Serializable]
public class Character
{
    [SerializeField]
    private string name;
    [SerializeField]
    private string surname;
    [SerializeField]
    private string sex;
    [SerializeField]
    private string image;
    [SerializeField]
    private Item[] itemsOwned;

    //simple getters
    public string Name => name;
    public string Surname => surname;
    public string Sex => sex;
    public string Image => image;
    public Item[] ItemsOwned => itemsOwned;
}

