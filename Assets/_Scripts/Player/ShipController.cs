using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class ShipController : MonoBehaviour, IRailTraversor
{
    //[SerializeField] Transform railPoint;
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

    //ShipInputFacade input;
    IShipInput input;
    LevelManager level;
    private Vector3 railPosition;
    private Quaternion railRotation;
    bool dashingRight;
    bool dashingLeft;
    float time = 0f;
    [SerializeField] float dashMaxTime = 1f;

    //public event Action<Vector3, Quaternion> onRailPointComputed;

    public Vector3 GetRailPosition() => railPosition;
    public Quaternion GetRailRotation() => railRotation;

    public void SetInput(IShipInput newInput)
    {
        if(input != null)
        {
            input.onDashLeft -= DashLeft;
            input.onDashRight -= DashRight;
        }
        newInput.onDashLeft += DashLeft;
        newInput.onDashRight += DashRight;
        this.input = newInput;
    }

    private void Awake()
    {
        //input = GetComponent<ShipInputFacade>();
        rb = GetComponent<Rigidbody>();
        level = FindObjectOfType<LevelManager>();
        //RailRotation = transform.rotation;

        /*input.dashRight += DashRight;
        input.dashLeft += DashLeft;*/
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
        if (this.input == null) return;

        Quaternion targetRot = MoveShip();
        targetRot = CheckDash(targetRot);
        targetRot = CheckTurning(targetRot);
        rb.MoveRotation(targetRot);
    }

    private Quaternion MoveShip()
    {
        level.currentSection.rail.Project(transform.position, out var projPos, out var projRot);
        railPosition = projPos;
        railRotation = Quaternion.Lerp(railRotation, projRot, projRotLerp * Time.fixedDeltaTime);

        Vector2 sideMotion = input.GetSideMotion();

        Quaternion horizontalRot = Quaternion.AngleAxis(sideMotion.x * maxHorizontalAngle, -Vector3.forward);
        Quaternion verticalRot = Quaternion.AngleAxis(sideMotion.y * maxVerticalAngle, -Vector3.right);
        Quaternion upRot = Quaternion.AngleAxis(sideMotion.x * maxUpAngle, Vector3.up);

        Quaternion targetRot = railRotation * upRot * horizontalRot * verticalRot;

        Vector3 speed = new Vector3(sideMotion.x * xSpeed, sideMotion.y * ySpeed, zSpeed);
        rb.velocity = projRot * speed;

        //onRailPointComputed?.Invoke(RailPosition, RailRotation);

        //railPoint.position = RailPosition;
        //railPoint.rotation = RailRotation;

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
            rb.velocity += railRotation * Vector3.right * speed;
        }
        return targetRot * Quaternion.Euler(0f, 0f, angle);
    }

    private Quaternion CheckTurning(Quaternion targetRot)
    {
        float angle = maxTurnAngle * -input.GetTilt();
        turnAngle = Mathf.Lerp(turnAngle, angle, turnAngleLerp * Time.fixedDeltaTime);
        return targetRot * Quaternion.Euler(0f, 0f, turnAngle);
    }

    
}