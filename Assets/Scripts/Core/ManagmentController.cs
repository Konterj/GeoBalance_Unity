using System.Collections.Generic;
using UnityEngine;

public class ManagmentController : MonoBehaviour
{
    public List<Controller> controllers;


    public void SetController()
    {
        bool IsMobile = Application.isMobilePlatform;
        for (int i = 0; i < controllers.Count; i++) 
        {
            controllers[i].ControllerList.enabled = false;
        }
        if (IsMobile)
        {
            Debug.Log("Is mobile");
            controllers[0].GetObject.SetActive(false);
            controllers[1].Equals(true);
            controllers[1].GetObject.SetActive(true);
        }
        else if(!IsMobile)
        {
            Debug.Log("Is not mobile");
            controllers[0].Equals(true);
            controllers[0].GetObject.SetActive(true);
            controllers[1].GetObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class Controller
{
    public string name;
    public MonoBehaviour ControllerList;
    public GameObject GetObject;
}
