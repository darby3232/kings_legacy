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

    public Dropdown expandDropdown;
    public Dropdown recruitDropdown;

   /* public Dropdown lordToAttackDropdown;
    public Dropdown armiesToSendDropdown;*/

    TurnManager tm; 

    // Start is called before the first frame update
    void Start()
    {
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

        tm = TurnManager.instance;

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
        List<string> expandAmounts = new List<string>();
        expandAmounts.Add("Choose Amount");
        int maxExpandAmount = Mathf.Min(tm.currentLord.GetArmies(), tm.currentLord.GetWealth());
        for(int i = 0; i <= maxExpandAmount; i++)
        {
            expandAmounts.Add(i.ToString());
        }
        expandDropdown.AddOptions(expandAmounts);
        
        playerExpandScreen.SetActive(true);
    }

    private void ExpandDropdownHandler(Dropdown expand)
    {
        int amountExpanded = expand.value;
        tm.currentLord.Expand(amountExpanded);
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
        List<string> recruitAmounts = new List<string>();
        recruitAmounts.Add("Choose Amount");
        Debug.Log("Current Wealth: " + tm.currentLord.GetWealth());
        for (int i = 0; i <= tm.currentLord.GetWealth(); i++)
        {
            recruitAmounts.Add(i.ToString());
        }
        recruitDropdown.AddOptions(recruitAmounts);

        playerRecruitScreen.SetActive(true);
    }

    private void RecruitDropdownHandler(Dropdown recruit)
    {
        int amountRecruited = recruit.value;
        tm.currentLord.Recruit(amountRecruited);
        playerRecruitScreen.SetActive(false);
        tm.currentGameState = TurnManager.GameState.EndOfTurn;
    }

    void AttackOnClick()
    {
        Debug.Log("Attacking - Not Yet Implemented");

        //Temporary
        tm.currentGameState = TurnManager.GameState.EndOfTurn;
    }

    void DoNothingOnClick()
    {
        Debug.Log("Doing Nothing");        

        tm.currentGameState = TurnManager.GameState.EndOfTurn;
    }

   

}
