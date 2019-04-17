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

    PlayerManager pm; 

    // Start is called before the first frame update
    void Start()
    {
        pm = PlayerManager.instance;

      

        yesTradeButton.onClick.AddListener(BeginTradeOnClick);
        noTradeButton.onClick.AddListener(DoNotTradeOnClick);

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
       
    void RefuseTradeOnClick()
    {
        pm.currentGameState = PlayerManager.GameState.EndOfTurn;
        askIfRecipientAgreesScreen.SetActive(false);
    }

    void AcceptTradeOnClick()
    {
        //do trade
        Player offeringPlayer = pm.currentTrade.offeringPlayer;
        Player recipientPlayer = pm.currentTrade.recipientPlayer;

        switch (pm.currentTrade.resourceOffered)
        {
            case Lord.ResourceTypes.Armies:
                offeringPlayer.SetArmies(offeringPlayer.GetArmies() - pm.currentTrade.amountResourceOffered);
                recipientPlayer.SetArmies(recipientPlayer.GetArmies() + pm.currentTrade.amountResourceOffered);
                break;
            case Lord.ResourceTypes.Land:
                offeringPlayer.SetLand(offeringPlayer.GetLandCount() - pm.currentTrade.amountResourceOffered);
                recipientPlayer.SetLand(recipientPlayer.GetLandCount() + pm.currentTrade.amountResourceOffered);
                break;
            case Lord.ResourceTypes.Wealth:
                offeringPlayer.SetWealth(offeringPlayer.GetWealth() - pm.currentTrade.amountResourceOffered);
                recipientPlayer.SetWealth(recipientPlayer.GetWealth() + pm.currentTrade.amountResourceOffered);
                break;
        }

        switch (pm.currentTrade.resourceToReceive)
        {
            case Lord.ResourceTypes.Armies:
                recipientPlayer.SetArmies(recipientPlayer.GetArmies() - pm.currentTrade.amountResourceReceived);
                offeringPlayer.SetArmies(offeringPlayer.GetArmies() + pm.currentTrade.amountResourceReceived);
                break;
            case Lord.ResourceTypes.Land:
                recipientPlayer.SetLand(recipientPlayer.GetLandCount() - pm.currentTrade.amountResourceReceived);
                offeringPlayer.SetLand(offeringPlayer.GetLandCount() + pm.currentTrade.amountResourceReceived);
                break;
            case Lord.ResourceTypes.Wealth:
                recipientPlayer.SetWealth(recipientPlayer.GetWealth() - pm.currentTrade.amountResourceReceived);
                offeringPlayer.SetWealth(offeringPlayer.GetWealth() + pm.currentTrade.amountResourceReceived);
                break;
        }

        pm.currentGameState = PlayerManager.GameState.EndOfTurn;
        askIfRecipientAgreesScreen.SetActive(false);
    }

    void DoNotTradeOnClick()
    {
        Debug.Log("Don't Trade");
        chooseToTradeScreen.SetActive(false);
        pm.currentGameState = PlayerManager.GameState.EndOfTurn;
    }

    void BeginTradeOnClick()
    {
        Debug.Log("Beginning Trade");

        pm.currentTrade.offeringPlayer = (Player)pm.currentLord;
        chooseToTradeScreen.SetActive(false);
        pm.currentGameState = PlayerManager.GameState.PlayerSpecifyingTrade;
        SetupTradeWhoDropdown();
    }

    void SetupTradeWhoDropdown()
    {
        tradeWithWhoDropdown.ClearOptions();
        List<string> lordNames = new List<string> { "Choose Trade Partner" };
        foreach (Player lord in pm.GetPlayers())
        {
            if (lord != pm.currentLord)
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

        foreach (Player lord in pm.GetPlayers())
        {
            if (tradeWithWho.captionText.text == lord.lordName)
                lordToTradeWith = lord;
        }

        if (lordToTradeWith == null)
        {
            Debug.Log("Lord Attacking Null");
        }
        else if (pm.currentLord == null)
        {
            Debug.Log("Player Attacking Null");
        }
        pm.currentTrade.recipientPlayer = lordToTradeWith;
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
        pm.currentTrade.resourceToReceive = resource;
        DropdownFix(tradeForWhat);
        chooseWhatToAskForScreen.SetActive(false);
        SetupTradeForWhatAmountDropdown();
    }

    void SetupTradeForWhatAmountDropdown()
    {
        tradeForWhatAmountDropdown.ClearOptions();
        List<string> resourceAmounts = new List<string> { "How Much?" };

        int maxAmountPossibleToTrade = 0; 
        if(pm.currentTrade.resourceOffered == Lord.ResourceTypes.Armies)
        {
            maxAmountPossibleToTrade = pm.currentTrade.recipientPlayer.GetArmies();
        }else if(pm.currentTrade.resourceOffered == Lord.ResourceTypes.Land)
        {
            maxAmountPossibleToTrade = pm.currentTrade.recipientPlayer.GetLandCount();
        }else if(pm.currentTrade.resourceOffered == Lord.ResourceTypes.Wealth)
        {
            maxAmountPossibleToTrade = pm.currentTrade.recipientPlayer.GetWealth();
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
        pm.currentTrade.amountResourceReceived = amountAskedFor;

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
        pm.currentTrade.resourceOffered = resource;

        DropdownFix(resourceOffered);
        chooseWhatToOfferScreen.SetActive(false);
        SetupWhatAmountToOffer();
    }

    void SetupWhatAmountToOffer()
    {
        offerWhatAmountDropdown.ClearOptions();
        List<string> resourceAmounts = new List<string> { "How Much?" };

        int maxAmountPossibleToTrade = 0;
        if (pm.currentTrade.resourceOffered == Lord.ResourceTypes.Armies)
        {
            maxAmountPossibleToTrade = pm.currentLord.GetArmies();
        }
        else if (pm.currentTrade.resourceOffered == Lord.ResourceTypes.Land)
        {
            maxAmountPossibleToTrade = pm.currentLord.GetLandCount();
        }
        else if (pm.currentTrade.resourceOffered == Lord.ResourceTypes.Wealth)
        {
            maxAmountPossibleToTrade = pm.currentLord.GetWealth();
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
        pm.currentTrade.amountResourceOffered = amount;

        DropdownFix(whatAmount);
        chooseHowMuchToOfferScreen.SetActive(false);
        askIfRecipientAgreesScreen.SetActive(true);
    }

    void DoNothingOnClick()
    {
        Debug.Log("Doing Nothing");        

        pm.currentGameState = PlayerManager.GameState.PlayerDecidingTrades;
    }

    void DropdownFix(Dropdown dropdown)
    {
        Destroy(dropdown.transform.Find("Dropdown List").gameObject);
    }
}
