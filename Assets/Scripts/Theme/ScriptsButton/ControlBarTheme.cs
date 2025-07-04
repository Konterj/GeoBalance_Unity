using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlBarTheme : MonoBehaviour
{
    public enum Theme { Default, Dark, Milk }
    [Header("Bar Settings")]
    public float DurationSmooth = 1f;
    [Header("Button Slider_Bar")]
    public Button ButtonInBar;

    public int value = 0;

    public Theme theme;
    private Coroutine CurrentCorountine;
    Vector3 posTemp = new(-47, 0);
    void Start()
    {
        ButtonInBar.onClick.AddListener(OnMovedPosBar);
        CurrentCorountine = StartCoroutine(OnPosMove(posTemp));
    }
    private void Update()
    {
        ApplySelectedTheme();
    }
    private void ApplySelectedTheme()
    {

        switch (theme)
        {
            case Theme.Default:
                posTemp.x = -47f;
                break;
            case Theme.Dark:
                posTemp.x = 0;
                break;
            case Theme.Milk:
                posTemp.x = 47f;
                break;
        }
    }

    public void OnSelectedValue()
    {
        switch (value) 
        {
            case 0: theme = Theme.Default; break;
            case 1: theme = Theme.Dark; break;
            case 2: theme = Theme.Milk; break;
        }
    }

    public void OnMovedPosBar()
    {
        value = (value + 1) % 3;
        if (CurrentCorountine != null)
        {
            StopCoroutine(CurrentCorountine);
        }
        CurrentCorountine = StartCoroutine(OnPosMove(posTemp));;
        OnSelectedValue();
    }
    private IEnumerator OnPosMove(Vector3 position)
    {
        Debug.Log("StartPos: " + ButtonInBar.transform.localPosition);
        float t_timer = 0f;
        while (t_timer < 1)
        {
            t_timer += Time.deltaTime / DurationSmooth;
            ButtonInBar.transform.localPosition = Vector3.Lerp(ButtonInBar.transform.localPosition, position, Mathf.Clamp01(t_timer));
            yield return null;
        }
        Debug.Log("StartPos: " + ButtonInBar.transform.localPosition.x + " Position: " + position);
        ButtonInBar.transform.localPosition = position;
        CurrentCorountine = null;
    }
}
