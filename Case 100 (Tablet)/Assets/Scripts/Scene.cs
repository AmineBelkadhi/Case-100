using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Scene")]
public class Scene : ScriptableObject
{
    public string sceneName;
    public int id;
    public Sprite image;
    public List<Items> items;
    public SceneParams sceneParams;

    public void OnEnable()
    {
        sceneParams = new SceneParams();
    }


}
