using System.Collections;
using UnityEngine;
using UnityEngine.Events; // ������� ��� UnityEvent

public class Panel_Ui_State : MonoBehaviour
{
    [Header("Target UI Element")]
    [Tooltip("������ ��� �������� (������) � ������ Ui_Controlling, ������� ����� �����������")]
    public string targetElementName;

    [Header("State GameObjects")]
    [Tooltip("GameObject, ������� �������, ����� ������ �������� (������ ���� ������)")]
    public GameObject UIPane_On;
    [Tooltip("GameObject, ������� �������, ����� ������ ������ (�����������)")]
    public GameObject UIPanel_Off;

    [Header("Dependencies")]
    [Tooltip("������ �� ��� ������ Ui_Controlling")]
    public Ui_Controlling controller;

    [Header("Animation Settings (Optional Overrides)")]
    [Tooltip("�������������� ������������ �������� ��������� (-1 = �� ���������)")]
    public float overrideShowDuration = -1f;
    [Tooltip("�������������� ������������ �������� ������������ (-1 = �� ���������)")]
    public float overrideHideDuration = -1f;
    // ����� �������� ���� ������ �� ��������� AnimationCurve, ���� ����� �������������� � ��

    [Header("Callbacks (Optional)")]
    [Tooltip("�������, ���������� ����� ���������� �������� ������")]
    public UnityEvent OnShowAnimationComplete; // ��� ������ ������ ������� ����� ������
    [Tooltip("�������, ���������� ����� ���������� �������� �������")]
    public UnityEvent OnHideAnimationComplete; // ��� ������ ������ ������� ����� �������

    // ���������� ���������
    private Coroutine activeStateChangeCoroutine = null;
    // ��������� ��������� ����� ���������� �� �����, � �� �������
    // private bool isCurrentlyVisible = false;

    void Start()
    {
        // ��������
        if (controller == null)
        {
            Debug.LogError($"'{gameObject.name}': Ui_Controlling �� ��������!", this);
            enabled = false;
            return;
        }
        if (string.IsNullOrEmpty(targetElementName))
        {
            Debug.LogError($"'{gameObject.name}': Target Element Name �� ������!", this);
            enabled = false;
            return;
        }
        // ��������� ��������� ������� On/Off ����� ������� � ��������� ��������� ��������� ��� ������� � �����
    }

    // --- ��������� ������ ��� ������ ����� ---

    /// <summary>
    /// ��������� �������� ������� (AnimateOut) ��� targetElementName,
    /// � ����� �� ���������� ������������ UIPane_On � ���������� UIPanel_Off.
    /// </summary>
    public void HidePanelAnimated()
    {
        // ���������, �� ���� �� ��� ������ �������� ��� ���� ������
        if (activeStateChangeCoroutine != null)
        {
            Debug.LogWarning($"'{gameObject.name}': ������� ������ ������ '{targetElementName}', ���� ���� ������ ��������.", this);
            // ����� ���� �������� �������, ���� ������ �����
            // StopCoroutine(activeStateChangeCoroutine);
            return;
        }
        activeStateChangeCoroutine = StartCoroutine(HideSequence());
    }

    /// <summary>
    /// ��������� �������� ��������� (AnimateIn) ��� targetElementName,
    /// ��������� UIPane_On �� ������ �������� � ����������� UIPanel_Off.
    /// �������� OnShowAnimationComplete ����� ����������.
    /// </summary>
    public void ShowPanelAnimated()
    {
        if (activeStateChangeCoroutine != null)
        {
            Debug.LogWarning($"'{gameObject.name}': ������� �������� ������ '{targetElementName}', ���� ���� ������ ��������.", this);
            // StopCoroutine(activeStateChangeCoroutine);
            return;
        }
        activeStateChangeCoroutine = StartCoroutine(ShowSequence());
    }

    /// <summary>
    /// ����������� ��������� ������: ���� ������ - ��������, ���� ������ - ����������.
    /// ���������� ������� ���������� UIPane_On ��� ����������� ���������.
    /// </summary>
    public void TogglePanelAnimated()
    {
        if (UIPane_On != null && UIPane_On.activeSelf)
        {
            // ���� ������ ������ ������� - ��������
            HidePanelAnimated();
        }
        else
        {
            // ���� ������ ��������� (��� �� ���������) - ����������
            ShowPanelAnimated();
        }
    }

    // --- �������� ��� ������������������ �������� ---

    private IEnumerator HideSequence()
    {
        if (controller == null || string.IsNullOrEmpty(targetElementName))
        {
            Debug.LogError($"'{gameObject.name}': ���������� ��������� HideSequence - ���������� ��� ��� �� ������.", this);
            activeStateChangeCoroutine = null;
            yield break; // ������� �� ��������
        }

        // 1. ��������� �������� ������������
        float duration = (overrideHideDuration >= 0) ? overrideHideDuration : controller.defaultDuration;
        // ����� ����� �������� ��������� ������, ���� ��� ���� � ���� �������
        controller.AnimateOut(targetElementName, duration /*, customMoveCurve, customFadeCurve */);

        // 2. ���� ��������� ����� ���������� ��������
        if (duration > 0)
        {
            yield return new WaitForSeconds(duration); // ����, ������ ���� ���� ������������
        }
        // ��� ������� �������� ����� �������� ��������� �������� yield return null;

        // 3. ��������� �������� ����� ��������
        if (UIPane_On != null) UIPane_On.SetActive(false);
        if (UIPanel_Off != null) UIPanel_Off.SetActive(true);

        // 4. �������� ������� ���������� (���� ���-�� �� ���� ��������)
        OnHideAnimationComplete?.Invoke(); // ���������� �����

        // 5. ���������� ���� ��������
        activeStateChangeCoroutine = null;
    }

    private IEnumerator ShowSequence()
    {
        if (controller == null || string.IsNullOrEmpty(targetElementName))
        {
            Debug.LogError($"'{gameObject.name}': ���������� ��������� ShowSequence - ���������� ��� ��� �� ������.", this);
            activeStateChangeCoroutine = null;
            yield break;
        }

        // 1. ���������� ������ ������ �� ��������
        if (UIPane_On != null) UIPane_On.SetActive(true);
        if (UIPanel_Off != null) UIPanel_Off.SetActive(false);
        // �����: ����� ������������� ��������� ���� ����� SetActive(true),
        // ����� Ui_Controlling ��������� ��������� startAlpha/startPos,
        // �������� ���� CanvasGroup ��� �������� ����������� ��� ����� ���� 0.
        yield return null; // ��������� ����� �� ������ ������

        // 2. ��������� �������� ���������
        float duration = (overrideShowDuration >= 0) ? overrideShowDuration : controller.defaultDuration;
        controller.AnimateIn(targetElementName, duration /*, customMoveCurve, customFadeCurve */);

        // 3. ���� ��������� ����� ����������
        if (duration > 0)
        {
            yield return new WaitForSeconds(duration);
        }

        // 4. ��������, ��� ������ ����� �������� (�� ������ ������ ��������)
        // if (UIPane_On != null) UIPane_On.SetActive(true); // ������ �� �����, �.�. ������ � ������

        // 5. �������� ������� ����������
        OnShowAnimationComplete?.Invoke();

        // 6. ���������� ���� ��������
        activeStateChangeCoroutine = null;
    }

    // --- �������� ���� ������ ������ �� ������ ����� ---

    /// <summary>
    /// ������ �������������: �������� ��� ������ ��� ������ ���� (���� �����).
    /// </summary>
    public void OnStartPlayAnim() // ������������ ��� �������, �.�. ��� �� ������ ��������
    {
        Debug.Log($"'{gameObject.name}': ������ OnStartPlayAnim - �������� ������ '{targetElementName}'.");
        HidePanelAnimated();
    }

    /// <summary>
    /// ������ �������������: ����������� ��������� ������.
    /// </summary>
    public void OnAimationPanel() // ������������, ��� �� ������ ��������
    {
        Debug.Log($"'{gameObject.name}': ������ OnAimationPanel - ����������� ������ '{targetElementName}'.");
        TogglePanelAnimated();
    }
}