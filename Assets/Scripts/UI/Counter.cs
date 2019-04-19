using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class Counter : MonoBehaviour
{
    public enum UICounterType { Recruit, Expand, Attack };

    public UICounterType counterType;
    private int count;
    private int maxCount;
    public Button upButton;
    public Button downButton;
    public Button acceptButton;

    public TextMeshProUGUI playerResourceText;

    private PlayerManager pm;

      
        

    public void ResetCounter()
    {
        count = 0;
        maxCount = 0;
        
        //set up buttons
        upButton.onClick.AddListener(IncreaseCount);
        downButton.onClick.AddListener(DecreaseCount);
        acceptButton.onClick.AddListener(AcceptDeal);
        
        //get max
        if (pm == null)
            pm = PlayerManager.instance;
        Lord currentLord = pm.currentLord;
        switch (counterType)
        {
            case UICounterType.Recruit:
                maxCount = currentLord.GetMaxRecruit();
                break;
            case UICounterType.Expand:
                maxCount = currentLord.GetMaxExpand();
                break;
            case UICounterType.Attack:
                maxCount = currentLord.GetArmies();
                break;            
        }
        playerResourceText.text = 0.ToString();

        Debug.Log("Max Count1: " + maxCount);
    }

    //STILL NEED TO SET NEXT GAME STATE IN HERE
    private void AcceptDeal()
    {
        Lord currentLord = pm.currentLord;
        switch (counterType)
        {
            case UICounterType.Recruit:
                currentLord.Recruit(count);
                break;
            case UICounterType.Expand:
                currentLord.Expand(count);
                break;
            case UICounterType.Attack:
                maxCount = currentLord.GetArmies();
                break;
        }

        //Go to next step
        //pm.currentGameState = PlayerManager.GameState.EndOfTurn;
    }

    private void IncreaseCount()
    {
        Debug.Log("Increase");
        Debug.Log(pm.currentGameState);

        if (count < maxCount)
        {
            count++;
            Debug.Log("count < maxCount");    
        }

        Debug.Log("Count: " + count);
        Debug.Log("MaxCount: " + maxCount);

        playerResourceText.text = count.ToString();
    }

    private void DecreaseCount()
    {
        Debug.Log("Decrease");
        Debug.Log(pm.currentGameState);

        if (count > 0)
            count--;
        playerResourceText.text = count.ToString();
    }


}
