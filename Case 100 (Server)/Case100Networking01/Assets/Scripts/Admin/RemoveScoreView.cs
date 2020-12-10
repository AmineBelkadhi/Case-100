using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CaseNetworking;

public class RemoveScoreView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI teamNbText;

    [SerializeField]
    private TMP_InputField scoreValueField;

    [SerializeField]
    private Button removeScoreButton;

    public void EnableScoreView(int teamNb, int connectionId)
    {
        if (!OperationPanelLock.operationPanelOn)
        {
            teamNbText.text = teamNb.ToString();
            gameObject.SetActive(true);
            removeScoreButton.onClick.RemoveAllListeners();
            removeScoreButton.onClick.AddListener(() => OnRemoveScore(teamNb, connectionId, UpdateType.remove));
            OperationPanelLock.OnOpen();
        }
    }

    public void DisableRemoveScorePanel()
    {
        gameObject.SetActive(false);
        OperationPanelLock.OnClose();
    }

    public void OnRemoveScore(int teamNb, int connectionId, UpdateType updateType)
    {
        int value = int.Parse(scoreValueField.text);
        if (value <= 0)
        {
            Debug.Log("You're trying to Remove 0 or negative score homie");
        }
        else
        {
            Debug.Log(string.Format("Checking then removing from team {0} {1} score",
             teamNb, value));
            gameObject.SetActive(false);
            Server.instance.SendRemoveScoreMessage(connectionId, value);
            InfoTextView.instance.SetInfo(string.Format("Remove {0} score from team {1}",
             teamNb, value));
            OperationPanelLock.OnClose();

            TeamView view = TeamManagement.instance.TeamNumberToTeamView(teamNb);
            if (view != null)
            {
                view.UpdateScore(value, updateType);
            }

            CaseLogs.instance.AddToLog(string.Format("Removed {0} score from team {1}", value, teamNb));

            scoreValueField.text = "";
        }
    }
}