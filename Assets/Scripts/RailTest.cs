using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailTest : MonoBehaviour
{
    [SerializeField] Rail rail;
    [SerializeField] Transform proj;
    [SerializeField] float speed = 1f;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rail.Project(transform.position, out var pos, out var rot);
        transform.rotation = rot;
        transform.position += transform.forward * speed * Time.deltaTime;

        proj.position = pos;
        proj.rotation = rot;
    }
}
