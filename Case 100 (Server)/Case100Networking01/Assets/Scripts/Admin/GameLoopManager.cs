using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameLoopManager : MonoBehaviour
{
    public static GameLoopManager instance;

    [SerializeField]
    private Scene currentScene;

    [SerializeField]
    private int currentItemId;

    [SerializeField]
    private int currentItemIndex;

    [SerializeField]
    private TextMeshProUGUI currentSceneText;

    [SerializeField]
    private int currentSceneIndex;

    [SerializeField]
    private int timePerItem;

    [SerializeField]
    private int scorePerItem;

    public void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        //first selected scene and it's first Item
        currentItemIndex = 0;
        currentSceneIndex = 0;
        currentScene = GameConfigManager.instance.gameData.selectedScenes[0];
        currentItemId = currentScene.selectedItemIds[0];
        scorePerItem = currentScene.scorePerItem;
        timePerItem = currentScene.timePerItem;

        // TimeManagement.instance.InitTimer(timePerItem, () => MoveToNextItem());
    }

    public void SceneTransition(int sceneIndex)
    {
        currentItemIndex = 0;
        currentSceneIndex = sceneIndex;

        currentSceneText.text = currentSceneIndex.ToString();

        currentScene = GameConfigManager.instance.gameData.selectedScenes[currentSceneIndex];
        currentItemId = currentScene.selectedItemIds[currentItemIndex];
        scorePerItem = currentScene.scorePerItem;
        timePerItem = currentScene.timePerItem;
        TimeManagement.instance.InitTimer(timePerItem, () => MoveToNextItem());

        Server.instance.SendItemMessage(TeamManagement.instance.GetActiveConnectionIds(),
                currentItemId);
    }

    public void SetSceneAndItem(Scene scene, int itemId)
    {
        currentScene = scene;
        currentItemId = itemId;
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    public void MoveToNextItem()
    {
        if (currentItemIndex < currentScene.selectedItemIds.Count - 1)
        {
            currentItemIndex++;
            currentItemId = GameConfigManager.instance.gameData.selectedScenes[currentSceneIndex].selectedItemIds[currentItemIndex];
            Server.instance.SendItemMessage(TeamManagement.instance.GetActiveConnectionIds(),
                currentItemId);
            Debug.Log("Next Item");
            TimeManagement.instance.InitTimer(currentScene.timePerItem, () => MoveToNextItem());
        }
        else
        {
            currentItemIndex = 0;
            MoveToNextScene();
        }
    }

    public void MoveToNextScene()
    {
        if (currentSceneIndex < GameConfigManager.instance.gameData.selectedScenes.Count - 1)
        {
            currentSceneIndex++;
            Server.instance.SendSceneMessage(TeamManagement.instance.GetActiveConnectionIds(),
             GameConfigManager.instance.gameData.selectedScenes[currentSceneIndex].id);
            Server.instance.SendSceneMessage(new List<int>() { ScreenManagement.instance.GetScreenConnectionId() },
            GameConfigManager.instance.gameData.selectedScenes[currentSceneIndex].id);
            SceneTransition(currentSceneIndex);
            Debug.Log("Next Scene");
        }
        else
        {
            Debug.Log("Game over homie");
            Server.instance.SendGameOverMessage(TeamManagement.instance.GetActiveConnectionIds());
            Server.instance.SendGameOverMessage(new List<int>() { ScreenManagement.instance.GetScreenConnectionId() });
        }
    }

    public IEnumerator StartGameCoroutine()
    {
        Init();
        Server.instance.SendSceneMessage(TeamManagement.instance.GetActiveConnectionIds(), currentScene.id);
        Server.instance.SendSceneMessage(new List<int>() { ScreenManagement.instance.GetScreenConnectionId() },
            currentScene.id);
        Server.instance.SendItemMessage(TeamManagement.instance.GetActiveConnectionIds(), currentItemId);

        yield return new WaitForSeconds(0.2f);

        Server.instance.SendStartGameMessage(TeamManagement.instance.GetActiveConnectionIds());
        Server.instance.SendStartGameMessage(new List<int>() { ScreenManagement.instance.GetScreenConnectionId() });

        TimeManagement.instance.InitTimer(currentScene.timePerItem, () => MoveToNextItem());
        //TimeManagement.instance.countDown.StartCountDown();
    }
}