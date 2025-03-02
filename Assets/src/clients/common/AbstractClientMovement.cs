using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractClientMovement : MonoBehaviour
{
    protected Transform destination { get; set; }
    protected Waypoint[] waypoints { get; set; }
    protected float waypointTolerance { get; set; } 
    protected short waypointIndex { get; set; }
    protected float walkingSpeed { get; set; } = 12;

    protected float angularSpeed { get; set; } = 500;
    protected float acceleration { get; set; } = 16;
    protected NavMeshAgent agent { get; set; }
    protected AbstractClientCombat abstractClientCombatIA { get; set; }
    protected abstract void Initialize();
    protected abstract void Move();
}
