using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnFigures : MonoBehaviour
{
    public List<Transform> figures;
    public List<Transform> SpawnPoints;
    public FigureSetting Setting;
    public Vector3 SizeFigures;
    [Header("Settings timer spawn")]
    public float timeMax = 5;
    public float timeMin = 3;
    //private var
    float Times;
    float AvergeValueForScaleMass;
    //Figures Temp var for save
    [SerializeField] public List<Transform> Figures;

    public void OnStartSpawn()
    {
        Times = Random.Range(timeMin, timeMax);
        AvergeValueForScaleMass = Random.Range(0.03f, 0.08f);
        Debug.Log($"This script, SpawnFigure, OnStartSpawn. Value: {AvergeValueForScaleMass}");
    }
    public void OnRandomSpawn()
    {
        //Set Random position
        Setting.OnRandomMassAndSize(ref SizeFigures, figures, ref AvergeValueForScaleMass, ref AvergeValueForScaleMass);
        int SpawnPointRandom = Random.Range(0, SpawnPoints.Count);

        for(int i = 0; i < figures.Count; i++)
        {
            figures[i].transform.position = SpawnPoints[SpawnPointRandom].position;
        }
        //FOr mass while
        foreach(Transform Shape in figures)
        {
            Shape.GetComponent<Rigidbody2D>().mass = AvergeValueForScaleMass;
            Shape.transform.localScale = SizeFigures;
            Debug.Log($"This script, SpawnFigure, OnRadnomSpawn. " +
            $"Scale: {AvergeValueForScaleMass}, Mass: {AvergeValueForScaleMass}, Real Mass figure {Shape.GetComponent<Rigidbody2D>().mass}");
        }


        //Spawn random figures
        int spawnRandomFigure = Random.Range(0, figures.Count);
        var Figure = Instantiate(figures[spawnRandomFigure], SpawnPoints[SpawnPointRandom].transform.position, Quaternion.identity, null);
        Figures.Add(Figure);

    }

    public void OnUpdateTimerSpawn()
    {

        Times -= TimeDeltaControl.ControlDeltaTime;
        if (Times < 0)
        {
            OnRandomSpawn();
            Times = Random.Range(timeMin, timeMax);
            AvergeValueForScaleMass = Random.Range(0.03f, 0.08f);
            Debug.Log($"This script, SpawnFigure, OnUpdateTimerSpawn. Value: {AvergeValueForScaleMass}");
        }
    }
}
