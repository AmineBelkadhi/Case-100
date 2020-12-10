using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameConfigManager : MonoBehaviour
{
    public GameConfigData gameData;
    public TMP_InputField teamNbField;
    public List<SceneView> sceneViews;
    public ToggleGroup settingsToggle;

    private DataOptionView _option;
    private Toggle _toggle;

    public static GameConfigManager instance;
    public TextMeshProUGUI errorMessage;

    public Transform parentPanel;

    [Header("Main Panels")]
    public GameObject scenesConfigPanel;

    public GameObject sceneSelectionPanel;
    public GameObject sceneConfigPrefab;
    public GameObject administrationPanel;

    public List<SceneConfigPanel> addedScenes = new List<SceneConfigPanel>();

    public Button startServerButton;

    [HideInInspector]
    public int currentScenePanelIndex = 0;

    [HideInInspector]
    public GameObject currentPane;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetConfig();
    }

    public void OnTeamFieldChanged(string value)
    {
        gameData.nbTeams = int.Parse(value);
    }

    public void SetConfig()
    {
        //ResetSettings();
        foreach (Toggle tog in settingsToggle.ActiveToggles())
        {
            _option = tog.GetComponent<DataOptionView>();
        }

        if (_option != null)
        {
            Debug.Log("after if");
            teamNbField.text = _option.optionGameData.nbTeams.ToString();

            foreach (Scene sc in _option.optionGameData.selectedScenes)
            {
                Debug.Log("foreach in");
                for (int i = 0; i < sceneViews.Count; i++)
                {
                    if (sc == sceneViews[i].scene)
                    {
                        Debug.Log("for in");
                        sceneViews[i].GetComponent<Toggle>().isOn = true;
                    }
                }
            }
        }
    }

    public void SetGameConfig(GameConfigData data)
    {
        foreach (Scene sc in data.selectedScenes)
        {
            foreach (SceneView view in sceneViews)
            {
                if (sc == view.scene)
                {
                    view.GetComponent<Toggle>().isOn = true;
                }
            }
        }

        SetTeamNumbers(data.nbTeams);
    }

    public void SetTeamNumbers(int value)
    {
        teamNbField.text = value.ToString();
        gameData.nbTeams = value;
    }

    public void SetDefaultConfig(GameConfigData data)
    {
        UnselectAllSceneViews();
        SetGameConfig(data);
        DisableScenesInteractables();
    }

    public void SetCustomSettings()
    {
        UnselectAllSceneViews();
        EnableScenesInteractables();
    }

    public void DisableScenesInteractables()
    {
        for (int i = 0; i < sceneViews.Count; i++)
        {
            _toggle = sceneViews[i].GetComponent<Toggle>();
            _toggle.interactable = false;
        }
    }

    public void EnableScenesInteractables()
    {
        for (int i = 0; i < sceneViews.Count; i++)
        {
            _toggle = sceneViews[i].GetComponent<Toggle>();
            _toggle.interactable = true;
        }
    }

    public void UnselectAllSceneViews()
    {
        foreach (SceneView sc in sceneViews)
        {
            sc.GetComponent<Toggle>().isOn = false;
        }
    }

    public bool VerifyFields()
    {
        if (gameData.selectedScenes.Count <= 0)
        {
            errorMessage.text = "You should select atleast one scene";
            return false;
        }
        else if (int.Parse(teamNbField.text) <= 0)
        {
            errorMessage.text = "You should have atleast 1 or more teams";
            return false;
        }

        return true;
    }

    public void OnContinueClick()
    {
        if (VerifyFields())
        {
            sceneSelectionPanel.SetActive(false);
            scenesConfigPanel.gameObject.SetActive(true);

            for (int i = 0; i < gameData.selectedScenes.Count; i++)
            {
                GameObject panel = Instantiate(sceneConfigPrefab, parentPanel);
                SceneConfigPanel cPaenl = panel.GetComponent<SceneConfigPanel>();
                cPaenl.scene = gameData.selectedScenes[i];
                addedScenes.Add(cPaenl);
                addedScenes[i].Init(gameData.selectedScenes[i]);
            }
        }

        if (addedScenes.Count == 1)
        {
            addedScenes[0].backwardButton.SetActive(false);
            addedScenes[0].forwardButton.SetActive(false);
        }
        else if (addedScenes.Count == 2)
        {
            addedScenes[0].backwardButton.SetActive(false);
            addedScenes[1].forwardButton.SetActive(false);
        }
        else if (addedScenes.Count > 2)
        {
            addedScenes[0].backwardButton.SetActive(false);
            addedScenes[addedScenes.Count - 1].forwardButton.SetActive(false);
        }

        addedScenes[0].gameObject.SetActive(true);
        currentScenePanelIndex = 0;
        currentPane = addedScenes[0].gameObject;
        EnablePanelAtIndex(0);

        for (int i = 0; i < addedScenes.Count; i++)
        {
            Debug.Log(addedScenes[i].scene.name);
        }
    }

    public void DisableCurrentPanel()
    {
        currentPane.SetActive(false);
    }

    public void EnablePanelAtIndex(int index)
    {
        currentPane.SetActive(false);
        addedScenes[index].gameObject.SetActive(true);
        currentPane = addedScenes[index].gameObject;
    }

    public bool CheckStartServerConditions()
    {
        bool ok = true;

        for (int i = 0; i < addedScenes.Count; i++)
        {
            if (addedScenes[i].scene.selectedItemIds.Count <= 0)
            {
                Debug.Log(addedScenes[i].scene.sceneName + " doesn't have shit selected");

                ok = false;
            }
            else if (int.Parse(addedScenes[i].timeField.text) <= 0 || addedScenes[i].timeField.text == string.Empty)
            {
                Debug.Log("Gotta give it some time homie");
                ok = false;
            }
            else if (int.Parse(addedScenes[i].scorePerItemField.text) < 0 || addedScenes[i].scorePerItemField.text == string.Empty)
            {
                Debug.Log("Gotta give sum positive score homie ");
                ok = false;
            }
        }

        if (ok)
        {
            startServerButton.interactable = true;
            return true;
        }
        else
        {
            startServerButton.interactable = false;
            return false;
        }
    }

    public void Reset()
    {
        sceneSelectionPanel.SetActive(true);
        errorMessage.text = "";
        scenesConfigPanel.SetActive(false);
        administrationPanel.SetActive(false);
        UnselectAllSceneViews();

        for (int i = 0; i < addedScenes.Count; i++)
        {
            addedScenes[i].scene.Reset();
        }

        gameData.Reset();

        addedScenes.Clear();
        foreach (Transform child in parentPanel)
        {
            Destroy(child.gameObject);
        }
    }

    public void InitialPanelsSetup()
    {
        Reset();
        //Add more functionality LATA
    }

    public void OnClickStartServer()
    {
        //to be moved
        gameData.nbTeams = int.Parse(teamNbField.text);

        //manage Panels
        administrationPanel.SetActive(true);
        scenesConfigPanel.SetActive(false);
        startServerButton.interactable = false;
        currentPane.SetActive(false);
    }
}