using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerRail : MonoBehaviour
{
    PlayerController player;
    Vector3 displacement;

    static readonly Vector3 forward = Vector3.forward;
    static readonly Vector3 up = Vector3.up;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        displacement = player.transform.InverseTransformDirection(transform.position - player.transform.position);
    }

    private void Update()
    {
        transform.position = player.RailPosition + player.transform.TransformDirection(displacement);
        Vector3 forward = player.RailRotation * FollowPlayerRail.forward;
        Vector3 up = player.RailRotation * FollowPlayerRail.up;
        transform.rotation = Quaternion.LookRotation(forward, up);
    }
}
