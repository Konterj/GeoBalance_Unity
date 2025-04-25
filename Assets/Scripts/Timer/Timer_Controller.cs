using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;
using YG.Insides;
using YG.Utils;
public class Timer_Controller : MonoBehaviour
{
    [SerializeField] public float CurrentTimer;
    [SerializeField] public float PreviousTimer;
    [SerializeField] public TextMeshProUGUI TimerText;
    [SerializeField] public TextMeshProUGUI TimerRecords;
    public void OnUpdateTime()
    {
        CurrentTimer += Time.deltaTime;
        PreviousTimer = CurrentTimer;
        OnDisplayShowTimer();
        OnSaveValueTimerLast();
    }
    public void OnDisplayShowTimer()
    {
        TimerText.text = $"{CurrentTimer:F2}";
        TimerRecords.text = $"{YG2.saves.maxTimer:F2}";
    }

    public void OnSaveValueTimerLast()
    {
        PreviousTimer = CurrentTimer;
        if (PreviousTimer > YG2.saves.maxTimer)
        {
            YG2.saves.maxTimer = PreviousTimer;
            YG2.SaveProgress();
        }

    }
}
