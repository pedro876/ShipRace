using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] Transform[] targets;
    [Header("Properties to follow")]
    [SerializeField] bool followPositionX = true;
    [SerializeField] bool followPositionY = true;
    [SerializeField] bool followPositionZ = true;
    [SerializeField] bool followRotation = false;
    [SerializeField] bool followScale = false;
    bool followPosition => followPositionX || followPositionY || followPositionZ;

    [Header("Follow method")]
    [SerializeField] bool positionLerp = true;
    [SerializeField] float positionLerpFactor = 4f;
    //[SerializeField] Vector3 displacement = Vector3.zero;
    [SerializeField] bool rotationLerp = true;
    [SerializeField] float rotationLerpFactor = 4f;
    [SerializeField] bool scaleLerp = true;
    [SerializeField] float scaleLerpFactor = 4f;

    private Vector3[] displacements;
    private Vector3[] forwards;
    private Vector3[] ups;
    //private Quaternion[] rotations;

    private void Awake()
    {
        //transform.SetParent(null);
        if(targets.Length > 0)
        {
            SetTargets(targets);
        }
    }

    public void SetTargets(Transform[] targets)
    {
        this.targets = new Transform[targets.Length];
        for (int i = 0; i < targets.Length; i++)
            this.targets[i] = targets[i];
        displacements = new Vector3[targets.Length];
        forwards = new Vector3[targets.Length];
        ups = new Vector3[targets.Length];
        //rotations = new Quaternion[targets.Length];
        for(int i = 0; i < targets.Length; i++)
        {
            displacements[i] = targets[i].InverseTransformDirection(transform.position - targets[i].position);
            //rotations[i] = Quaternion.LookRotation(
            //targets[i].InverseTransformDirection(transform.forward),
            //targets[i].InverseTransformDirection(transform.up));
            forwards[i] = targets[i].InverseTransformDirection(transform.forward);
            ups[i] = targets[i].InverseTransformDirection(transform.up);
        }
    }

    public void SetTarget(Transform target)
    {
        Transform[] targets = new Transform[] { target };
        SetTargets(targets);
    }

    public void ClearTransform()
    {
        ClearDisplacements();
        ClearRotations();
    }

    public void ClearDisplacements()
    {
        for(int i = 0; i < targets.Length; i++)
        {
            displacements[i] = Vector3.zero;
        }
    }

    public void ClearRotations()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            forwards[i] = Vector3.forward;
            ups[i] = Vector3.up;
        }
    }

    public void RemoveTarget()
    {
        SetTargets(new Transform[0]);
    }

    private void LateUpdate()
    {
        AverageTransform();
    }

    private void AverageTransform(bool interpolate = true)
    {
        if (targets.Length <= 0)
            return;

        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        Vector3 scale = Vector3.zero;

        for(int i = 0; i < targets.Length; i++)
        {
            if (followPosition)
                position += CalculatePosition(i, interpolate);
            if (followRotation)
            {
                rotation = Quaternion.Lerp(rotation, CalculateRotation(i, interpolate), 1f/(i+1f));
            }
            if (followScale)
                scale += CalculateScale(i, interpolate);
        }

        position /= targets.Length;
        //rotation /= targets.Length;
        scale /= targets.Length;

        if (followPosition)
        {
            transform.position = new Vector3(
                followPositionX ? position.x : transform.position.x,
                followPositionY ? position.y : transform.position.y,
                followPositionZ ? position.z : transform.position.z);
        }
        if (followRotation)
            transform.rotation = rotation;
        if (followScale)
            transform.localScale = scale;
    }

    private Vector3 CalculatePosition(int index, bool interpolate)
    {
        Vector3 targetPos = targets[index].position + targets[index].TransformDirection(displacements[index]);
        if (positionLerp && interpolate)
        {
            return Vector3.Lerp(transform.position, targetPos, positionLerpFactor * Time.deltaTime);
        }
        else
        {
            return targetPos;
        }
    }

    private Quaternion CalculateRotation(int index, bool interpolate)
    {
        Quaternion targetRot = Quaternion.LookRotation(targets[index].TransformDirection(forwards[index]),
            targets[index].TransformDirection(ups[index]));
        if (rotationLerp && interpolate)
        {
            return Quaternion.Lerp(transform.rotation, targetRot, rotationLerpFactor * Time.deltaTime);
        }
        else
        {
            return targetRot;
        }
    }

    private Vector3 CalculateScale(int index, bool interpolate)
    {
        if (scaleLerp && interpolate)
        {
            return Vector3.Lerp(transform.localScale, targets[index].localScale, scaleLerpFactor * Time.deltaTime);
        }
        else
        {
            return targets[index].localScale;
        }
    }

    private void OnEnable()
    {
        AverageTransform(false);
    }
}
