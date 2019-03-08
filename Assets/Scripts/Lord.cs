using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lord : MonoBehaviour
{

    public Color LordsColor;

    private bool isKing;
    private float specialLandChance; 
    
    private int numberArmies;
    private int numberLands; //replace with list when implementing special Lands
    private int numberWealth;


    public Lord(bool isKing, Color lordsColor, float specialLandChance)
    {
        this.isKing = isKing;
        LordsColor = lordsColor;
        this.specialLandChance = specialLandChance; 
    }

    public int GetWealth()
    {
        return numberWealth; 
    }

    public void GainArmies(int numWealthSpent)
    {
        if(numWealthSpent <= numberWealth)
        {
            numberArmies += numWealthSpent; 
        }
    }


    public int GetArmies()
    {
        return numberArmies; 
    }

    public int GetLands()
    {
        return numberLands;
    }
}
