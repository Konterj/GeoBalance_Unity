using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FalseOrTruePauseButton : MonoBehaviour
{
    public bool IsPausedCollised = false;
    public Button pauseBtn;
    public void Update()
    {
        if (IsPausedCollised)
        {
            pauseBtn.enabled = false;
        }
        else
        {
            pauseBtn.enabled = true;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject)
        {
            IsPausedCollised = true;
        }   
    }
}
