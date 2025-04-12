using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpawnFigures spawnFigures;
    public FigureSetting figureSetting;
    public ManagmentController managmentController;
    // Start is called before the first frame update
    void Start()
    {
        spawnFigures.Setting = figureSetting;
        spawnFigures.OnStartSpawn();
        managmentController.SetController();
    }

    // Update is called once per frame
    void Update()
    {
        spawnFigures.OnUpdateTimerSpawn();
    }
}
