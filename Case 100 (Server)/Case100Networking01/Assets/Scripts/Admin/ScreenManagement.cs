using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenManagement : MonoBehaviour
{
    public static ScreenManagement instance;

    [SerializeField]
    private Image screenImage;

    [Header("Screen Image Sprites")]
    [SerializeField]
    private Sprite screenOnSprite;

    [SerializeField]
    private Sprite screenOffSprite;

    public TextMeshProUGUI screenConnectionText;

    private int connectionId = -1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Server.instance.clientDisconnectionEvent += OnScreenDisconnected;
        screenImage.color = Color.red;
    }

    public void OnScreenConnected(int id)
    {
        SetConnectionId(id);
        screenImage.color = Color.green;

        if (screenConnectionText != null)
            screenConnectionText.text = "Screen Connected!";
        InfoTextView.instance.SetInfo("Established connection with the screen APP");
    }

    public void OnScreenDisconnected(int id)
    {
        if (id == connectionId)
        {
            SetConnectionId(-1);
            screenImage.color = Color.red;

            if (screenConnectionText != null)
                screenConnectionText.text = "Screen Not Connected!";

            InfoTextView.instance.SetInfo("Connection lost with the screen APP");
        }
    }

    public void SetConnectionId(int id)
    {
        connectionId = id;
    }

    public int GetScreenConnectionId()
    {
        return connectionId;
    }

    public bool screenConnected()
    {
        return connectionId != -1;
    }
}