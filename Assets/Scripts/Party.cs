using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Party")]
public class Party: ScriptableObject
{
    public List<CharacterStat> charactersInParty; 
}
