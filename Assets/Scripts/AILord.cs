using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILord : Lord
{
    public enum AILordAction { Nothing, Expand, Recruit, Attack };

    private string lastAction; 

    public AILord(bool isKing, Color lordsColor, float specialLandChance, int startingWealth, int startingArmies, int startingLand, string name)
        : base(isKing, lordsColor, specialLandChance, startingWealth, startingArmies, startingLand, name)
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
                TurnManager.instance.currentGameState = TurnManager.GameState.EndOfTurn;
                break;
            case AILordAction.Recruit:
                lastAction = "Recruited " + GetWealth() + " Armies";
                Recruit(GetWealth());
                TurnManager.instance.currentGameState = TurnManager.GameState.EndOfTurn;
                break;
            case AILordAction.Expand:
                int maxExpansion = Mathf.Min(GetWealth(), GetArmies());
                lastAction = "Expanded " + maxExpansion + " Lands";
                Expand(maxExpansion);
                TurnManager.instance.currentGameState = TurnManager.GameState.EndOfTurn;
                break;
            case AILordAction.Attack:
                Lord lordToAttack = GetRandomLordToAttack(); 
                if(lordToAttack != null)
                {
                    lastAction = "Attacked " + lordToAttack.lordName;
                    TurnManager.instance.CreateBattle(this, lordToAttack);
                }
                else
                {
                    lastAction = "No Lords to Attack, Passed";
                }
                TurnManager.instance.currentGameState = TurnManager.GameState.EndOfTurn;
                break;
        }
    }

    private Lord GetRandomLordToAttack()
    {
        List<Lord> lordsToAttack = new List<Lord>(); 

        if(TurnManager.instance.GetPlayerLand() > 0)
        {
            lordsToAttack.Add(TurnManager.instance.GetPlayerInstance());
        }

        foreach(Lord lord in TurnManager.instance.GetAILords())
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
