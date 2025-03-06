using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class PostalWorkersMovementIA : AbstractClientMovement
{
    private bool canMove = false;
    private PostalWorkersCombatIA CombatIA;
    void Start()
    {
        Initialize();
        walkingSpeed = 6;
    }

    void Update()
    {
        Move();
    }

    protected override void Move()
    {
        if (agent.remainingDistance <= CombatIA.attackRange && !canMove) {
            canMove = CombatIA.canAttack = true;
        }
        if (CombatIA.isFuryMode()) {
            walkingSpeed = 12;
        }
        else
        {
            walkingSpeed = 6;
        }
    }
    protected override void Initialize() {
        destination = GameObject.FindWithTag(Const.POSTA_DESTINATION_TAG).transform;
        agent = GetComponent<NavMeshAgent>();
        CombatIA = GetComponent<PostalWorkersCombatIA>();

        agent.speed = walkingSpeed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = abstractClientCombatIA.attackRange;
        agent.SetDestination(destination.position);
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
