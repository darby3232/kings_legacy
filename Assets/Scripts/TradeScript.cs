using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TradeScript : MonoBehaviour
{
    public GameObject acceptTradeOfferScreen;
    public TextMeshProUGUI tradeOfferDescription;
    public Button acceptTradeOffer;
    public Button rejectTradeOffer;

    public GameObject tradeScreen;
    public TMP_Dropdown offerResourceDropdown;
    public TMP_Dropdown offerAmountDropdown;
    public TMP_Dropdown receiveResourceDropdown;
    public TMP_Dropdown receiveAmountDropdown;
    public Button sendTradeButton;

    public TMP_Dropdown whoToTradeWithDropdown;

    private PlayerManager pm;

    private Lord lordToOfferTrade;

    
    // Start is called before the first frame update
    public void SetupTradeScreen()
    {
        if (pm == null)
            pm = PlayerManager.instance;

        //Get player's offer
        FillResourcesDropdown(pm.currentLord, offerResourceDropdown);
        UpdateOfferAmountDropdown();

        Debug.Log("finished player");

        //Get Player to trade with
        whoToTradeWithDropdown.onValueChanged.RemoveAllListeners();
        whoToTradeWithDropdown.value = 0;
        List<string> playerNames = new List<string>();
        foreach(Lord lord in pm.GetPlayers())
        {
            if(lord != pm.currentLord)
                playerNames.Add(lord.lordName);
        }
        whoToTradeWithDropdown.ClearOptions();
        whoToTradeWithDropdown.AddOptions(playerNames);
        UpdateTradePartner();
        whoToTradeWithDropdown.onValueChanged.AddListener(delegate
        {
            UpdateTradePartner();
        });


        //Values changed stuff
        offerResourceDropdown.onValueChanged.RemoveAllListeners();
        offerResourceDropdown.value = 0;
        offerResourceDropdown.onValueChanged.AddListener(delegate
        {
            UpdateOfferAmountDropdown();
        });
        receiveResourceDropdown.onValueChanged.RemoveAllListeners();
        receiveResourceDropdown.value = 0;
        receiveResourceDropdown.onValueChanged.AddListener(delegate
        {
            UpdateReceiveAmountDropdown();
        });

        UpdateReceiveAmountDropdown();
        sendTradeButton.onClick.AddListener(SendTradeOffer);

    }

    public void SendTradeOffer()
    {
        string tradeText = pm.currentLord.lordName + " offers " + lordToOfferTrade.lordName + ": \n" +
                            offerAmountDropdown.value.ToString() + " " + offerResourceDropdown.captionText.text + " for \n" + 
                            receiveAmountDropdown.value.ToString() + " " + receiveResourceDropdown.captionText.text;
        tradeOfferDescription.text = tradeText;

        //set up buttons
        acceptTradeOffer.onClick.AddListener(AcceptTradeOffer);
        rejectTradeOffer.onClick.AddListener(RejectTradeOffer);

        tradeScreen.SetActive(false);
        acceptTradeOfferScreen.SetActive(true);
    }

    public void AcceptTradeOffer()
    {
        Debug.Log(offerResourceDropdown.captionText.text);

        //trade the goods
        Lord offerLord = pm.currentLord; 
        //handles offer exchange
        if(offerResourceDropdown.captionText.text == "Wealth")
        {
            offerLord.SetWealth(offerLord.GetWealth() - offerAmountDropdown.value + 1);
            lordToOfferTrade.SetWealth(lordToOfferTrade.GetWealth() + offerAmountDropdown.value - 1);
        }
        else if(offerResourceDropdown.captionText.text == "Land")
        {
            offerLord.SetLand(offerLord.GetLandCount() - offerAmountDropdown.value + 1);
            lordToOfferTrade.SetLand(lordToOfferTrade.GetLandCount() + offerAmountDropdown.value - 1);
        }
        else if(offerResourceDropdown.captionText.text == "Armies")
        {
            offerLord.SetArmies(offerLord.GetArmies() - offerAmountDropdown.value + 1);
            lordToOfferTrade.SetArmies(lordToOfferTrade.GetArmies() + offerAmountDropdown.value - 1);
        }

        //handles receive exchange
        if (receiveResourceDropdown.captionText.text == "Wealth")
        {
            lordToOfferTrade.SetWealth(lordToOfferTrade.GetWealth() - receiveAmountDropdown.value + 1);
            offerLord.SetWealth(offerLord.GetWealth() + receiveAmountDropdown.value - 1);
        }
        else if (receiveResourceDropdown.captionText.text == "Land")
        {
            lordToOfferTrade.SetLand(lordToOfferTrade.GetLandCount() - receiveAmountDropdown.value + 1);
            offerLord.SetLand(offerLord.GetLandCount() + receiveAmountDropdown.value - 1);
        }
        else if (receiveResourceDropdown.captionText.text == "Armies")
        {
            lordToOfferTrade.SetArmies(lordToOfferTrade.GetArmies() - receiveAmountDropdown.value + 1);
            offerLord.SetArmies(offerLord.GetArmies() + receiveAmountDropdown.value - 1);
        }

        acceptTradeOfferScreen.SetActive(false);
    }

    public void RejectTradeOffer()
    {
        //move to end of turn
        acceptTradeOfferScreen.SetActive(false);
    }

    private void UpdateTradePartner()
    {
        //get value from whoToTradeWit
        Debug.Log("Trade With: " + whoToTradeWithDropdown.captionText.text);

        foreach (Lord lord in pm.GetPlayers())
        {
            if (whoToTradeWithDropdown.captionText.text == lord.lordName)
                lordToOfferTrade = lord;
        }

        Debug.Log("Actually trading with: " + lordToOfferTrade.lordName);

        //Update values in the 
        FillResourcesDropdown(lordToOfferTrade, receiveResourceDropdown);
        UpdateReceiveAmountDropdown();
    }

    private void UpdateOfferAmountDropdown()
    {
        int maxAmount = 0; 
        if(offerResourceDropdown.captionText.text == "Armies")
        {
            maxAmount = pm.currentLord.GetArmies();
        }
        else if(offerResourceDropdown.captionText.text == "Land")
        {
            maxAmount = pm.currentLord.GetLandCount();
        }
        else if(offerResourceDropdown.captionText.text == "Wealth")
        {
            maxAmount = pm.currentLord.GetWealth();
        }
        List<string> amounts = new List<string>();
        //get the max amount
        for(int i = 0; i <= maxAmount; i++)
        {
            amounts.Add(i.ToString());
        }
        offerAmountDropdown.ClearOptions();
        offerAmountDropdown.AddOptions(amounts);
        offerAmountDropdown.value = 0; 
    }

    private void UpdateReceiveAmountDropdown()
    {
        int maxAmount = 0;
        if (receiveResourceDropdown.captionText.text == "Armies")
        {
            maxAmount = lordToOfferTrade.GetArmies();
        }
        else if (receiveResourceDropdown.captionText.text == "Land")
        {
            maxAmount = lordToOfferTrade.GetLandCount();
        }
        else if (receiveResourceDropdown.captionText.text == "Wealth")
        {
            maxAmount = lordToOfferTrade.GetWealth();
        }
        List<string> amounts = new List<string>();
        //get the max amount
        for (int i = 0; i <= maxAmount; i++)
        {
            amounts.Add(i.ToString());
        }
        receiveAmountDropdown.ClearOptions();
        receiveAmountDropdown.AddOptions(amounts);
        receiveAmountDropdown.value = 0;
    }

    private void FillResourcesDropdown(Lord lord, TMP_Dropdown resourceDropdown)
    {
        List<string> resources = new List<string>();
        if(lord.GetArmies() > 0)
        {
            resources.Add("Armies");
        }
        if(lord.GetLandCount() > 0)
        {
            resources.Add("Land");
        }
        if(lord.GetWealth() > 0)
        {
            resources.Add("Wealth");
        }
        resourceDropdown.ClearOptions();
        resourceDropdown.AddOptions(resources);
    }

}
