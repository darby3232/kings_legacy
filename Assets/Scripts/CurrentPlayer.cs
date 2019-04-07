using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CurrentPlayer : MonoBehaviour
{
    private TurnManager tm;
    private TextMeshProUGUI playerResourceText;

    void Start()
    {
        tm = TurnManager.instance;
        playerResourceText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        playerResourceText.text = tm.currentLord.lordName;
    }

}
