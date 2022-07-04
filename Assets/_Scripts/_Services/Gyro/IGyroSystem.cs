using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGyroSystem
{
    public Quaternion GetRawAttitude();
    public Quaternion GetAttitude();
    public void Recalibrate();
    public bool IsWorking();
}
