using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TunnelColliderFollow : MonoBehaviour
{
    [SerializeField] PlayerController player;

    Rigidbody rb;
    [SerializeField] float rotLerp = 8f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(player.RailPosition);
        rb.MoveRotation(Quaternion.Lerp(transform.rotation, player.RailRotation, rotLerp * Time.fixedDeltaTime));
    }
}
