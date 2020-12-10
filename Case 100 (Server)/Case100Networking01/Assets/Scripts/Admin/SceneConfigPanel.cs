using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneConfigPanel : MonoBehaviour
{
    public Scene scene;

    public Image sceneImage;
    public Transform panel;
    public TextMeshProUGUI sceneNameText;

    public GameObject itemButton;

    // public List<GameObject> itemButtons;

    public GameObject forwardButton;
    public GameObject backwardButton;

    public TMP_InputField timeField;
    public TMP_InputField scorePerItemField;

    public void Init()
    {
        sceneImage.sprite = scene.image;

        for (int i = 0; i < scene.items.Count; i++)
        {
            //Instantiate button
            //Add Item component to it
            //add a listener to it
            GameObject go = Instantiate(itemButton, panel);
            ItemView item = go.AddComponent<ItemView>();
            item.id = scene.items[i].id;
            item.scene = scene;
        }
    }

    private void TimeValueChangeCheck()
    {
        scene.timePerItem = int.Parse(timeField.text);
    }

    private void ScoreValueChangeCheck()
    {
        scene.scorePerItem = int.Parse(scorePerItemField.text);
    }

    public void Init(Scene scene)
    {
        scene.timePerItem = int.Parse(timeField.text);
        scene.scorePerItem = int.Parse(scorePerItemField.text);

        timeField.onValueChanged.AddListener(delegate
        { TimeValueChangeCheck(); });

        scorePerItemField.onValueChanged.AddListener(delegate
        { ScoreValueChangeCheck(); });

        sceneImage.sprite = scene.image;
        sceneNameText.text = scene.sceneName;
        for (int i = 0; i < scene.items.Count; i++)
        {
            GameObject go = Instantiate(itemButton, panel);
            ItemView item = go.AddComponent<ItemView>();
            item.id = scene.items[i].id;
            item.scene = scene;
            item.GetComponent<Image>().sprite = scene.items[i].image;
        }
    }

    public void OnClickForward()
    {
        if (GameConfigManager.instance.currentScenePanelIndex < GameConfigManager.instance.addedScenes.Count - 1)
        {
            GameConfigManager.instance.currentScenePanelIndex++;
            GameConfigManager.instance.EnablePanelAtIndex(GameConfigManager.instance.currentScenePanelIndex);
        }
    }

    public void OnClickBackwards()
    {
        if (GameConfigManager.instance.currentScenePanelIndex > 0)
        {
            GameConfigManager.instance.currentScenePanelIndex--;
            GameConfigManager.instance.EnablePanelAtIndex(GameConfigManager.instance.currentScenePanelIndex);
        }
    }

    public void OnTimeModification()
    {
        GameConfigManager.instance.CheckStartServerConditions();
    }

    public void OnScorePerItemModification()
    {
        GameConfigManager.instance.CheckStartServerConditions();
    }
}