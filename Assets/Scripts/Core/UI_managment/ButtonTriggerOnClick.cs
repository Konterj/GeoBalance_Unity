using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class ButtonTriggerOnClick : MonoBehaviour
{

    [Header("����� � ������������")]
    [Tooltip("���������� ���� GameObject, �� ������� ����� ��� ������ Ui_Controlling")]
    public Ui_Controlling uiController;

    [Header("���� ��������")]
    [Tooltip("������ ��� �������� �� ������ 'Ui Elements' � Ui_Controlling, ������� ��������� ��� ������")]
    public string targetElementName;

    [Tooltip("��������, ����������� ��� ����� �� ��� ������")]
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
            Debug.LogError($"'{gameObject.name}': ������ �� Ui_Controlling �� ����������� � ����������!", this);
            // ��������� ������, ����� �� �� ������� ������ ������
            enabled = false; // 'enabled = false' ������������� ���������� Update, FixedUpdate � �.�. ��� ����� �������
            return; // ������� �� Awake
        }
        if (string.IsNullOrEmpty(targetElementName)) 
        {
            Debug.LogError($"'{gameObject.name}': ��� �������� �������� (Target Element Name) �� �������!", this);
            enabled = false;
            return;
        }
        if(button != null)
        {
            button.onClick.AddListener(HandleClick);
        }
        else
        {
            Debug.LogError($"'{gameObject.name}': ��������� Button �� ������, ���� �� ���������!", this);
            enabled = false;
        }
    }

    public void HandleClick()
    {
        if (uiController == null || string.IsNullOrEmpty(targetElementName))
        {
            Debug.LogError($"'{gameObject.name}': ���������� ��������� HandleClick - �� ������� ������!", this);
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
                // ����������� ��� ��������, ������������ � �����������
                uiController.AnimateAllIn();
                break;

            case actionOnClick.AnimateAllOut:
                // ����������� ��� ��������, ������������ � �����������
                uiController.AnimateAllOut(); // �������� ����� �����
                break;
            default:
                Debug.LogWarning($"'{gameObject.name}': ����������� �������� '{ActionOnClick}'", this);
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
