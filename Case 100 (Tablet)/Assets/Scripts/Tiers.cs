using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiers: MonoBehaviour
{
    //public ScoreInfo UInfo;
    public List<ScoreInfo> scoreInfos;
    public static Tiers tierInstance;

    private void Awake()
    {
        tierInstance = this;

    }
   /* public int GetScore(float Timer) {



    }*/
}
