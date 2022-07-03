using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputAdapter : MonoBehaviour, IShipInput
{
    public event Action onDashRight;
    public event Action onDashLeft;

    Vector2 rawSideMotion = Vector2.zero;
    Vector2 sideMotion = Vector2.zero;
    float tilt = 0f;
    bool slowingDown = false;

    [SerializeField] float sideMotionLerpFactor = 10f;
    //[SerializeField] float sideMotionExponent = 0.5f;

    public void OnTilt(InputValue value)
    {
        tilt = value.Get<float>();
    }

    public void OnDashRight()
    {
        onDashRight?.Invoke();
    }

    public void OnDashLeft()
    {
        onDashLeft.Invoke();
    }

    public void OnSideMotion(InputValue value)
    {
        rawSideMotion = value.Get<Vector2>();
        Debug.Log(rawSideMotion);
    }

    private void Update()
    {
        sideMotion = Vector2.Lerp(sideMotion, rawSideMotion, sideMotionLerpFactor * Time.deltaTime);
    }

    #region API

    public Vector2 GetRawSideMotion()
    {
        return rawSideMotion;
    }

    public Vector2 GetSideMotion()
    {
        return sideMotion;
    }

    public float GetTilt()
    {
        return tilt;
    }

    public bool IsSlowingDown()
    {
        return slowingDown;
    }

    #endregion
}
