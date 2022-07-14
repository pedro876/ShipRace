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
    ShipDestroy shipDestroy;

    private IShipInput nullInput;
    private IShipInput playerInput;

    private Vector3 originalPos;
    private Quaternion originalRot;

    private float score = 0;

    public Transform GetShipTransform() => ship.transform;

    public void ResetPlayer()
    {
        //ship.gameObject.SetActive(true);
        
        //shipDestroy.gameObject.SetActive(false);
        ship.ResetAndBlock();
        ship.SetInput(nullInput);
        shipDestroy.Place(ship.transform.position, ship.transform.rotation);
        shipDestroy.Reconstruct();
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
    }

    public void ReleaseInput()
    {
        ship.SetInput(playerInput);
    }

    public void BlockMotion()
    {
        ship.Block();
    }

    public void ReleaseMotion()
    {
        ship.Release();
    }

    public int GetScore()
    {
        return Mathf.FloorToInt(score * 0.01f)*10;
    }

    private void Awake()
    {
        cam = GetComponentInChildren<CameraController>();
        ship = GetComponentInChildren<ShipController>();
        shipRailPoint = GetComponentInChildren<RailProjection>();
        tunnelCollider = GetComponentInChildren<RigidbodyFollow>();
        shipDestroy = GetComponentInChildren<ShipDestroy>();

        shipRailPoint.SetTraversor(ship);
        cam.SetTarget(ship.transform, shipRailPoint.transform);

        //playerInput = GetComponent<PlayerInputAdapter>();
        nullInput = new NullInput();
        ship.SetInput(nullInput);
        ship.SetConfig(shipConfig);
        ship.SetStats(shipStats);
        ship.onDead += () =>
        {
            Debug.Log("Player died");
            BlockInput();
            BlockMotion();
            shipDestroy.Place(ship.transform.position, ship.transform.rotation);
            shipDestroy.ExplodeParts(ship.transform.forward);
            ship.HideModel();
            //ship.gameObject.SetActive(false);
            GameManager.instance.SetState(GameManager.GameState.GameOver);
        };

        shipDestroy.gameObject.SetActive(false);
        shipDestroy.onFinishedReconstruction += () =>
        {
            //ship.gameObject.SetActive(true);
            ship.ShowModel();
            shipDestroy.gameObject.SetActive(false);
        };

        /*GameManager.instance.onStateChanged += state =>
        {
            if (state != GameManager.GameState.GameOver)
            {
                shipDestroy.Reconstruct();
            }
        };*/

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

    private void FixedUpdate()
    {
        CheckScore();
    }

    private void CheckScore()
    {
        
        if (ship.IsAlive())
        {
            float distance = shipRailPoint.distanceTraversed;
            if (!playerInput.IsSlowingDown())
            {
                if (playerInput.IsSpeedingUp())
                    distance *= 2f;
                score += distance;
            }
        }
    }
}
