using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Возможно, понадобится: using UnityEngine.SceneManagement; для перезапуска

public class GameManager : MonoBehaviour
{
    [Header("Game Logic Components")] // Организуем инспектор
    public SpawnFigures spawnFigures;
    public FigureSetting figureSetting;
    public ManagmentController managmentController;

    [Header("UI State Controllers")]
    public Panel_Ui_State mainMenuState; // Ссылка на Panel_Ui_State для главного меню
    public Panel_Ui_State gameplayUiState; // Ссылка на Panel_Ui_State для интерфейса во время игры (счет и т.п.)
    public Panel_Ui_State gameOverState; // Ссылка на Panel_Ui_State для экрана Game Over
    public Panel_Ui_State settingsState; // (Если есть) Ссылка на Panel_Ui_State для настроек
    // Добавь другие, если нужно

    // --- Существующие переменные ---
    bool isPlay = false;

    void Start() // Используем Start для начальной установки UI
    {
        // При запуске игры обычно показываем главное меню
        isPlay = false; // Убедимся, что игра не идет
        SetupInitialUI();
    }

    void Update()
    {
        OnUpdateTimerForSpawn();
    }

    void SetupInitialUI()
    {
        // Показываем главное меню, скрываем остальное
        mainMenuState?.ShowPanelAnimated(); // ?. - безопасный вызов, если ссылка не назначена
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

    // --- Обновляем метод OnPlayButtonGame ---
    public void OnPlayButtonGame() // Вызывается кнопкой Play из главного меню
    {
        if (isPlay) return; // Предотвращаем повторный запуск

        isPlay = true;
        managmentController.SetController(); // Настраиваем управление платформой
        spawnFigures.Setting = figureSetting;
        spawnFigures.OnStartSpawn(); // Запускаем спаун фигур

        // --- ДОБАВЛЯЕМ ЛОГИКУ ПЕРЕКЛЮЧЕНИЯ UI ---
        mainMenuState?.HidePanelAnimated(); // Скрываем главное меню
        gameplayUiState?.ShowPanelAnimated(); // Показываем игровой UI
        gameOverState?.HidePanelAnimated(); // Убедимся, что Game Over скрыт
    }

    // --- ДОБАВЛЯЕМ МЕТОД ДЛЯ Game Over ---
    public void TriggerGameOver()
    {
        if (!isPlay) return; // Если игра уже не идет, ничего не делаем

        isPlay = false;
        // Останавливаем спаун (нужно добавить метод в SpawnFigures)
        // spawnFigures.OnStopSpawn();
        // Можно остановить движение платформы и т.д.
        // managmentController.DisableController();

        // --- ПОКАЗЫВАЕМ Game Over UI ---
        gameplayUiState?.HidePanelAnimated(); // Скрываем игровой UI
        gameOverState?.ShowPanelAnimated(); // Показываем панель Game Over
    }

    // --- ДОБАВЛЯЕМ МЕТОД ДЛЯ Рестарта ---
    public void RestartGame() // Вызывается кнопкой Restart из Game Over меню
    {
        isPlay = false; // Игра еще не идет

        // Вариант 1: Простой сброс UI к начальному состоянию (без перезагрузки сцены)
        // Нужно будет также сбросить позицию платформы, очистить фигуры и т.д.
        // ResetGameState(); // Нужен метод для сброса
        gameOverState?.HidePanelAnimated();
        mainMenuState?.ShowPanelAnimated(); // Возвращаемся в главное меню

        // Вариант 2: Перезагрузка сцены (проще для полного сброса)
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --- ДОБАВЛЯЕМ МЕТОД ДЛЯ Настроек (если нужно) ---
    public void ShowSettingsPanel()
    {
        // Например, скрываем меню, показываем настройки
        mainMenuState?.HidePanelAnimated();
        settingsState?.ShowPanelAnimated();
    }

    public void HideSettingsPanel()
    {
        // Скрываем настройки, показываем меню
        settingsState?.HidePanelAnimated();
        mainMenuState?.ShowPanelAnimated();
    }
}