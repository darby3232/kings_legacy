using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class ExpandCounter : MonoBehaviour
{

    private int count;
    private int maxCount;
    public Button upButton;
    public Button downButton;
    public Button acceptButton;

    public TextMeshProUGUI landsGained;
    public TextMeshProUGUI armiesAfterExpand;
    public TextMeshProUGUI wealthAfterExpand;

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
           
        maxCount = currentLord.GetMaxExpand();

        landsGained.text = 0.ToString();
        armiesAfterExpand.text = currentLord.GetArmies().ToString();
        wealthAfterExpand.text = currentLord.GetWealth().ToString();
    }

    //STILL NEED TO SET NEXT GAME STATE IN HERE
    private void AcceptDeal()
    {
        
        pm.currentLord.Expand(count);
       
    }

    private void IncreaseCount()
    {
        Debug.Log(pm.currentGameState);

        if (count < maxCount)
        {
            count++;
        }

        landsGained.text = count.ToString();
        armiesAfterExpand.text = (pm.currentLord.GetArmies() - count).ToString();
        wealthAfterExpand.text = (pm.currentLord.GetWealth() - count).ToString();
    }

    private void DecreaseCount()
    {
        Debug.Log(pm.currentGameState);

        if (count > 0)
            count--;
        landsGained.text = count.ToString();
        armiesAfterExpand.text = (pm.currentLord.GetArmies() - count).ToString();
        wealthAfterExpand.text = (pm.currentLord.GetWealth() - count).ToString();
    }


}
