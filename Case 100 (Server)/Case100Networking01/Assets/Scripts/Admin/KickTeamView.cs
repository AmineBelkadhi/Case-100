using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KickTeamView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI kickText;

    [SerializeField]
    private Button yesButton;

    [SerializeField]
    private Button noButton;

    public void EnableKickPanel(int teamNb, int connectionId)
    {
        if (!OperationPanelLock.operationPanelOn)
        {
            kickText.text = "Are you sure you want to kick team " + teamNb.ToString() + "?";
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(() => OnKick(teamNb, connectionId));
            OperationPanelLock.OnOpen();
            gameObject.SetActive(true);
        }
    }

    public void OnNoClick()
    {
        gameObject.SetActive(false);
        OperationPanelLock.OnClose();
    }

    private void OnKick(int teamNb, int connectionId)
    {
        gameObject.SetActive(false);
        Server.instance.SendKickMessage(connectionId);
        InfoTextView.instance.SetInfo(string.Format("Kicked team {0} from the game", teamNb));
        Server.instance.KickButton(connectionId);
        TeamManagement.instance.OnKick(teamNb);
        OperationPanelLock.OnClose();
    }
}