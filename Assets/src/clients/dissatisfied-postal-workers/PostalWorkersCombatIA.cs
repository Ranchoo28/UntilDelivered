using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PostalWorkersCombatIA : AbstractClientCombat
{

    private PostalWorkersMovementIA movement;
    private bool changeTarget = false;
    private bool isStarting = false; //Serve per risolvere il bug che appena iniziano sparano, se non si capisce il bug vai nel livello 3 e caccia is starting dall'if nell'update
    private bool isInFuryMode,furyModeStarted;
    private Animator animator;
    [SerializeField]private GameObject weapon;

    void Awake() {
        movement = GetComponent<PostalWorkersMovementIA>();
        maxHealth = 200f;
        damage = 1f;
        gold = 50;
        isInFuryMode = false;
        furyModeStarted = false;
        animator = GetComponent<Animator>();
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
        else if(!isStarting)
        {
            isStarting = true;
        }
        else
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
            weapon.SetActive(false);
        }
        if (!furyModeStarted)
        {
            StartCoroutine(FuryMode());
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
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);
            canAttack = false;
            LookAtPosta();
            projectile.GetComponent<ProjectileScript>().CreateProjectile(projectile, new Vector3(transform.position.x, transform.position.y+2.5f, transform.position.z), Quaternion.identity, damage);
            yield return new WaitForSeconds(attackSpeed);
            canAttack = true;
            weapon.SetActive(true);
        }

    }

    private IEnumerator FuryMode()
    {
        furyModeStarted = true;
        yield return new WaitForSeconds(10);
        if (!isInFuryMode)
        {
            isInFuryMode = true;
            ActivateFuryMode();
            yield return new WaitForSeconds(5);
            DeactivateFuryMode();
            isInFuryMode = false;
        }
        furyModeStarted = false;
    }

    private void ActivateFuryMode()
    {
        attackSpeed = 0.5f;
        damage = 2;
    }

    private void DeactivateFuryMode()
    {
        attackSpeed = 1f;
        damage = 1;
    }

    public bool isFuryMode()
    {
        return isInFuryMode;
    }

    protected override void Initialize() {
        attackSpeed = 1f;
        attackRange = 20;

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