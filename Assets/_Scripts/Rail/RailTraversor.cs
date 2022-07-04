using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailTraversor : MonoBehaviour, IRailTraversor
{
    Rail rail;
    Vector3 railPos = Vector3.zero;
    Quaternion railRot = Quaternion.identity;

    public Vector3 GetRailPosition() => railPos;
    public Quaternion GetRailRotation() => railRot;

    public void SetRail(Rail newRail)
    {
        this.rail = newRail;
        Project();
    }

    private void FixedUpdate()
    {
        if(rail != null)
        {
            Project();
        }
    }

    private void Project()
    {
        rail.Project(transform.position, out railPos, out railRot);
    }
}
