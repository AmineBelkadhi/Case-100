using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public int id;
    public string nom;
    public ScoreManager score;
    //public TotalItem totalItem;
    public static Items itemInstance;
    public StartTheGame startTheGame;
    

    private void Awake()
    {
        itemInstance = this;
    }




    //public void OnMouseDown()
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Vector2 touchPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y);
            RaycastHit2D hit= Physics2D.Raycast(touchPos,Vector2.zero, 100f);
        //Debug.Log(hit.transform.name);
            Touch touch = Input.GetTouch(0);

            if (hit.transform != null)
            {

                if (startTheGame.activeId == hit.transform.GetComponent<Items>().id && CountDown.instance.currentTime > 0 && Client.instance.PausedPanelExist() == false)
                {
                    if (touch.phase == TouchPhase.Ended)
                    {



                        hit.transform.gameObject.SetActive(false); //delete the selected object
                                                                   // startTheGame.idList.Remove(startTheGame.idList[startTheGame.index]); //supprimer l'id qu'index a déjà pris
                                                                   // startTheGame.itemsList.RemoveAt(startTheGame.index);



                        TrackClick.totalclicks = 0; //set total of clicks to 0 after a successful click

                        Debug.Log(string.Format("totalclick est à {0} apres avoir trouvé l objet", TrackClick.totalclicks));

                        score.AddScore();
                        Client.instance.SendFoundItemMessage(startTheGame.activeId, score.score);
                    }

                }
            }
        }
        
        

    }


}
