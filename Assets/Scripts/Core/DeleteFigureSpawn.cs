using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteFigureSpawn : MonoBehaviour
{
    [SerializeField] public GameManager manager;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!manager.isPlay)
        {
            Destroy(collision.gameObject);
        }
        else
        {
            //Nothing
        }
    }
}
