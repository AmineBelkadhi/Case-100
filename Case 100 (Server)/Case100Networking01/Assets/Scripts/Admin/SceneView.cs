using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneView : MonoBehaviour
{
    public Scene scene;

    private void Awake()
    {
        GetComponent<Toggle>().onValueChanged.AddListener((on) => { OnToggle(on); });
        GetComponent<Image>().sprite = scene.image;
    }

    public void OnToggle(bool on)
    {
        if (GameConfigManager.instance != null)
        {
            if (on)
            {
                GameConfigManager.instance.gameData.selectedScenes.Add(scene);
            }
            else
            {
                GameConfigManager.instance.gameData.selectedScenes.Remove(scene);
            }
        }
    }
}