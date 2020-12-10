using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Params
{
    public int nbTeam;
    public int sceneId;
    public List<int> itemIds;
    public int sceneTime;

    public Params()
    {
        nbTeam = 0;
        sceneId = 0;
        itemIds = new List<int>();
        sceneTime = 0;
    }
}