using System.Collections.Generic;
using UnityEngine;
using YG;


public class ManagmentController : MonoBehaviour
{
    public List<Controller> controllers;
    public bool isMobile = true;
    public bool isControlling = false;
    public void SetController()
    {
        Debug.Log("ChoseController" + YG2.saves.SaveController_ForMobile);
        if (isControlling)
        {        //bool IsMobile = Application.isMobilePlatform;
            for (int i = 0; i < controllers.Count; i++)
            {
                controllers[i].ControllerList.enabled = true;

            }
            if (Application.isMobilePlatform)
            {
                controllers[1].ObjectUI.SetActive(true);
                Debug.Log("Is mobile");
                //Switch
                switch(YG2.saves.SaveController_ForMobile)
                {
                    case 0:
                        controllers[0].ObjectUI.SetActive(false);
                        controllers[2].ControllerList.enabled = true;
                        controllers[2].ObjectUI.SetActive(true); break;
                        case 1:
                        controllers[0].ObjectUI.SetActive(false);
                        controllers[3].ObjectUI.SetActive(true);
                        controllers[3].ControllerList.enabled = true; break;
                }
            }
            else if (!Application.isMobilePlatform)
            {
                Debug.Log("Is not mobile");
                controllers[0].ControllerList.enabled = true;
                controllers[0].ObjectUI.SetActive(true);
                //Refactoring pls
                controllers[1].ObjectUI.SetActive(false);
                controllers[2].ObjectUI.SetActive(false);
                controllers[2].ControllerList.enabled = false;
            }
        }
        else if(!isControlling)
        {
            for (int i = 0; i < controllers.Count; i++) 
            {
                controllers[i].ObjectUI.SetActive(false);
                controllers[i].ControllerList.enabled = false;
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
