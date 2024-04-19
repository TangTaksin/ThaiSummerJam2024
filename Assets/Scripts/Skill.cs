using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public element element;
    public int Power;
    public int meltPower;
    public int turnCost;

    public Skill(string sName, element ele, int pow, int tc)
    {
        skillName = sName;
        element = ele;
        Power = pow;
        turnCost = tc;
    }
}

public class CombineSkill
{
    public static string[,] nameTable = new string[,]
    {

    };

    // Ele Combi
    // E+E| Fi | Wa | Wo | Ic | No
    // Fi | Fi | No | Fi | Wa | Fi
    // Wa | No | Wa | Wo | Wa | Wa
    // Wo | Fi | Wo | Wo | Ic | Wo
    // Ic | Wa | Wa | Ic | Ic | IC
    // No | Fi | Wa | Wo | Ic | No

    public static element[,] elementTable = new element[,]
    {
        { element.fire, element.none, element.fire, element.water, element.fire},
        { element.none, element.water, element.wood, element.water, element.water},
        { element.fire, element.wood, element.wood, element.ice, element.wood},
        { element.water, element.water, element.ice, element.ice, element.ice},
        { element.fire, element.water, element.wood, element.ice, element.none},
    };

    //Pow mul
    // E+E| Fi | Wa | Wo | Ic | No
    // Fi | 4  | 1  | 3  | 2  | 1
    // Wa | 1  | 4  | 3  | 4  | 1
    // Wo | 3  | 3  | 4  | 3  | 1
    // Ic | 2  | 4  | 3  | 4  | 1
    // No | 1  | 1  | 1  | 1  | 4

    static float[,] PowerTable = new float[,] // [Attacker, Receiver]
    { 
        {   4,    1,    3,    2,    1}, // fire  -> fi, wa, wo, ic, no 
        {   1,    4,    3,    4,    1}, // water -> fi, wa, wo, ic, no 
        {   3,    3,    4,    3,    1}, // wood  -> fi, wa, wo, ic, no 
        {   2,    4,    3,    4,    1}, // ice   -> fi, wa, wo, ic, no 
        {   1,    1,    1,    1,    4}  // none  -> fi, wa, wo, ic, no 
    };

    //Tc cut
    // E+E| Fi | Wa | Wo | Ic | No
    // Fi | 1  | 4  | 2  | 3  | 4
    // Wa | 4  | 1  | 2  | 1  | 4
    // Wo | 2  | 2  | 1  | 2  | 4
    // Ic | 3  | 1  | 2  | 1  | 4
    // No | 4  | 4  | 4  | 4  | 1

    static float[,] CostTable = new float[,] // [Attacker, Receiver]
    {
        {   1,    4,    2,    3,    4}, // fire  -> fi, wa, wo, ic, no 
        {   4,    1,    2,    1,    4}, // water -> fi, wa, wo, ic, no 
        {   2,    2,    1,    2,    4}, // wood  -> fi, wa, wo, ic, no 
        {   3,    1,    2,    1,    4}, // ice   -> fi, wa, wo, ic, no 
        {   4,    4,    4,    4,    1}  // none  -> fi, wa, wo, ic, no 
    };
}