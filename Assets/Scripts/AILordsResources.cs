﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AILordsResources : MonoBehaviour
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
        string allAILords = "";
        foreach(AILord lord in pm.GetAILords()){
            //Update Player Resources
            string lordNameInfo = lord.lordName + "\n";
            string lordWealthInfo = "Wealth: " + lord.GetWealth() + "\n";
            string lordArmyInfo = "Armies: " + lord.GetArmies() + "\n";
            string lordLandInfo = "Land: " + lord.GetLandCount() + "\n";
            string lordLastActionInfo = "Last Move: " + lord.GetLastActionName() + "\n";

            allAILords += lordNameInfo + lordWealthInfo + lordArmyInfo + lordLandInfo + lordLastActionInfo + "\n";
        }

        playerResourceText.text = allAILords;
    }
}
