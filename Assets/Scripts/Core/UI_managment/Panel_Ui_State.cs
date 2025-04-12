using System.Collections;
using UnityEngine;
using UnityEngine.Events; // Добавим для UnityEvent

public class Panel_Ui_State : MonoBehaviour
{
    [Header("Target UI Element")]
    [Tooltip("Точное имя элемента (панели) в списке Ui_Controlling, который нужно анимировать")]
    public string targetElementName;

    [Header("State GameObjects")]
    [Tooltip("GameObject, который активен, когда панель ПОКАЗАНА (обычно сама панель)")]
    public GameObject UIPane_On;
    [Tooltip("GameObject, который активен, когда панель СКРЫТА (опционально)")]
    public GameObject UIPanel_Off;

    [Header("Dependencies")]
    [Tooltip("Ссылка на ваш скрипт Ui_Controlling")]
    public Ui_Controlling controller;

    [Header("Animation Settings (Optional Overrides)")]
    [Tooltip("Переопределить длительность анимации появления (-1 = по умолчанию)")]
    public float overrideShowDuration = -1f;
    [Tooltip("Переопределить длительность анимации исчезновения (-1 = по умолчанию)")]
    public float overrideHideDuration = -1f;
    // Можно добавить сюда ссылки на кастомные AnimationCurve, если нужно переопределять и их

    [Header("Callbacks (Optional)")]
    [Tooltip("Событие, вызываемое ПОСЛЕ ЗАВЕРШЕНИЯ анимации ПОКАЗА")]
    public UnityEvent OnShowAnimationComplete; // Для вызова других методов после показа
    [Tooltip("Событие, вызываемое ПОСЛЕ ЗАВЕРШЕНИЯ анимации СКРЫТИЯ")]
    public UnityEvent OnHideAnimationComplete; // Для вызова других методов после скрытия

    // Внутреннее состояние
    private Coroutine activeStateChangeCoroutine = null;
    // Состояние видимости лучше определять по факту, а не хранить
    // private bool isCurrentlyVisible = false;

    void Start()
    {
        // Проверки
        if (controller == null)
        {
            Debug.LogError($"'{gameObject.name}': Ui_Controlling не назначен!", this);
            enabled = false;
            return;
        }
        if (string.IsNullOrEmpty(targetElementName))
        {
            Debug.LogError($"'{gameObject.name}': Target Element Name не указан!", this);
            enabled = false;
            return;
        }
        // Начальную установку панелей On/Off лучше сделать в отдельном менеджере состояний или вручную в сцене
    }

    // --- Публичные методы для вызова извне ---

    /// <summary>
    /// Запускает анимацию СКРЫТИЯ (AnimateOut) для targetElementName,
    /// и после ее завершения деактивирует UIPane_On и активирует UIPanel_Off.
    /// </summary>
    public void HidePanelAnimated()
    {
        // Проверяем, не идет ли уже другая анимация для этой панели
        if (activeStateChangeCoroutine != null)
        {
            Debug.LogWarning($"'{gameObject.name}': Попытка скрыть панель '{targetElementName}', пока идет другая анимация.", this);
            // Можно либо прервать текущую, либо просто выйти
            // StopCoroutine(activeStateChangeCoroutine);
            return;
        }
        activeStateChangeCoroutine = StartCoroutine(HideSequence());
    }

    /// <summary>
    /// Запускает анимацию ПОЯВЛЕНИЯ (AnimateIn) для targetElementName,
    /// активируя UIPane_On ДО начала анимации и деактивируя UIPanel_Off.
    /// Вызывает OnShowAnimationComplete после завершения.
    /// </summary>
    public void ShowPanelAnimated()
    {
        if (activeStateChangeCoroutine != null)
        {
            Debug.LogWarning($"'{gameObject.name}': Попытка показать панель '{targetElementName}', пока идет другая анимация.", this);
            // StopCoroutine(activeStateChangeCoroutine);
            return;
        }
        activeStateChangeCoroutine = StartCoroutine(ShowSequence());
    }

    /// <summary>
    /// Переключает видимость панели: если видима - скрывает, если скрыта - показывает.
    /// Использует текущую активность UIPane_On для определения состояния.
    /// </summary>
    public void TogglePanelAnimated()
    {
        if (UIPane_On != null && UIPane_On.activeSelf)
        {
            // Если панель сейчас активна - скрываем
            HidePanelAnimated();
        }
        else
        {
            // Если панель неактивна (или не назначена) - показываем
            ShowPanelAnimated();
        }
    }

    // --- Корутины для последовательности действий ---

    private IEnumerator HideSequence()
    {
        if (controller == null || string.IsNullOrEmpty(targetElementName))
        {
            Debug.LogError($"'{gameObject.name}': Невозможно выполнить HideSequence - контроллер или имя не заданы.", this);
            activeStateChangeCoroutine = null;
            yield break; // Выходим из корутины
        }

        // 1. Запускаем анимацию исчезновения
        float duration = (overrideHideDuration >= 0) ? overrideHideDuration : controller.defaultDuration;
        // Здесь можно передать кастомные кривые, если они есть в этом скрипте
        controller.AnimateOut(targetElementName, duration /*, customMoveCurve, customFadeCurve */);

        // 2. Ждем примерное время завершения анимации
        if (duration > 0)
        {
            yield return new WaitForSeconds(duration); // Ждем, только если есть длительность
        }
        // Для большей точности можно добавить небольшую задержку yield return null;

        // 3. Выполняем действия ПОСЛЕ анимации
        if (UIPane_On != null) UIPane_On.SetActive(false);
        if (UIPanel_Off != null) UIPanel_Off.SetActive(true);

        // 4. Вызываем событие завершения (если кто-то на него подписан)
        OnHideAnimationComplete?.Invoke(); // Безопасный вызов

        // 5. Сбрасываем флаг корутины
        activeStateChangeCoroutine = null;
    }

    private IEnumerator ShowSequence()
    {
        if (controller == null || string.IsNullOrEmpty(targetElementName))
        {
            Debug.LogError($"'{gameObject.name}': Невозможно выполнить ShowSequence - контроллер или имя не заданы.", this);
            activeStateChangeCoroutine = null;
            yield break;
        }

        // 1. Активируем нужные панели ДО анимации
        if (UIPane_On != null) UIPane_On.SetActive(true);
        if (UIPanel_Off != null) UIPanel_Off.SetActive(false);
        // Важно: может потребоваться подождать кадр после SetActive(true),
        // чтобы Ui_Controlling корректно определил startAlpha/startPos,
        // особенно если CanvasGroup был добавлен динамически или альфа была 0.
        yield return null; // Небольшая пауза на всякий случай

        // 2. Запускаем анимацию появления
        float duration = (overrideShowDuration >= 0) ? overrideShowDuration : controller.defaultDuration;
        controller.AnimateIn(targetElementName, duration /*, customMoveCurve, customFadeCurve */);

        // 3. Ждем примерное время завершения
        if (duration > 0)
        {
            yield return new WaitForSeconds(duration);
        }

        // 4. Убедимся, что панель точно включена (на случай ошибок анимации)
        // if (UIPane_On != null) UIPane_On.SetActive(true); // Обычно не нужно, т.к. делали в начале

        // 5. Вызываем событие завершения
        OnShowAnimationComplete?.Invoke();

        // 6. Сбрасываем флаг корутины
        activeStateChangeCoroutine = null;
    }

    // --- Заменяем твои пустые методы на вызовы наших ---

    /// <summary>
    /// Пример использования: скрывает эту панель при старте игры (если нужно).
    /// </summary>
    public void OnStartPlayAnim() // Переименовал для ясности, т.к. это не колбэк анимации
    {
        Debug.Log($"'{gameObject.name}': Вызван OnStartPlayAnim - скрываем панель '{targetElementName}'.");
        HidePanelAnimated();
    }

    /// <summary>
    /// Пример использования: переключает видимость панели.
    /// </summary>
    public void OnAimationPanel() // Переименовал, это не колбэк анимации
    {
        Debug.Log($"'{gameObject.name}': Вызван OnAimationPanel - переключаем панель '{targetElementName}'.");
        TogglePanelAnimated();
    }
}