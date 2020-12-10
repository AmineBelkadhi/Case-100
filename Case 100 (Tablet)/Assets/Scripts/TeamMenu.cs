using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TeamMenu : MonoBehaviour
{


    public GameObject disabledPanel; //server panel
    public GameObject enablePanel; //teams panel
    public GameObject buttonPrefab; // prefab des boutons à spawn
    public List<Button> teamButtons =  new List<Button>(); //list de boutons des teams
    



    public void ShowTeams()
    {
        StartCoroutine(Wait());      
    }

    public void instantiateTeamButtons(List<int> teamNbs)
    {
        foreach(int i in teamNbs)
        {
            GameObject btnGo = Instantiate(buttonPrefab, enablePanel.transform);
            Button btn = btnGo.GetComponent<Button>();
            teamButtons.Add(btn);
            btn.onClick.AddListener(() => Client.instance.SendJoinRequestMessage(i));
            btnGo.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
        }
    }

    public void ClearButtons()
    {
        for(int i= 0; i< teamButtons.Count; i++)
        {
            Button btn = teamButtons[i];
            Destroy(teamButtons[i].gameObject);

        }
        teamButtons.Clear();
    }

    IEnumerator Wait()
    {
        

        yield return new WaitForSeconds(1);

        disabledPanel.gameObject.SetActive(false); //disable server panel
        enablePanel.gameObject.SetActive(true); //enable team panel

        

    }
}
