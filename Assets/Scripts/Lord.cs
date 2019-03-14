using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lord : MonoBehaviour
{

    public enum LordAction { Nothing, Attack, Expand, Recruit };

    public Color LordsColor;
    public Vector3 armyPlacement;

    protected bool isKing;
    protected float specialLandChance; 
    
    protected int armies;
    protected int lands; //Land [] lands;
    protected int wealth;

    

    public void ReceiveIncome()
    {
        //INTEREST
        int interestIncome = wealth / 2;
        //LAND INCOME
        int landIncome = lands;
        wealth += interestIncome + landIncome;
    }

    public int NumberHomelessSoldiers()
    {
        return (lands * 2) - armies;
    }

    public void SpendToMaintainArmies()
    {
        int homelessSoldiers = (lands * 2) - armies;
        int wealthLeft = wealth - homelessSoldiers;

        if(wealthLeft < 0)
        {
            armies += wealthLeft;
            wealth = 0;
        }
        else
        {
            wealth = wealthLeft;
        }
    }

    public void ReleaseArmies()
    {
        int homelessSoldiers = (lands * 2) - armies;
        armies -= homelessSoldiers;
    }

    public void Recruit(int wealthSpent)
    {
        wealth -= wealthSpent;
        armies += wealthSpent;
    }

    public void Expand(int resourcesSpent)
    {
        wealth -= resourcesSpent;
        armies -= resourcesSpent;

        lands += resourcesSpent;
    }

    public int GetWealth()
    {
        return wealth; 
    }
    
    public int GetArmies()
    {
        return armies; 
    }

    public void SetLand(int numLands)
    {
        this.lands = numLands;
    }

    public int GetLand()
    {
        return lands;
    }

}
