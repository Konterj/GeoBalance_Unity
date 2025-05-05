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
        // Получаем данные акселерометра
        Vector3 accel = Input.acceleration;
        string accelInfo = $"Accelerometer:\n  x: {accel.x:F2}, y: {accel.y:F2}, z: {accel.z:F2}";

        // Инициализируем строку для информации о гироскопе
        string gyroInfo = "";

        // Проверяем поддержку гироскопа системой
        if (SystemInfo.supportsGyroscope)
        {
            // Если гироскоп не включен, включаем его
            if (!Input.gyro.enabled)
                Input.gyro.enabled = true;

            // Читаем данные гироскопа
            Vector3 rotationRate = Input.gyro.rotationRate;         // Скорость вращения в градусах/сек
            Vector3 userAcceleration = Input.gyro.userAcceleration;   // Ускорение без гравитации
            Quaternion attitude = Input.gyro.attitude;                // Ориентация устройства

            gyroInfo = $"Gyroscope:\n" +
                       $"  Rotation Rate (deg/sec): x: {rotationRate.x:F2}, y: {rotationRate.y:F2}, z: {rotationRate.z:F2}\n" +
                       $"  User Acceleration:      x: {userAcceleration.x:F2}, y: {userAcceleration.y:F2}, z: {userAcceleration.z:F2}\n" +
                       $"  Attitude (Euler angles): {attitude.eulerAngles}";
        }
        else
        {
            gyroInfo = "Gyroscope is not supported on this device.";
        }

        // Выводим также текущую угловую скорость платформы и поддержку сенсоров
        string extraInfo = $"Platform Angular Velocity: {platform.angularVelocity:F2}\n" +
                           $"Supports Accelerometer: {SystemInfo.supportsAccelerometer}\n" +
                           $"Supports Gyroscope:     {SystemInfo.supportsGyroscope}";

        // Собираем все данные в единый вывод
        debug.text = $"{accelInfo}\n\n{gyroInfo}\n\n{extraInfo}";
    }

}
