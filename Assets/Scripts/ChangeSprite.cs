using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ChangeSprite : MonoBehaviour
{
    public Button button;
    public Sprite spriteOld;
    public Sprite spriteNew;

    public void change()
    {
        if (button.GetComponent<Image>().sprite == spriteNew)
        {
            button.GetComponent<Image>().sprite = spriteOld;
        }
        else {
            button.GetComponent<Image>().sprite = spriteNew;
        }
    }
}