using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public enum GameState { StartOfTurn, PlayerChoosingAction, PlayerSpecifyingAction, BattleOccuring, PlayerIncome, EndOfTurn, GameOver }

   [System.Serializable]
    public struct LordInfo
    {
        public bool isKing;
        public Color lordsColor;
        public float specialLandChance;
        public int startingLandCount;
        public int startingWealth;
        public int startingArmies;
        public string name;
    }

    public static TurnManager instance = null;
    public GameState currentGameState = GameState.StartOfTurn;
    public bool playerActionMenuShown = false;


    public LordInfo playerInfo;
    public LordInfo [] aiLordsInfo;

    public Lord currentLord;
    
    private Player player;
    private List<AILord> aiLords = new List<AILord>();

    private void Awake()
    {
               
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a TurnManager.
            Destroy(gameObject);
                     
    }

    private void Start()
    {
        //CREATE and set up THE PLAYERS
        player = new Player(playerInfo.isKing, playerInfo.lordsColor, playerInfo.specialLandChance, playerInfo.startingWealth, playerInfo.startingArmies, playerInfo.startingLandCount, "Player");
        player.PrintLord();
        for (int i = 0; i < aiLordsInfo.Length; i++)
        {
            aiLords.Add(new AILord(aiLordsInfo[i].isKing, aiLordsInfo[i].lordsColor, aiLordsInfo[i].specialLandChance, aiLordsInfo[i].startingWealth, aiLordsInfo[i].startingArmies, aiLordsInfo[i].startingLandCount, aiLordsInfo[i].name));
            aiLords[i].PrintLord();
        }

        //Give each lord their next player
        player.SetNextLord(aiLords[0]);
        for (int i = 0; i < aiLords.Count - 1; i++)
        {
            aiLords[i].SetNextLord(aiLords[i + 1]);
        }

        currentLord = player;
        currentGameState = GameState.StartOfTurn;
    }

    private void Update()
    {
        
        switch (currentGameState)
        {
            case GameState.StartOfTurn:
                //Check if win
                if(currentLord.GetLandCount() <= 0)
                {
                    //go to next lord, this lord is out
                    JumpToNextLord();
                }
                else if(currentLord.GetLandCount() >= 5)
                {
                    //WIN
                    Debug.Log(currentLord.lordName + " won!");
                    currentGameState = GameState.GameOver;
                }
                else
                {
                    currentGameState = GameState.PlayerIncome;
                }
                
                break;
            case GameState.PlayerIncome:
                currentLord.ReceiveIncome();
                currentLord.SpendToMaintainArmies();
                currentGameState = GameState.PlayerChoosingAction;
                break;
            case GameState.PlayerChoosingAction:

                if(currentLord is AILord)
                {
                    //do random AI thing
                    ((AILord)currentLord).TakeRandomAction();
                }
                //Otherwise, do nothing as it is handled by the UI
                break;
            case GameState.PlayerSpecifyingAction:
                //Do nothing, this is handled by the UI
                break;
            case GameState.BattleOccuring:
                //Do Nothing
                break;
            case GameState.EndOfTurn:
                //print current lord
                currentLord.PrintLord();

                JumpToNextLord();

                currentGameState = GameState.StartOfTurn;
                break;
            case GameState.GameOver:
                
                Debug.Log("GameOver");

               
                break;
        }

    }

    private void JumpToNextLord()
    {
        //Go to start of next player's turn
        if (currentLord.GetNextLord() != null)
        {
            currentLord = currentLord.GetNextLord();
        }
        else
        {
            currentLord = player;
        }
    }

    //Getter methods for our player
    public int GetPlayerWealth()
    {
        return player.GetWealth(); 
    }
    public int GetPlayerArmies()
    {
        return player.GetArmies();
    }
    public int GetPlayerLand()
    {
        return player.GetLandCount();
    }
}



