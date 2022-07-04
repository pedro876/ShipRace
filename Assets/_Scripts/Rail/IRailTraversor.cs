using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRailTraversor
{
    public Vector3 GetRailPosition();
    public Quaternion GetRailRotation();
}
