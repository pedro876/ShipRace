using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField] Transform railStartRot;
    [SerializeField] Transform railEndRot;

    LineRenderer line;
    float[] distancesToStart;
    Vector3[] lineDirections;
    float totalDistance;

    // Start is called before the first frame update
    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        distancesToStart = new float[line.positionCount];
        lineDirections = new Vector3[line.positionCount];
        distancesToStart[0] = 0f;
        lineDirections[0] = transform.InverseTransformDirection(railStartRot.forward);
        for(int i = 1; i < line.positionCount; i++)
        {
            distancesToStart[i] = (GetPosition(i) - GetPosition(i - 1)).magnitude + distancesToStart[i-1];
            if(i < line.positionCount - 1)
            {
                lineDirections[i] = (
                    (GetPosition(i) - GetPosition(i - 1)).normalized +
                    (GetPosition(i+1) - GetPosition(i)).normalized) * 0.5f;
                //lineDirections[i] = (line.GetPosition(i + 1) - line.GetPosition(i)).normalized;
            }
            else
            {
                lineDirections[i] = transform.InverseTransformDirection(railEndRot.forward);
            }
        }
        totalDistance = distancesToStart[distancesToStart.Length - 1];
    }

    private Vector3 GetPosition(int index)
    {
        return /*transform.rotation * */line.GetPosition(index);
    }

    public void Project(Vector3 position, out Vector3 projPosition, out Quaternion projRotation)
    {
        position = transform.InverseTransformPoint(position);

        int closestEdge = 0;
        Vector3 closestProj = railStartRot.position;
        float minDistance = Mathf.Infinity;
        for(int i = 0; i < line.positionCount-1; i++)
        {
            Vector3 origin = GetPosition(i);
            Vector3 destiny = GetPosition(i + 1);
            Vector3 proj = GeometryOps.ProjectOnEdge(ref position, ref origin, ref destiny);
            float distance = Vector3.Distance(position, proj);
            if(distance < minDistance)
            {
                minDistance = distance;
                closestEdge = i;
                closestProj = proj;
            }
        }

        /*if (closestEdge >= line.positionCount - 1)
        {
            projRotation = railEndRot.rotation;
        }
        else
        {*/
        float distanceToSegmentStart = Vector3.Distance(closestProj, GetPosition(closestEdge));
        float segmentLerp = distanceToSegmentStart / (distancesToStart[closestEdge + 1] - distancesToStart[closestEdge]);
        Vector3 forward = Vector3.Lerp(lineDirections[closestEdge], lineDirections[closestEdge + 1], segmentLerp);
        forward = transform.TransformDirection(forward);
        //Vector3 forward = lineDirections[closestEdge];

        float distanceToStart = distancesToStart[closestEdge] + distanceToSegmentStart;
        float lineLerp = distanceToStart / totalDistance;
        Vector3 up = Quaternion.Lerp(railStartRot.rotation, railEndRot.rotation, lineLerp) * Vector3.up;
        //up = railStartRot.TransformDirection(up);
        projRotation = Quaternion.LookRotation(forward, up);
        
        projPosition = transform.TransformPoint(closestProj);
        //}
    }
}
