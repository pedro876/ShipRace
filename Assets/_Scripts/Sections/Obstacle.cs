using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float colliderScale = 3f;
    [SerializeField] PhysicMaterial physicMat;
    RotateAround rotator;

    public int idx;

    public void Pause()
    {
        rotator.enabled = false;
    }

    public void Resume()
    {
        rotator.enabled = true;
    }

    private void Awake()
    {
        var obj = new GameObject("obstacleCollider", typeof(MeshCollider));
        var collider = obj.GetComponent<MeshCollider>();
        collider.material = physicMat;
        var meshFilter = GetComponent<MeshFilter>();
        collider.sharedMesh = meshFilter.sharedMesh;
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = new Vector3(1f, 1f, colliderScale);
        rotator = GetComponent<RotateAround>();
    }
}
