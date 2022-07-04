using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyFollow : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField]private Transform target;
    [SerializeField] bool posLerp = true;
    [SerializeField] bool rotLerp = true;
    [SerializeField] float posLerpFactor = 20f;
    [SerializeField] float rotLerpFactor = 20f;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null) return;

        if (posLerp)
        {
            rb.MovePosition(Vector3.Lerp(rb.position, target.position, posLerpFactor * Time.fixedDeltaTime));
        }
        else
        {
            rb.MovePosition(target.position);
        }
        if (rotLerp)
        {
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, target.rotation, rotLerpFactor * Time.fixedDeltaTime));
        }
        else
        {
            rb.MovePosition(target.position);
        }
    }
}
