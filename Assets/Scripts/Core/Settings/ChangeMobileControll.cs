using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class ChangeMobileControll : MonoBehaviour
{
    [SerializeField] public List<Btn_Name> btn_s;
    public ManagmentController Controller;
    public void AddListnersButtons()
    {
        if (btn_s != null) 
        {
            btn_s[0].btn_Change.onClick.AddListener(ButtonsChange);
            btn_s[1].btn_Change.onClick.AddListener(AccelarotChange);
        }
    }

    public void AccelarotChange()
    {
        Controller.SetController();
        YG2.saves.SaveController_ForMobile = 1;
        YG2.SaveProgress();
    }
    public void ButtonsChange()
    {
        Controller.SetController();
        YG2.saves.SaveController_ForMobile = 0;
        YG2.SaveProgress();
    }
}

[System.Serializable]
public class Btn_Name
{
    public string Name;
    public Button btn_Change;
}
