// Убедись, что эти using есть в начале файла
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ui_Controlling : MonoBehaviour
{
    [Tooltip("Перетащите сюда все UI элементы, которыми будет управлять этот контроллер")]
    public List<UI_ELEMENTS> uiElements;

    [Tooltip("Длительность анимации по умолчанию в секундах")]
    public float defaultDuration = 0.5f;

    // Словарь для отслеживания активных анимаций
    private Dictionary<RectTransform, Coroutine> activeAnimation = new Dictionary<RectTransform, Coroutine>();

    // Инициализация при запуске
    private void Awake()
    {
        InitializeElements();
    }

    // Метод для первоначальной настройки элементов из списка
    public void InitializeElements()
    {
        if (uiElements == null)
        {
            uiElements = new List<UI_ELEMENTS>();
            return;
        }

        foreach (UI_ELEMENTS element in uiElements)
        {
            if (element != null && element.ObjectUI != null)
            {
                // Проверяем/добавляем CanvasGroup ПЕРЕД сохранением значений
                if (element.canvasGroup == null)
                {
                    element.canvasGroup = element.ObjectUI.GetComponent<CanvasGroup>();
                    if (element.canvasGroup == null)
                    {
                        element.canvasGroup = element.ObjectUI.gameObject.AddComponent<CanvasGroup>();
                    }
                }
                // Сохраняем начальные значения ПОСЛЕ настройки CanvasGroup
                element.OnSetDefaultValue();

                // Устанавливаем начальную альфу
                if (element.canvasGroup != null)
                {
                    element.canvasGroup.alpha = element.originalAlpha;
                }
            }
        }
    }

    // --- ПУБЛИЧНЫЕ МЕТОДЫ ДЛЯ ЗАПУСКА АНИМАЦИИ ---

    /// <summary>
    /// Запускает анимацию "ухода" (к targetOutPos, alpha=0) для элемента по имени.
    /// </summary>
    public void AnimateOut(string elementName, float? duration = null, AnimationCurve customCurveAnimation = null, AnimationCurve customFadeOut = null)
    {
        UI_ELEMENTS element = FindElementByName(elementName);
        if (element == null) return;

        float animDuration = duration ?? defaultDuration;
        // Используем общую кривую движения или специфичную для ухода (если бы была element.moveOutPose)
        AnimationCurve moveCurveToUse = customCurveAnimation ?? element.movePose;
        // Используем кривую ИСЧЕЗНОВЕНИЯ
        AnimationCurve fadeCurveToUse = customFadeOut ?? element.fadeIn; // Используем поле для исчезновения

        // Цель - позиция "ухода" и альфа 0
        StartElementAnimation(element, element.targetPos, 0f, animDuration, moveCurveToUse, fadeCurveToUse);
    }

    /// <summary>
    /// Запускает анимацию "возвращения" (к originalPose, originalAlpha) для элемента по имени.
    /// </summary>
    public void AnimateIn(string elementName, float? duration = null, AnimationCurve customCurveAnimation = null, AnimationCurve customFadeIn = null)
    {
        UI_ELEMENTS element = FindElementByName(elementName);
        if (element == null) return;

        float animDuration = duration ?? defaultDuration;
        // Используем общую кривую движения или специфичную для прихода (если бы была element.moveInPose)
        AnimationCurve moveCurveToUse = customCurveAnimation ?? element.movePose;
        // Используем кривую ПОЯВЛЕНИЯ
        AnimationCurve fadeCurveToUse = customFadeIn ?? element.fadeIn; // Используем поле для появления

        // Цель - ОРИГИНАЛЬНАЯ позиция и ОРИГИНАЛЬНАЯ альфа
        StartElementAnimation(element, element.targetPos, element.originalAlpha, animDuration, moveCurveToUse, fadeCurveToUse);
    }

    /// <summary>
    /// Запускает анимацию "ухода" (к targetOutPos, alpha=0) для ВСЕХ элементов в списке.
    /// </summary>
    public void AnimateAllOut(float? duration = null, bool useIndividualCurves = true)
    {
        float animDuration = duration ?? defaultDuration;
        foreach (UI_ELEMENTS element in uiElements)
        {
            if (element != null && element.ObjectUI != null && element.canvasGroup != null)
            {
                // Используем индивидуальные кривые элемента для "ухода"
                AnimationCurve moveCurveToUse = element.movePose;
                AnimationCurve fadeCurveToUse = element.fadeOut; // Используем кривую исчезновения

                StartElementAnimation(element, element.targetPos, 0f, animDuration, moveCurveToUse, fadeCurveToUse);
            }
        }
    }

    /// <summary>
    /// Запускает анимацию "возвращения" (к originalPose, originalAlpha) для ВСЕХ элементов в списке.
    /// </summary>
    public void AnimateAllIn(float? duration = null, bool useIndividualCurves = true)
    {
        float animDuration = duration ?? defaultDuration;
        foreach (UI_ELEMENTS element in uiElements)
        {
            if (element != null && element.ObjectUI != null && element.canvasGroup != null)
            {
                // Используем индивидуальные кривые элемента для "прихода"
                AnimationCurve moveCurveToUse = element.movePose; // Или element.moveInPose
                AnimationCurve fadeCurveToUse = element.fadeIn; // Используем кривую появления

                StartElementAnimation(element, element.originalPose, element.originalAlpha, animDuration, moveCurveToUse, fadeCurveToUse);
            }
        }
    }

    // --- ВНУТРЕННИЕ МЕТОДЫ ---

    /// <summary>
    /// Подготавливает и запускает корутину анимации для указанного элемента.
    /// </summary>
    private void StartElementAnimation(UI_ELEMENTS element, Vector2 targetPos, float targetAlpha, float duration, AnimationCurve moveCurve, AnimationCurve fadeCurve)
    {
        if (element == null || element.ObjectUI == null || element.canvasGroup == null)
        {
            return;
        }

        // Останавливаем предыдущую анимацию для этого элемента
        if (activeAnimation.TryGetValue(element.ObjectUI, out Coroutine runningCoroutine) && runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }

        // Определяем начальные значения перед стартом новой
        Vector2 startPos = element.ObjectUI.anchoredPosition;
        float startAlpha = element.canvasGroup.alpha;

        // Запускаем корутину и сохраняем ссылку на нее
        Coroutine newCoroutine = StartCoroutine(AnimateElementRoutine(
            element, startPos, targetPos, startAlpha, targetAlpha, duration, moveCurve, fadeCurve
        ));
        activeAnimation[element.ObjectUI] = newCoroutine;
    }

    /// <summary>
    /// Ищет элемент в списке 'uiElements' по строковому имени.
    /// </summary>
    private UI_ELEMENTS FindElementByName(string elementName)
    {
        if (uiElements == null || uiElements.Count == 0)
        {
            return null;
        }

        UI_ELEMENTS foundElement = uiElements.FirstOrDefault(e => e != null && e.name == elementName);

        if (foundElement == null)
        {
            // В чистой версии можно убрать Debug.LogError или оставить для критических ошибок
            // Debug.LogError($"Элемент с именем '{elementName}' не найден!", this.gameObject);
        }
        return foundElement;
    }

    /// <summary>
    /// Корутина, выполняющая плавную анимацию позиции и альфы со временем.
    /// </summary>
    IEnumerator AnimateElementRoutine(UI_ELEMENTS element, Vector2 startPos, Vector2 endPose, float startAlpha, float endAlpha, float duration,
        AnimationCurve moveCurve, AnimationCurve fadeCurve)
    {
        float currentTime = 0f;
        // Кэшируем ссылки для небольшого ускорения доступа внутри цикла
        RectTransform rectTransform = element.ObjectUI;
        CanvasGroup canvasGroup = element.canvasGroup;

        // Основной цикл анимации
        while (currentTime < duration)
        {
            // Проверка на случай, если объект уничтожили во время анимации
            if (rectTransform == null || canvasGroup == null)
            {
                // Если объект уничтожен, пытаемся убрать его из словаря активных анимаций
                if (element.ObjectUI != null && activeAnimation.ContainsKey(element.ObjectUI))
                    activeAnimation.Remove(element.ObjectUI);
                yield break; // Прерываем корутину
            }

            // Обновление времени и линейного прогресса (0..1)
            currentTime += Time.deltaTime;
            float linearT = (duration > 0f) ? Mathf.Clamp01(currentTime / duration) : 1f;

            // Получение "сглаженного" прогресса из кривых анимации
            float easedMoveT = (moveCurve != null && moveCurve.keys.Length > 0) ? moveCurve.Evaluate(linearT) : linearT;
            float easedFadeT = (fadeCurve != null && fadeCurve.keys.Length > 0) ? fadeCurve.Evaluate(linearT) : linearT;

            // Интерполяция и применение значений к компонентам
            rectTransform.anchoredPosition = Vector2.LerpUnclamped(startPos, endPose, easedMoveT);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, easedFadeT);

            // Ожидание следующего кадра
            yield return null;
        }

        // Гарантированная установка конечных значений после завершения цикла
        if (rectTransform != null) rectTransform.anchoredPosition = endPose;
        if (canvasGroup != null) canvasGroup.alpha = endAlpha;

        // Очистка словаря активных анимаций для этого элемента
        if (rectTransform != null && activeAnimation.ContainsKey(rectTransform))
        {
            activeAnimation[rectTransform] = null;
        }
    }
}