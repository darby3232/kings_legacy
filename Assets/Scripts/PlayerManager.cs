using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public enum GameState { BeforeSetup, StartOfTurn, PlayerChoosingAction, PlayerSpecifyingAction, PlayerDecidingTrades, PlayerSpecifyingTrade, PlayerIncome, EndOfTurn, RoundOver, GameOver }

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

    public static PlayerManager instance = null;
    public GameState currentGameState = GameState.BeforeSetup;
    public PlayerIcon iconHandler;

    public LordInfo playerInfo;
    public LordInfo [] aiLordsInfo;
    public Lord currentLord;
    public int kingPointsToWin;

    private int playerCount;

    private List<Player> players = new List<Player>();
    private List<AILord> aiLords = new List<AILord>();

    private string[] playerNames = { "Thornberg", "Birla", "Alfi", "Ynghildr", "Signi", "Hildingr" };


    private void Awake()
    {
               
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a PlayerManager.
            Destroy(gameObject);
                     
    }

    public void SetupPlayers(int playerCount)
    {
        this.playerCount = playerCount;

        //CREATE and set up THE PLAYERS
        for(int i = 0; i < playerCount; i++)
        {
            players.Add(new Player(playerInfo.isKing, playerInfo.lordsColor, playerInfo.specialLandChance, playerInfo.startingWealth, playerInfo.startingArmies, playerInfo.startingLandCount, playerNames[i], (uint)i));
            players[i].PrintLord();
        }

        //Give each lord their next player
        for(int i = 0; i < playerCount - 1; i++)
        {
            players[i].SetNextLord(players[i + 1]);
        }
        players[playerCount - 1].SetNextLord(players[0]);

        currentLord = players[0];
        currentGameState = GameState.StartOfTurn;
    }

    public int GetPlayerCount()
    {
        return playerCount;
    }

    public void ResetPlayers()
    {
        //CREATE and set up THE PLAYERS
        for (int i = 0; i < playerCount; i++)
        {
            if (players[i].IsKing())
            {
                Debug.Log("Giving a player king status");
                //Give king starting stats
                players[i].SetLand(playerInfo.startingLandCount + 1);
                players[i].SetWealth(playerInfo.startingWealth + 1);
                players[i].SetArmies(playerInfo.startingArmies + 1);
            }
            else
            {
                //Give normal stats
                players[i].SetLand(playerInfo.startingLandCount);
                players[i].SetWealth(playerInfo.startingWealth);
                players[i].SetArmies(playerInfo.startingArmies);
            }
        }

        currentLord = currentLord.GetNextLord();
        currentGameState = GameState.StartOfTurn;

    }

    private void Update()
    {
        
        switch (currentGameState)
        {
            case GameState.StartOfTurn:
                iconHandler.UpdatePlayerIcons();
                //Check if win
                if(currentLord.GetLandCount() <= 0 && currentLord.GetArmies() <= 0 && currentLord.GetWealth() <= 0)
                {
                    //go to next lord, this lord is out
                    JumpToNextLord();
                }
                else if(currentLord.GetLandCount() >= 5)
                {
                    //WIN
                    Debug.Log(currentLord.lordName + " won the round!");
                    currentGameState = GameState.RoundOver;
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
            case GameState.PlayerDecidingTrades:
                //Do nothing, this is handled by the UI
                break;
            case GameState.PlayerSpecifyingTrade:

                break;
            case GameState.EndOfTurn:
                //print current lord
                currentLord.PrintLord();

                JumpToNextLord();

                currentGameState = GameState.StartOfTurn;
                break;
            case GameState.RoundOver:
                //check winner -> currentPlayer
                currentLord.IncreaseKingPoints();

                //Make sure old king does not persist
                foreach(Player player in players)
                {
                    if(player != currentLord && player.IsKing())
                    {
                        player.RemoveKingStatus();
                    }
                }
                
                //check if current player has won the game
                if(currentLord.GetKingPoints() > kingPointsToWin)
                {
                    //Win the game
                    currentGameState = currentGameState = GameState.GameOver;
                }
                else
                {
                    ResetPlayers();
                    currentGameState = GameState.StartOfTurn;
                }

                break;
            case GameState.GameOver:

                Debug.Log(currentLord.lordName + " has won the game!");
                break;
            
        }

    }

    public void EndTurn()
    {
        currentGameState = GameState.EndOfTurn;
    }

    private void JumpToNextLord()
    {
       currentLord = currentLord.GetNextLord();  
    }

    public List<AILord> GetAILords()
    {
        return aiLords;
    }

    public List<Player> GetPlayers()
    {
        return players;
    }
    

}



