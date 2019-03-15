using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public enum GameState { PlayerChoosingAction, BattleOccuring, PlayerIncome }
    
    [System.Serializable]
    public struct LordInfo
    {
        public bool isKing;
        public Color lordsColor;
        public float specialLandChance;
        public int startingLandCount;
        public int startingWealth;
    }

    public static TurnManager instance = null;
    public GameState currentGameState; 

    public LordInfo playerInfo;
    public LordInfo [] aiLordsInfo;

    Player player;
    AILord [] aiLords;


    private void Awake()
    {

        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        player = new Player(playerInfo.isKing, playerInfo.lordsColor, playerInfo.specialLandChance, playerInfo.startingWealth, playerInfo.startingLandCount);

        for (int i = 0; i < aiLords.Length; i++)
        {
            aiLords[i] = new AILord(aiLordsInfo[i].isKing, aiLordsInfo[i].lordsColor, aiLordsInfo[i].specialLandChance, aiLordsInfo[i].startingWealth, aiLordsInfo[i].startingLandCount);
        }

        currentGameState = GameState.PlayerIncome; 
    }

    private void Update()
    {
        
        switch (currentGameState)
        {
            case GameState.PlayerIncome:

                break;
            case GameState.PlayerChoosingAction:

                break;
            case GameState.BattleOccuring:

                break;
        }

    }



    /*SHOULD THESE ACTIONS GO INTO A DIFFERENT CLASS?*/
    //Player Actions
    public void PlayerExpand(int amount)
    {
        if(amount > player.GetWealth() || amount > player.GetArmies())
        {
            amount = Mathf.Min(player.GetWealth(), player.GetArmies());
        }
        player.Expand(amount);
    }
   

    public void PlayerRecruit(int amount)
    {
        if (amount > player.GetWealth())
        {
            amount = player.GetWealth();
        }
        player.Recruit(amount);
    }

    //Attack
    
    public void PlayerDoNothing()
    {
        //Set Next State?
    }
}
