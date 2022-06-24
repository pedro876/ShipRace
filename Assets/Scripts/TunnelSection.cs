using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelSection : MonoBehaviour
{
    [SerializeField] Transform tunnelEnd;
    [SerializeField] List<GameObject> obstaclePrefabs;
    [SerializeField] Transform[] obstaclePoints;
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

        if (withObstacles)
        {
            foreach (var point in obstaclePoints)
            {
                int rnd = Random.Range(0, obstaclePrefabs.Count);
                var obstacle = Instantiate(obstaclePrefabs[rnd], point, true);
                obstacle.transform.localPosition = Vector3.zero;
                obstacles.Add(obstacle);
            }
        }
    }
}
