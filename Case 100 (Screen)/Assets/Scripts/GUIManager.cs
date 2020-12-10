using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public CanvasGroup myCanvas;
    public float FadeDuration=15f;

    void Start()
    {
        StartCoroutine(Fade.FadeCanvas(myCanvas, 1f, 0f, FadeDuration));
    }




}
