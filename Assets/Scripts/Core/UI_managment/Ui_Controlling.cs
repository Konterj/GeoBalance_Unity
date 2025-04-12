// �������, ��� ��� using ���� � ������ �����
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ui_Controlling : MonoBehaviour
{
    [Tooltip("���������� ���� ��� UI ��������, �������� ����� ��������� ���� ����������")]
    public List<UI_ELEMENTS> uiElements;

    [Tooltip("������������ �������� �� ��������� � ��������")]
    public float defaultDuration = 0.5f;

    // ������� ��� ������������ �������� ��������
    private Dictionary<RectTransform, Coroutine> activeAnimation = new Dictionary<RectTransform, Coroutine>();

    // ������������� ��� �������
    private void Awake()
    {
        InitializeElements();
    }

    // ����� ��� �������������� ��������� ��������� �� ������
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
                // ���������/��������� CanvasGroup ����� ����������� ��������
                if (element.canvasGroup == null)
                {
                    element.canvasGroup = element.ObjectUI.GetComponent<CanvasGroup>();
                    if (element.canvasGroup == null)
                    {
                        element.canvasGroup = element.ObjectUI.gameObject.AddComponent<CanvasGroup>();
                    }
                }
                // ��������� ��������� �������� ����� ��������� CanvasGroup
                element.OnSetDefaultValue();

                // ������������� ��������� �����
                if (element.canvasGroup != null)
                {
                    element.canvasGroup.alpha = element.originalAlpha;
                }
            }
        }
    }

    // --- ��������� ������ ��� ������� �������� ---

    /// <summary>
    /// ��������� �������� "�����" (� targetOutPos, alpha=0) ��� �������� �� �����.
    /// </summary>
    public void AnimateOut(string elementName, float? duration = null, AnimationCurve customCurveAnimation = null, AnimationCurve customFadeOut = null)
    {
        UI_ELEMENTS element = FindElementByName(elementName);
        if (element == null) return;

        float animDuration = duration ?? defaultDuration;
        // ���������� ����� ������ �������� ��� ����������� ��� ����� (���� �� ���� element.moveOutPose)
        AnimationCurve moveCurveToUse = customCurveAnimation ?? element.movePose;
        // ���������� ������ ������������
        AnimationCurve fadeCurveToUse = customFadeOut ?? element.fadeIn; // ���������� ���� ��� ������������

        // ���� - ������� "�����" � ����� 0
        StartElementAnimation(element, element.targetPos, 0f, animDuration, moveCurveToUse, fadeCurveToUse);
    }

    /// <summary>
    /// ��������� �������� "�����������" (� originalPose, originalAlpha) ��� �������� �� �����.
    /// </summary>
    public void AnimateIn(string elementName, float? duration = null, AnimationCurve customCurveAnimation = null, AnimationCurve customFadeIn = null)
    {
        UI_ELEMENTS element = FindElementByName(elementName);
        if (element == null) return;

        float animDuration = duration ?? defaultDuration;
        // ���������� ����� ������ �������� ��� ����������� ��� ������� (���� �� ���� element.moveInPose)
        AnimationCurve moveCurveToUse = customCurveAnimation ?? element.movePose;
        // ���������� ������ ���������
        AnimationCurve fadeCurveToUse = customFadeIn ?? element.fadeIn; // ���������� ���� ��� ���������

        // ���� - ������������ ������� � ������������ �����
        StartElementAnimation(element, element.targetPos, element.originalAlpha, animDuration, moveCurveToUse, fadeCurveToUse);
    }

    /// <summary>
    /// ��������� �������� "�����" (� targetOutPos, alpha=0) ��� ���� ��������� � ������.
    /// </summary>
    public void AnimateAllOut(float? duration = null, bool useIndividualCurves = true)
    {
        float animDuration = duration ?? defaultDuration;
        foreach (UI_ELEMENTS element in uiElements)
        {
            if (element != null && element.ObjectUI != null && element.canvasGroup != null)
            {
                // ���������� �������������� ������ �������� ��� "�����"
                AnimationCurve moveCurveToUse = element.movePose;
                AnimationCurve fadeCurveToUse = element.fadeOut; // ���������� ������ ������������

                StartElementAnimation(element, element.targetPos, 0f, animDuration, moveCurveToUse, fadeCurveToUse);
            }
        }
    }

    /// <summary>
    /// ��������� �������� "�����������" (� originalPose, originalAlpha) ��� ���� ��������� � ������.
    /// </summary>
    public void AnimateAllIn(float? duration = null, bool useIndividualCurves = true)
    {
        float animDuration = duration ?? defaultDuration;
        foreach (UI_ELEMENTS element in uiElements)
        {
            if (element != null && element.ObjectUI != null && element.canvasGroup != null)
            {
                // ���������� �������������� ������ �������� ��� "�������"
                AnimationCurve moveCurveToUse = element.movePose; // ��� element.moveInPose
                AnimationCurve fadeCurveToUse = element.fadeIn; // ���������� ������ ���������

                StartElementAnimation(element, element.originalPose, element.originalAlpha, animDuration, moveCurveToUse, fadeCurveToUse);
            }
        }
    }

    // --- ���������� ������ ---

    /// <summary>
    /// �������������� � ��������� �������� �������� ��� ���������� ��������.
    /// </summary>
    private void StartElementAnimation(UI_ELEMENTS element, Vector2 targetPos, float targetAlpha, float duration, AnimationCurve moveCurve, AnimationCurve fadeCurve)
    {
        if (element == null || element.ObjectUI == null || element.canvasGroup == null)
        {
            return;
        }

        // ������������� ���������� �������� ��� ����� ��������
        if (activeAnimation.TryGetValue(element.ObjectUI, out Coroutine runningCoroutine) && runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }

        // ���������� ��������� �������� ����� ������� �����
        Vector2 startPos = element.ObjectUI.anchoredPosition;
        float startAlpha = element.canvasGroup.alpha;

        // ��������� �������� � ��������� ������ �� ���
        Coroutine newCoroutine = StartCoroutine(AnimateElementRoutine(
            element, startPos, targetPos, startAlpha, targetAlpha, duration, moveCurve, fadeCurve
        ));
        activeAnimation[element.ObjectUI] = newCoroutine;
    }

    /// <summary>
    /// ���� ������� � ������ 'uiElements' �� ���������� �����.
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
            // � ������ ������ ����� ������ Debug.LogError ��� �������� ��� ����������� ������
            // Debug.LogError($"������� � ������ '{elementName}' �� ������!", this.gameObject);
        }
        return foundElement;
    }

    /// <summary>
    /// ��������, ����������� ������� �������� ������� � ����� �� ��������.
    /// </summary>
    IEnumerator AnimateElementRoutine(UI_ELEMENTS element, Vector2 startPos, Vector2 endPose, float startAlpha, float endAlpha, float duration,
        AnimationCurve moveCurve, AnimationCurve fadeCurve)
    {
        float currentTime = 0f;
        // �������� ������ ��� ���������� ��������� ������� ������ �����
        RectTransform rectTransform = element.ObjectUI;
        CanvasGroup canvasGroup = element.canvasGroup;

        // �������� ���� ��������
        while (currentTime < duration)
        {
            // �������� �� ������, ���� ������ ���������� �� ����� ��������
            if (rectTransform == null || canvasGroup == null)
            {
                // ���� ������ ���������, �������� ������ ��� �� ������� �������� ��������
                if (element.ObjectUI != null && activeAnimation.ContainsKey(element.ObjectUI))
                    activeAnimation.Remove(element.ObjectUI);
                yield break; // ��������� ��������
            }

            // ���������� ������� � ��������� ��������� (0..1)
            currentTime += Time.deltaTime;
            float linearT = (duration > 0f) ? Mathf.Clamp01(currentTime / duration) : 1f;

            // ��������� "�����������" ��������� �� ������ ��������
            float easedMoveT = (moveCurve != null && moveCurve.keys.Length > 0) ? moveCurve.Evaluate(linearT) : linearT;
            float easedFadeT = (fadeCurve != null && fadeCurve.keys.Length > 0) ? fadeCurve.Evaluate(linearT) : linearT;

            // ������������ � ���������� �������� � �����������
            rectTransform.anchoredPosition = Vector2.LerpUnclamped(startPos, endPose, easedMoveT);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, easedFadeT);

            // �������� ���������� �����
            yield return null;
        }

        // ��������������� ��������� �������� �������� ����� ���������� �����
        if (rectTransform != null) rectTransform.anchoredPosition = endPose;
        if (canvasGroup != null) canvasGroup.alpha = endAlpha;

        // ������� ������� �������� �������� ��� ����� ��������
        if (rectTransform != null && activeAnimation.ContainsKey(rectTransform))
        {
            activeAnimation[rectTransform] = null;
        }
    }
}