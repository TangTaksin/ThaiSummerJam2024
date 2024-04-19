using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharStatusUI : MonoBehaviour
{
    public TextMeshProUGUI nameTxt, hpTxt, mpTxt, lvlTxt;
    public Slider hpSlider;
    public Image mpGuageImage, sprite;
    public GameObject WeaknessBracket;

    CharacterStat charStat;

    private void Update()
    {
        UpdateUI();
    }

    public void AssignStat(CharacterStat _charStat)
    {
        charStat = _charStat;
    }

    public void UpdateUI()
    {
        sprite.sprite = charStat.portraits;
        lvlTxt.text = string.Format("Lv.{0}", charStat.level);
        nameTxt.text = charStat.name;
        hpTxt.text = string.Format("{0}/{1}", charStat.curhealth,charStat.baseHealth);

        hpSlider.value = (float)charStat.curhealth / (float)charStat.baseHealth;
        if (mpGuageImage)
            mpGuageImage.fillAmount = (float)charStat.curMeltingPoint / (float)charStat.baseMeltingPoint;
    }
}
