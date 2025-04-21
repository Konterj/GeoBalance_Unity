using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTrash : MonoBehaviour
{
    public GameManager manager;
    public Timer_Controller controller;
    public Animation_On_Click click;
    bool isFall = false;
    bool isLastmove = true;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isFall = true;
        if (isFall)
        {
            if (isLastmove) 
            {
                manager.OnShowPanelGameOver();
                isLastmove = false;
                Debug.Log("this is last fall");
            }
        }
        Destroy(collision.gameObject);
    }

    public void OnStartTrueLastObject()
    {
        isLastmove=true;
    }
}
