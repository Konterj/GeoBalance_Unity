using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDeleteLastFigures : MonoBehaviour
{
    public SpawnFigures settingFigure;
    public float DurationTransition = 1;

    public void OnStartDeleteFigures()
    {
        OnSearchFindFigures();
    }
    public void OnSearchFindFigures()
    {
        if (settingFigure.Figures.Count > 0)
        {
            {
                Debug.Log("All find for transition");
                OnAlphaTransition();
            }
        }
    }
    public void OnAlphaTransition()
    {
        StartCoroutine(AlphaTransition());
    }
    public void OnDeleteGameObjects()
    {

        for (int i = settingFigure.Figures.Count - 1; i >= 0; i--) 
        {
            if (settingFigure.Figures[i] != null) 
            {
                Destroy(settingFigure.Figures[i].gameObject);
            }
        }
        settingFigure.Figures.Clear();
        Debug.Log("Delete figures");
    }

    IEnumerator AlphaTransition()
    {
        float T_time = 0f;
        var SpriteRender = new List<SpriteRenderer>();
        foreach (var sprite in settingFigure.Figures)
        {
            if (sprite != null)
            {
                SpriteRender.Add(sprite.GetComponent<SpriteRenderer>());
            }
        }

        while (T_time <= 1f) 
        {
            T_time += TimeDeltaControl.AnimDeltaTime / DurationTransition;
            
            foreach (var Fig in SpriteRender)
            {
                if (Fig != null) 
                {
                    Color CurrentColor = Fig.color;
                    CurrentColor.a = Mathf.Lerp(1f, 0f, T_time);
                    Fig.color = CurrentColor;
                    Debug.Log("Transition alpha: " + Fig.color.a);
                }
            }
            yield return null;
        }
        OnDeleteGameObjects();
    }
}
