using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackClick : MonoBehaviour
{
    public static int totalclicks = 0;
    public KeyCode mouseclick; //asign mouseclick to Mouse0 in Unity (Left click)
    

   
    void Update()
    {
        if (Input.GetKeyDown(mouseclick))
        {
            totalclicks += 1; //increase total clicks by 1 if the left button is clicked
        }

        if(totalclicks > 3)
        {
            //penalty
            totalclicks = 0;
        }
        
    }
}
