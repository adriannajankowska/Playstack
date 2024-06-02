using UnityEngine;
using System;

[Serializable]
public class Solution
{
    [SerializeField]
    private string puzzleId;
    [SerializeField]
    private string name;
    [SerializeField]
    private string sex;



    //simple getters
    public string PuzzleId => puzzleId;
    public string Name => name;
    public string Sex => sex;
}

