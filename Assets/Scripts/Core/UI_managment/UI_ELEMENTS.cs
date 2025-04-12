
using UnityEngine;
[System.Serializable]
public class UI_ELEMENTS
{
    [Tooltip("Уникальное имя для идентификации элемента в коде")]
    public string name;
    [Tooltip("Описание для чего эта кнопка")]
    public string description;
    [Tooltip("Звук для кнопки")]
    public AudioClip audioClip;
    [Header("Default Animation Curves")]
    [Tooltip("Кривая по умолчанию для анимации движения")]
    public AnimationCurve movePose;
    [Tooltip("Кривая по умолчанию для анимации затухания (альфа-канала)")]
    public AnimationCurve fadeOut;
    [Tooltip("Кривая по умолчанию для анимации появления (альфа-канала)")]
    public AnimationCurve fadeIn;
    [Header("Object reference")]
    [Tooltip("Ссылка на компонент которая отвечает за размер, и положение")]
    public RectTransform ObjectUI;
    [Tooltip("Компонент для управления прозрачностью. Будет найден или добавлен автоматически, если не указан.")]
    public CanvasGroup canvasGroup;
    //Position Coordinate
    [Header("Target position for move")]
    [Tooltip("Нужен для анимации, укажите позицию")]
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