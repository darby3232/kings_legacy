using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandRandomImage : MonoBehaviour
{

    public GameObject[] images; 

    public void ChooseRandomImage()
    {
        for(int i = 0; i < images.Length; i++)
        {
            images[i].SetActive(false);
        }

        int randomImgIndex = Random.Range(0, 5);
        images[randomImgIndex].SetActive(true);

    }

}
