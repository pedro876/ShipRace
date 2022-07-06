using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullInput : IShipInput
{
    public event Action onDashRight;
    public event Action onDashLeft;

    private readonly Vector2 sideMotion;

    public NullInput()
    {
        sideMotion = Vector2.zero;
    }

    public Vector2 GetRawSideMotion()
    {
        return sideMotion;
    }

    public Vector2 GetSideMotion()
    {
        return sideMotion;
    }

    public float GetTilt()
    {
        return 0f;
    }

    public bool IsSlowingDown()
    {
        return false;
    }

    public bool IsSpeedingUp()
    {
        return false;
    }
}
