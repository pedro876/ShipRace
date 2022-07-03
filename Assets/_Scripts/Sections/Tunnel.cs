using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunnel : MonoBehaviour
{
    [SerializeField] Transform tunnelEnd;
    [SerializeField] List<GameObject> obstaclePrefabs;
    /*[SerializeField] */Transform obstaclePoint;
    List<GameObject> obstacles;
    Quaternion originalRotation;

    public Rail rail { get; private set; }

    //public float Length => length;

    public Transform TunnelEnd => tunnelEnd;

    private void Awake()
    {
        obstacles = new List<GameObject>();
        rail = GetComponentInChildren<Rail>();
        originalRotation = transform.rotation;
        obstaclePoint = transform;
        /*if (obstaclePoint == null)
            obstaclePoint = transform;*/
    }

    public void Place(Vector3 position, Quaternion rotation, bool withObstacles = true)
    {
        transform.position = position;
        transform.rotation = rotation * originalRotation;
        Randomize(withObstacles);
    }

    private void Randomize(bool withObstacles)
    {
        foreach(var obstacle in obstacles)
        {
            Destroy(obstacle);
        }
        obstacles.Clear();

        if (withObstacles && obstaclePoint != null)
        {
            int rnd = Random.Range(0, obstaclePrefabs.Count);
            var obstacle = Instantiate(obstaclePrefabs[rnd], obstaclePoint, true);
            obstacle.transform.localPosition = Vector3.zero;
            int rndRot = Random.Range(0, 4);
            obstacle.transform.localRotation = Quaternion.Euler(0f, 0f, 90f * rndRot);
            obstacles.Add(obstacle);
        }
    }
}
