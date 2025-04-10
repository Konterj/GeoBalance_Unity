using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D platform;
    public float SpeedRotated = 5f;

    // Update is called once per frame
    void Update()
    {
        OnControllingRotated();
    }

    public void OnControllingRotated()
    {
        Vector3 dir = Vector3.zero;
        dir.z = Input.acceleration.x;

        if(dir.sqrMagnitude > 1) dir.Normalize();
        
        if(platform.angularVelocity < 80)
        {
            platform.AddTorque(dir.sqrMagnitude * SpeedRotated * Time.fixedDeltaTime);
        }
        else if(platform.angularVelocity > -80)
        {
            platform.AddTorque(dir.sqrMagnitude * SpeedRotated * Time.fixedDeltaTime);
        }
        if (platform.angularVelocity > 180 || platform.angularVelocity < -180)
        {
            platform.angularVelocity *= 0.95f;
        }
    }
}
