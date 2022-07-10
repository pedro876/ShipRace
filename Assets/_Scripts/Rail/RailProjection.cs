using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailProjection : MonoBehaviour
{
    IRailTraversor traversor;

    public float distanceTraversed { get; private set; } = 0;
    Vector3 lastPosition;

    public void SetTraversor(IRailTraversor newTraversor)
    {
        this.traversor = newTraversor;
        lastPosition = traversor.GetRailPosition();
    }

    private void FixedUpdate()
    {
        if (traversor == null) return;

        transform.position = traversor.GetRailPosition();
        transform.rotation = traversor.GetRailRotation();
        distanceTraversed = Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
    }
}
