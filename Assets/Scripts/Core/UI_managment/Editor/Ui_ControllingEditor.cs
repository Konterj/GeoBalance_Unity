using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
[CustomEditor(typeof(Ui_Controlling))]
public class Ui_ControllingEditor : Editor
{
   public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Ui_Controlling controller = (Ui_Controlling)target;
        EditorGUILayout.Space();
        if(GUILayout.Button("��������/�������� �������� �� �������"))
        {
            AddOrUpdateTriggers(controller);
        }
        EditorGUILayout.Space();
    }

    private void AddOrUpdateTriggers(Ui_Controlling controller)
    {
        if (controller.uiElements == null || controller.uiElements.Count == 0)
        {
            Debug.LogWarning("������ uiElements ����. ������ ��������� ��������.");
            return; // �������, ���� ������ ����
        }
        int addedCount = 0;
        int updateCount = 0;
        int skippedCount = 0;

        List<Object> objectsToRecord = new List<Object> { controller };
        foreach (var element in controller.uiElements)
        {
            if(element?.ObjectUI != null)
            {
                objectsToRecord.Add(element.ObjectUI.gameObject);
            }
        }
        Undo.RecordObjects(objectsToRecord.ToArray(), "Add/Update Button Triggers");

        foreach(UI_ELEMENTS element in controller.uiElements)
        {
            if(element == null || element.ObjectUI == null)
            {
                Debug.LogWarning($"������� ��������: ������� null ��� ��� ������ �� ObjectUI.");
                skippedCount++;
                continue; // ��������� � ���������� �������� ������
            }

            Button buttonComponent = element.ObjectUI.GetComponent<Button>();
            if(buttonComponent != null)
            {
                ButtonTriggerOnClick trigger = element.ObjectUI.GetComponent<ButtonTriggerOnClick>();
                if (trigger == null) 
                {
                    trigger = Undo.AddComponent<ButtonTriggerOnClick>(element.ObjectUI.gameObject);
                    Debug.Log($"�������� ButtonAnimationTrigger � '{element.name}'.", element.ObjectUI.gameObject);
                    addedCount++;
                }
                else
                {
                    Debug.Log($"ButtonAnimationTrigger ��� ���������� �� '{element.name}', ��������� ���������.", element.ObjectUI.gameObject);
                    updateCount++;
                }
                trigger.uiController = controller;
                trigger.targetElementName = element.name;
                trigger.ActionOnClick = ButtonTriggerOnClick.actionOnClick.AnimateOut;
                EditorUtility.SetDirty(trigger.gameObject);
            }
            else
            {
                Debug.Log($"������� �������� '{element.name}': ����������� ��������� Button.", element.ObjectUI.gameObject);
                skippedCount++;
            }
        }
        Debug.Log($"��������� ��������� ���������. ���������: {addedCount}, ���������: {updateCount}, ���������: {skippedCount}. �� �������� ��������� �����!");
    }
}
