using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterZone : MonoBehaviour
{
    [SerializeField] List<Party> PartyList;
    [SerializeField] BattleInfo InfoTranfers;

    Party ChooseRandomParty()
    {
            return PartyList[Random.Range(0, PartyList.Count - 1)];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var p = collision.GetComponent<Player>();

        if (PartyList.Count > 0 && p)
        {
            var e = ChooseRandomParty();
            var s = SceneManager.GetActiveScene().name;

            InfoTranfers.playerParty = p.GetParty();
            InfoTranfers.enemyParty = e;
            InfoTranfers.sceneName = s;
            InfoTranfers.position = p.transform.position;

            SceneManager.LoadSceneAsync("battle");

        }
    }
}
