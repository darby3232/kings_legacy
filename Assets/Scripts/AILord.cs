using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILord : Lord
{
    public enum AILordAction { Nothing, Expand, Recruit };


    public AILord(bool isKing, Color lordsColor, float specialLandChance, int startingWealth, int startingArmies, int startingLand, string name)
        : base(isKing, lordsColor, specialLandChance, startingWealth, startingArmies, startingLand, name)
    {
        //extra actions
    }


    public void TakeRandomAction()
    {
        AILordAction action = (AILordAction)Random.Range(0, 2);

        switch (action)
        {
            case AILordAction.Nothing:
                Debug.Log(lordName + " is doing nothing");
                TurnManager.instance.currentGameState = TurnManager.GameState.EndOfTurn;
                break;
            case AILordAction.Recruit:
                Debug.Log(lordName + " is recruiting");
                Recruit(GetWealth());
                TurnManager.instance.currentGameState = TurnManager.GameState.EndOfTurn;
                break;
            case AILordAction.Expand:
                Debug.Log(lordName + " is expanding");
                Expand(Mathf.Min(GetWealth(), GetArmies()));
                TurnManager.instance.currentGameState = TurnManager.GameState.EndOfTurn;
                break;
        }

    }

}
