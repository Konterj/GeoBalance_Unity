using System.Collections.Generic;
using UnityEngine;


public class ManagmentController : MonoBehaviour
{
    public List<Controller> controllers;
    public bool isMobile = true;
    public bool isControlling = false;
    public void SetController()
    {
        if (isControlling)
        {        //bool IsMobile = Application.isMobilePlatform;
            for (int i = 0; i < controllers.Count; i++)
            {
                controllers[i].ControllerList.enabled = false;
            }
            if (Application.isMobilePlatform)
            {
                Debug.Log("Is mobile");
                controllers[0].ObjectUI.SetActive(false);
                controllers[1].ControllerList.enabled = true;
                controllers[1].ObjectUI.SetActive(true);
            }
            else if (!Application.isMobilePlatform)
            {
                Debug.Log("Is not mobile");
                controllers[0].ControllerList.enabled = true;
                controllers[0].ObjectUI.SetActive(true);
                controllers[1].ObjectUI.SetActive(false);
            }
        }
        else if(!isControlling)
        {
            for (int i = 0; i < controllers.Count; i++) 
            {
                controllers[i].ObjectUI.SetActive(false);
                controllers[i].ControllerList.enabled =false;
            }
        }

    }
}

[System.Serializable]
public class Controller
{
    public string name;
    public MonoBehaviour ControllerList;
    public GameObject ObjectUI;
}
