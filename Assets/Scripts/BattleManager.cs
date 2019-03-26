using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private bool battleInProgress;

    private Lord attacker;
    private Lord defender;

    public void Update()
    {
        //timer logic
    }

    private void BattleStep()
    {
        if (defender.GetArmies() <= 0)
        {
            WinBattle(true);
        }else if(attacker.GetArmies() <= 0)
        {
            WinBattle(false);
        }
    }

    private void WinBattle(bool attackerWon)
    {
        if(attackerWon)
        {
            int landTaken = (defender.GetLandCount() - 1) / 3; //3 is 2 + 1 to round up the answer

            attacker.SetLand(attacker.GetLandCount() + landTaken);
            defender.SetLand(defender.GetLandCount() - landTaken);
        }
        else
        {
            int landTaken = (attacker.GetLandCount() - 1) / 3; //3 is 2 + 1 to round up the answer

            attacker.SetLand(attacker.GetLandCount() - landTaken);
            defender.SetLand(defender.GetLandCount() + landTaken);
        }
    }

    public void BeginBattle(Lord attacker, Lord defender)
    {
        if (battleInProgress)
        {
            Debug.Log("Battle existed at begin battle");
        }
        battleInProgress = true;

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
