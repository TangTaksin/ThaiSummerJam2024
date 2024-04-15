using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/CharacterData")]
public class CharacterStat : ScriptableObject
{
    public string charName;
    public int level;

    [Header("lvl 1 stats")]

    [Space]
    public int baseHealth;
    public int baseMeltingPoint;
    public int curhealth;
    public int curMeltingPoint;

    [Space]
    public int vitality;
    public int attack;
    public int defence;
    public int agility;
    public int luck;

    [Space]
    public List<Skill> initSkillList;

    [Space]
    int currentOrder;

    public void SetOrder(int order)
    {
        currentOrder = order;
    } 

    public int GetOrder()
    {
        return currentOrder;
    }

    public Skill AITakeAction()
    {
        var cskill = initSkillList[0];
        return cskill;
    }
}
