using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataOptionView : MonoBehaviour
{
    public GameConfigData optionGameData;

    private OptionsToggle toggle;

    private void Awake()
    {
        toggle = GetComponent<OptionsToggle>();
    }

    public void OnToggleDefault()
    {
        if (toggle.isOn)
        {
            GameConfigManager.instance.SetDefaultConfig(optionGameData);
        }
    }

    public void OnToggleCustom()
    {
        if (toggle.isOn)
        {
            GameConfigManager.instance.SetCustomSettings();
        }
    }
}