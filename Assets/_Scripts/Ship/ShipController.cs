using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;
using UnityEngine.Events;

public class ShipController : MonoBehaviour, IRailTraversor
{
    [SerializeField] UnityEvent onSpeedUp;
    [SerializeField] LayerMask deathMask;
    [SerializeField] TrailRenderer leftTrail;
    [SerializeField] TrailRenderer rightTrail;
    [SerializeField] TrailRenderer propulsorTrail;
    private IShipInput input;
    private ShipStats stats;
    private ShipConfig config;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private LevelManager level;
    private Vector3 railPosition;
    private Quaternion railRotation;
    private bool dashingRight;
    private bool dashingLeft;
    private float time = 0f;
    private float tiltAngle = 0f;
    private float maxSpeed;
    private float minSpeed;
    private bool blocked = true;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private bool alive = true;

    public Vector3 GetRailPosition() => railPosition;
    public Quaternion GetRailRotation() => railRotation;
    public event Action onDead;

    public void ResetAndBlock()
    {
        tiltAngle = 0f;
        time = 0f;
        dashingRight = false;
        dashingLeft = false;
        Block();
        transform.position = originalPos;
        transform.rotation = originalRot;
        alive = true;
        CalculateProjection();
    }

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
        maxSpeed = stats.zSpeed * stats.speedUpMultiplier;
        minSpeed = stats.zSpeed * stats.slowDownMultiplier;
    }

    public void Block()
    {
        blocked = true;
        rb.isKinematic = true;
    }
    public void Release()
    {
        blocked = false;
        rb.isKinematic = false;
    }

    public bool IsAlive()
    {
        return !blocked && alive;
    }

    public void ShowModel()
    {
        meshRenderer.enabled = true;
    }

    public void HideModel()
    {
        meshRenderer.enabled = false;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        level = FindObjectOfType<LevelManager>();
        meshRenderer = GetComponent<MeshRenderer>();
        originalPos = transform.position;
        originalRot = transform.rotation;
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

    private void Update()
    {
        leftTrail.emitting = !blocked && !input.IsSlowingDown();
        rightTrail.emitting = !blocked && !input.IsSlowingDown();
        bool wasEmitting = propulsorTrail.emitting;
        propulsorTrail.emitting = !blocked && input.IsSpeedingUp() && !input.IsSlowingDown();
        if(propulsorTrail.emitting && !wasEmitting)
        {
            onSpeedUp?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        Quaternion projRot = CalculateProjection();

        if (blocked || this.input == null) return;
        Quaternion targetRot = MoveShip(projRot);
        targetRot = CheckDash(targetRot);
        targetRot = CheckTilt(targetRot);
        rb.MoveRotation(targetRot);
    }

    private Quaternion CalculateProjection()
    {
        level.currentSection.rail.Project(transform.position, out var projPos, out var projRot);
        railPosition = projPos;
        railRotation = Quaternion.Lerp(railRotation, projRot, config.projRotLerp * Time.fixedDeltaTime);
        return projRot;
    }

    private Quaternion MoveShip(Quaternion projRot)
    {

        Vector2 sideMotion = input.GetSideMotion();

        Quaternion horizontalRot = Quaternion.AngleAxis(sideMotion.x * config.maxHorizontalAngle, -Vector3.forward);
        Quaternion verticalRot = Quaternion.AngleAxis(sideMotion.y * config.maxVerticalAngle, -Vector3.right);
        Quaternion upRot = Quaternion.AngleAxis(sideMotion.x * config.maxUpAngle, Vector3.up);

        Quaternion targetRot = railRotation * upRot * horizontalRot * verticalRot;


        float currentSpeed = (Quaternion.Inverse(projRot) * rb.velocity).z;
        bool speedingUp = input.IsSpeedingUp() && !input.IsSlowingDown();
        bool slowingDown = input.IsSlowingDown();

        float newZSpeed = currentSpeed;
        if (speedingUp)
        {
            float acc = stats.acceleration * stats.speedUpMultiplier;
            newZSpeed += acc * Time.fixedDeltaTime;
            if (newZSpeed > maxSpeed)
                newZSpeed = maxSpeed;
        }
        else if (slowingDown)
        {
            float acc = -stats.acceleration;
            newZSpeed += acc * Time.fixedDeltaTime;
            if (newZSpeed < minSpeed)
                newZSpeed = minSpeed;
        }
        else
        {
            if (currentSpeed == stats.zSpeed)
                newZSpeed = stats.zSpeed;
            else
            {
                float acc = stats.acceleration;
                if (currentSpeed < stats.zSpeed)
                {
                    newZSpeed += acc * Time.fixedDeltaTime;
                    if (newZSpeed > stats.zSpeed)
                        newZSpeed = stats.zSpeed;
                }
                else if(currentSpeed > stats.zSpeed)
                {
                    newZSpeed -= acc * Time.fixedDeltaTime;
                    if(newZSpeed < stats.zSpeed)
                    {
                        newZSpeed = stats.zSpeed;
                    }
                }
            }
        }
        Vector3 speed = new Vector3(sideMotion.x * stats.xSpeed, sideMotion.y * stats.ySpeed, newZSpeed);
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

    private void OnTriggerEnter(Collider other)
    {
        if (!IsAlive()) return;
        if(deathMask == (deathMask | 1 << other.gameObject.layer)){
            alive = false;
            onDead?.Invoke();
        }
    }
}