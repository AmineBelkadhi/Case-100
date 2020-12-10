using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itName;
    public Sprite image;

    public int GetId()
    {
        return id;
    }
}