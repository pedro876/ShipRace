using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float zSpeed = 10f;
    [SerializeField] float xSpeed = 30f;
    [SerializeField] float ySpeed = 20f;
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
        
    }

    private void FixedUpdate()
    {
        Quaternion horizontalRot = Quaternion.AngleAxis(input.leftAxis.x * maxHorizontalAngle, -Vector3.forward);
        Quaternion verticalRot = Quaternion.AngleAxis(input.leftAxis.y * maxVerticalAngle, -Vector3.right);

        rb.MoveRotation(horizontalRot * verticalRot);
        //transform.rotation = horizontalRot * verticalRot;

        Vector3 speed = new Vector3(input.leftAxis.x * xSpeed, input.leftAxis.y * ySpeed, zSpeed);
        //rb.MovePosition(transform.position + speed * Time.fixedDeltaTime);
        rb.velocity = speed;
    }
}
