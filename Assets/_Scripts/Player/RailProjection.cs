using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailProjection : MonoBehaviour
{
    IRailTraversor traversor;
    public void SetTraversor(IRailTraversor newTraversor)
    {
        this.traversor = newTraversor;
    }

    private void FixedUpdate()
    {
        if (traversor == null) return;

        transform.position = traversor.GetRailPosition();
        transform.rotation = traversor.GetRailRotation();
    }
}
