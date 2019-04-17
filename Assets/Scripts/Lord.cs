using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lord
{
    public enum ResourceTypes { Land, Wealth, Armies };

    public Color LordsColor;
    public Vector3 armyPlacement;
    public string lordName;

    protected bool isKing;
    protected float specialLandChance; 
    
    protected int armies = 0;
    protected int lands = 0;
    protected int wealth = 0;
    protected Lord nextLord;


    public Lord(bool isKing, Color lordsColor, float specialLandChance, int startingWealth, int startingArmies, int startingLand, string name)
    {
        this.isKing = isKing;
        LordsColor = lordsColor;
        this.specialLandChance = specialLandChance;
        armies = startingArmies;
        wealth = startingWealth;
        lands = startingLand;
        lordName = name;
    }

    public void SetNextLord(Lord nextLord)
    {
        this.nextLord = nextLord;
    }

    public Lord GetNextLord()
    {
        return nextLord;
    }

    public void ReceiveIncome()
    {
        //INTEREST
        int interestIncome = wealth / 2;
       
        //LAND INCOME
        int landIncome = lands;

        wealth += interestIncome + landIncome;
    }

    public int NumberLandlessSoldiers()
    {
        int landLessSoldiers = armies - (lands * 2); 
        return Mathf.Max(0, landLessSoldiers);
    }

    public void SpendToMaintainArmies()
    {
        int wealthLeft = wealth - NumberLandlessSoldiers();

        if(wealthLeft < 0)
        {
            armies = Mathf.Max(0, armies + wealthLeft);
            wealth = 0;
        }
        else
        {
            wealth = wealthLeft;
        }
    }

   
    public void Recruit(int wealthSpent)
    {
        if (wealthSpent > GetWealth())
        {
            wealthSpent = GetWealth();
        }
        wealth -= wealthSpent;
        armies += wealthSpent;
    }

    public void Expand(int resourcesSpent)
    {
        if (resourcesSpent > GetWealth() || resourcesSpent > GetArmies())
        {
            resourcesSpent = Mathf.Min(GetWealth(), GetArmies());
        }

        wealth -= resourcesSpent;
        armies -= resourcesSpent;

        lands += resourcesSpent;
    }

    public int GetMaxRecruit()
    {
        return wealth; 
    }

    public int GetMaxExpand()
    {
        return Mathf.Min(wealth, armies);
    }

    public int GetWealth()
    {
        return wealth; 
    }
    
    public void SetWealth(int newWealth)
    {
        this.wealth = newWealth;
    }

    public int GetArmies()
    {
        return armies; 
    }

    public void SetArmies(int newArmies)
    {
        this.armies = newArmies;
    }

    public void ArmyLosesBattle()
    {
        armies--;
    }

    public void SetLand(int numLands)
    {
        this.lands = numLands;
    }

    public int GetLandCount()
    {
        return lands;
    }

    public void PrintLord()
    {
        string nameString = "Lord: " + lordName;
        string landString = "Land: " + lands;
        string armiesString = "Armies: " + armies;
        string wealthString = "Wealth: " + wealth;

        Debug.Log(nameString + " | " + landString + " | " + armiesString + " | " + wealthString);
    }
}
