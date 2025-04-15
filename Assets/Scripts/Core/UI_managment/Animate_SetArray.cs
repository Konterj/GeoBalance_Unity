using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate_SetArray : MonoBehaviour
{
    public string Name;
    public List<ElementAnimate> animate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class ElementsAnimate
{
    [Header("Animation Settings")]
    [SerializeField] public List<ElementAnimate> elementAnim;
    public List<ElementAnimate> Element { get => elementAnim; set => elementAnim = value; }
    public Panel_Ui_State state;
}
