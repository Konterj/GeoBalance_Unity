using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Ui_State : MonoBehaviour
{
    public List<ElementPanel> panel;
    

}

[System.Serializable]
public class ElementPanel
{
    public string name;
    public GameObject panel;
    public CanvasGroup canvasGroup;
    public float originalAlphaChannel;

    public void OnSetDefaultValue()
    {
        if(canvasGroup == null)
        {
            panel.AddComponent<CanvasGroup>();
            canvasGroup = panel.GetComponent<CanvasGroup>();
            originalAlphaChannel = canvasGroup.alpha;
        }
    }
}