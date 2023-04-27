using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform progress;
    [SerializeField] private TMP_Text text;

    public float Progress
    {
        get => progressValue;
        set
        {
            if (value is > 1f or < 0f)
                Debug.LogWarning("ProgressBar should use values in range <0,1>!");

            progressValue = value;
            progress.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value * progressWidth);
        }
    }

    public string Text
    {
        get => text.text;
        set => text.text = value;
    }

    private float progressWidth;
    private float progressValue;

    private void Awake()
    {
        progressWidth = progress.rect.width;
    }
}
