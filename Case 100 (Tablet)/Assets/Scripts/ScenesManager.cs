using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesManager : MonoBehaviour
{

    public static ScenesManager instance;

    public List<Scene> scenes;
    public List<Scene> selectedScenes = new List<Scene>();
    public SceneObject activeScene;
    public List<SceneObject> sceneObjects;


    private void Awake()
    {
        instance = this;
    }

    public Scene getSceneById(int id)
    {
        return scenes.Find(scene => scene.id == id);

    }

    public void AddScene(int id, List<int> itemIds, int timePerItem,int scorePerItem)
    {
        
        selectedScenes.Add(getSceneById(id));
        getSceneById(id).sceneParams.selectedItemIds = itemIds;
        getSceneById(id).sceneParams.scorePerItem = scorePerItem;
        getSceneById(id).sceneParams.timePerItem = timePerItem;
        Debug.Log(getSceneById(id).sceneParams.scorePerItem);

    }

  
    public void ActivateScene (int id)
    {
        SceneObject sObject = sceneObjects.Find(o => o.scene.id == id);
     
        if (sObject!= null)
        {
            Debug.Log("aya winek" +id);
            activeScene.gameObject.SetActive(false);
            sObject.gameObject.SetActive(true);
            activeScene = sObject;
        }
    }
}
