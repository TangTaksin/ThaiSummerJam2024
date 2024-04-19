using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] VictorySlot[] victorySlots;

    public void DistributeExp(Party playerParty, int storedExp)
    {
        var splitedExp = storedExp / playerParty.charactersInParty.Count;
        List<int> levelTimes = new List<int>();

        for (int i = 0; i < victorySlots.Length; i++)
        {
            if (i < playerParty.charactersInParty.Count)
            {
                levelTimes.Add(playerParty.charactersInParty[i]._levelSys.AddExp(splitedExp));

                playerParty.charactersInParty[i].UpdateLevelAfterBattle();

                victorySlots[i].gameObject.SetActive(true);
                victorySlots[i].SetUpSlot(
                    playerParty.charactersInParty[i],
                    levelTimes[i], 
                    splitedExp);
            }
            else
            {
                victorySlots[i].gameObject.SetActive(false);
            }
        }

        
        
    }
}
