using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public FalseOrTruePauseButton pauseBtn;
    public float DurationSlowly = 1f;
    public bool isPlay = false;

    public bool isPause = false;

    float td = 0f;
    float StartTimeOrReturn;

    public Button playButtonPause;

    void Start()
    {
        isPlay = false; // Убедимся, что игра не идет
        isPause = false;
    }

    void Update()
    {
        if (isPause)
        {
            return;
        }
        OnUpdateTimerForSpawn();
        GetPlatform.OnSetWhenWe_GameOver(isPlay);
    }

    public void OnShowPanelPause()
    {
        playButtonPause.enabled = false;
        isPause = true;
        OnStartTime();
        StartCoroutine(OnSlowlyTimeDelta());
    }

    public void OnResumePause()
    {   
        isPause = false;
        playButtonPause.enabled = true;
        OnReturnTime();
        StartCoroutine(OnReturnTimeDelta());
    }

    public void OnResetGameInMenu()
    {
        isPlay = false;
        pauseBtn.IsPausedCollised = false;
        Time.timeScale = 1f;
        TimeDeltaControl.ControlDelta = 1f;
        isPause = false;
        timerController.CurrentTimer = 0f;
        timerController.PreviousTimer = 0f;
        TransitionDelete.OnStartDeleteFigures();
        managmentController.isControlling = false;
        managmentController.SetController();
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
        if (isPause) return;
        if (isPlay) return;
        TransitionDelete.OnStartDeleteFigures();
        pauseBtn.IsPausedCollised = false;
        timerController.CurrentTimer = 0;
        timerController.PreviousTimer = 0;
        managmentController.isControlling = true;
        isPlay = true;
        managmentController.SetController();
        spawnFigures.Setting = figureSetting;
        spawnFigures.OnStartSpawn();
        trash.OnStartTrueLastObject();
    }

    //Coroutines
    //Time slowly
    public IEnumerator OnSlowlyTimeDelta()
    {
        td = Mathf.Clamp01(td);
       
        while(td < 1f)
        {
            td += Time.unscaledDeltaTime / DurationSlowly;
            TimeDeltaControl.ControlDelta = Mathf.LerpUnclamped(StartTimeOrReturn, 0f, td);
            Time.timeScale = Mathf.Lerp(1f, 0f, td);
            yield return null;
        }
        StartTimeOrReturn = TimeDeltaControl.ControlDelta;
        TimeDeltaControl.ControlDelta = 0f;
        TimeDeltaControl.AnimDelta = 1f;
    }
    //Time return normally
    public IEnumerator OnReturnTimeDelta()
    {
        td = Mathf.Clamp01(td);
        while(td > 0f)
        {
            td -= Time.unscaledDeltaTime / DurationSlowly;
            TimeDeltaControl.ControlDelta = Mathf.LerpUnclamped(StartTimeOrReturn, 1f, td);
            Time.timeScale = Mathf.Lerp(0f, 1f, td);
            yield return null;
        }
        TimeDeltaControl.ControlDelta = 1f;
        Time.timeScale = 1f;
    }

    public void OnStartTime()
    {
        td = 0f;
        StartTimeOrReturn = TimeDeltaControl.ControlDelta;
    }

    public void OnReturnTime()
    {
        StartTimeOrReturn = TimeDeltaControl.ControlDelta;
        td = 1f;
    }
}