using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{
    public TextMeshProUGUI nameTxt, powerTxt, turnTxt;
    Button _button;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
    }

    public void MakeSelectable(bool _bool)
    {
        _button.interactable = _bool;
    }

    public void SetSkillData(Skill skill)
    {
        nameTxt.text = skill.skillName;
        powerTxt.text = skill.Power.ToString();
        turnTxt.text = skill.turnCost.ToString();
    }
}
