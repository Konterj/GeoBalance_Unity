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
        float dir_z = Input.acceleration.y;
        dir_z = Mathf.Clamp(dir_z, -0.5f, 0.5f);
        float torque = dir_z * SpeedRotated * TimeDeltaControl.ControlDeltaTime;
        
        if (torque > 0 && platform.angularVelocity < MaxAngularVelocity || torque < 0 && platform.angularVelocity > -MaxAngularVelocity) 
        {
            platform.AddTorque(torque);
        }
        //clamp
        platform.angularVelocity = Mathf.Clamp(platform.angularVelocity, -MaxAngularVelocity, MaxAngularVelocity);
        
    }

    public void OnDisplayShowDebug()
    {
        // �������� ������ �������������
        Vector3 accel = Input.acceleration;
        string accelInfo = $"Accelerometer:\n  x: {accel.x:F2}, y: {accel.y:F2}, z: {accel.z:F2}";

        // �������������� ������ ��� ���������� � ���������
        string gyroInfo = "";

        // ��������� ��������� ��������� ��������
        if (SystemInfo.supportsGyroscope)
        {
            // ���� �������� �� �������, �������� ���
            if (!Input.gyro.enabled)
                Input.gyro.enabled = true;

            // ������ ������ ���������
            Vector3 rotationRate = Input.gyro.rotationRate;         // �������� �������� � ��������/���
            Vector3 userAcceleration = Input.gyro.userAcceleration;   // ��������� ��� ����������
            Quaternion attitude = Input.gyro.attitude;                // ���������� ����������

            gyroInfo = $"Gyroscope:\n" +
                       $"  Rotation Rate (deg/sec): x: {rotationRate.x:F2}, y: {rotationRate.y:F2}, z: {rotationRate.z:F2}\n" +
                       $"  User Acceleration:      x: {userAcceleration.x:F2}, y: {userAcceleration.y:F2}, z: {userAcceleration.z:F2}\n" +
                       $"  Attitude (Euler angles): {attitude.eulerAngles}";
        }
        else
        {
            gyroInfo = "Gyroscope is not supported on this device.";
        }

        // ������� ����� ������� ������� �������� ��������� � ��������� ��������
        string extraInfo = $"Platform Angular Velocity: {platform.angularVelocity:F2}\n" +
                           $"Supports Accelerometer: {SystemInfo.supportsAccelerometer}\n" +
                           $"Supports Gyroscope:     {SystemInfo.supportsGyroscope}";

        // �������� ��� ������ � ������ �����
        debug.text = $"{accelInfo}\n\n{gyroInfo}\n\n{extraInfo}";
    }

}
