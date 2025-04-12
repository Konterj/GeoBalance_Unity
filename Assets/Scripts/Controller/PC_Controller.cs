using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Controller : MonoBehaviour
{
    [SerializeField] public Rigidbody2D platform;
    public float RotationSpeed = 2000f;
    public float MaxAngle = 80f;
    public float BrakingDamp = 0.95f;
    void Update()
    {
        OnRotatePlatform();
    }

    public void OnRotatePlatform()
    {
        float dir_Rotate = Input.GetAxis("Horizontal") * RotationSpeed;

        if (dir_Rotate < 0 && platform.rotation < MaxAngle || dir_Rotate > 0&&platform.rotation > -MaxAngle)
        {
            platform.AddTorque(-dir_Rotate * Time.fixedDeltaTime);
        }
        else
        {
            platform.angularVelocity *= BrakingDamp;
        }
    }

}
