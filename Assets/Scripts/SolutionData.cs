using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SolutionData", menuName = "ScriptableObjects/SolutionData", order = 1)]
public class SolutionData : ScriptableObject
{
    public string puzzleId;
    public string name;
    public string sex;
}
