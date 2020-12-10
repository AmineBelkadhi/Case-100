using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTheGame : MonoBehaviour
{
    public GameObject disabledBackground;
    public GameObject disabledPanel;
    public GameObject enabledBackground;
    public GameObject enabledTimeScorePanel;
    public GameObject objectToFindPanel;
    public Text objectText;
    public int index;
    public int numObj;
    //public List<int> idList;
    public List<Items> itemsList;
    public int activeId;
    public int currentSceneIndex;
   // public CountDown cDown;//liste composé des classes Items (contient id et nom et l objet)
    public static StartTheGame startTheGame;
    //public IEnumerator coroutine;

    private void Awake()
    {
        startTheGame = this;
    }

    void Start()
    {
        //coroutine = Wait();

        /*  idList.Add(1);
          idList.Add(2);
          idList.Add(3);
          idList.Add(4);
          */

        currentSceneIndex = 0;

        //Debug.Log(index);
    }

    public void OnClickStartTheGame()
    {
        disabledBackground.gameObject.SetActive(false);
        disabledPanel.gameObject.SetActive(false);
        //enabledBackground.gameObject.SetActive(true);
        enabledTimeScorePanel.gameObject.SetActive(true);
       // StartCoroutine(Wait(ScenesManager.instance.selectedScenes[0].items, ScenesManager.instance.selectedScenes[0].sceneParams.selectedItemIds));
      //  Debug.Log(ScenesManager.instance.selectedScenes[0].id);
      //  Debug.Log(ScenesManager.instance.selectedScenes[0].sceneParams.selectedItemIds[0]);
        //CountDown.instance.StartTimer();


       


    }
   /* public void StartScene()
    {
        StartCoroutine(Wait(ScenesManager.instance.selectedScenes[currentSceneIndex].items, ScenesManager.instance.selectedScenes[currentSceneIndex].sceneParams.selectedItemIds));
        Debug.Log(ScenesManager.instance.selectedScenes[currentSceneIndex].id);
        Debug.Log(ScenesManager.instance.selectedScenes[currentSceneIndex].sceneParams.selectedItemIds[0]);
        CountDown.instance.StartTimer();
    }*/
   


    
    public void PopUpWindowDisable()
    {
        objectToFindPanel.gameObject.SetActive(false);
    }

    public bool PanelEnableTest()
    {
        if (objectToFindPanel.activeInHierarchy ==false)
        {
            return false;
        }
        else
        {
            return true;
        }
    }




    public bool TestTime()
    {
        if (CountDown.instance.time == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

   public IEnumerator Wait(List<Items> itemsList, List<int> idList)
    {  
            for (int i = 0; i < ScenesManager.instance.selectedScenes[0].sceneParams.selectedItemIds.Count; i++)
            {
           // if (CountDown.instance.currentTime > 0) {
                // index = Random.Range(0, idList.Count);//index prends un numero de l id random 
                index = i;
                Debug.Log(i);
                Debug.Log(index);
                //Debug.Log(string.Format("l index a pris cette valeur: {0}", index));
                //Debug.Log(string.Format("l'id est {0}", idList[index]));
                objectToFindPanel.gameObject.SetActive(true); //set the "object to find" panel true
                objectText.text = string.Format("Trouver l'objet suivant: {0} ", FindItemById(Client.instance.idItem,itemsList).nom); /*FindItemById(idList[index]).id*/ //champs texte prends le nom de l objet à trouver
                activeId = Client.instance.idItem;
                yield return new WaitForSeconds(CountDown.instance.time);//afficher la panel chaque "time" donné
                TrackClick.totalclicks = 0;
                

              /* if(ObjectExist(idList[index]))
                {
                    idList.Remove(idList[index]);
                }*/
           // }

            if (CountDown.instance.currentTime == 0 && i < numObj)
                
                CountDown.instance.StartTimer();
                    
               
            }
       
             
    }
    public void ItemToFind(int id)
    {
        CountDown.instance.StartTimer();
        objectToFindPanel.gameObject.SetActive(true); //set the "object to find" panel true
        objectText.text = string.Format("Trouver l'objet suivant: {0} ", FindItemById(Client.instance.idItem, itemsList).nom);
        activeId = id;
    }


    /*public int GetIndex()
    {
        return index;
    }*/

   
    public int getId(int id)
    {
        return id;
    }
    public Items FindItemById(int id, List<Items> itemsList)
    {
        return itemsList.Find(x => x.id == id);
    }

}
