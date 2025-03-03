using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine.AI;
using UnityEngine;

public class KamikazeMovement : AbstractClientMovement
{
    // Trigger that acts as the finder
    private TargetFinder targetFinder;
    // Kamikaze Combat logic
    private Kamikaze kamikaze;
    // Minimum distance to the target
    private float minDistance = 3f;

    void Start()
    {
        // Retrieve the trigger and the explosion procedures
        targetFinder = GetComponentInChildren<TargetFinder>();
        kamikaze = GetComponent<Kamikaze>();
        Initialize();
    }

    void Update()
    {
        if(!agent.isStopped)
            if (destination != null)
            {
                Move();
            }
            else // Find a new target if the current one is destroyed
                targetFinder.enableTrigger();
    }

    protected override void Move(){
        float distanceToTarget = Vector3.Distance(agent.transform.position, destination.position);

        if (distanceToTarget > minDistance)
            agent.SetDestination(destination.position);
        else
        {   // When the distance is low enough, stop the agent and start the explosion procedure
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            kamikaze.explode();
        }
    }

    // Initialize the agent
    protected override void Initialize() {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = walkingSpeed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = 1;
    }

    // Set the target to follow
    public void SetTarget(Transform target)
    {
        destination = target;
        agent.SetDestination(destination.position);
    }
}
