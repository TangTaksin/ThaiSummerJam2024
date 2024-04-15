using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{
    public TextMeshProUGUI nameTxt, powerTxt, turnTxt;
    Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    public Button GetButton()
    {
        return _button;
    }
}
