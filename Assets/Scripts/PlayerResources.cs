using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerResources : MonoBehaviour
{

    private TurnManager tm;
    private TextMeshProUGUI playerResourceText;

    void Start()
    {
        tm = TurnManager.instance;
        playerResourceText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update Player Resources
        string playerWealthInfo = "Wealth: " + tm.GetPlayerWealth() + "\n";
        string playerArmyInfo = "Armies: " + tm.GetPlayerArmies() + "\n";
        string playerLandInfo = "Land Count: " + tm.GetPlayerLand();

        playerResourceText.text = "Player: \n" + playerWealthInfo + playerArmyInfo + playerLandInfo;
    }
}
