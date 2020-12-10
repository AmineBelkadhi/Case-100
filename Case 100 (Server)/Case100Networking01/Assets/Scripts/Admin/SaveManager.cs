using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public GameObject administrationMenu;
    public GameObject loadPanel;
    public GameObject mainMenu;

    public void Load()
    {
        loadPanel.SetActive(false);
        mainMenu.SetActive(false);
        administrationMenu.SetActive(true);
    }

    public void ClickLoad()
    {
        loadPanel.SetActive(true);
    }
}