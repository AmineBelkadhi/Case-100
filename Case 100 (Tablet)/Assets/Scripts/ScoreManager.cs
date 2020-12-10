using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
        


public class ScoreManager : MonoBehaviour
{
    //public List<Tiers> tiersList;
    public Text scoreText;
    public int score = 0;
    


    public void AddScore()
    {
        for (int i=0; i < Tiers.tierInstance.scoreInfos.Count; i++)
        {
            
            if((CountDown.instance.GetTime() >= Tiers.tierInstance.scoreInfos[i].minTime) && (CountDown.instance.GetTime() <= Tiers.tierInstance.scoreInfos[i].maxTime))
            {
                score += Tiers.tierInstance.scoreInfos[i].score;
                scoreText.text = string.Format("Score: {0}", score);
            }
        }
    }
     public void AddScore(int addedScore)
    {
        score += addedScore;
        scoreText.text = string.Format("Score: {0}", score);
    }

    public void RemoveScore(int removedScore)
    {
        if (score - removedScore > 0)
        {
            score -= removedScore;
            
        }
        else
            score = 0;
        scoreText.text = string.Format("Score: {0}", score);
    }




    /* public void AddScore()
     {
         score += tiers.userInfos
        // float time = CountDown.instance.GetTime();
         //Debug.Log(gameObject.name + " " + time);
     }*/


}
