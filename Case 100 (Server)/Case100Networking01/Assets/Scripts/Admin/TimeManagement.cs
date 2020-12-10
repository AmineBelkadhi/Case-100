using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TimeManagement : MonoBehaviour
{
    public static TimeManagement instance;

    [Header("Panels")]
    [SerializeField]
    private GameObject addTimePanel;

    [SerializeField]
    private GameObject removeTimePanel;

    [Header("Texts")]
    public TextMeshProUGUI countDownText;

    public Timer countDown;

    [Header("Buttons")]
    [SerializeField]
    private Button addTimeButton;

    [SerializeField]
    private Button removeTimeButton;

    private void Awake()
    {
        instance = this;
    }

    public void OnCountDownUpdate()
    {
        countDownText.text = countDown.CurrentTimeFormat();
    }

    public void InitTimer(int duration, Action onTimeUp)
    {
        countDown = new Timer(duration, onTimeUp, () => OnCountDownUpdate());
        StopAllCoroutines();
        StartCoroutine(countDown.StartCountDown());
    }

    public void StartCountDown()
    {
        if (countDown != null)
        {
            StopAllCoroutines();
            StartCoroutine(countDown.StartCountDown());
        }
    }

    public void OnClickAddTime()
    {
        if (!OperationPanelLock.operationPanelOn)
        {
            addTimePanel.SetActive(true);
            OperationPanelLock.OnOpen();
        }
    }

    public void OnClickRemoveTime()
    {
        if (!OperationPanelLock.operationPanelOn)
        {
            removeTimePanel.SetActive(true);
            OperationPanelLock.OnOpen();
        }
    }

    public void OnClickPauseTime()
    {
        addTimePanel.SetActive(false);
        removeTimePanel.SetActive(false);
    }

    public void SetTimeManagementToNotInteractable()
    {
        addTimeButton.interactable = false;
        removeTimeButton.interactable = false;
    }

    public void SetTimeManagementToInteractable()
    {
        addTimeButton.interactable = true;
        removeTimeButton.interactable = true;
    }
}