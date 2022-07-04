using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class uses a FlyWeightPattern, encapsulating the stats
/// of a particular type of ship. This way, if a multiplayer mode
/// was implemented and multiple player used the same ship, these
/// field would not be duplicated in memory. Also, this type of
/// object can be used to define different types of ships.
/// </summary>
[CreateAssetMenu(menuName ="ConfigurationFiles/ShipStats")]
public class ShipStats : ScriptableObject
{
    public float zSpeed = 100f;
    public float xSpeed = 30f;
    public float ySpeed = 20f;
    public float dashSpeed = 120f;
    public float dashTime = 0.75f;
    public float tiltAngle = 70f;
}
