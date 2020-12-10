using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public float currentTime;
    public float time;
    public Text timeText;
    public static CountDown instance;

    private void Awake()
    {
        instance = this;
    }

    public void StartTimer()
    {
        currentTime = time;
        timeText.text = string.Format("{0:0}:{1:00}", Mathf.Floor(currentTime / 60), currentTime % 60);
        StopAllCoroutines(); 
        StartCoroutine(Timer());
       
    }

    IEnumerator Timer ()
    {

        while (currentTime>0)
        {
            if (Client.instance.paused == false) {
                currentTime--;

                yield return new WaitForSeconds(1f);
                timeText.text = string.Format("{0:0}:{1:00}", Mathf.Floor(currentTime / 60), currentTime % 60);
            }
            else
            {
                yield return new WaitUntil(()=>Client.instance.paused == false);
            }

        }

    }
    
    public void StopTimer()
    {
        StopCoroutine(Timer());
    }
    public float GetTime()
    {
        return currentTime;
    }

}
