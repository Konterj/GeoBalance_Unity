using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class ButtonTriggerOnClick : MonoBehaviour
{

    [Header("Связь с Контроллером")]
    [Tooltip("Перетащите сюда GameObject, на котором висит ваш скрипт Ui_Controlling")]
    public Ui_Controlling uiController;

    [Header("Цель Анимации")]
    [Tooltip("Точное имя элемента из списка 'Ui Elements' в Ui_Controlling, которым управляет эта кнопка")]
    public string targetElementName;

    [Tooltip("Действие, выполняемое при клике на эту кнопку")]
    public actionOnClick ActionOnClick = actionOnClick.AnimateOut;
    public enum actionOnClick
    {
        AnimateOut,
        AnimateIn,
        AnimateAllIn,
        AnimateAllOut
    };

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (uiController == null) {
            Debug.LogError($"'{gameObject.name}': Ссылка на Ui_Controlling не установлена в инспекторе!", this);
            // Выключаем скрипт, чтобы он не вызывал ошибок дальше
            enabled = false; // 'enabled = false' останавливает выполнение Update, FixedUpdate и т.д. для этого скрипта
            return; // Выходим из Awake
        }
        if (string.IsNullOrEmpty(targetElementName)) 
        {
            Debug.LogError($"'{gameObject.name}': Имя целевого элемента (Target Element Name) не указано!", this);
            enabled = false;
            return;
        }
        if(button != null)
        {
            button.onClick.AddListener(HandleClick);
        }
        else
        {
            Debug.LogError($"'{gameObject.name}': Компонент Button не найден, хотя он требуется!", this);
            enabled = false;
        }
    }

    public void HandleClick()
    {
        if (uiController == null || string.IsNullOrEmpty(targetElementName))
        {
            Debug.LogError($"'{gameObject.name}': Невозможно выполнить HandleClick - не хватает данных!", this);
            return;
        }
        switch(ActionOnClick)
        {
            case actionOnClick.AnimateOut:
                uiController.AnimateOut(targetElementName); 
                break;
            case actionOnClick.AnimateIn: 
                uiController.AnimateIn(targetElementName);
                break;
            case actionOnClick.AnimateAllIn:
                // Анимировать ВСЕ элементы, определенные в контроллере
                uiController.AnimateAllIn();
                break;

            case actionOnClick.AnimateAllOut:
                // Анимировать ВСЕ элементы, определенные в контроллере
                uiController.AnimateAllOut(); // Вызываем новый метод
                break;
            default:
                Debug.LogWarning($"'{gameObject.name}': Неизвестное действие '{ActionOnClick}'", this);
                break;
        }
    }

    public void OnDestroy()
    {
        if (button != null) {
        
        button.onClick.RemoveListener(HandleClick);
        }

    }
}
