using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    // The vertices
    public Vector3 v0;
    public Vector3 v1;

    // The indices in a mesh
    public int i0;
    public int i1;

    public Edge(ref Vector3 v0, ref Vector3 v1)
    {
        this.v0 = v0;
        this.v1 = v1;
        i0 = 0;
        i1 = 1;
    }

    public Edge(ref Vector3 v0, ref Vector3 v1, int i0, int i1)
    {
        this.v0 = v0;
        this.v1 = v1;
        this.i0 = i0;
        this.i1 = i1;
    }
}
