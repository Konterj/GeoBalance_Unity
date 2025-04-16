using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTrash : MonoBehaviour
{
    public GameManager manager;
    public Timer_Controller controller;
    bool isFall = false;
    bool isLastmove = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isFall = true;
        isLastmove = true;
        if (isFall)
        {
            manager.OnShowPanelGameOver();
            if (isLastmove) 
            {
                manager.OnShowPanelGameOver();
                isLastmove = false;
                Debug.Log("this is last fall");
            }
        }
        manager.OnShowPanelGameOver();
        controller.OnSaveValueTimerLast();
        Destroy(collision.gameObject);
    }
}
