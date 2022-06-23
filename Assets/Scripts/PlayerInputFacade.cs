using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputFacade : MonoBehaviour
{
    [Header("Left Axis")]
    [SerializeField] float leftAxisLerpFactor = 10f;
    [SerializeField] float leftAxisExponent = 0.5f;
    [SerializeField] float maxVerticalGyroAngle = 45f;
    [SerializeField] float maxHorizontalGyroAngle = 45f;
    public Vector2 rawLeftAxis { get; private set; }
    public Vector2 leftAxis { get; private set; }
    //private float leftAxisX;
    //private float leftAxisY;

    GyroFacade gyro;
    GameControls controls;

    private void Awake()
    {
        gyro = FindObjectOfType<GyroFacade>();
        controls = new GameControls();
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
