using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDeltaControl : MonoBehaviour
{
    [HideInInspector]
    [SerializeField] public static float ControlDelta { get; set; } = 1f;
    [HideInInspector]   
    [SerializeField] public static float AnimDelta{ get; set; } = 1f;

    public static float ControlDeltaTime => Time.deltaTime * ControlDelta;
    public static float AnimDeltaTime => Time.unscaledDeltaTime * AnimDelta;

}
