using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictorySlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lvlTxt, nameTxt, expTxt, lvlUpNumTxt, expGainTxt;
    [SerializeField] Slider expSlide;
    [SerializeField] Image portrait;

    public void SetUpSlot(CharacterStat stat, int lvlUpAmount, int expGain)
    {
        lvlTxt.text = stat.level.ToString();
        nameTxt.text = stat.name.ToString();
        expTxt.text = string.Format("{0}/{1}", stat.level, stat._levelSys.GetNextExpNeeded());
        expSlide.value = stat.exp / stat._levelSys.GetNextExpNeeded();

        lvlUpNumTxt.text = string.Format("+{0} level(s).", lvlUpAmount);
        expGainTxt.text = string.Format("+{0} exp", expGain);
    }
}
