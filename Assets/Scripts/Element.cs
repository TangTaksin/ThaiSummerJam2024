using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum element
{
    fire = 0,
    water = 1,
    wood = 2,
    ice = 3,
    none = 4
}

public class Element
{
    //atk\tar| Fire | Water | Wood | Ice | None
    // Fire  |  -1  |  0.5  |  2   |  2  |  1
    // Water |   2  |  -1   |  0.5 |  1  |  1
    // Wood  |  0.5 |   2   |  -1  | 0.5 |  1
    // Ice   |  0.5 |   1   |   2  |  -1 |  1
    // None  |  1   |   1   |   1  |  1  |  1

    static float[,] affinityTable = new float[,] // [Attacker, Receiver]
    { {  -1,  .5f,    2,    2,    1}, // fire  -> fi, wa, wo, ic, no 
      {   2,   -1,  .5f,    1,    1}, // water -> fi, wa, wo, ic, no 
      { .5f,    2,   -1,  .5f,    1}, // wood  -> fi, wa, wo, ic, no 
      { .5f,    1,    2,   -1,    1}, // ice   -> fi, wa, wo, ic, no 
      {   1,    1,    1,    1,    1}  // none  -> fi, wa, wo, ic, no 
    };

    static float[,] meltedTable = new float[,] // [Attacker, Receiver]
    { {   2,    2,    2,    2,    1}, // fire  -> fi, wa, wo, ic, no 
      {   2,    2,    2,    2,    1}, // water -> fi, wa, wo, ic, no 
      {   2,    2,    2,    2,    1}, // wood  -> fi, wa, wo, ic, no 
      {   2,    1,    2,    2,    1}, // ice   -> fi, wa, wo, ic, no 
      {   1,    1,    1,    1,    1}  // none  -> fi, wa, wo, ic, no 
    };

    public static float CheckAffinity(int attacker, int receiver, bool isMelt)
    {
        var v = (!isMelt) ? affinityTable[attacker, receiver] : meltedTable[attacker, receiver];

        return v;
    }
}
