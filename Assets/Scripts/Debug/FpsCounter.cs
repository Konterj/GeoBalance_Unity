using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI textUGUI;
    float fps;
    // Update is called once per frame
    void Update()
    {
        OnCalcueFps();
        OnDisplayShowFPs();
    }
    void OnCalcueFps()
    {
         fps = (int)(1f / Time.unscaledDeltaTime);
    }

    void OnDisplayShowFPs()
    {
        textUGUI.text = $"Fps: {fps}";
    }
}
