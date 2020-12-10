using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoTextView : MonoBehaviour
{
    TextMeshProUGUI infoText;

    public static InfoTextView instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        infoText = GetComponent<TextMeshProUGUI>();

    }

    
    public void SetInfo(string text)
    {
        gameObject.SetActive(false);
        infoText.text = text;
        gameObject.SetActive(true);
       

    }
}
