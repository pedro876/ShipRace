using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] ShipConfig shipConfig;
    [SerializeField] ShipStats shipStats;

    private CameraController cam;
    private ShipController ship;
    private RailProjection shipRailPoint;
    private RigidbodyFollow tunnelCollider;

    public void SetStats(ShipStats newStats)
    {
        this.shipStats = newStats;
        ship.SetStats(newStats);
    }

    private void Awake()
    {
        cam = GetComponentInChildren<CameraController>();
        ship = GetComponentInChildren<ShipController>();
        shipRailPoint = GetComponentInChildren<RailProjection>();
        tunnelCollider = GetComponentInChildren<RigidbodyFollow>();

        shipRailPoint.SetTraversor(ship);
        cam.SetTarget(ship.transform, shipRailPoint.transform);

        IShipInput input = GetComponent<PlayerInputAdapter>();
        ship.SetInput(input);
        ship.SetConfig(shipConfig);
        ship.SetStats(shipStats);

        tunnelCollider.SetTarget(shipRailPoint.transform);
    }
}
