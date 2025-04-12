using System.Collections.Generic;
using UnityEngine;


public class ManagmentController : MonoBehaviour
{
    public List<Controller> controllers;
    public bool isMobile = true;
    
    public void SetController()
    {

        //bool IsMobile = Application.isMobilePlatform;
        for (int i = 0; i < controllers.Count; i++) 
        {
            controllers[i].ControllerList.enabled = false;
        }
        if (isMobile)
        {
            Debug.Log("Is mobile");
            controllers[0].ObjectUI.SetActive(false);
            controllers[1].ControllerList.enabled = true;
            controllers[1].ObjectUI.SetActive(true);
        }
        else if(!isMobile)
        {
            Debug.Log("Is not mobile");
            controllers[0].ControllerList.enabled=true;
            controllers[0].ObjectUI.SetActive(true);
            controllers[1].ObjectUI.SetActive(false);
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
