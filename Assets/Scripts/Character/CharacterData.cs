using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    public string name;
    public string surname;
    public string sex;
    public string image;
    public ItemData[] itemsOwned;

    [TextArea]
    public string validationLog; // field to store validation logs
}
