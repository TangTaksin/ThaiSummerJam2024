using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeSlot : MonoBehaviour
{
    [SerializeField] Image characterImg;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Sprite defaultSlotSprite;

    public void SetSlot(Sprite img)
    {
        if (img == null)
            img = defaultSlotSprite;

        characterImg.sprite = img;
    }
}
