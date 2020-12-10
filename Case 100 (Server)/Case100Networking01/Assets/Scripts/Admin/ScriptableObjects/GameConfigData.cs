using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Game Config")]
[System.Serializable]
public class GameConfigData : ScriptableObject
{
    public int nbTeams;
    public List<Scene> selectedScenes = new List<Scene>();
    public bool preDefined;

    private void OnEnable()
    {
        if (!preDefined)
        {
            nbTeams = 0;
            selectedScenes.Clear();
        }
    }

    public void Reset()
    {
        nbTeams = 0;
        selectedScenes.Clear();
    }
}