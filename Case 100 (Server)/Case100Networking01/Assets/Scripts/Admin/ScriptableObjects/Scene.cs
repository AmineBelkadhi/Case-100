using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Scene")]
public class Scene : ScriptableObject
{
    public string sceneName;
    public int id;
    public Sprite image;
    public List<Item> items;
    public List<int> selectedItemIds = new List<int>();

    public int scorePerItem;
    public int timePerItem;

    [SerializeField]
    private bool predefined;

    public void OnEnable()
    {
        if (!predefined)
        {
            selectedItemIds.Clear();
        }
    }

    public void Reset()
    {
        selectedItemIds.Clear();
    }
}