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

    [SerializeField] Party playerSide, enemySide;

    PlayerInput _playerInput;
    InputAction _confirmAction, _backAction;

    GameObject lastSelectedButton;

    [Header("Character Sprites")]
    [SerializeField] List<SpriteRenderer> playerSprites;
    [SerializeField] List<SpriteRenderer> enemySprites;

    [Header("UI")]
    [SerializeField] GameObject _pointer;
    [SerializeField] GameObject playerChoicePanel, firstSelectMain, firstSelectSkill;
    [SerializeField] GameObject SkillPanel, skillAnContainer;
    [SerializeField] TextMeshProUGUI infoText;

    [Header("Info")]
    [SerializeField] CharStatusUI[] playerStatus;
    [SerializeField] CharStatusUI[] enemyStatus;
    [SerializeField] SkillUI[] skillSlot;
    [SerializeField] TextMeshProUGUI skillAnouncer;

    [Header("Buttons")]
    [SerializeField] Button[] MainOptions;

    public delegate void CombatEvent(Party player, Party enemy);
    public static CombatEvent OnCombat;

    private void Start()
    {
        cam = Camera.main;

        OnCombat += InitializeBattle;

        _playerInput = GetComponent<PlayerInput>();
        playerChoicePanel.SetActive(false);


        //Debug Start
        InitializeBattle(playerSide, enemySide);
    }
    
    private void OnDisable()
    {
        OnCombat -= InitializeBattle;
    }

    private void Update()
    {
        var cgo = EventSystem.current.currentSelectedGameObject;

        
        if (cgo && _pointer)
        {
            _pointer.SetActive(true);
            _pointer.transform.position = cgo.transform.position;
        }
        else
            _pointer.SetActive(false);
    }

    

    private void InitializeBattle(Party player, Party enemy)
    {
        //SceneManager.LoadSceneAsync("battle");

        //Get All Combatant
        foreach (var character in player.charactersInParty)
        {
            combatantList.Add(character);
        }

        foreach (var character in enemy.charactersInParty)
        {
            combatantList.Add(character);
        }

        //
        foreach (var character in combatantList)
        {
            character.SetOrder(character.agility);
        }

        //decide initial order
        combatantList.Sort(CompareOrder);

        currentCombatant = combatantList[0];
        SideCheck();
    }

    //the sorting will happend every time there's a change in order
    private static int CompareOrder(CharacterStat x, CharacterStat y)
    {
        if (x == null)
        {
            if (y == null)
            {
                // If x is null and y is null, they're
                // equal.
                return 1;
            }
            else
            {
                // If x is null and y is not null, y
                // is greater.
                return -1;
            }
        }
        else
        {
            // If x is not null...
            //
            if (y == null)
            // ...and y is null, x is greater.
            {
                return 1;
            }
            else
            {
                // ...and y is not null, compare the
                // lengths of the two strings.
                //
                int retval = x.GetOrder().CompareTo(y.GetOrder());

                if (retval != 0)
                {
                    // If the strings are not of equal length,
                    // the longer string is greater.
                    //
                    return retval;
                }
                else
                {
                    // If the strings are of equal length,
                    // x win
                    y.SetOrder(y.GetOrder()+1);
                    return 1;
                }
            }
        }
    }
    
    void SideCheck()
    {
        print(currentCombatant);

        if (enemySide.charactersInParty.Contains(currentCombatant))
        {
            currentState = state.enemyTurn;
        }

        if(playerSide.charactersInParty.Contains(currentCombatant))
        {
            
            currentState = state.playerTurn;

            ToMainActionMenu();
        }
    }

    #region player input

    enum playermenu { none, main, skill, fusion, targetSelect, confirmation}
    playermenu curMenu = playermenu.none;

    public void ToMainActionMenu()
    {
        curMenu = playermenu.main;
        EventSystem.current.SetSelectedGameObject(firstSelectMain);

        playerChoicePanel.SetActive(true);

        SkillPanel.SetActive(false);
        foreach (var b in MainOptions)
        {
            b.interactable = true;
        }
    }

    public void ToSkillMenu()
    {
        curMenu = playermenu.skill;

        EventSystem.current.SetSelectedGameObject(firstSelectSkill);
        SkillPanel.SetActive(true);
        foreach (var b in MainOptions)
        {
            b.interactable = false;
        }
    }

    Skill selectedSkill;

    public void ChooseSkill(int index)
    {
        selectedSkill = currentCombatant.initSkillList[index];
    }

    public void ChooseAllyTarget(int targetIndex)
    {

    }

    public void ChooseEnemyTarget(int targetIndex)
    {

    }

    #endregion

}
