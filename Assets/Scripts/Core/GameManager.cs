using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ��������, �����������: using UnityEngine.SceneManagement; ��� �����������

public class GameManager : MonoBehaviour
{
    [Header("Game Logic Components")] // ���������� ���������
    public SpawnFigures spawnFigures;
    public FigureSetting figureSetting;
    public ManagmentController managmentController;

    [Header("UI State Controllers")]
    public Panel_Ui_State mainMenuState; // ������ �� Panel_Ui_State ��� �������� ����
    public Panel_Ui_State gameplayUiState; // ������ �� Panel_Ui_State ��� ���������� �� ����� ���� (���� � �.�.)
    public Panel_Ui_State gameOverState; // ������ �� Panel_Ui_State ��� ������ Game Over
    public Panel_Ui_State settingsState; // (���� ����) ������ �� Panel_Ui_State ��� ��������
    // ������ ������, ���� �����

    // --- ������������ ���������� ---
    bool isPlay = false;

    void Start() // ���������� Start ��� ��������� ��������� UI
    {
        // ��� ������� ���� ������ ���������� ������� ����
        isPlay = false; // ��������, ��� ���� �� ����
        SetupInitialUI();
    }

    void Update()
    {
        OnUpdateTimerForSpawn();
    }

    void SetupInitialUI()
    {
        // ���������� ������� ����, �������� ���������
        mainMenuState?.ShowPanelAnimated(); // ?. - ���������� �����, ���� ������ �� ���������
        gameplayUiState?.HidePanelAnimated();
        gameOverState?.HidePanelAnimated();
        settingsState?.HidePanelAnimated();
    }

    public void OnUpdateTimerForSpawn()
    {
        if (isPlay)
        {
            spawnFigures.OnUpdateTimerSpawn();
        }
    }

    // --- ��������� ����� OnPlayButtonGame ---
    public void OnPlayButtonGame() // ���������� ������� Play �� �������� ����
    {
        if (isPlay) return; // ������������� ��������� ������

        isPlay = true;
        managmentController.SetController(); // ����������� ���������� ����������
        spawnFigures.Setting = figureSetting;
        spawnFigures.OnStartSpawn(); // ��������� ����� �����

        // --- ��������� ������ ������������ UI ---
        mainMenuState?.HidePanelAnimated(); // �������� ������� ����
        gameplayUiState?.ShowPanelAnimated(); // ���������� ������� UI
        gameOverState?.HidePanelAnimated(); // ��������, ��� Game Over �����
    }

    // --- ��������� ����� ��� Game Over ---
    public void TriggerGameOver()
    {
        if (!isPlay) return; // ���� ���� ��� �� ����, ������ �� ������

        isPlay = false;
        // ������������� ����� (����� �������� ����� � SpawnFigures)
        // spawnFigures.OnStopSpawn();
        // ����� ���������� �������� ��������� � �.�.
        // managmentController.DisableController();

        // --- ���������� Game Over UI ---
        gameplayUiState?.HidePanelAnimated(); // �������� ������� UI
        gameOverState?.ShowPanelAnimated(); // ���������� ������ Game Over
    }

    // --- ��������� ����� ��� �������� ---
    public void RestartGame() // ���������� ������� Restart �� Game Over ����
    {
        isPlay = false; // ���� ��� �� ����

        // ������� 1: ������� ����� UI � ���������� ��������� (��� ������������ �����)
        // ����� ����� ����� �������� ������� ���������, �������� ������ � �.�.
        // ResetGameState(); // ����� ����� ��� ������
        gameOverState?.HidePanelAnimated();
        mainMenuState?.ShowPanelAnimated(); // ������������ � ������� ����

        // ������� 2: ������������ ����� (����� ��� ������� ������)
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --- ��������� ����� ��� �������� (���� �����) ---
    public void ShowSettingsPanel()
    {
        // ��������, �������� ����, ���������� ���������
        mainMenuState?.HidePanelAnimated();
        settingsState?.ShowPanelAnimated();
    }

    public void HideSettingsPanel()
    {
        // �������� ���������, ���������� ����
        settingsState?.HidePanelAnimated();
        mainMenuState?.ShowPanelAnimated();
    }
}