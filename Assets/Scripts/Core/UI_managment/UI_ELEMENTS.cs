
using UnityEngine;
[System.Serializable]
public class UI_ELEMENTS
{
    [Tooltip("���������� ��� ��� ������������� �������� � ����")]
    public string name;
    [Tooltip("�������� ��� ���� ��� ������")]
    public string description;
    [Tooltip("���� ��� ������")]
    public AudioClip audioClip;
    [Header("Default Animation Curves")]
    [Tooltip("������ �� ��������� ��� �������� ��������")]
    public AnimationCurve movePose;
    [Tooltip("������ �� ��������� ��� �������� ��������� (�����-������)")]
    public AnimationCurve fadeOut;
    [Tooltip("������ �� ��������� ��� �������� ��������� (�����-������)")]
    public AnimationCurve fadeIn;
    [Header("Object reference")]
    [Tooltip("������ �� ��������� ������� �������� �� ������, � ���������")]
    public RectTransform ObjectUI;
    [Tooltip("��������� ��� ���������� �������������. ����� ������ ��� �������� �������������, ���� �� ������.")]
    public CanvasGroup canvasGroup;
    //Position Coordinate
    [Header("Target position for move")]
    [Tooltip("����� ��� ��������, ������� �������")]
    public Vector2 targetPos;
    [HideInInspector] public Vector2 originalPose;
    [HideInInspector] public float originalAlpha = 1f;


    public void OnSetDefaultValue()
    {
        if(ObjectUI != null)
        {
            originalPose = ObjectUI.anchoredPosition;
        }
        if(canvasGroup != null)
        {
            originalAlpha = canvasGroup.alpha;
        }
        else
        {
            originalAlpha = 1f;
        }
    }
}