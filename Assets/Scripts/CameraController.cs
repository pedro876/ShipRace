using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    PlayerController target;
    Vector3 distanceToTarget;
    GyroFacade gyro;
    LevelManager level;
    [SerializeField] float rotLerp = 10f;
    [SerializeField] float posLerp = 10f;

    // Start is called before the first frame update
    void Awake()
    {
        gyro = FindObjectOfType<GyroFacade>();
        level = FindObjectOfType<LevelManager>();
        target = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        distanceToTarget = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //level.currentSection.rail.Project(transform.position, out var projPos, out var projRot);
        Quaternion projRot = target.RailRotation;
        Vector3 camUp = Vector3.ProjectOnPlane(projRot * gyro.Attitude * Vector3.up, projRot * Vector3.forward);
        Quaternion targetRot = Quaternion.LookRotation(projRot * Vector3.forward, camUp);
        Vector3 targetPos = target.transform.position + projRot * distanceToTarget;
        transform.position = Vector3.Lerp(transform.position, targetPos, posLerp * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotLerp * Time.fixedDeltaTime);
    }
}
