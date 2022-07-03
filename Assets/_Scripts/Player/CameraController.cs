using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraController : MonoBehaviour
{
    Transform ship;
    Transform railPoint;
    Vector3 distanceToTarget;
    GyroFacade gyro;
    LevelManager level;
    [SerializeField] float rotLerp = 10f;
    [SerializeField] float maxDistanceFromRail = 16f;

    // Start is called before the first frame update
    void Awake()
    {
        gyro = FindObjectOfType<GyroFacade>();
        level = FindObjectOfType<LevelManager>();
    }

    public void SetTarget(Transform ship, Transform railPoint)
    {
        this.ship = ship;
        this.railPoint = railPoint;
        distanceToTarget = transform.position - ship.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Assert.IsNotNull(this.ship, $"There is no ship assigned");
        Assert.IsNotNull(this.railPoint, $"There is no rail point assigned");

        Quaternion projRot = railPoint.rotation;
        Vector3 camUp = Vector3.ProjectOnPlane(projRot * gyro.Attitude * Vector3.up, projRot * Vector3.forward);
        Quaternion targetRot = Quaternion.LookRotation(projRot * Vector3.forward, camUp);
        Vector3 targetPos = ship.position + projRot * distanceToTarget;

        Vector3 targetPosOnRail = railPoint.InverseTransformPoint(targetPos);
        float originalZ = targetPosOnRail.z;
        targetPosOnRail.z = 0f;
        if(targetPosOnRail.magnitude > maxDistanceFromRail)
        {
            targetPosOnRail = targetPosOnRail.normalized * maxDistanceFromRail;
        }
        targetPosOnRail.z = originalZ;
        targetPos = railPoint.TransformPoint(targetPosOnRail);
        transform.position = targetPos;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotLerp * Time.fixedDeltaTime);
    }
}
