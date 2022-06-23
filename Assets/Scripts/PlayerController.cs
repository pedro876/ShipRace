using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float zSpeed = 10f;
    [SerializeField] float xySpeed = 3f;
    [SerializeField] float maxHorizontalAngle = 45f;
    [SerializeField] float maxVerticalAngle = 45f;
    Rigidbody rb;

    PlayerInputFacade input;

    private void Awake()
    {
        input = GetComponent<PlayerInputFacade>();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = gyro.Attitude;
        //rb.velocity = transform.forward * speed;

        Quaternion horizontalRot = Quaternion.AngleAxis(input.leftAxis.x * maxHorizontalAngle, -Vector3.forward);
        Quaternion verticalRot = Quaternion.AngleAxis(input.leftAxis.y * maxVerticalAngle, -Vector3.right);

        transform.rotation = horizontalRot * verticalRot;

        Vector3 speed = transform.forward * zSpeed + Vector3.right * input.leftAxis.x * xySpeed;
        rb.velocity = speed;
    }
}
