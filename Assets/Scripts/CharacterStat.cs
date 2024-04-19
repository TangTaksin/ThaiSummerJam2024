using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/CharacterData")]
public class CharacterStat : ScriptableObject
{
    public string charName;
    public int level;
    public int exp;
    public int sdPerLevel = 5;
    public AnimationCurve expCurve;

    public element[] elements;

    public LevelSystem _levelSys;

    [Header("lvl 1 stats")]

    [Space]
    public int baseHealth;
    public int baseMeltingPoint;
    public int curhealth;
    public int curMeltingPoint;

    [Space]
    public int baseVitality;
    public int baseAttack;
    public int baseDefence;
    public int baseAgility;
    public int baseLuck;

    [Header("Added Stats")]
    [Space]
    public int addedVitality;
    public int addedAttack;
    public int addedDefence;
    public int addedAgility;
    public int addedLuck;

    [Header("Final Stats")]
    [Space]
    public int vitality;
    public int attack;
    public int defence;
    public int agility;
    public int luck;

    public enum behaviourType
    {
        RandomTarget = 0,
        TargetWeak = 1,
        TargetStrong = 2,
        Healer = 3,
        Allround = 4
    }

    [Space]
    public behaviourType enemyTendancy;

    [Space]
    public List<Skill> initSkillList;

    int currentOrder;

    [Space]
    public Sprite portraits;
    public RuntimeAnimatorController animationSet;
    public bool flipSprite;

    CombatChar assignActor;

    public void ResetStat()
    {
        level = 0;
        exp = 0;

        addedVitality = 0;
        addedAttack = 0;
        addedDefence = 0;
        addedAgility = 0;
        addedLuck = 0;

    }

    public void setUpBattle()
    {
        if (_levelSys == null)
        {
            _levelSys = new LevelSystem();
        }
        _levelSys.SetUpLevel(level, exp, expCurve);

        //set up stats

        luck = baseLuck + addedLuck;
        vitality = baseVitality + addedVitality;
        attack = baseAttack + addedAttack + (luck / 10);
        defence = baseDefence + addedDefence;
        agility = baseAgility + addedAgility + (luck / 10);

        baseHealth = vitality * 10;
        baseMeltingPoint = 100 + (defence * 10) + (luck / 10);

        curhealth = baseHealth;
        curMeltingPoint = 0;
    }


    public void UpdateLevelAfterBattle()
    {
        level = _levelSys.GetLevel();
        exp = _levelSys.GetExp();
    }


    public void SetOrder(int order)
    {
        if (order < 1)
            order = 1;
        currentOrder = order;
    } 

    public int GetOrder()
    {
        return currentOrder;
    }

    public void SetActor(CombatChar actor)
    {
        assignActor = actor;
    }

    public CombatChar GetActor()
    {
        return assignActor;
    }

}
