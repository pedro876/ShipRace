using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    Rigidbody rb;
    GyroFacade gyro;

    private void Awake()
    {
        gyro = FindObjectOfType<GyroFacade>();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = gyro.Attitude;
        rb.velocity = transform.forward * speed;
    }
}
