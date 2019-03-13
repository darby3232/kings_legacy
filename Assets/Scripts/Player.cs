using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Lord
{
    
    public Player(bool isKing, Color lordsColor, float specialLandChance)
    {
        this.isKing = isKing;
        LordsColor = lordsColor;
        this.specialLandChance = specialLandChance;
    }

    

}
