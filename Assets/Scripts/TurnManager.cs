using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    [System.Serializable]
    public struct LordInfo
    {
        public bool isKing;
        public Color lordsColor;
        public float specialLandChance;
        public int startingLandCount;
        public int startingWealth;
    }

    public LordInfo playerInfo;
    public LordInfo [] aiLordsInfo;

    Player player;
    AILord [] aiLords; 

    // Start is called before the first frame update
    void Start()
    {
        player = new Player(playerInfo.isKing, playerInfo.lordsColor, playerInfo.specialLandChance, playerInfo.startingWealth, playerInfo.startingLandCount);

        for(int i = 0; i < aiLords.Length; i++)
        {
            aiLords[i] = new AILord(aiLordsInfo[i].isKing, aiLordsInfo[i].lordsColor, aiLordsInfo[i].specialLandChance, aiLordsInfo[i].startingWealth, aiLordsInfo[i].startingLandCount);
        }
    }


    //Player Actions
    public void PlayerExpand(int amount)
    {
        if(amount > player.GetWealth() || amount > player.GetArmies())
        {
            amount = Mathf.Min(player.GetWealth(), player.GetArmies());
        }
        player.Expand(amount);
    }
    public void PlayerCreateExpandDropdown()
    {

    }


    public void PlayerRecruit(int amount)
    {
        if (amount > player.GetWealth())
        {
            amount = player.GetWealth();
        }
        player.Recruit(amount);
    }

    //Attack
    
    public void PlayerDoNothing()
    {

    }
}
