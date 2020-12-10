using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickControl : MonoBehaviour
{


    private void OnMouseDown()
    {
        
        gameObject.SetActive(false); //delete the selected object
        TrackClick.totalclicks = 0; //set total of clicks to 0 after a successful click
        
    }
}
