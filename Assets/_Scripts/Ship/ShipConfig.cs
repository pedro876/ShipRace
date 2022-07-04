using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class uses a FlyWeightPattern, encapsulating the configuration
/// of ship motion for all ships in game. This can also be used
/// to define different types of configurations.
/// </summary>
[CreateAssetMenu(menuName ="ConfigurationFiles/ShipConfiguration")]
public class ShipConfig : ScriptableObject
{
    public float maxHorizontalAngle = 25f;
    public float maxVerticalAngle = 15f;
    public float maxUpAngle = 15f;
    public float projRotLerp = 10f;
    public float tiltAngleLerp = 10f;
    public float dashAngle = 360f;
    public AnimationCurve dashCurve;
    public AnimationCurve dashSpeedCurve;
}
