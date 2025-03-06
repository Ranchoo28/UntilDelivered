using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class PostalWorkersMovementIA : AbstractClientMovement
{
    private bool canMove = false;
    private PostalWorkersCombatIA CombatIA;
    private Animator animator;

    void Start()
    {
        
        Initialize();
        walkingSpeed = 6;
        animator = GetComponent<Animator>();
        animator.SetInteger("Speed", (int)walkingSpeed);
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        Move();
    }

    protected override void Move()
    {
        if (CombatIA.isFuryMode())
        {
            walkingSpeed = 12;
        }
        else
        {
            walkingSpeed = 6;
        }
        animator.SetInteger("Speed", (int)walkingSpeed);
        agent.speed = walkingSpeed;
        if (agent.remainingDistance <= abstractClientCombatIA.attackRange && !canMove) {
            canMove = abstractClientCombatIA.canAttack = true;
        }
    }
    protected override void Initialize() {
        destination = GameObject.FindWithTag(Const.POSTA_DESTINATION_TAG).transform;
        agent = GetComponent<NavMeshAgent>();

        abstractClientCombatIA = GetComponent<PostalWorkersCombatIA>();
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
