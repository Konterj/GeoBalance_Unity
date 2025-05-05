using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeCurve : MonoBehaviour
{
    [SerializeField] public List<Themes> Themes;
    //private
    float T_Time;


    void Start()
    {
        
    }

    public void OnSetElementTheme()
    {

    }

    public void OnGetOriginalTheme()
    {

    }

    IEnumerator ThemeChange()
    {

        yield return null;
    }
}

[System.Serializable]

public class Themes
{
    public string name;
    public string description;
    public List<Color> color;
    public List<GameObject> UIObjectForTheme;
    [SerializeField] public AnimationCurve ThemeCurveTransition;
    [SerializeField] public float DurationThemes;
    public bool IsParralelTransition;

}