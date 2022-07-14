using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] float minAngularSpeed = 0f;
    [SerializeField] float maxAngularSpeed = 10f;
    [SerializeField] Vector3 axis = Vector3.forward;

    Rigidbody rb;
    float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rotSpeed = Random.Range(minAngularSpeed, maxAngularSpeed) * Mathf.Sign(Random.Range(-1f, 1f));
    }


    private void FixedUpdate()
    {
        Vector3 worldAxis = transform.TransformDirection(axis);
        rb.MoveRotation(Quaternion.AngleAxis(rotSpeed * Time.fixedDeltaTime, worldAxis) * transform.rotation);
    }
}
