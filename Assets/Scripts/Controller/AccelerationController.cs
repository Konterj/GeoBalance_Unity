using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AccelerationController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D platform;
    public float SpeedRotated = 5f;
    public float MaxAngularVelocity = 80f;
    public TextMeshProUGUI debug;

    // Update is called once per frame
    void Update()
    {
        OnControllingRotated();
        OnDisplayShowDebug();
    }

    public void OnControllingRotated()
    {
        float dir = Input.acceleration.x;
        float torque = dir * SpeedRotated * Time.fixedDeltaTime;

        if (torque > 0 && platform.angularVelocity < MaxAngularVelocity || torque < 0 && platform.angularVelocity > -MaxAngularVelocity) 
        {
            platform.AddTorque(torque);
        }

        if (platform.angularVelocity > 180 || platform.angularVelocity < -180)
        {
            platform.angularVelocity *= 0.95f;
        }
        //clamp
        platform.angularVelocity = Mathf.Clamp(platform.angularVelocity, -MaxAngularVelocity, MaxAngularVelocity);
    }

    public void OnDisplayShowDebug()
    {
        debug.text = $"Dir.z: {Input.acceleration.x:F2},            platform.angular: {platform.angularVelocity}";
    }
}
