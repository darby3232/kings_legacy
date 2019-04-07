using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle
{
    private Lord attacker;
    private Lord defender;

    public bool BattleStep()
    {
        

        //check for win
        if (defender.GetArmies() <= 0)
        {
            WinBattle(true);
            return true;
        }else if(attacker.GetArmies() <= 0)
        {
            WinBattle(false);
            return true;
        }

        //battle logic
        int coinFlip = Random.Range(0, 2);
        if (coinFlip == 0)
        {
            attacker.ArmyLosesBattle();
        }
        else
        {
            defender.ArmyLosesBattle();
        }


        return false;
    }

    private void WinBattle(bool attackerWon)
    {
        if(attackerWon)
        {
            int landTaken = (defender.GetLandCount()) / 2; 

            attacker.SetLand(attacker.GetLandCount() + landTaken);
            defender.SetLand(defender.GetLandCount() - landTaken);
        }
        else
        {
            int landTaken = (attacker.GetLandCount()) / 2; 

            attacker.SetLand(attacker.GetLandCount() - landTaken);
            defender.SetLand(defender.GetLandCount() + landTaken);
        }
    }

    public void CreateBattle(Lord attacker, Lord defender)
    {       
        this.attacker = attacker;
        this.defender = defender;

        if (attacker.GetArmies() <= 0)
        {
            Debug.Log("Attacker with less than 0 Armies Attacking");
        }

        //should make sure attacker has more than 0 armies when attacking
        if (defender.GetArmies() <= 0)
        {
            WinBattle(true);
        }
    }
}
