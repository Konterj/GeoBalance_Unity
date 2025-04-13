using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Logic Components")] // ���������� ���������
    public SpawnFigures spawnFigures;
    public FigureSetting figureSetting;
    public ManagmentController managmentController;

    bool isPlay = false;

    void Start()
    {
        isPlay = false; // ��������, ��� ���� �� ����

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
        }
    }
    public void OnPlayButtonGame()
    {
        if (isPlay) return; 

        isPlay = true;
        managmentController.SetController();
        spawnFigures.Setting = figureSetting;
        spawnFigures.OnStartSpawn(); 

    }
}