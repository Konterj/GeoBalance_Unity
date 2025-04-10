using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpawnFigures spawnFigures;
    public FigureSetting figureSetting;
    // Start is called before the first frame update
    void Start()
    {
        spawnFigures.Setting = figureSetting;
        spawnFigures.OnStartSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        spawnFigures.OnUpdateTimerSpawn();
    }
}
