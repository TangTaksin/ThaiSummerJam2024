using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatChar : MonoBehaviour
{
    Camera cam;
    Animator _animator;

    SpriteRenderer _spriteRenderer;
    CharacterStat _characterStat;

    bool isDown, isMelted , isDelayed;

    [SerializeField] Button _button;

    public delegate void HpEvent(CombatChar sender, int value);
    public static HpEvent OnDown;

    private void OnEnable()
    {
        isDown = false;
        cam = Camera.main;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //update button pos
        if (_button.gameObject.activeSelf)
            _button.gameObject.transform.position = cam.WorldToScreenPoint(this.gameObject.transform.position);
    }


    public Button GetButton()
    {
        return _button;
    }

    public CharacterStat GetStat()
    {
        return _characterStat;
    }

    public bool GetMelted()
    {
        return isMelted;
    }


    public void MakeSelectable(bool _bool)
    {
        if (!isDown)
            _button.gameObject.SetActive(_bool);
    }


    public void PlayAnimation(string animName)
    {
        _animator?.Play(animName);
    }


    public void AssignCharacterStat(CharacterStat charstat)
    {
        _characterStat = charstat;
        _characterStat.setUpBattle();

        if (charstat.animationSet!=null)
        {
            _animator.runtimeAnimatorController = charstat.animationSet;
        }

        charstat.SetActor(this);

        _spriteRenderer.flipX = charstat.flipSprite;
    }

    public void SelectAsTarget()
    {
        Combat.OnCharTarget?.Invoke(this);
    }

    public void TakeDamage(int damage, int melt)
    {
        _characterStat.curhealth -= damage;
        _characterStat.curMeltingPoint += Mathf.Abs(melt);
        print("receive " + damage + " | remaining hp : " + _characterStat.curhealth);

        PlayAnimation("hurt_anim");

        CheckHealth();


    }

    void CheckHealth()
    {
        isDown = (_characterStat.curhealth <= 0);

        if (isDown)
        {
            _characterStat.curhealth = 0;

            Down();
        }

        if (_characterStat.curhealth > _characterStat.baseHealth)
        {
            _characterStat.curhealth = _characterStat.baseHealth;
        }


        isMelted = (_characterStat.curMeltingPoint >= _characterStat.baseMeltingPoint);

        if (isMelted)
        {
            _characterStat.curMeltingPoint = _characterStat.baseMeltingPoint;

            if (!isDelayed)
            {
                print("Heat Delay");
                _characterStat.SetOrder(14);
                isDelayed = true;
            }
        }
    }

    public void ResetMelted()
    {
        if ( isMelted )
        {
            _characterStat.curMeltingPoint = 0;
            isMelted = false;
            isDelayed = false;
        }
    }


    void Down()
    {
        OnDown?.Invoke(this, _characterStat.exp);
        PlayAnimation("down_anim");
    }

    //AI

    public (Skill, CombatChar) MakeDecision(List<CharacterStat> mySide, List<CharacterStat> theirSide)
    {
        Skill skill = null;
        CombatChar target = null;

        var et = (int)_characterStat.enemyTendancy;

        if (et == (int)CharacterStat.behaviourType.Allround)
        {
            et = Random.Range(0, 5);
        }

        if (et == (int)CharacterStat.behaviourType.Healer)
        {
            if (mySide.Count > 1)
            {
                et = (int)CharacterStat.behaviourType.RandomTarget;
            }
            else
            {
                //Find Lowest HP member
                mySide.Sort(ExtensionMethod.CompareHP);
                target = mySide[0].GetActor();

                //ChooseSkill
                Random.Range(0, _characterStat.initSkillList.Count);
            }
        }

        switch (et)
        {
            case (int)CharacterStat.behaviourType.RandomTarget:
                
                //ChooseTarget
                target = theirSide[Random.Range(0, theirSide.Count)].GetActor();
                //ChooseSkill
                skill = _characterStat.initSkillList[Random.Range(0, _characterStat.initSkillList.Count)];

                break;
            case (int)CharacterStat.behaviourType.TargetWeak:
                break;
            case (int)CharacterStat.behaviourType.TargetStrong:
                break;
        }

        return (skill, target);
    }
}
