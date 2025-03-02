using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ClientMovementIA : AbstractClientMovement
{
    private bool canMove = false;
    void Start()
    {
        Initialize();
    }

    void Update()
    {
        Move();
    }

    protected override void Move()
    {
        if (agent.remainingDistance <= baseClientCombatIA.attackRange && !canMove) {
            canMove = baseClientCombatIA.canAttack = true;
        }
    }

    protected override void Initialize() {
        //waypointTolerance = 1.5f;
        //waypointIndex = 0;
        destination = GameObject.FindWithTag(Const.POSTA_DESTINATION_TAG).transform;
        //waypoints = GameObject.Find(Tags.PATH_TAG).GetComponentsInChildren<Waypoint>();
        agent = GetComponent<NavMeshAgent>();
        baseClientCombatIA = GetComponent<BaseClientCombatIA>();

        agent.speed = walkingSpeed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = baseClientCombatIA.attackRange;
        agent.SetDestination(destination.position);
        //waypointIndex++;
    }

    public void UpdateDestination(Transform obj )
    {
        agent.SetDestination(obj.position);
    }

    public void ResetDestination()
    {
        agent.SetDestination(destination.position);
    }

}
