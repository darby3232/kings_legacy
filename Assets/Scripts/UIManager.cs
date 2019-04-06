using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Button expandButton;
    public Button recruitButton;
    public Button attackButton;
    public Button doNothingButton;

    public GameObject playerActionScreen;
    public GameObject playerExpandScreen;
    public GameObject playerRecruitScreen;
    public GameObject playerAttackScreen;

    public Dropdown expandDropdown;
    public Dropdown recruitDropdown;
    public Dropdown lordToAttackDropdown;

    TurnManager tm; 

    // Start is called before the first frame update
    void Start()
    {
        tm = TurnManager.instance;

        expandButton.onClick.AddListener(ExpandOnClick);
        recruitButton.onClick.AddListener(RecruitOnClick);
        attackButton.onClick.AddListener(AttackOnClick);
        doNothingButton.onClick.AddListener(DoNothingOnClick);

        expandDropdown.onValueChanged.AddListener(delegate
        {
            ExpandDropdownHandler(expandDropdown);
        });

        recruitDropdown.onValueChanged.AddListener(delegate
         {
             RecruitDropdownHandler(recruitDropdown);
         });

        lordToAttackDropdown.onValueChanged.AddListener(delegate
        {
            AttackDropdownHandler(lordToAttackDropdown);
        });

        //Set all screens to non-active
        playerActionScreen.SetActive(false);
        playerExpandScreen.SetActive(false);
        playerRecruitScreen.SetActive(false);

    }

    private void Update()
    {
        if(tm.currentGameState == TurnManager.GameState.PlayerChoosingAction && !(tm.currentLord is AILord))
        {
            playerActionScreen.SetActive(true);
        }
        else
        {
            playerActionScreen.SetActive(false);
        }        
    }

    void ExpandOnClick()
    {
        Debug.Log("Expanding");

        tm.currentGameState = TurnManager.GameState.PlayerSpecifyingAction;
        SetupExpandDropdown();
    }

    void SetupExpandDropdown()
    {
        expandDropdown.ClearOptions();
        List<string> expandAmounts = new List<string> { "Choose Amount" };
        int maxExpandAmount = Mathf.Min(tm.currentLord.GetArmies(), tm.currentLord.GetWealth());
        for(int i = 0; i <= maxExpandAmount; i++)
        {
            expandAmounts.Add(i.ToString());
        }
        expandDropdown.AddOptions(expandAmounts);
        //We need to remove listeners to change the value
        expandDropdown.onValueChanged.RemoveAllListeners();
        expandDropdown.value = 0;
        expandDropdown.onValueChanged.AddListener(delegate
        {
            ExpandDropdownHandler(expandDropdown);
        });
        expandDropdown.RefreshShownValue();
        playerExpandScreen.SetActive(true);
    }

    private void ExpandDropdownHandler(Dropdown expand)
    {
        int amountExpanded = expand.value;
        tm.currentLord.Expand(amountExpanded);
        DropdownFix(expandDropdown);
        playerExpandScreen.SetActive(false);
        tm.currentGameState = TurnManager.GameState.EndOfTurn;
    }

    void RecruitOnClick()
    {
        Debug.Log("Recruiting");

        tm.currentGameState = TurnManager.GameState.PlayerSpecifyingAction;
        SetupRecruitDropdown();
    }

    void SetupRecruitDropdown()
    {
        recruitDropdown.ClearOptions();
        List<string> recruitAmounts = new List<string> { "Choose Amount" };
        for (int i = 0; i <= tm.currentLord.GetWealth(); i++)
        {
            recruitAmounts.Add(i.ToString());
        }
        recruitDropdown.AddOptions(recruitAmounts);
        //We need to remove listeners to change the value
        recruitDropdown.onValueChanged.RemoveAllListeners();
        recruitDropdown.value = 0;
        recruitDropdown.onValueChanged.AddListener(delegate
        {
            RecruitDropdownHandler(recruitDropdown);
        });
        recruitDropdown.RefreshShownValue();
        playerRecruitScreen.SetActive(true);
    }

    private void RecruitDropdownHandler(Dropdown recruit)
    {
        int amountRecruited = recruit.value;
        tm.currentLord.Recruit(amountRecruited);
        DropdownFix(recruitDropdown);
        playerRecruitScreen.SetActive(false);
        tm.currentGameState = TurnManager.GameState.EndOfTurn;
    }

    void AttackOnClick()
    {
        Debug.Log("Attacking");
        tm.currentGameState = TurnManager.GameState.PlayerSpecifyingAction;
        SetupAttackDropdown();
    }

    void SetupAttackDropdown()
    {
        lordToAttackDropdown.ClearOptions();
        List<string> lordNames = new List<string> { "Choose Defender" }; 
        foreach(AILord lord in tm.GetAILords())
        {
            if(lord.GetLandCount() > 0)
                lordNames.Add(lord.lordName);
        }
        foreach(Player lord in tm.GetPlayers())
        {
            if(lord.GetLandCount() > 0 && lord != tm.currentLord)
            {
                lordNames.Add(lord.lordName);
            }        
        }
        lordToAttackDropdown.AddOptions(lordNames);
        //We need to remove listeners to change the value
        lordToAttackDropdown.onValueChanged.RemoveAllListeners();
        lordToAttackDropdown.value = 0;
        lordToAttackDropdown.onValueChanged.AddListener(delegate
        {
            AttackDropdownHandler(lordToAttackDropdown);
        });
        lordToAttackDropdown.RefreshShownValue();
        playerAttackScreen.SetActive(true);
    }

    private void AttackDropdownHandler(Dropdown lordString)
    {
        Lord lordToAttack = null;
           
        foreach(Lord lord in tm.GetAILords())
        {
            if (lordString.captionText.text == lord.lordName)
                lordToAttack = lord; 
        }
        foreach (Lord lord in tm.GetPlayers())
        {
            if (lordString.captionText.text == lord.lordName)
                lordToAttack = lord;
        }

        if(lordToAttack == null)
        {
            Debug.Log("Lord Attacking Null");
        }else if(tm.currentLord == null)
        {
            Debug.Log("Player Attacking Null");
        }
        tm.CreateBattle(tm.currentLord, lordToAttack);
        DropdownFix(lordToAttackDropdown);
        playerAttackScreen.SetActive(false);
        tm.currentGameState = TurnManager.GameState.BattleOccuring;
    }

    void DoNothingOnClick()
    {
        Debug.Log("Doing Nothing");        

        tm.currentGameState = TurnManager.GameState.EndOfTurn;
    }

    void DropdownFix(Dropdown dropdown)
    {
        Destroy(dropdown.transform.Find("Dropdown List").gameObject);
    }
}
