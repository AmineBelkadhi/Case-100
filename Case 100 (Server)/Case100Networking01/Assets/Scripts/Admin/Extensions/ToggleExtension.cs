using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleExtension : Toggle
{
    [SerializeField]
    public GameConfigData GameData { get; set; }
}