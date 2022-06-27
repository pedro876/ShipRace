using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    PlayerController target;
    [SerializeField] Transform playerRailPoint;
    Vector3 distanceToTarget;
    GyroFacade gyro;
    LevelManager level;
    [SerializeField] float rotLerp = 10f;
    [SerializeField] float posLerp = 10f;
    /*[SerializeField] float maxY = 10f;
    [SerializeField] float maxX = 10f;*/
    [SerializeField] float maxDistanceFromRail = 16f;

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
        Quaternion projRot = playerRailPoint.rotation;
        Vector3 camUp = Vector3.ProjectOnPlane(projRot * gyro.Attitude * Vector3.up, projRot * Vector3.forward);
        Quaternion targetRot = Quaternion.LookRotation(projRot * Vector3.forward, camUp);
        Vector3 targetPos = target.transform.position + projRot * distanceToTarget;

        Vector3 targetPosOnRail = playerRailPoint.InverseTransformPoint(targetPos);
        float originalZ = targetPosOnRail.z;
        targetPosOnRail.z = 0f;
        if(targetPosOnRail.magnitude > maxDistanceFromRail)
        {
            targetPosOnRail = targetPosOnRail.normalized * maxDistanceFromRail;
        }
        targetPosOnRail.z = originalZ;
        /*if(targetPosOnRail.y > maxY)
        {
            targetPosOnRail.y = maxY;
        }
        else if(targetPosOnRail.y < -maxY)
        {
            targetPosOnRail.y = -maxY;
        }
        else if(targetPosOnRail.x > maxX)
        {
            targetPosOnRail.x = maxX;
        }
        else if(targetPosOnRail.x < -maxX)
        {
            targetPosOnRail.x = -maxX;
        }*/
        targetPos = playerRailPoint.TransformPoint(targetPosOnRail);
        //Debug.Log(targetPosOnRail.y);

        transform.position = Vector3.Lerp(transform.position, targetPos, posLerp * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotLerp * Time.fixedDeltaTime);
    }
}
