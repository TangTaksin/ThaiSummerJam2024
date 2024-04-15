using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatChar : MonoBehaviour
{
    Animator _animator;
    Image _image;
    CharacterStat _characterStat;

    public void AssignCharacterStat(CharacterStat charstat)
    {
        _characterStat = charstat;
    }
}
