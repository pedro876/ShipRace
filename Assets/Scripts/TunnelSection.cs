using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelSection : MonoBehaviour
{
    [SerializeField] float length = 60f;
    [SerializeField] List<GameObject> obstaclePrefabs;
    [SerializeField] Transform[] obstaclePoints;
    List<GameObject> obstacles;

    public float Length => length;

    private void Awake()
    {
        obstacles = new List<GameObject>();
    }

    public void Place(Vector3 position, Quaternion rotation, bool withObstacles = true)
    {
        transform.localPosition = position;
        transform.localRotation = rotation;
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
