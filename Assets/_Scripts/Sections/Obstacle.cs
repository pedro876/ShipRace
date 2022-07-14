using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [HideInInspector] public int idx;
    [SerializeField] float colliderScale = 3f;
    [SerializeField] PhysicMaterial physicMat;


    const float minAngularSpeed = 20f;
    const float maxAngularSpeed = 30f;
    private Vector3 axis = Vector3.forward;

    Rigidbody rb;
    float rotSpeed;
    private bool paused = false;

    public void Pause()
    {
        paused = true;
    }

    public void Resume()
    {
        paused = false;
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
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rotSpeed = Random.Range(minAngularSpeed, maxAngularSpeed) * Mathf.Sign(Random.Range(-1f, 1f)) * GameManager.instance.GetDifficulty();
    }

    private void FixedUpdate()
    {
        if (paused) return;
        Vector3 worldAxis = transform.TransformDirection(axis);
        rb.MoveRotation(Quaternion.AngleAxis(rotSpeed * Time.fixedDeltaTime, worldAxis) * transform.rotation);
    }
}
