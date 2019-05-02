using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class RecruitCounter : MonoBehaviour
{

    private int count;
    private int maxCount;
    public Button upButton;
    public Button downButton;
    public Button acceptButton;

    public TextMeshProUGUI armiesRecruited;
    public TextMeshProUGUI wealthAfterExpand;
    public TextMeshProUGUI homelessSoldiers;


    private PlayerManager pm;

    public void ResetCounter()
    {
        count = 0;
        maxCount = 0;

        //set up buttons
        upButton.onClick.RemoveAllListeners();
        downButton.onClick.RemoveAllListeners();
        acceptButton.onClick.RemoveAllListeners();


        upButton.onClick.AddListener(IncreaseCount);
        downButton.onClick.AddListener(DecreaseCount);
        acceptButton.onClick.AddListener(AcceptDeal);

        //get max
        if (pm == null)
            pm = PlayerManager.instance;
        Lord currentLord = pm.currentLord;
       
        maxCount = currentLord.GetMaxRecruit();

        armiesRecruited.text = 0.ToString();
        wealthAfterExpand.text = currentLord.GetWealth().ToString();
        homelessSoldiers.text = pm.currentLord.NumberLandlessSoldiers(count + pm.currentLord.GetArmies()).ToString();
    }

    //STILL NEED TO SET NEXT GAME STATE IN HERE
    private void AcceptDeal()
    {
        Lord currentLord = pm.currentLord;
       
        currentLord.Recruit(count);
                      
    }

    private void IncreaseCount()
    {
        Debug.Log(pm.currentGameState);

        if (count < maxCount)
        {
            count++;
        }      

        armiesRecruited.text = count.ToString();
        wealthAfterExpand.text = (pm.currentLord.GetWealth()  - count).ToString();
        homelessSoldiers.text = pm.currentLord.NumberLandlessSoldiers(count + pm.currentLord.GetArmies()).ToString();
    }

    private void DecreaseCount()
    {
        Debug.Log(pm.currentGameState);

        if (count > 0)
            count--;
        armiesRecruited.text = count.ToString();
        wealthAfterExpand.text = (pm.currentLord.GetWealth() - count).ToString();
        homelessSoldiers.text = pm.currentLord.NumberLandlessSoldiers(count + pm.currentLord.GetArmies()).ToString();
    }


}
