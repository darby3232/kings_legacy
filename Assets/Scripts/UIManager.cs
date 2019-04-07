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

    public GameObject chooseToTradeScreen;
    public GameObject whoToTradeWithScreen;
    public GameObject chooseWhatToOfferScreen;
    public GameObject chooseHowMuchToOfferScreen;
    public GameObject chooseWhatToAskForScreen;
    public GameObject chooseWhatAmountToAskForScreen;
    public GameObject askIfRecipientAgreesScreen;

    public Button yesTradeButton;
    public Button noTradeButton;

    public Button acceptTradeButton;
    public Button refuseTradeButton;

    public Dropdown tradeWithWhoDropdown;
    public Dropdown tradeForWhatDropdown;
    public Dropdown tradeForWhatAmountDropdown;
    public Dropdown offerWhatResourceDropdown;
    public Dropdown offerWhatAmountDropdown;
    
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

        yesTradeButton.onClick.AddListener(BeginTradeOnClick);
        noTradeButton.onClick.AddListener(DoNothingOnClick);

        acceptTradeButton.onClick.AddListener(AcceptTradeOnClick);
        refuseTradeButton.onClick.AddListener(RefuseTradeOnClick); 

        //Set all screens to non-active
        playerActionScreen.SetActive(false);
        playerExpandScreen.SetActive(false);
        playerRecruitScreen.SetActive(false);
        chooseToTradeScreen.SetActive(false);
        whoToTradeWithScreen.SetActive(false);
        chooseWhatToOfferScreen.SetActive(false);
        chooseHowMuchToOfferScreen.SetActive(false);
        chooseWhatToAskForScreen.SetActive(false);
        chooseWhatAmountToAskForScreen.SetActive(false);
        askIfRecipientAgreesScreen.SetActive(false);

}

    private void Update()
    {
        if(tm.currentGameState == TurnManager.GameState.PlayerChoosingAction && !(tm.currentLord is AILord))
        {
            playerActionScreen.SetActive(true);
            chooseToTradeScreen.SetActive(false);
        }
        else if(tm.currentGameState == TurnManager.GameState.PlayerDecidingTrades && !(tm.currentLord is AILord))
        {
            chooseToTradeScreen.SetActive(true);
            playerActionScreen.SetActive(false);
        }
        else
        {
            playerActionScreen.SetActive(false);
            chooseToTradeScreen.SetActive(false);
        }
    }


    void RefuseTradeOnClick()
    {
        tm.currentGameState = TurnManager.GameState.EndOfTurn;
        askIfRecipientAgreesScreen.SetActive(false);
    }

    void AcceptTradeOnClick()
    {
        //do trade

        Player offeringPlayer = tm.currentTrade.offeringPlayer;
        Player recipientPlayer = tm.currentTrade.recipientPlayer;

        switch (tm.currentTrade.resourceOffered)
        {
            case Lord.ResourceTypes.Armies:
                offeringPlayer.SetArmies(offeringPlayer.GetArmies() - tm.currentTrade.amountResourceOffered);
                recipientPlayer.SetArmies(recipientPlayer.GetArmies() + tm.currentTrade.amountResourceOffered);
                break;
            case Lord.ResourceTypes.Land:
                offeringPlayer.SetLand(offeringPlayer.GetLandCount() - tm.currentTrade.amountResourceOffered);
                recipientPlayer.SetLand(recipientPlayer.GetLandCount() + tm.currentTrade.amountResourceOffered);
                break;
            case Lord.ResourceTypes.Wealth:
                offeringPlayer.SetWealth(offeringPlayer.GetWealth() - tm.currentTrade.amountResourceOffered);
                recipientPlayer.SetWealth(recipientPlayer.GetWealth() + tm.currentTrade.amountResourceOffered);
                break;
        }

        switch (tm.currentTrade.resourceToReceive)
        {
            case Lord.ResourceTypes.Armies:
                recipientPlayer.SetArmies(recipientPlayer.GetArmies() - tm.currentTrade.amountResourceReceived);
                offeringPlayer.SetArmies(offeringPlayer.GetArmies() + tm.currentTrade.amountResourceReceived);
                break;
            case Lord.ResourceTypes.Land:
                recipientPlayer.SetLand(recipientPlayer.GetLandCount() - tm.currentTrade.amountResourceReceived);
                offeringPlayer.SetLand(offeringPlayer.GetLandCount() + tm.currentTrade.amountResourceReceived);
                break;
            case Lord.ResourceTypes.Wealth:
                recipientPlayer.SetWealth(recipientPlayer.GetWealth() - tm.currentTrade.amountResourceReceived);
                offeringPlayer.SetWealth(offeringPlayer.GetWealth() + tm.currentTrade.amountResourceReceived);
                break;
        }

        tm.currentGameState = TurnManager.GameState.EndOfTurn;
        askIfRecipientAgreesScreen.SetActive(false);
    }

    void DoNotTradeOnClick()
    {
        Debug.Log("Don't Trade");
        chooseToTradeScreen.SetActive(false);
        tm.currentGameState = TurnManager.GameState.EndOfTurn;
    }

    void BeginTradeOnClick()
    {
        Debug.Log("Beginning Trade");

        tm.currentTrade.offeringPlayer = (Player)tm.currentLord;
        chooseToTradeScreen.SetActive(false);
        tm.currentGameState = TurnManager.GameState.PlayerSpecifyingTrade;
        SetupTradeWhoDropdown();
    }

    void SetupTradeWhoDropdown()
    {
        tradeWithWhoDropdown.ClearOptions();
        List<string> lordNames = new List<string> { "Choose Trade Partner" };
        foreach (Player lord in tm.GetPlayers())
        {
            if (lord != tm.currentLord)
            {
                lordNames.Add(lord.lordName);
            }
        }
        tradeWithWhoDropdown.AddOptions(lordNames);
        //We need to remove listeners to change the value
        tradeWithWhoDropdown.onValueChanged.RemoveAllListeners();
        tradeWithWhoDropdown.value = 0;
        tradeWithWhoDropdown.onValueChanged.AddListener(delegate
        {
            TradeWithWhoDropdownHandler(tradeWithWhoDropdown);
        });
        tradeWithWhoDropdown.RefreshShownValue();
        whoToTradeWithScreen.SetActive(true);
    }

    private void TradeWithWhoDropdownHandler(Dropdown tradeWithWho)
    {
        Player lordToTradeWith = null;

        foreach (Player lord in tm.GetPlayers())
        {
            if (tradeWithWho.captionText.text == lord.lordName)
                lordToTradeWith = lord;
        }

        if (lordToTradeWith == null)
        {
            Debug.Log("Lord Attacking Null");
        }
        else if (tm.currentLord == null)
        {
            Debug.Log("Player Attacking Null");
        }
        tm.currentTrade.recipientPlayer = lordToTradeWith;
        DropdownFix(tradeWithWho);
        whoToTradeWithScreen.SetActive(false);
        SetupTradeForWhatDropdown();
    }

    
    void SetupTradeForWhatDropdown()
    {
        tradeForWhatDropdown.ClearOptions();
        List<string> resourceNames = new List<string> { "Choose What you Want", "Land", "Wealth", "Armies" };
        tradeForWhatDropdown.AddOptions(resourceNames);

        tradeForWhatDropdown.onValueChanged.RemoveAllListeners();
        tradeForWhatDropdown.value = 0;
        tradeForWhatDropdown.onValueChanged.AddListener(delegate
        {
            TradeForWhatDropdownHandler(tradeForWhatDropdown);
        });
        tradeForWhatDropdown.RefreshShownValue();
        chooseWhatToAskForScreen.SetActive(true);
    }

    private void TradeForWhatDropdownHandler(Dropdown tradeForWhat)
    {
        Lord.ResourceTypes resource = (Lord.ResourceTypes) tradeForWhat.value - 1;
        tm.currentTrade.resourceToReceive = resource;
        DropdownFix(tradeForWhat);
        chooseWhatToAskForScreen.SetActive(false);
        SetupTradeForWhatAmountDropdown();
    }

    void SetupTradeForWhatAmountDropdown()
    {
        tradeForWhatAmountDropdown.ClearOptions();
        List<string> resourceAmounts = new List<string> { "How Much?" };

        int maxAmountPossibleToTrade = 0; 
        if(tm.currentTrade.resourceOffered == Lord.ResourceTypes.Armies)
        {
            maxAmountPossibleToTrade = tm.currentTrade.recipientPlayer.GetArmies();
        }else if(tm.currentTrade.resourceOffered == Lord.ResourceTypes.Land)
        {
            maxAmountPossibleToTrade = tm.currentTrade.recipientPlayer.GetLandCount();
        }else if(tm.currentTrade.resourceOffered == Lord.ResourceTypes.Wealth)
        {
            maxAmountPossibleToTrade = tm.currentTrade.recipientPlayer.GetWealth();
        }
        
        for(int i = 0; i <= maxAmountPossibleToTrade; i++)
        {
            resourceAmounts.Add(i.ToString());
        }

        tradeForWhatAmountDropdown.AddOptions(resourceAmounts);

        tradeForWhatAmountDropdown.onValueChanged.RemoveAllListeners();
        tradeForWhatAmountDropdown.value = 0;
        tradeForWhatAmountDropdown.onValueChanged.AddListener(delegate
        {
            AskForWhatAmountHandler(tradeForWhatAmountDropdown);
        });
        tradeForWhatAmountDropdown.RefreshShownValue();
        chooseWhatAmountToAskForScreen.SetActive(true);
    }

    void AskForWhatAmountHandler(Dropdown whatAmount)
    {
        int amountAskedFor = whatAmount.value - 1;
        tm.currentTrade.amountResourceReceived = amountAskedFor;

        DropdownFix(whatAmount);
        chooseWhatAmountToAskForScreen.SetActive(false);
        SetupWhatToOffer();
    }

    void SetupWhatToOffer()
    {
        offerWhatResourceDropdown.ClearOptions();
        List<string> resourceNames = new List<string> { "Choose What you Want", "Land", "Wealth", "Armies" };
        offerWhatResourceDropdown.AddOptions(resourceNames);

        offerWhatResourceDropdown.onValueChanged.RemoveAllListeners();
        offerWhatResourceDropdown.value = 0;
        offerWhatResourceDropdown.onValueChanged.AddListener(delegate
        {
            WhatToOfferDropdownHandler(offerWhatResourceDropdown);
        });
        offerWhatResourceDropdown.RefreshShownValue();
        chooseWhatToOfferScreen.SetActive(true);
    }

    void WhatToOfferDropdownHandler(Dropdown resourceOffered)
    {
        Lord.ResourceTypes resource = (Lord.ResourceTypes)resourceOffered.value - 1;
        tm.currentTrade.resourceOffered = resource;

        DropdownFix(resourceOffered);
        chooseWhatToOfferScreen.SetActive(false);
        SetupWhatAmountToOffer();
    }

    void SetupWhatAmountToOffer()
    {
        offerWhatAmountDropdown.ClearOptions();
        List<string> resourceAmounts = new List<string> { "How Much?" };

        int maxAmountPossibleToTrade = 0;
        if (tm.currentTrade.resourceOffered == Lord.ResourceTypes.Armies)
        {
            maxAmountPossibleToTrade = tm.currentLord.GetArmies();
        }
        else if (tm.currentTrade.resourceOffered == Lord.ResourceTypes.Land)
        {
            maxAmountPossibleToTrade = tm.currentLord.GetLandCount();
        }
        else if (tm.currentTrade.resourceOffered == Lord.ResourceTypes.Wealth)
        {
            maxAmountPossibleToTrade = tm.currentLord.GetWealth();
        }

        for (int i = 0; i <= maxAmountPossibleToTrade; i++)
        {
            resourceAmounts.Add(i.ToString());
        }

        offerWhatAmountDropdown.AddOptions(resourceAmounts);

        offerWhatAmountDropdown.onValueChanged.RemoveAllListeners();
        offerWhatAmountDropdown.value = 0;
        offerWhatAmountDropdown.onValueChanged.AddListener(delegate
        {
            OfferWhatAmountDropdownHandler(offerWhatAmountDropdown);
        });
        offerWhatAmountDropdown.RefreshShownValue();
        chooseHowMuchToOfferScreen.SetActive(true);
    }

    private void OfferWhatAmountDropdownHandler(Dropdown whatAmount)
    {
        int amount = whatAmount.value - 1;
        tm.currentTrade.amountResourceOffered = amount;

        DropdownFix(whatAmount);
        chooseHowMuchToOfferScreen.SetActive(false);
        askIfRecipientAgreesScreen.SetActive(true);
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
        int amountExpanded = expand.value - 1;
        tm.currentLord.Expand(amountExpanded);
        DropdownFix(expandDropdown);
        playerExpandScreen.SetActive(false);
        tm.currentGameState = TurnManager.GameState.PlayerDecidingTrades;
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
        int amountRecruited = recruit.value - 1;
        tm.currentLord.Recruit(amountRecruited);
        DropdownFix(recruitDropdown);
        playerRecruitScreen.SetActive(false);
        tm.currentGameState = TurnManager.GameState.PlayerDecidingTrades;
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

        tm.currentGameState = TurnManager.GameState.PlayerDecidingTrades;
    }

    void DropdownFix(Dropdown dropdown)
    {
        Destroy(dropdown.transform.Find("Dropdown List").gameObject);
    }
}
