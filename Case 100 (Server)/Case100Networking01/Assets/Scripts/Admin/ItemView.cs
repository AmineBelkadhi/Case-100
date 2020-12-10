using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    public int id;
    public Scene scene;

    public void Awake()
    {
        gameObject.GetComponent<Toggle>().onValueChanged.AddListener((on) => { onToggle(on); });
    }

    public void onToggle(bool on)
    {
        if (on)
        {
            scene.selectedItemIds.Add(id);
            GameConfigManager.instance.CheckStartServerConditions();
        }
        else
        {
            scene.selectedItemIds.Remove(id);
            GameConfigManager.instance.CheckStartServerConditions();
        }
    }
}