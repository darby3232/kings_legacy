using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{

    public GameObject[] playerIcons;

    PlayerManager pm; 

    private void Awake()
    {
        pm = PlayerManager.instance;
    }

    // Update is called once per frame
    public void UpdatePlayerIcons()
    {
        for(int i = 0; i < pm.GetPlayerCount(); i++)
        {
            playerIcons[i].SetActive(false);
        }
        playerIcons[pm.currentLord.lordId].SetActive(true);
    }
}
