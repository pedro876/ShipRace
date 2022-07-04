using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullGyroSystem : IGyroSystem
{
    Quaternion noRot;

    public NullGyroSystem()
    {
        noRot = Quaternion.identity;
    }

    public Quaternion GetAttitude()
    {
        return noRot;
    }

    public Quaternion GetRawAttitude()
    {
        return noRot;
    }

    public bool IsWorking()
    {
        return false;
    }

    public void Recalibrate()
    {
        
    }
}
