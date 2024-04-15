using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterZone : MonoBehaviour
{
    [SerializeField] List<Party> PartyList;

    Party ChooseRandomParty()
    {
            return PartyList[Random.Range(0, PartyList.Count - 1)];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PartyList.Count > 0)
        {
            var p = collision.GetComponent<Player>().GetParty();
            var e = ChooseRandomParty();

            Combat.OnCombat.Invoke(p,e);
        }
    }
}
