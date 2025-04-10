using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureSetting : MonoBehaviour
{
    //Public Color colorest figures;

    public void OnRandomMassAndSize(ref Vector3 SizeFigure, List<Transform> figure, ref float Scale,ref float Mass)
    {
        float massFactor = 100f;
        Scale = Mathf.Round(Scale * massFactor) / 100f;
        SizeFigure = new Vector3(Scale, Scale, 0f);
        //Mass
        Mass = Mathf.Round(Scale * massFactor) / 10f;
        Debug.Log($"This script, FigureSetting OnRandomMAssSize. Scale: {Scale}, Mass: {Mass}");
    }
    /*public virtual void OnRandomColor()
    {

    }*/
}
