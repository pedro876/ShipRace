using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GyroFacade : MonoBehaviour
{
    public Quaternion RawAttitude => gyroEnabled ? GyroToUnity() : Quaternion.identity;
    public Quaternion Attitude => gyroRef * RawAttitude;
    public bool gyroEnabled { get; private set; } = false;

    public event Action onGyroEnabled;
    Quaternion correctionQuaternion;
    Quaternion gyroRef;

    public void Recalibrate()
    {
        gyroRef = Quaternion.Inverse(RawAttitude);
    }

    void Start()
    {
        EnableGyroscope();
        correctionQuaternion = Quaternion.Euler(90f, 0f, 0f);
        gyroRef = Quaternion.identity;
    }

    void Update()
    {
        EnableGyroscope();
        transform.rotation = Attitude;
    }

    void EnableGyroscope()
    {
        if (UnityEngine.InputSystem.Gyroscope.current != null && AttitudeSensor.current != null)
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
            InputSystem.EnableDevice(AttitudeSensor.current);
            if (!gyroEnabled)
            {
                gyroEnabled = true;
                onGyroEnabled?.Invoke();
            }
        }
    }

    Quaternion GyroToUnity()
    {
        Quaternion q = AttitudeSensor.current.attitude.ReadValue();
        return correctionQuaternion * new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
