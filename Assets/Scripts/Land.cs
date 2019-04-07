using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    public enum SpecialLandType { Mountain, Barbarian };
    private int numberOfLandTypes = 2;


    private SpecialLandType landType;
    private bool isSpecialLand; 



    public Land(float specialLandChance)
    {
        //Random Chance to get a Special Land based on the player's luck
        if(Random.value < specialLandChance)
        {
            isSpecialLand = true;
            //cast a random value in the enum range to the SpecialLandType
            landType = (SpecialLandType)Random.Range(0, numberOfLandTypes - 1);
        }
        else
        {
            isSpecialLand = false;
        }
    }

}
