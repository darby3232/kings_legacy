using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CurrentPlayer : MonoBehaviour
{
    private PlayerManager pm;
    private TextMeshProUGUI playerResourceText;

    void Start()
    {
        pm = PlayerManager.instance;
        playerResourceText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        playerResourceText.text = pm.currentLord.lordName;
    }

}
