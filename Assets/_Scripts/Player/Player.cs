using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Assertions;

public class Player : MonoBehaviour
{
    [SerializeField] ShipConfig shipConfig;
    [SerializeField] ShipStats shipStats;

    private CameraController cam;
    private ShipController ship;
    private RailProjection shipRailPoint;
    private RigidbodyFollow tunnelCollider;

    private IShipInput nullInput;
    private IShipInput playerInput;

    private bool inputBlocked = true;
    private bool motionBlocked = true;

    private Vector3 originalPos;
    private Quaternion originalRot;

    private int score = 0;

    public Transform GetShipTransform() => ship.transform;

    public void ResetPlayer()
    {
        ship.ResetAndBlock();
        ship.SetInput(nullInput);
        score = 0;
    }

    public void SetStats(ShipStats newStats)
    {
        Assert.IsNotNull(newStats);
        this.shipStats = newStats;
        ship.SetStats(newStats);
    }

    public void SetInput(IShipInput playerInput)
    {
        this.playerInput = playerInput;
    }

    public void BlockInput()
    {
        ship.SetInput(nullInput);
        inputBlocked = true;
    }

    public void ReleaseInput()
    {
        ship.SetInput(playerInput);
        inputBlocked = false;
    }

    public void BlockMotion()
    {
        ship.Block();
        motionBlocked = true;
    }

    public void ReleaseMotion()
    {
        ship.Release();
        motionBlocked = false;
    }

    private void Awake()
    {
        cam = GetComponentInChildren<CameraController>();
        ship = GetComponentInChildren<ShipController>();
        shipRailPoint = GetComponentInChildren<RailProjection>();
        tunnelCollider = GetComponentInChildren<RigidbodyFollow>();

        shipRailPoint.SetTraversor(ship);
        cam.SetTarget(ship.transform, shipRailPoint.transform);

        //playerInput = GetComponent<PlayerInputAdapter>();
        nullInput = new NullInput();
        ship.SetInput(nullInput);
        ship.SetConfig(shipConfig);
        ship.SetStats(shipStats);

        tunnelCollider.SetTarget(shipRailPoint.transform);
    }

    private void Start()
    {
        GameManager.instance.onStateChanged += state =>
        {
            if(state == GameManager.GameState.Game)
            {
                ReleaseInput();
                ReleaseMotion();
            }
            else
            {
                BlockInput();
                BlockMotion();
            }
        };
    }

    
}
