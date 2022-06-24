using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
    // The vertices
    public Vector3 v0;
    public Vector3 v1;
    public Vector3 v2;

    // The indices in a mesh
    public int i0;
    public int i1;
    public int i2;

    public Triangle(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2)
    {
        this.v0 = v0;
        this.v1 = v1;
        this.v2 = v2;
        i0 = 0;
        i1 = 1;
        i2 = 2;
    }

    public Triangle(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2, int i0, int i1, int i2)
    {
        this.v0 = v0;
        this.v1 = v1;
        this.v2 = v2;
        this.i0 = i0;
        this.i1 = i1;
        this.i2 = i2;
    }

    public void DrawOnGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(this.v0, this.v1);
        Gizmos.DrawLine(this.v1, this.v2);
        Gizmos.DrawLine(this.v0, this.v2);
    }

    public void DrawOnGizmos(Color color, Vector3 displacement)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(this.v0 + displacement, this.v1 + displacement);
        Gizmos.DrawLine(this.v1 + displacement, this.v2 + displacement);
        Gizmos.DrawLine(this.v0 + displacement, this.v2 + displacement);
    }
}
