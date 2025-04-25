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
    public Panel_Ui_State state;
    public DeleteTrash trash;
    public Platform_Manager GetPlatform;
    public TransitionDeleteLastFigures TransitionDelete;

    public bool isPlay = false;
    void Start()
    {
        isPlay = false; // Убедимся, что игра не идет
    }

    void Update()
    {
        OnUpdateTimerForSpawn();
        GetPlatform.OnSetWhenWe_GameOver(isPlay);
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
        state.OnSetActivePanelWhenActivePanel("GameOver");
        managmentController.SetController();
        _Click.OnStartAnimationGroup("Panel_GameOver");
    }
    public void OnPlayButtonGame()
    {
        if (isPlay) return;
        TransitionDelete.OnStartDeleteFigures();
        timerController.CurrentTimer = 0;
        timerController.PreviousTimer = 0;
        managmentController.isControlling = true;
        isPlay = true;
        managmentController.SetController();
        spawnFigures.Setting = figureSetting;
        spawnFigures.OnStartSpawn();
        trash.OnStartTrueLastObject();
    }
}