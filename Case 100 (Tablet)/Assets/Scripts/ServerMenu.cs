using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CaseNetworking;
using TMPro;

public class ServerMenu : MonoBehaviour
{


    public GameObject buttonPrefab;
    public GameObject disabledMenuPanel;
    public GameObject panel;
    public TeamMenu teamMenu;




    public void ShowServers()
    {
        StartCoroutine(Wait());
    }




    IEnumerator Wait()
    {
        disabledMenuPanel.gameObject.SetActive(false); //disable Join button
        

        yield return new WaitForSeconds(1);

        panel.gameObject.SetActive(true); //enable server panel

        
        

    }

    public void instantiateServerButton(string svIP)
    {
       GameObject btnGo = Instantiate(buttonPrefab, panel.transform);
        Button btn = btnGo.GetComponent<Button>();
        btn.onClick.AddListener(() => Client.instance.AttemptConnection(svIP, CaseNet_Config.gamePort));
        btn.onClick.AddListener(() => teamMenu.ShowTeams());
        btnGo.GetComponentInChildren<TextMeshProUGUI>().text = svIP;
    }
}