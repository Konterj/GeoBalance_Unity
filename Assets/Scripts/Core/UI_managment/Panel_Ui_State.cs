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
    public string Name;
    public List<ElementAnimate> Animate;
    public GameObject SetPanelActive;
    public enum SetActive { ActivePanelOn, ActivePanelOff };
    public SetActive setAnim;

}