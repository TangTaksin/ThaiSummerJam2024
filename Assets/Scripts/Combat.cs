using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Combat : MonoBehaviour
{
    Camera cam;

    enum state { playerTurn, enemyTurn }
    state currentState;

    List<CharacterStat> combatantList = new List<CharacterStat>();
    CharacterStat currentCombatant;
    CombatChar curCombatChar;

    [SerializeField] Party playerSide, enemySide;

    PlayerInput _playerInput;
    InputAction _confirmAction, _backAction;

    GameObject lastSelectedButton;

    string lastSceneBeforeFight;

    int storedExp = 0;

    [Header("Character Sprites")]
    [SerializeField] List<CombatChar> playerSprites;
    [SerializeField] List<CombatChar> enemySprites;

    [Header("UI")]
    [SerializeField] GameObject _pointer;
    [SerializeField] GameObject playerChoicePanel, firstSelectMain, firstSelectSkill, firstSelectTarget;
    [SerializeField] GameObject SkillPanel, skillAnContainer;
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] VictoryScreen VictoryScreen;
    [SerializeField] GameObject DeafeatedScreen;

    [Header("Info")]
    [SerializeField] BattleInfo battleInfo;
    [SerializeField] CharStatusUI[] playerStatus;
    [SerializeField] CharStatusUI[] enemyStatus;
    [SerializeField] SkillUI[] skillSlot;
    [SerializeField] TextMeshProUGUI skillAnouncer;
    [SerializeField] DamageNum damageNum;
    [SerializeField] Gradient damageNumColorRange;
    [SerializeField] TimeLine timeline;


    [Header("Buttons")]
    [SerializeField] Button[] MainOptions;

    public delegate void CombatEvent(Party player, Party enemy);
    public static CombatEvent OnCombat;

    public delegate void TargetEvent(CombatChar targetChar);
    public static TargetEvent OnCharTarget;

    private void Start()
    {
        cam = Camera.main;

        OnCombat += InitializeBattle;
        OnCharTarget += PlayerSelectedTarget;
        CombatChar.OnDown += OnDown;

        _playerInput = GetComponent<PlayerInput>();
        playerChoicePanel?.SetActive(false);


        InitializeBattle(battleInfo.playerParty, battleInfo.enemyParty);
    }

    private void OnDisable()
    {
        OnCombat -= InitializeBattle;
        OnCharTarget -= PlayerSelectedTarget;
    }

    private void Update()
    {
        //print(EventSystem.current.currentSelectedGameObject);

        var cgo = EventSystem.current?.currentSelectedGameObject;

        if (_pointer)
        {
            if (cgo)
            {
                _pointer.SetActive(true);
                _pointer.transform.position = cgo.transform.position;
            }
            else
                _pointer.SetActive(false);
        }
    }

    void AssignChar()
    {
        //Assign UI

        //Player side
        for (int i = 0; i < playerStatus.Length; i++)
        {
            if (i + 1 <= playerSide.charactersInParty.Count)
            {
                playerStatus[i].AssignStat(playerSide.charactersInParty[i]);
                playerStatus[i].gameObject.SetActive(true);
            }
            else
                playerStatus[i].gameObject.SetActive(false);
        }

        //Enemy side
        for (int i = 0; i < enemyStatus.Length; i++)
        {
            if (i + 1 <= enemySide.charactersInParty.Count)
            {
                enemyStatus[i].AssignStat(enemySide.charactersInParty[i]);
                enemyStatus[i].gameObject.SetActive(true);
            }
            else
                enemyStatus[i].gameObject.SetActive(false);
        }

        //Assign Onscreen Char

        //Player side
        for (int i = 0; i < playerSprites.Count; i++)
        {
            if (i + 1 <= playerSide.charactersInParty.Count)
            {
                playerSprites[i].AssignCharacterStat(playerSide.charactersInParty[i]);
                playerSprites[i].gameObject.SetActive(true);
            }
            else
                playerSprites[i].gameObject.SetActive(false);

            playerSprites[i].MakeSelectable(playerSprites[i].gameObject.activeSelf);
        }

        //Enemy side
        for (int i = 0; i < enemySprites.Count; i++)
        {
            if (i + 1 <= enemySide.charactersInParty.Count)
            {
                enemySprites[i].AssignCharacterStat(enemySide.charactersInParty[i]);
                enemySprites[i].gameObject.SetActive(true);
            }
            else
                enemySprites[i].gameObject.SetActive(false);

            enemySprites[i].MakeSelectable(enemySprites[i].gameObject.activeSelf);
        }

    }

    private void InitializeBattle(Party player, Party enemy)
    {

        lastSceneBeforeFight = battleInfo.sceneName ; // scene

        storedExp = 0;

        //Get All Combatant
        foreach (var character in player.charactersInParty)
        {
            combatantList.Add(character);
        }

        foreach (var character in enemy.charactersInParty)
        {
            combatantList.Add(character);
        }

        //Assign UI and On-Screen Char
        AssignChar();

        //make char unselectable
        EnableTarget(false);

        //decide initial order
        foreach (var character in combatantList)
        {
            var x = 99 / (character.agility + Random.Range(0, character.luck));
            x = Mathf.CeilToInt(ExtensionMethod.Remap(x, 0, 99, 1, 14));
            character.SetOrder(x);
        }

        StartCoroutine(DelayTransition(0.5f));
    }

    void NextCharacter()
    {

        combatantList.Sort(ExtensionMethod.CompareOrder);

        currentCombatant = combatantList[0];

        var turnToSubtract = currentCombatant.GetOrder();

        foreach(var c in combatantList)
        {
            c.SetOrder(c.GetOrder() - (turnToSubtract - 1));
        }

        curCombatChar = currentCombatant.GetActor();

        curCombatChar.ResetMelted();

        print(currentCombatant);

        if (enemySide.charactersInParty.Contains(currentCombatant))
        {
            currentState = state.enemyTurn;

            EnemyChoice();
            playerChoicePanel.SetActive(false);
        }

        if (playerSide.charactersInParty.Contains(currentCombatant))
        {

            currentState = state.playerTurn;

            ToMainActionMenu();
        }

        timeline.GetAllCombatTant(combatantList);
    }

    #region player input

    enum playermenu { none, main, skill, fusion, targetSelect }
    playermenu curMenu = playermenu.none;

    public void OnBack()
    {
        switch (curMenu)
        {
            case playermenu.skill:
                ToMainActionMenu();
                break;
            case playermenu.targetSelect:
                ToSkillMenu();
                break;

        }
    }

    void ActiveMainButton(bool _bool)
    {
        foreach (var b in MainOptions)
        {
            b.interactable = _bool;
        }
    }

    public void ToMainActionMenu()
    {
        curMenu = playermenu.main;
        EventSystem.current.SetSelectedGameObject(firstSelectMain);

        currentCombatant.GetActor().PlayAnimation("idle_anim");

        playerChoicePanel.SetActive(true);

        SkillPanel.SetActive(false);
        ActiveMainButton(true);

        EnableTarget(false);
    }

    void ReadSkillList()
    {
        var skillnumb = currentCombatant.initSkillList.Count;

        //Activate skill slot according to the number of skills

        for (int i = 0; i < skillSlot.Length; i++)
        {
            if (i + 1 <= skillnumb)
            {
                skillSlot[i].SetSkillData(currentCombatant.initSkillList[i]);
                skillSlot[i].gameObject.SetActive(true);
            }
            else
                skillSlot[i].gameObject.SetActive(false);
        }
    }

    public void ToSkillMenu()
    {
        curMenu = playermenu.skill;

        currentCombatant.GetActor().PlayAnimation("idle_anim");

        //Read combatant skill list
        ReadSkillList();

        EventSystem.current.SetSelectedGameObject(firstSelectSkill);
        SkillPanel.SetActive(true);
        
        EnableSkillButton(true);

        ActiveMainButton(false);
        EnableTarget(false);

    }

    Skill selectedSkill;

    public void ChooseSkill(int index)
    {
        selectedSkill = currentCombatant.initSkillList[index];
        currentCombatant.GetActor().PlayAnimation("ready_anim");

        timeline.SetPreview(true ,1 + selectedSkill.turnCost);
        ToTargetSelection();
    }

    private void EnableTarget(bool _bool)
    {
        foreach (var c in playerSprites)
        {
            if (c.gameObject.activeSelf)
                c.MakeSelectable(_bool);
        }

        foreach (var c in enemySprites)
        {
            if (c.gameObject.activeSelf)
                c.MakeSelectable(_bool);
        }
    }

    private void EnableSkillButton(bool _bool)
    {
        foreach (var ss in skillSlot)
        {
            if (ss.gameObject.activeSelf)
                ss.MakeSelectable(_bool);
                
        }
    }

    void ToTargetSelection()
    {
        curMenu = playermenu.targetSelect;

        EnableSkillButton(false);


        EnableTarget(true);
        EventSystem.current.SetSelectedGameObject(enemySprites[0].GetButton().gameObject);
    }

    CombatChar selectedChar;

    void PlayerSelectedTarget(CombatChar target)
    {
        EnableTarget(false);
        timeline.SetPreview(false, 1);
        selectedChar = target;

        print(selectedChar);
        StartCoroutine(DelayBeforeAction(.5f));
    }

    #endregion


    #region enemy input

    void EnemyChoice()
    {
        //see all combatant
        var decided = curCombatChar.MakeDecision(enemySide.charactersInParty, playerSide.charactersInParty);
        selectedSkill = decided.Item1;
        selectedChar = decided.Item2;

        StartCoroutine(DelayBeforeAction(.5f));
    }

    #endregion


    IEnumerator DelayBeforeAction(float delay)
    {
        yield return new WaitForSeconds(delay);
        ExecuteSkill();
    }

    void ExecuteSkill()
    {
        print(selectedSkill + " on " + selectedChar);

        var isEnemy = enemySide.charactersInParty.Contains(selectedChar.GetStat());

        //Elemental calculation
        //[0]+[1]
        var eleModi = 0f;
        if (selectedChar.GetStat().elements.Length > 0)
        {
            for (int i = 0; i < selectedChar.GetStat().elements.Length; i++)
            {
                print("iteration : " + i);
                eleModi += Element.CheckAffinity((int)selectedSkill.element, (int)selectedChar.GetStat().elements[i], selectedChar.GetMelted());
            }
        }
        else
        {
            eleModi = 1f;
        }

        // get current combat char atk
        var atk = currentCombatant.attack;
        // get lvl
        var lvl = currentCombatant.level;
        // get skill power
        var sp = selectedSkill.Power;
        // get target defence
        var def = selectedChar.GetStat().defence;
        
        //deal damage
        float dmg = ((sp + atk) - def);
        print("before modi : " + dmg);
        dmg *= eleModi;
        print("After " + eleModi + " modi : " + dmg);

        var mp = selectedChar.GetStat().baseMeltingPoint;

        var htm = Mathf.CeilToInt(selectedChar.GetStat().baseMeltingPoint / 100);
        var melt = Mathf.CeilToInt(mp /htm * eleModi);

        currentCombatant.GetActor().PlayAnimation("cast_anim");

        //color
        var colorEva = ExtensionMethod.Remap(eleModi, -4, 4, 0, 1);
        var txtColor = damageNumColorRange.Evaluate(colorEva);

        damageNum.PopUpText(Mathf.Abs(dmg).ToString(), txtColor, cam.WorldToScreenPoint(selectedChar.transform.position));

        selectedChar.TakeDamage(Mathf.CeilToInt(dmg), melt * isEnemy.GetHashCode());

        //Apply delay
        var delay = selectedSkill.turnCost;
        currentCombatant.SetOrder(currentCombatant.GetOrder() + delay);

        timeline.GetAllCombatTant(combatantList);

        BattleEndCheck();

        StartCoroutine(DelayTransition(0.5f));
    }

    IEnumerator DelayTransition(float second)
    {
        yield return new WaitForSeconds(second);
        if (playerLose || enemyLose)
            EndBattle();
        else
            NextCharacter();
    }

    bool playerLose = false;
    bool enemyLose = false;

    void OnDown(CombatChar sender,int value)
    {
        var ss = sender.GetStat();

        if (enemySide.charactersInParty.Contains(sender.GetStat()))
        {
            storedExp += ss.exp;
        }

    }

    void BattleEndCheck()
    {
        foreach (var p in playerSide.charactersInParty)
        {
            if (p.curhealth <= 0)
            {
                playerLose = true;
            }
            else
            {
                playerLose = false;
                break;
            }
        }

        foreach (var e in enemySide.charactersInParty)
        {
            if (e.curhealth <= 0)
            {
                enemyLose = true;
            }
            else
            {
                enemyLose = false;
                break;
            }
        }
    }

    void EndBattle()
    {
        if (playerLose)
        {
            // display lose screen
            DeafeatedScreen.SetActive(true);

            return;
        }

        if (enemyLose)
        {
            // display win screen
            VictoryScreen.gameObject.SetActive(true);
            VictoryScreen.DistributeExp(playerSide, storedExp);

            return;
        }
    }

    public void ReturnToOverWorld()
    {
        SceneManager.LoadScene(lastSceneBeforeFight);
    }
}
