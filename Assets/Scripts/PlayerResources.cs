using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerResources : MonoBehaviour
{

    private PlayerManager pm;
    private TextMeshProUGUI playerResourceText;

    void Start()
    {
        pm = PlayerManager.instance;
        playerResourceText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        string allPlayers = "";
        foreach (Player lord in pm.GetPlayers())
        {
            //Update Player Resources
            string lordNameInfo = lord.lordName + "\n";
            string lordWealthInfo = "Wealth: " + lord.GetWealth() + "\n";
            string lordArmyInfo = "Armies: " + lord.GetArmies() + "\n";
            string lordLandInfo = "Land: " + lord.GetLandCount() + "\n";

            allPlayers += lordNameInfo + lordWealthInfo + lordArmyInfo + lordLandInfo + "\n";
        }
        playerResourceText.text = allPlayers;

    }
}
