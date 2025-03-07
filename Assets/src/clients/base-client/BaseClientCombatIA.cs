using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BaseClientCombatIA : AbstractClientCombat
{

    private ClientMovementIA movement;
    private bool changeTarget = false;
    private bool isStarting = false; //Serve per risolvere il bug che appena iniziano sparano, se non si capisce il bug vai nel livello 3 e caccia is starting dall'if nell'update
    void Awake() {
        movement = GetComponent<ClientMovementIA>();
        if (gameObject.name == "BossClient(Clone)")
        {
            maxHealth = 200f;
            damage = 5f;
            gold = 50;
        }
        else
        {
            maxHealth = 20f;
            damage = 1.5f;
            gold = 10;
        }
        Initialize();
    }

    void Update() {
        SearchObstacles();
        if (changeTarget)
        {
            UpdateTarget();
        }
        if (agent.remainingDistance <= agent.stoppingDistance && isStarting) {
            StartCoroutine(Attack());
        }
        else
        {
            isStarting = true;
        }
        
    }

    private void SearchObstacles()
    {
        RaycastHit hit;
        Vector3 direction = transform.forward;
        float rayDistance = attackRange;
        int layerToAvoid = LayerMask.GetMask("Projectile");
        if (Physics.Raycast(transform.position, direction, out hit, rayDistance, ~layerToAvoid))
        {
            if ((hit.collider.CompareTag("PostaTarget") && hit.collider.name != "Poste"))
            {
                changeTarget = true;
            }
        }else if(changeTarget){
             changeTarget = false;
             movement.ResetDestination();
        }
    }

    private void UpdateTarget()
    {
        RaycastHit hit;
        Vector3 direction = transform.forward;
        float rayDistance = attackRange;

        if (Physics.Raycast(transform.position, direction, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("PostaTarget"))
            {
                movement.UpdateDestination(hit.collider.gameObject.transform);
            }
        }
    }


    protected override IEnumerator Attack() {
        if (canAttack && (agent.velocity.magnitude <= 0.1f)) {
            canAttack = false;
            LookAtPosta();
            projectile.GetComponent<ProjectileScript>().CreateProjectile(projectile, transform.position, Quaternion.identity, damage);
            yield return new WaitForSeconds(attackSpeed);
            canAttack = true;
        }
    }

    protected override void Initialize() {
        attackSpeed = 2;
        attackRange = 15;

        isLookingAtPosta = false;
        currentHealth = maxHealth;
        canAttack = false;

        healthScript = GetComponentInChildren<HealthScript>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void LookAtPosta() {
        if(isLookingAtPosta) return;
        Vector3 direction = GameObject.FindWithTag(Const.POSTA_TARGET_TAG).transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);
        isLookingAtPosta = true;
    }
}