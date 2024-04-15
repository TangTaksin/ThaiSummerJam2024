using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public int Power;
    public int meltPower;
    public int turnCost;
}
