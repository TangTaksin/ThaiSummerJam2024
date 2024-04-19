using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNum : MonoBehaviour
{
    TextMeshProUGUI _tmp;
    Animator _anim;

    private void Start()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
        _anim = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public void PopUpText(string txt, Color color,Vector2 pos)
    {
        _tmp.text = txt;
        _tmp.color = color;
        transform.position = pos;

        gameObject.SetActive(true);
        _anim.Play("damage_num");
    }

    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
