using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
//using System;

public class ShipController : MonoBehaviour, IRailTraversor
{
    private IShipInput input;
    private ShipStats stats;
    private ShipConfig config;
    private Rigidbody rb;
    private LevelManager level;
    private Vector3 railPosition;
    private Quaternion railRotation;
    private bool dashingRight;
    private bool dashingLeft;
    private float time = 0f;
    private float tiltAngle = 0f;

    public Vector3 GetRailPosition() => railPosition;
    public Quaternion GetRailRotation() => railRotation;

    public void SetInput(IShipInput newInput)
    {
        Assert.IsNotNull(newInput, "Input assigned is null");
        if(input != null)
        {
            input.onDashLeft -= DashLeft;
            input.onDashRight -= DashRight;
        }
        newInput.onDashLeft += DashLeft;
        newInput.onDashRight += DashRight;
        this.input = newInput;
    }

    public void SetConfig(ShipConfig config)
    {
        Assert.IsNotNull(config, "Config assigned is null");
        this.config = config;
    }

    public void SetStats(ShipStats stats)
    {
        Assert.IsNotNull(stats, "Stats assigned is null");
        this.stats = stats;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        level = FindObjectOfType<LevelManager>();
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
        targetRot = CheckTilt(targetRot);
        rb.MoveRotation(targetRot);
    }

    private Quaternion MoveShip()
    {
        level.currentSection.rail.Project(transform.position, out var projPos, out var projRot);
        railPosition = projPos;
        railRotation = Quaternion.Lerp(railRotation, projRot, config.projRotLerp * Time.fixedDeltaTime);

        Vector2 sideMotion = input.GetSideMotion();

        Quaternion horizontalRot = Quaternion.AngleAxis(sideMotion.x * config.maxHorizontalAngle, -Vector3.forward);
        Quaternion verticalRot = Quaternion.AngleAxis(sideMotion.y * config.maxVerticalAngle, -Vector3.right);
        Quaternion upRot = Quaternion.AngleAxis(sideMotion.x * config.maxUpAngle, Vector3.up);

        Quaternion targetRot = railRotation * upRot * horizontalRot * verticalRot;

        Vector3 speed = new Vector3(sideMotion.x * stats.xSpeed, sideMotion.y * stats.ySpeed, stats.zSpeed);
        rb.velocity = projRot * speed;

        return targetRot;
    }

    private Quaternion CheckDash(Quaternion targetRot)
    {
        float angle = 0f;
        if(dashingRight || dashingLeft)
        {
            time += Time.deltaTime;
            if(time > stats.dashTime)
            {
                time = stats.dashTime;
                dashingRight = false;
                dashingLeft = false;
            }
            angle = config.dashCurve.Evaluate(time / stats.dashTime) * config.dashAngle;
            if (dashingRight)
                angle = -angle;

            float speed = config.dashSpeedCurve.Evaluate(time / stats.dashTime) * stats.dashSpeed;
            if (dashingLeft)
                speed = -speed;
            rb.velocity += railRotation * Vector3.right * speed;
        }
        return targetRot * Quaternion.Euler(0f, 0f, angle);
    }

    private Quaternion CheckTilt(Quaternion targetRot)
    {
        float angle = stats.tiltAngle * -input.GetTilt();
        tiltAngle = Mathf.Lerp(tiltAngle, angle, config.tiltAngleLerp * Time.fixedDeltaTime);
        return targetRot * Quaternion.Euler(0f, 0f, tiltAngle);
    }

    
}