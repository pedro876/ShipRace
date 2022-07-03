using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IShipInput
{
    public Vector2 GetRawSideMotion(); // raw left axis
    public Vector2 GetSideMotion(); // smooth left axis
    public event Action onDashRight;
    public event Action onDashLeft;
    public float GetTilt(); // turning -1 to 1
    public bool IsSlowingDown();
}
