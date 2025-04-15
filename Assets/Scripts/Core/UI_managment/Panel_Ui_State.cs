using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Ui_State : MonoBehaviour
{
    public List<ElementPanel> panel;

    private ElementPanel currentPanel;

    public void OnSetActivePanel()
    {
        //First
        if (currentPanel.setActiveFirst == ElementPanel.SetActive.ActivePanelOn)
        {
            currentPanel.FirstPanel.SetActive(true);
        }
        else if (currentPanel.setActiveFirst == ElementPanel.SetActive.ActivePanelOff) 
        {
            currentPanel.FirstPanel.SetActive(false);
        }
        //Last
        if (currentPanel.setActiveLast == ElementPanel.SetActive.ActivePanelOn) 
        {
            currentPanel.LastPanel.SetActive(true);
        }
        else if(currentPanel.setActiveLast == ElementPanel.SetActive.ActivePanelOff)
        {
            currentPanel.LastPanel.SetActive(false);
        }
    }

    public void OnSetNameElement(string name)
    {
       currentPanel = panel.Find(p => p.Name == name);
    }
}

[System.Serializable]
public class ElementPanel
{
    public string Name;
    public GameObject FirstPanel;
    public GameObject LastPanel; 
    public enum SetActive { ActivePanelOn, ActivePanelOff };
    public SetActive setActiveFirst;
    public SetActive setActiveLast;
}