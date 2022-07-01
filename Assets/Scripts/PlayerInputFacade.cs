using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInputFacade : MonoBehaviour
{
    [Header("Left Axis")]
    [SerializeField] float leftAxisLerpFactor = 10f;
    [SerializeField] float leftAxisExponent = 0.5f;
    [SerializeField] float maxVerticalGyroAngle = 45f;
    [SerializeField] float maxHorizontalGyroAngle = 45f;
    public Vector2 rawLeftAxis { get; private set; }
    public Vector2 leftAxis { get; private set; }
    public event Action dashRight;
    public event Action dashLeft;
    //private float leftAxisX;
    //private float leftAxisY;

    GyroFacade gyro;
    GameControls controls;
    float lastTouchPos = 0f;

    public float turning { get; private set; }

    private void Awake()
    {
        gyro = FindObjectOfType<GyroFacade>();
        controls = new GameControls();
        controls.ShipControls.DashRight.performed += ctx => dashRight?.Invoke();
        controls.ShipControls.DashLeft.performed += ctx => dashLeft?.Invoke();
        controls.ShipControls.DashTouch.performed += ctx =>
        {
            if(lastTouchPos > Screen.width / 2)
            {
                dashRight?.Invoke();
            }
            else
            {
                dashLeft?.Invoke();
            }
        };
        controls.ShipControls.TouchPos.performed += ctx =>
        {
            Vector2 pos = ctx.ReadValue<Vector2>();
            turning = (pos.x / Screen.width) * 2f - 1f;
            turning = Mathf.Pow(Mathf.Abs(turning), 0.5f) * Mathf.Sign(turning);
            lastTouchPos = pos.x;
        };
        controls.ShipControls.StopTouch.canceled += ctx =>
        {
            turning = 0f;
        };
        controls.ShipControls.Turning.performed += ctx => turning = ctx.ReadValue<float>();
        controls.ShipControls.Turning.canceled += ctx => turning = 0f;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        CalculateLeftAxis();

        leftAxis = Vector3.Lerp(leftAxis, rawLeftAxis, leftAxisLerpFactor * Time.deltaTime);
    }

    private void CalculateLeftAxis()
    {
        if (gyro.working)
        {
            Quaternion attitude = gyro.Attitude;
            Vector3 gyroForward = Vector3.ProjectOnPlane(attitude * Vector3.forward, Vector3.right);
            float verticalAngle = -Vector3.SignedAngle(Vector3.forward, gyroForward, Vector3.right);
            float verticalAxis = Mathf.Clamp(verticalAngle / maxVerticalGyroAngle, -1f, 1f);
            if (verticalAxis != 0f)
                verticalAxis = Mathf.Pow(Mathf.Abs(verticalAxis), leftAxisExponent) * Mathf.Sign(verticalAxis);

            Vector3 gyroUp = Vector3.ProjectOnPlane(attitude * Vector3.up, Vector3.forward);
            float horizontalAngle = -Vector3.SignedAngle(Vector3.up, gyroUp, Vector3.forward);
            float horizontalAxis = Mathf.Clamp(horizontalAngle / maxHorizontalGyroAngle, -1f, 1f);
            if (horizontalAxis != 0f)
                horizontalAxis = Mathf.Pow(Mathf.Abs(horizontalAxis), leftAxisExponent) * Mathf.Sign(horizontalAxis);

            rawLeftAxis = new Vector2(horizontalAxis, verticalAxis);
        }
        else
        {
            rawLeftAxis = new Vector2(
                controls.ShipControls.LeftHorizontalAxis.ReadValue<float>(),
                controls.ShipControls.LeftVerticalAxis.ReadValue<float>()
            );
        }
    }
}
