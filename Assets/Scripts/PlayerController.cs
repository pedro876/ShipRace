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
    [SerializeField] float maxUpAngle = 45f;
    [SerializeField] float projRotLerp = 10f;
    [SerializeField] float maxTurnAngle = 80f;
    float turnAngle = 0f;
    [SerializeField] float turnAngleLerp = 10f;
    Rigidbody rb;
    [SerializeField] float dashAngle = 1080f;
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] AnimationCurve dashCurve;
    [SerializeField] AnimationCurve dashSpeedCurve;

    PlayerInputFacade input;
    LevelManager level;
    public Vector3 RailPosition { get; private set; }
    public Quaternion RailRotation { get; private set; }
    bool dashingRight;
    bool dashingLeft;
    float time = 0f;
    [SerializeField] float dashMaxTime = 0f;

    private void Awake()
    {
        input = GetComponent<PlayerInputFacade>();
        rb = GetComponent<Rigidbody>();
        level = FindObjectOfType<LevelManager>();
        RailRotation = transform.rotation;

        input.dashRight += DashRight;
        input.dashLeft += DashLeft;
    }

    private void DashRight()
    {
        if(!dashingLeft && !dashingRight)
        {
            time = 0f;
            dashingRight = true;
        }
    }

    private void DashLeft()
    {
        if (!dashingLeft && !dashingRight)
        {
            time = 0f;
            dashingLeft = true;
        }
    }

    private void FixedUpdate()
    {
        Quaternion targetRot = MoveShip();
        targetRot = CheckDash(targetRot);
        targetRot = CheckTurning(targetRot);
        rb.MoveRotation(targetRot);
    }

    private Quaternion MoveShip()
    {
        level.currentSection.rail.Project(transform.position, out var projPos, out var projRot);
        RailPosition = projPos;
        RailRotation = Quaternion.Lerp(RailRotation, projRot, projRotLerp * Time.fixedDeltaTime);

        Quaternion horizontalRot = Quaternion.AngleAxis(input.leftAxis.x * maxHorizontalAngle, -Vector3.forward);
        Quaternion verticalRot = Quaternion.AngleAxis(input.leftAxis.y * maxVerticalAngle, -Vector3.right);
        Quaternion upRot = Quaternion.AngleAxis(input.leftAxis.x * maxUpAngle, Vector3.up);

        Quaternion targetRot = RailRotation * upRot * horizontalRot * verticalRot;

        Vector3 speed = new Vector3(input.leftAxis.x * xSpeed, input.leftAxis.y * ySpeed, zSpeed);
        rb.velocity = projRot * speed;

        railPoint.position = RailPosition;
        railPoint.rotation = RailRotation;

        return targetRot;
    }

    private Quaternion CheckDash(Quaternion targetRot)
    {
        float angle = 0f;
        if(dashingRight || dashingLeft)
        {
            time += Time.deltaTime;
            if(time > dashMaxTime)
            {
                time = dashMaxTime;
                dashingRight = false;
                dashingLeft = false;
            }
            angle = dashCurve.Evaluate(time / dashMaxTime) * dashAngle;
            if (dashingRight)
                angle = -angle;

            float speed = dashSpeedCurve.Evaluate(time / dashMaxTime) * dashSpeed;
            if (dashingLeft)
                speed = -speed;
            rb.velocity += RailRotation * Vector3.right * speed;
        }
        return targetRot * Quaternion.Euler(0f, 0f, angle);
    }

    private Quaternion CheckTurning(Quaternion targetRot)
    {
        float angle = maxTurnAngle * -input.turning;
        turnAngle = Mathf.Lerp(turnAngle, angle, turnAngleLerp * Time.fixedDeltaTime);
        return targetRot * Quaternion.Euler(0f, 0f, turnAngle);
    }
}