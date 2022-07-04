using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerInputAdapter : MonoBehaviour, IShipInput
{
    [SerializeField] private float gyroExponent = 0.5f;
    [SerializeField] private float maxVerticalGyroAngle = 15f;
    [SerializeField] private float maxHorizontalGyroAngle = 15f;
    [SerializeField]private float sideMotionLerpFactor = 10f;

    private Vector2 rawSideMotion = Vector2.zero;
    private Vector2 sideMotion = Vector2.zero;
    private float tilt = 0f;
    private bool slowingDown = false;
    private float lastTouchPos = 0f;
    private PlayerInput input;

    private void Start()
    {
        
        input = GetComponent<PlayerInput>();
        InputAction sideMotion = input.actions["SideMotion"];
        InputAction dashRight = input.actions["DashRight"];
        InputAction dashLeft = input.actions["DashLeft"];
        InputAction tilt = input.actions["Tilt"];
        InputAction slowDown = input.actions["SlowDown"];
        InputAction dashTouch = input.actions["DashTouch"];
        sideMotion.performed += OnSideMotion;
        tilt.performed += OnTilt;
        tilt.canceled += OnTilt;
        dashRight.performed += OnDashRight;
        dashLeft.performed += OnDashLeft;
        slowDown.started += OnSlowDown;
        slowDown.canceled += OnSlowDown;
        dashTouch.performed += OnDashTouch;
        #if (UNITY_ANDROID || UNITY_EDITOR)
            EnhancedTouchSupport.Enable();
        #endif
    }

    private void Update()
    {
        sideMotion = Vector2.Lerp(sideMotion, rawSideMotion, sideMotionLerpFactor * Time.deltaTime);
        ProcessTouches();
        ProcessGyro();
    }

    private void ProcessTouches()
    {
#if !(UNITY_ANDROID || UNITY_EDITOR)
            return;
#endif
#if UNITY_EDITOR
        if (input.currentControlScheme != "Mobile") return;
#endif
        //Debug.Log(Input.touchSupported);
        //if (!Input.touchSupported) return;
        var activeTouches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;
        //UnityEngine.InputSystem.EnhancedTouch.TouchHistory

        if (activeTouches.Count == 0)
        {
            tilt = 0f;
            return;
        }

        lastTouchPos = 0f;

        for (int i = 0; i < activeTouches.Count; i++)
        {
            lastTouchPos += activeTouches[i].screenPosition.x;
        }
        lastTouchPos /= activeTouches.Count;
        tilt = (lastTouchPos / Screen.width) * 2f - 1f;
        tilt = Mathf.Pow(Mathf.Abs(tilt), 0.5f) * Mathf.Sign(tilt);
    }

    private void ProcessGyro()
    {
        IGyroSystem gyro = GameManager.serviceLocator.GetService<IGyroSystem>();
        if (!gyro.IsWorking()) return;
        #if !(UNITY_ANDROID || UNITY_EDITOR)
            return;
        #endif
        #if UNITY_EDITOR
            if (input.currentControlScheme != "Mobile") return;
        #endif

        Quaternion attitude = gyro.GetAttitude();
        Vector3 gyroForward = Vector3.ProjectOnPlane(attitude * Vector3.forward, Vector3.right);
        float verticalAngle = -Vector3.SignedAngle(Vector3.forward, gyroForward, Vector3.right);
        float verticalAxis = Mathf.Clamp(verticalAngle / maxVerticalGyroAngle, -1f, 1f);
        if (verticalAxis != 0f)
            verticalAxis = Mathf.Pow(Mathf.Abs(verticalAxis), gyroExponent) * Mathf.Sign(verticalAxis);

        Vector3 gyroUp = Vector3.ProjectOnPlane(attitude * Vector3.up, Vector3.forward);
        float horizontalAngle = -Vector3.SignedAngle(Vector3.up, gyroUp, Vector3.forward);
        float horizontalAxis = Mathf.Clamp(horizontalAngle / maxHorizontalGyroAngle, -1f, 1f);
        if (horizontalAxis != 0f)
            horizontalAxis = Mathf.Pow(Mathf.Abs(horizontalAxis), gyroExponent) * Mathf.Sign(horizontalAxis);

        rawSideMotion = new Vector2(horizontalAxis, verticalAxis);
    }

#region CallbacksForPlayerInput
    private void OnSideMotion(InputAction.CallbackContext ctx)
    {
        rawSideMotion = ctx.ReadValue<Vector2>();
    }

    private void OnDashTouch(InputAction.CallbackContext ctx)
    {
        if (lastTouchPos > Screen.width / 2)
        {
            OnDashRight(ctx);
        }
        else
        {
            OnDashLeft(ctx);
        }
    }

    private void OnDashRight(InputAction.CallbackContext ctx)
    {
        onDashRight?.Invoke();
    }

    private void OnDashLeft(InputAction.CallbackContext ctx)
    {
        onDashLeft.Invoke();
    }

    private void OnTilt(InputAction.CallbackContext ctx)
    {
        tilt = ctx.ReadValue<float>();
    }

    private void OnSlowDown(InputAction.CallbackContext ctx)
    {
        slowingDown = ctx.ReadValueAsButton();
    }
#endregion

#region API
    public event Action onDashRight;
    public event Action onDashLeft;
    public Vector2 GetRawSideMotion()
    {
        return rawSideMotion;
    }

    public Vector2 GetSideMotion()
    {
        return sideMotion;
    }

    public float GetTilt()
    {
        return tilt;
    }

    public bool IsSlowingDown()
    {
        return slowingDown;
    }

#endregion
}
