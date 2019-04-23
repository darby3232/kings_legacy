using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILord : Lord
{
    public enum AILordAction { Nothing, Expand, Recruit, Attack };

    private string lastAction; 

    public AILord(bool isKing, Color lordsColor, float specialLandChance, int startingWealth, int startingArmies, int startingLand, string name, uint id)
        : base(isKing, lordsColor, specialLandChance, startingWealth, startingArmies, startingLand, name, id)
    {
        //extra actions
        lastAction = "First Move";
    }


    public void TakeRandomAction()
    {
        AILordAction action = (AILordAction)Random.Range(0, 4);

        switch (action)
        {
            case AILordAction.Nothing:
                lastAction = "Nothing";
                PlayerManager.instance.currentGameState = PlayerManager.GameState.EndOfTurn;
                break;
            case AILordAction.Recruit:
                lastAction = "Recruited " + GetWealth() + " Armies";
                Recruit(GetWealth());
                PlayerManager.instance.currentGameState = PlayerManager.GameState.EndOfTurn;
                break;
            case AILordAction.Expand:
                int maxExpansion = Mathf.Min(GetWealth(), GetArmies());
                lastAction = "Expanded " + maxExpansion + " Lands";
                Expand(maxExpansion);
                PlayerManager.instance.currentGameState = PlayerManager.GameState.EndOfTurn;
                break;
            case AILordAction.Attack:
                Lord lordToAttack = GetRandomLordToAttack(); 
                if(lordToAttack != null)
                {
                    lastAction = "Attacked " + lordToAttack.lordName;
                    Battle battle = new Battle(this, lordToAttack);
                    while (!battle.BattleStep())
                    { }
                    
                }
                else
                {
                    lastAction = "No Lords to Attack, Passed";
                }
                PlayerManager.instance.currentGameState = PlayerManager.GameState.EndOfTurn;
                break;
        }
    }

    private Lord GetRandomLordToAttack()
    {
        List<Lord> lordsToAttack = new List<Lord>();

        foreach (Lord lord in PlayerManager.instance.GetPlayers())
        {
            if (lord.GetArmies() > 0)
            {
                lordsToAttack.Add(lord);
            }
        }

        foreach (Lord lord in PlayerManager.instance.GetAILords())
        {
            if(lord.GetArmies() > 0 && lord != this)
            {
                lordsToAttack.Add(lord);
            }
        }

        return lordsToAttack[Random.Range(0, lordsToAttack.Count)];
    }

    public string GetLastActionName()
    {
        return lastAction;
    }

}
