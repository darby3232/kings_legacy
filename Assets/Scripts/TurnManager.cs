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

    public int playerCount; 

    public LordInfo playerInfo;
    public LordInfo [] aiLordsInfo;

    public Lord currentLord;
    
    private List<Player> players = new List<Player>();
    private List<AILord> aiLords = new List<AILord>();

    private Battle currentBattle; 

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
        for(int i = 0; i < playerCount; i++)
        {
            players.Add(new Player(playerInfo.isKing, playerInfo.lordsColor, playerInfo.specialLandChance, playerInfo.startingWealth, playerInfo.startingArmies, playerInfo.startingLandCount, "Player" + i + 1));
            players[i].PrintLord();
        }


       
        for (int i = 0; i < aiLordsInfo.Length; i++)
        {
            aiLords.Add(new AILord(aiLordsInfo[i].isKing, aiLordsInfo[i].lordsColor, aiLordsInfo[i].specialLandChance, aiLordsInfo[i].startingWealth, aiLordsInfo[i].startingArmies, aiLordsInfo[i].startingLandCount, aiLordsInfo[i].name));
            aiLords[i].PrintLord();
        }

        //Give each lord their next player
        for(int i = players.Count - 1; i > 0; i--)
        {
            players[i].SetNextLord(players[i - 1]);
        }

        players[0].SetNextLord(aiLords[0]);
        for (int i = 0; i < aiLords.Count - 1; i++)
        {
            aiLords[i].SetNextLord(aiLords[i + 1]);
        }
        aiLords[aiLords.Count - 1].SetNextLord(players[players.Count - 1]);

        currentLord = players[0];
        currentBattle = new Battle();
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
                if(currentBattle.BattleStep())
                {
                    currentGameState = GameState.EndOfTurn;
                }
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
       currentLord = currentLord.GetNextLord();  
    }

    /*//Getter methods for our player
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
    }*/
   
    public List<AILord> GetAILords()
    {
        return aiLords;
    }

    public List<Player> GetPlayers()
    {
        return players;
    }
    
    //Get a Battle Creator
    public void CreateBattle(Lord attacker, Lord defender)
    {
        currentBattle.CreateBattle(attacker, defender);
    }

}



