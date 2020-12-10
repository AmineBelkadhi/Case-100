using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private GameObject configPanel;

    public void OnClickStartNewServer()
    {
        mainMenuPanel.SetActive(false);
        configPanel.SetActive(true);
    }

    public void OnClickLoadExistingGame()
    {
    }
}