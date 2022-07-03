using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private CameraController cam;
    private ShipController ship;
    private RailProjection shipRailPoint;

    private void Awake()
    {
        cam = GetComponentInChildren<CameraController>();
        ship = GetComponentInChildren<ShipController>();
        shipRailPoint = GetComponentInChildren<RailProjection>();

        shipRailPoint.SetTraversor(ship);
        cam.SetTarget(ship.transform, shipRailPoint.transform);

        IShipInput input = GetComponent<PlayerInputAdapter>();
        ship.SetInput(input);
    }
}
