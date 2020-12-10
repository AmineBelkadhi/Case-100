using UnityEngine;
using TMPro;
using CaseNetworking;

public class RemoveTimeView : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField timeValueField;

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        OperationPanelLock.OnClose();
    }

    public void RemoveTimePanel()
    {
        if (ValidTimeField())
        {
            InfoTextView.instance.SetInfo(string.Format("Removed {0} time", timeValueField.text));
            Server.instance.SendRemoveTimeMessage(TeamManagement.instance.GetActiveConnectionIds(), int.Parse(timeValueField.text));
            TimeManagement.instance.countDown.RemoveTime(int.Parse(timeValueField.text));
            CaseLogs.instance.AddToLog(string.Format("Removed {0} time", timeValueField.text));
            timeValueField.text = "";
            ClosePanel();
        }
    }

    private bool ValidTimeField()
    {
        int value = int.Parse(timeValueField.text);

        if (value > 0)
        {
            return true;
        }
        return false;
    }
}