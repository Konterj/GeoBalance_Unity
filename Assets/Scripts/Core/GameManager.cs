using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Logic Components")] // Организуем инспектор
    public SpawnFigures spawnFigures;
    public FigureSetting figureSetting;
    public ManagmentController managmentController;
    public Timer_Controller timerController;
    public Animation_On_Click _Click;

    public bool isPlay = false;

    void Start()
    {
        isPlay = false; // Убедимся, что игра не идет
    }

    void Update()
    {
        OnUpdateTimerForSpawn();
    }

    public void OnUpdateTimerForSpawn()
    {
        if (isPlay)
        {
            spawnFigures.OnUpdateTimerSpawn();
            timerController.OnUpdateTime();
        }
    }
    public void OnShowPanelGameOver()
    {
        isPlay = false;
        managmentController.isControlling = false;
        managmentController.SetController();
        _Click.OnStartAnimationGroup("Panel_GameOver");
    }
    public void OnPlayButtonGame()
    {
        if (isPlay) return;
        managmentController.isControlling = true;
        isPlay = true;
        managmentController.SetController();
        spawnFigures.Setting = figureSetting;
        spawnFigures.OnStartSpawn(); 
    }
}