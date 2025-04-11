using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Controller : MonoBehaviour
{
    [SerializeField] public Rigidbody2D platform;
    public float RotationSpeed = 2000f;
    void Update()
    {
        OnRotatePlatform();
    }

    public void OnRotatePlatform()
    {
        float dir_Rotate = Input.GetAxis("Horizontal") * RotationSpeed * Time.fixedDeltaTime;
        platform.AddTorque(-dir_Rotate);
    }

}
