
using UnityEngine;

public class TrackClick : MonoBehaviour
{
    public static int totalclicks = 0;
    public KeyCode mouseclick; //asign mouseclick to Mouse0 in Unity (Left click)
    public ScoreManager scoreManager;
    public int penaltyValue;
    public StartTheGame startTheGame;
    

   
    void Update()
    {
        if (Input.touchCount > 0 )
            
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began && startTheGame.PanelEnableTest() == false && Client.instance.PausedPanelExist() ==false)
            {
                totalclicks += 1; //increase total clicks by 1 if the left button is clicked

                Debug.Log(string.Format("votre total de clique est {0}", totalclicks));
            }
            

            if (totalclicks > 3 && touch.phase == TouchPhase.Ended) //si on dépasse 3cliques
            {

                /* if (scoreManager.score == 0)//si le score est déjà à 0 la team ne reçoit pas de pénalité juste le reset des clicks
                 {
                     //totalclicks = 0;//reset totalclicks
                     Debug.Log(string.Format("votre total de clique est reset à {0}", totalclicks));
                 }*/

                if (scoreManager.score - penaltyValue >= 0)//si le score est différent de 0 (supérieur à 0)
                {
                    scoreManager.score = scoreManager.score - penaltyValue; //pénalité dans leur score
                                                                            
                    scoreManager.scoreText.text = scoreManager.score.ToString();
                    Debug.Log(string.Format("votre total de clique apres penalty{0}", totalclicks));
                }

                else {
                    scoreManager.score = 0;
                    scoreManager.scoreText.text = scoreManager.score.ToString();
                }
                    
                

            }
        }

        
        
    }
}
