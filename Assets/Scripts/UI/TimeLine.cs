using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLine : MonoBehaviour
{
    [SerializeField] TimeSlot[] slots;
    [SerializeField] RectTransform orderPreview;

    public void GetAllCombatTant(List<CharacterStat> allCombatant)
    {
        foreach (var s in slots)
        {
            s.SetSlot(null);
        }

        foreach (var c in allCombatant)
        {
            var co = c.GetOrder();

            slots[co -1 ].SetSlot(c.portraits);
        }

        return;
    }

    public void SetPreview(bool active,int order)
    {
        orderPreview.gameObject.SetActive(active);
        orderPreview.position = slots[order - 1].transform.position;
    }
}
