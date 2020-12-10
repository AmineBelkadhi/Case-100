using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    
        public static IEnumerator FadeCanvas(CanvasGroup canvas, float startAlpha, float endAlpha, float duration)
        {
           
            var startTime = Time.time;
            var endTime = Time.time + duration;
            var elapsedTime = 0f;

            
            canvas.alpha = startAlpha;
        Debug.Log("test");
            while (Time.time <= endTime)
            {
                elapsedTime = Time.time - startTime; 
                var percentage = 1 / (duration / elapsedTime); 
                if (startAlpha > endAlpha) 
                {
                    canvas.alpha = startAlpha - percentage; 
                }
                else 
                {
                    canvas.alpha = startAlpha + percentage; 
                }

                yield return new WaitForEndOfFrame(); 
            }
            canvas.alpha = endAlpha; 
        }
}
