using System;
using System.Collections;
using UnityEngine;

public class Timer
{
    public int duration { get; private set; }

    public float startTime { get; set; }
    private float nextUpdate;

    public bool isPaused = false;

    public int timeElapsed;
    public int currentTime;

    private WaitForSeconds OneSecondYield = new WaitForSeconds(1f);

    private float pausedTime;

    private Action onTimeUp;
    private Action onUpdate;

    private TimeSpan timeFormat;

    public Timer(int duration, Action onTimeUp, Action onUpdate)
    {
        this.duration = duration;
        this.onTimeUp = onTimeUp;
        this.onUpdate = onUpdate;
    }

    public Timer(Action onUpdate)
    {
        this.onUpdate = onUpdate;
    }

    public IEnumerator StartTimer()
    {
        currentTime = 0;
        startTime = Time.time;
        while (!isPaused)
        {
            yield return OneSecondYield;

            currentTime += 1;
            onUpdate();

            while (isPaused)
            {
                yield return null;
            }
        }
    }

    public IEnumerator StartCountDown()
    {
        currentTime = duration;

        startTime = Time.time;

        while (currentTime >= 0)
        {
            while (isPaused)
            {
                yield return null;
            }

            onUpdate();
            currentTime -= 1;
            yield return OneSecondYield;
        }

        onTimeUp();
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }

    public string CurrentTimeFormat()
    {
        return TimeSpan.FromSeconds(currentTime).ToString();
    }

    public void AddTime(int valueInSeconds)
    {
        currentTime += valueInSeconds;
    }

    public void RemoveTime(int valueInSeconds)
    {
        if (currentTime - valueInSeconds < 0)
        {
            currentTime = 0;
        }
        else
        {
            currentTime -= valueInSeconds;
        }
    }
}