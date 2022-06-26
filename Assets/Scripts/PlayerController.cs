using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform railPoint;
    [SerializeField] float zSpeed = 10f;
    [SerializeField] float xSpeed = 30f;
    [SerializeField] float ySpeed = 20f;
    [SerializeField] float maxHorizontalAngle = 45f;
    [SerializeField] float maxVerticalAngle = 45f;
    [SerializeField] float projRotLerp = 10f;
    Rigidbody rb;

    PlayerInputFacade input;
    LevelManager level;
    public Vector3 RailPosition { get; private set; }
    public Quaternion RailRotation { get; private set; }

    private void Awake()
    {
        input = GetComponent<PlayerInputFacade>();
        rb = GetComponent<Rigidbody>();
        level = FindObjectOfType<LevelManager>();
        RailRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        level.currentSection.rail.Project(transform.position, out var projPos, out var projRot);
        RailPosition = projPos;
        RailRotation = Quaternion.Lerp(RailRotation, projRot, projRotLerp * Time.fixedDeltaTime);

        Quaternion horizontalRot = Quaternion.AngleAxis(input.leftAxis.x * maxHorizontalAngle, -Vector3.forward);
        Quaternion verticalRot = Quaternion.AngleAxis(input.leftAxis.y * maxVerticalAngle, -Vector3.right);

        Quaternion targetRot = RailRotation * horizontalRot * verticalRot;
        rb.MoveRotation(targetRot);
        //transform.rotation = horizontalRot * verticalRot;

        Vector3 speed = new Vector3(input.leftAxis.x * xSpeed, input.leftAxis.y * ySpeed, zSpeed);
        //rb.AddForce(speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        //Debug.Log(rb.velocity.magnitude);
        rb.velocity = projRot * speed;

        railPoint.position = RailPosition;
        railPoint.rotation = RailRotation;

        /*rb.MovePosition(new Vector3(input.leftAxis.x * xSpeed, input.leftAxis.y * ySpeed, 0f) * Time.fixedDeltaTime);
        rb.MovePosition(Vector3.forward * zSpeed * Time.fixedDeltaTime);
        rb.velocity = Vector3.zero;*/
    }
}
