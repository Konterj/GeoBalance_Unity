using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Setting_manager : MonoBehaviour
{
    public Save_Settings Save_;
    public ChangeMobileControll ChangeMobile;

    public void OnStartGetSave()
    {
        ChangeMobile.AddListnersButtons();
        Debug.Log("Mobile: " + YG2.saves.SaveController_ForMobile);
    }
    public void OnSetTheme()
    {

    }
    public void OnSetControllerMobile()
    {

    }
}
