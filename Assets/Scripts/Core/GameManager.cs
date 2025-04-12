using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpawnFigures spawnFigures;
    public FigureSetting figureSetting;
    public ManagmentController managmentController;

    //Var private
    bool isPlay = false;
    
    // Update is called once per frame
    void Update()
    {
        OnUpdateTimerForSpawn();
    }

    public void OnUpdateTimerForSpawn()
    {
        if (isPlay)
        {
            spawnFigures.OnUpdateTimerSpawn();
        }
    }

    public void OnPlayButtonGame()
    {
        isPlay = true;
        managmentController.SetController();
        spawnFigures.Setting = figureSetting;
        spawnFigures.OnStartSpawn();
    }
}
