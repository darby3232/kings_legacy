using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILord : Lord
{
    public AILord(bool isKing, Color lordsColor, float specialLandChance, int startingWealth, int startingLand)
    {
        this.isKing = isKing;
        LordsColor = lordsColor;
        this.specialLandChance = specialLandChance;
        this.wealth = startingWealth;
        this.lands = startingWealth;
    }


}
