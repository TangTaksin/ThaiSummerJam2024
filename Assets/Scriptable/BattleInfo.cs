using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/BattleInfo")]
public class BattleInfo : ScriptableObject
{
    public Party playerParty, enemyParty;
    public string sceneName;
    public Vector2 position;

    public void refreshInfo()
    {
        playerParty = null;
        enemyParty = null;
        sceneName = null;
        position = Vector2.zero;
    }
}
