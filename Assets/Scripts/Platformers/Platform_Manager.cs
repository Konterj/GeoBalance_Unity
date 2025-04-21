using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Manager : MonoBehaviour
{
    public FixedJoint2D platform;
    public float minFrequency = 0.05f;
    public float maxFrequency = 2f;
    public float DurationSmooth = 1f;
    public float LerpTime;
    // Update is called once per frame
    public void OnSetWhenWe_GameOver(bool isPlay)
    {   
            LerpTime = Mathf.Clamp01(LerpTime);
           
            if (isPlay)
            {
                LerpTime += Time.deltaTime / DurationSmooth;
                platform.frequency = Mathf.Lerp(maxFrequency, minFrequency, LerpTime);
            }
            else
            {   
                LerpTime -= Time.deltaTime / DurationSmooth;
                platform.frequency = Mathf.Lerp(maxFrequency, minFrequency, LerpTime);
            }
        
    }
}
