using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTests : MonoBehaviour
{
    public Dictionary<int, int> cons = new Dictionary<int, int>();

    public void Start()
    {
        Debug.Log("Start");

        for (int i = 0; i < 5; i++)
        {
            cons.Add(i, -1);
        }

        cons[2] = 6;

        for (int i = 0; i < 5; i++)
        {
            Debug.Log(cons[i]);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}