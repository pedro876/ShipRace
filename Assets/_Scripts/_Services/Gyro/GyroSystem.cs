using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GyroSystem : MonoBehaviour, IGyroSystem
{
    private Quaternion rawAttitude = Quaternion.identity;
    private Quaternion attitude = Quaternion.identity;
    private bool working = false;
    //public event Action onGyroConnected;
    private Quaternion correctionQuaternion;
    private Quaternion gyroRef;

    public Quaternion GetRawAttitude() => rawAttitude;
    public Quaternion GetAttitude() => attitude;
    public bool IsWorking() => working;
    public void Recalibrate() => gyroRef = Quaternion.Inverse(rawAttitude);

    private void Start()
    {
        TryEnableGyroscope();
        correctionQuaternion = Quaternion.Euler(90f, 0f, 0f);
        gyroRef = Quaternion.identity;

        GameManager.instance.onStateChanged += state =>
        {
            if (state == GameManager.GameState.CountDown)
            {
                Recalibrate();
            }
        };
    }

    private void Update()
    {
        TryEnableGyroscope();
        if (working && !GameManager.instance.IsUsingGamepad)
        {
            rawAttitude = GyroToUnity();
            attitude = gyroRef * rawAttitude;
            
        }
        else
        {
            rawAttitude = Quaternion.identity;
            attitude = rawAttitude;
        }
        transform.rotation = attitude;
    }

    private void TryEnableGyroscope()
    {
#if !UNITY_WEBGL
        if (UnityEngine.InputSystem.Gyroscope.current != null && AttitudeSensor.current != null)
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
            InputSystem.EnableDevice(AttitudeSensor.current);
            if (!working)
            {
                working = true;
                //onGyroConnected?.Invoke();
                GameManager.serviceLocator.RegisterService<IGyroSystem>(this);
                Debug.Log("Gyro is enabled and working");
            }
        }
#endif
    }

    private Quaternion GyroToUnity()
    {
        Quaternion q = AttitudeSensor.current.attitude.ReadValue();
        return correctionQuaternion * new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    
}
