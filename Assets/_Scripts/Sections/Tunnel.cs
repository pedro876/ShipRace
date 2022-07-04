using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunnel : MonoBehaviour
{
    public Transform tunnelEnd { get; private set; }
    public Rail rail { get; private set; }
    public Obstacle obstacle { get; private set; }
    [HideInInspector] public int sectionIndex = -1;
    [SerializeField] bool isCurve;
    Quaternion originalRotation;
    Transform obstaclePoint;
    public bool IsCurve() => isCurve;

    public void Initialize(Vector3 position, Quaternion rotation, Obstacle obstacle)
    {
        transform.position = position;
        transform.rotation = rotation * originalRotation;
        this.obstacle = obstacle;
        if (obstacle != null)
        {
            obstacle.transform.SetParent(obstaclePoint);
            obstacle.transform.localPosition = Vector3.zero;
            int rndRot = Random.Range(0, 4);
            obstacle.transform.localRotation = Quaternion.Euler(0f, 0f, 90f * rndRot);
        }
    }

    private void Awake()
    {
        rail = GetComponentInChildren<Rail>();
        obstaclePoint = transform;
        tunnelEnd = FindChildWithTag("TunnelEnd");
        originalRotation = transform.rotation;
    }

    private Transform FindChildWithTag(string tag)
    {
        Transform foundChild = null;
        for (int i = 0; i < transform.childCount && foundChild == null; i++)
        {
            var child = transform.GetChild(i);
            if (child.tag == tag)
                foundChild = child;
        }
        return foundChild;
    }
}
