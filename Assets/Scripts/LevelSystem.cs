using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem
{
    int level;
    int exp;
    AnimationCurve expCurve;
    int nextExpLvl;

    public void SetUpLevel(int _level, int _exp, AnimationCurve _curve)
    {
        level = _level;
        exp = _exp;
        expCurve = _curve;

        EvaluateExpCurve();
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetExp()
    {
        return exp;
    }

    public int GetNextExpNeeded()
    {
        return nextExpLvl;
    }


    void EvaluateExpCurve()
    {
        nextExpLvl = (int)expCurve.Evaluate(level);
    }

    public int AddExp(int toAdd)
    {
        var levelUpTimes = 0;
        exp += toAdd;

        Debug.Log(string.Format("Received {0} exp", toAdd));

        while (exp >= nextExpLvl)
        {
            
            Debug.Log(string.Format("Level Up! : {0} / {1} exp", exp, nextExpLvl));

            level++;
            levelUpTimes++;
            exp -= nextExpLvl;

            EvaluateExpCurve();
        }

        Debug.Log(string.Format("Level Up by {0}", levelUpTimes));

        return levelUpTimes;
    }
}
