using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotCamera : MonoBehaviour
{
    [SerializeField] Transform lookAt;
    [SerializeField] float radius = 1f;
    [SerializeField] float angularVelocity = 20f;
    Vector3 pos;
    Vector3 forward;

    private void Start()
    {
        pos = transform.position;
        forward = transform.forward;
        transform.position += transform.up * radius;
    }

    void FixedUpdate()
    {
        transform.RotateAround(pos, forward, angularVelocity * Time.fixedDeltaTime);
        transform.LookAt(lookAt.position);
    }
}