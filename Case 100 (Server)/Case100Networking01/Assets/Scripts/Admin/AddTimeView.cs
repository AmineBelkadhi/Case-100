using UnityEngine;
using TMPro;
using CaseNetworking;

public class AddTimeView : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField timeValueField;

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        OperationPanelLock.OnClose();
    }

    public void AddTimeButton()
    {
        if (ValidTimeField())
        {
            InfoTextView.instance.SetInfo(string.Format("Added {0} time", timeValueField.text));
            Server.instance.SendAddTimeMessage(TeamManagement.instance.GetActiveConnectionIds(), int.Parse(timeValueField.text));
            TimeManagement.instance.countDown.AddTime(int.Parse(timeValueField.text));
            CaseLogs.instance.AddToLog(string.Format("Added {0} time", timeValueField.text));
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