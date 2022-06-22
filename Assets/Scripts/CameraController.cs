using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    Vector3 distanceToTarget;
    GyroFacade gyro;

    // Start is called before the first frame update
    void Awake()
    {
        gyro = FindObjectOfType<GyroFacade>();
    }

    private void Start()
    {
        distanceToTarget = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camUp = Vector3.ProjectOnPlane(gyro.Attitude * Vector3.up, Vector3.forward);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, camUp);
        transform.position = target.position + distanceToTarget;
    }
}
