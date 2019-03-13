using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lord : MonoBehaviour
{

    public Color LordsColor;

    protected bool isKing;
    protected float specialLandChance; 
    
    protected int numberArmies;
    protected int numberLands; //replace with list when implementing special Lands
    protected int numberWealth;


    public int GetWealth()
    {
        return numberWealth; 
    }

    //not sure if this function should go here
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

    public void SetLand(int numLands)
    {
        this.numberLands = numLands;
    }

    public int GetLand()
    {
        return numberLands;
    }

    public void Attack(Lord attacker, Lord defender)
    {

           
    }
}
