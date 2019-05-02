using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Lord
{    
    public Player(bool isKing, Color lordsColor, float specialLandChance, int startingWealth, int startingArmies, int startingLand, string name, uint id)
        : base(isKing, lordsColor, specialLandChance, startingWealth, startingArmies, startingLand, name, id)
    {
        //extra actions
    }


}
