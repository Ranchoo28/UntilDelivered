using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DisabledClientMovementIA : AbstractClientMovement
{
    private bool canMove = false;
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        

    }


    protected override void Move()
    {
        if (agent.remainingDistance <= abstractClientCombatIA.attackRange && !canMove)
        {
            canMove = abstractClientCombatIA.canAttack = true;
        }
    }

    protected override void Initialize()
    {
        //waypointTolerance = 1.5f;
        //waypointIndex = 0;
        destination = GameObject.FindWithTag(Const.POSTA_DESTINATION_TAG).transform;
        //waypoints = GameObject.Find(Tags.PATH_TAG).GetComponentsInChildren<Waypoint>();
        agent = GetComponent<NavMeshAgent>();
        abstractClientCombatIA = GetComponent<DisabledClientCombatIA>();

        agent.speed = walkingSpeed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = abstractClientCombatIA.attackRange;
        agent.SetDestination(destination.position);

        
        //waypointIndex++;
    }

    public void UpdateDestination(Transform obj)
    {
        agent.SetDestination(obj.position);
    }

    public void ResetDestination()
    {
        agent.SetDestination(destination.position);
    }


}
