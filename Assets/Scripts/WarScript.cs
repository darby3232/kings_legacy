using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class WarScript : MonoBehaviour
{
    public float battleSecLength;

    public GameObject attackScreen;
    public GameObject victoryScreen;
    public TextMeshProUGUI winnerMessage;
    public Button victoryContinueButton;

    public Button acceptButton;
    public TextMeshProUGUI attackerMessage;
    public TextMeshProUGUI defenderMessage;
    public TextMeshProUGUI attackerArmies;
    public TextMeshProUGUI defenderArmies;

    public TextMeshProUGUI attackerName;
    public TextMeshProUGUI defenderName;


    public AudioClip battleSound;
    public AudioSource soundsSource;

    public TMP_Dropdown defenderDropdown;

    private PlayerManager pm;
    private Lord currentLordSelected;
    private Battle battle;

    public void SetupAttackScreen()
    {
        if (pm == null)
            pm = PlayerManager.instance;

        //set attacker name
        attackerName.text = pm.currentLord.lordName;

        //set winner message to nothing
        winnerMessage.text = "";

        acceptButton.onClick.RemoveAllListeners();
        victoryContinueButton.onClick.RemoveAllListeners();

        //set up accept buttons
        acceptButton.onClick.AddListener(BattleAccepted);
        victoryContinueButton.onClick.AddListener(VictoryContinueButton);

        //set up the attacker's info
        attackerArmies.text = "Armies: " + pm.currentLord.GetArmies();
        attackerMessage.text = "Risks Losing:\n" + pm.currentLord.GetLandCount() / 2 + " Land";

        //set up the dropdown
        defenderDropdown.onValueChanged.RemoveAllListeners();
        defenderDropdown.value = 0;
        defenderDropdown.onValueChanged.AddListener(delegate
        {
            DefenderDropdownHandler(defenderDropdown);
        });
        defenderDropdown.RefreshShownValue();

        //give dropdown it's values
        defenderDropdown.ClearOptions();
        List<string> lordNames = new List<string>();
        foreach (AILord lord in pm.GetAILords())
        {
            if (lord.GetLandCount() > 0)
                lordNames.Add(lord.lordName);
        }
        foreach (Player lord in pm.GetPlayers())
        {
            if (lord.GetLandCount() > 0 && lord != pm.currentLord)
            {
                lordNames.Add(lord.lordName);
            }
        }
        defenderDropdown.AddOptions(lordNames);

        currentLordSelected = findDefenderLord(defenderDropdown);
        ResetDefenderMessages();
    }

    private void BattleAccepted()
    {
        battle = new Battle(pm.currentLord, currentLordSelected);



        BattleStep();
    }

    private void BattleStep()
    {
        if (battle.BattleStep())
        {
            //Do Battle Won
            Debug.Log("Someone Won the Battle");
            //Show the Victory Screen
            attackScreen.SetActive(false);
            ShowVictoryScreen();
        }
        else
        {
            soundsSource.PlayOneShot(battleSound);
            Debug.Log("One Step...");
            ResetAttackerMessages();
            ResetDefenderMessages();
            Invoke("BattleStep", battleSecLength);
        }
    }

    private void ShowVictoryScreen()
    {
        //Set Winner Text
        winnerMessage.text = battle.GetWinner().lordName + "'s warriors have emerged victorious!";
        victoryScreen.SetActive(true);
    }

    private void VictoryContinueButton()
    {
        victoryScreen.SetActive(false);
        //pm.currentGameState = PlayerManager.GameState.EndOfTurn;
    }

    private void DefenderDropdownHandler(TMP_Dropdown dropdown)
    {
        //change the defender message and army count
        currentLordSelected = findDefenderLord(dropdown);
        ResetDefenderMessages();
    }

    private Lord findDefenderLord(TMP_Dropdown dropdown)
    {
        Lord lordToAttack = null;

        foreach (Lord lord in pm.GetAILords())
        {
            if (dropdown.captionText.text == lord.lordName)
                lordToAttack = lord;
        }
        foreach (Lord lord in pm.GetPlayers())
        {
            if (dropdown.captionText.text == lord.lordName)
                lordToAttack = lord;
        }
        return lordToAttack;
    }

    private void ResetAttackerMessages()
    {
        //set up the attacker's info
        attackerArmies.text = "Armies:\n" + pm.currentLord.GetArmies();
        attackerMessage.text = "Risks Losing:\n" + pm.currentLord.GetLandCount() / 2 + " Land";
    }

    private void ResetDefenderMessages()
    {
        defenderName.text = currentLordSelected.lordName;
        defenderArmies.text = "Armies:\n" + currentLordSelected.GetArmies();
        defenderMessage.text = "Would Lose:\n" + currentLordSelected.GetLandCount() / 2 + " Land";
    }
}
