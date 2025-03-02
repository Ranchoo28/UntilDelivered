using UnityEngine;
using System.Collections;

public class MortaioBehavior : AbstractAttackBuilding {
    [Header("--- References ---")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    // I valori di default sono quelli migliori per il mortaio (NON TOCCARE, GRAZIE)
    [Header("--- Mortar Settings (NON TOCCARE) ---")]
    [Range(30f, 60f)] public float radius = 50f;
    [Range(1f, 30f)] public float minLaunchSpeed = 18f;
    [Range(50f, 100f)] public float maxLaunchSpeed = 120f;
    [Range(1f, 30f)] public float reloadTime = 6f;
    [Range(0f, 1f)] public float targetingPredictionFactor = 0.35f;
    [Range(1f, 15f)] public float damage = 15f;

    [Header("--- Debug ---")]
    public bool showTrajectory = true;
    public bool showRadius = true;
    public int trajectorySteps = 50;
    public float trajectoryTimeStep = 0.1f;

    private GameObject currentTarget;
    private bool isReloading = false;
    private Vector3 lastTargetPosition;
    private Vector3 targetVelocity;
    private float lastValidationTime = 0f;
    private readonly float targetValidationInterval = 0.1f;
    private int price = 100;

    public override int Price {
        get => price;
        set => price = value;
    }

    void Start() {
        if (firePoint == null) {
            firePoint = transform;
        }
        targetVelocity = Vector3.zero;
        lastValidationTime = Time.time;
    }

    void Update() {
        if (Time.time >= lastValidationTime + targetValidationInterval) {
            ValidateCurrentTarget();
            lastValidationTime = Time.time;
        }

        UpdateTarget();
        UpdateTargetTracking();

        if (currentTarget != null && !isReloading) {
            Vector3 predictedPosition = PredictTargetPosition();

            if (Vector3.Distance(transform.position, predictedPosition) > radius) {
                LoseTarget();
                return;
            }

            isReloading = true;
            StartCoroutine(Fire());
        }
    }

    private void ValidateCurrentTarget() {
        if (currentTarget != null) {
            if (!currentTarget.activeInHierarchy ||
                Vector3.Distance(transform.position, currentTarget.transform.position) > radius ||
                !currentTarget.CompareTag("Enemy")) {
                LoseTarget();
            }
        }
    }

    private void LoseTarget() {
        currentTarget = null;
        targetVelocity = Vector3.zero;
    }

    private void UpdateTargetTracking() {
        if (currentTarget != null && currentTarget.activeInHierarchy) {
            Vector3 currentPosition = currentTarget.transform.position;
            targetVelocity = (currentPosition - lastTargetPosition) / Time.deltaTime;
            lastTargetPosition = currentPosition;
        }
        else {
            targetVelocity = Vector3.zero;
        }
    }

    private Vector3 PredictTargetPosition() {
        if (currentTarget == null) return Vector3.zero;

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
        float estimatedFlightTime = distanceToTarget / minLaunchSpeed;

        Vector3 predictedPosition = currentTarget.transform.position +
            (targetVelocity * estimatedFlightTime * targetingPredictionFactor);

        if (Vector3.Distance(transform.position, predictedPosition) > radius) {
            Vector3 directionToTarget = (predictedPosition - transform.position).normalized;
            predictedPosition = transform.position + directionToTarget * radius;
        }

        return predictedPosition;
    }

    private void UpdateTarget() {
        if (currentTarget != null && currentTarget.activeInHierarchy &&
            Vector3.Distance(transform.position, currentTarget.transform.position) <= radius &&
            currentTarget.CompareTag("Enemy")) {
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Collider col in hitColliders) {
            if (col != null && col.gameObject.activeInHierarchy && col.CompareTag("Enemy")) {
                float distanceToEnemy = Vector3.Distance(transform.position, col.transform.position);
                if (distanceToEnemy < closestDistance) {
                    closestDistance = distanceToEnemy;
                    closestEnemy = col.gameObject;
                }
            }
        }

        if (closestEnemy != null) {
            currentTarget = closestEnemy;
            lastTargetPosition = closestEnemy.transform.position;
            targetVelocity = Vector3.zero;
        }
    }

    private IEnumerator Fire() {
        if (currentTarget != null && currentTarget.activeInHierarchy) {
            Vector3 predictedPosition = PredictTargetPosition();

            if (IsTargetReachable(predictedPosition)) {
                Vector3 launchVelocity = CalculateVelocity(predictedPosition);
              
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                projectile.GetComponent<MortaioProjectile>().SetDamage(damage);

                if (projectile.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
                    rb.velocity = launchVelocity;
                }
            }

            yield return new WaitForSeconds(reloadTime);
            isReloading = false;
        }
        else {
            LoseTarget();
            isReloading = false;
        }
    }

    private Vector3 CalculateVelocity(Vector3 targetPosition) {
        Vector3 displacement = targetPosition - firePoint.position;
        float range = new Vector3(displacement.x, 0f, displacement.z).magnitude; 
        float height = displacement.y;

        float baseAngle = 45f;
        float heightFactor = Mathf.Atan2(height, range) * Mathf.Rad2Deg;
        float speedFactor = Mathf.Lerp(20f, 5f, (minLaunchSpeed - 20f) / 20f); 

        float optimalAngle = baseAngle + heightFactor + speedFactor;
        optimalAngle = Mathf.Clamp(optimalAngle, 35f, 75f);
        float angleInRadians = optimalAngle * Mathf.Deg2Rad;

        // ----------------------------------------------------------------------------------------------------------------------------------------------
        // Calcolo del quadrato della velocità di lancio richiesta per raggiungere il bersaglio (Chi avrebbe mai detto che fisica sarebbe tornata utile?)
        // Formula derivata dall'equazione del moto parabolico:
        // Velocity^2 = (g * range^2) / [2 * cos^2(angle) * (range * tan(angle) - height)]
        // Dove:
        // - g è l'accelerazione gravitazionale
        // - range è la distanza orizzontale al bersaglio
        // - angle è l'angolo di lancio
        // - height è la differenza di altezza tra il punto di lancio e il bersaglio
        // ----------------------------------------------------------------------------------------------------------------------------------------------

        float cos = Mathf.Cos(angleInRadians);
        float sin = Mathf.Sin(angleInRadians);
        float velocitySquared = (Physics.gravity.magnitude * range * range) /
                              (2f * cos * cos * (range * Mathf.Tan(angleInRadians) - height));

        float speed = Mathf.Sqrt(velocitySquared);
        speed = Mathf.Clamp(speed, minLaunchSpeed, maxLaunchSpeed);

        Vector3 horizontalDir = new Vector3(displacement.x, 0f, displacement.z).normalized;
        float horizontalSpeed = speed * cos;
        float verticalSpeed = speed * sin;

        return new Vector3(
            horizontalDir.x * horizontalSpeed,
            verticalSpeed,
            horizontalDir.z * horizontalSpeed
        );
    }


    private bool IsTargetReachable(Vector3 targetPosition) {
        float horizontalDistance = Vector3.Distance(
            new Vector3(firePoint.position.x, 0f, firePoint.position.z),
            new Vector3(targetPosition.x, 0f, targetPosition.z)
        );

        if (horizontalDistance > radius) return false;

        float heightDifference = targetPosition.y - firePoint.position.y;
        float minRequiredVelocitySquared = Physics.gravity.magnitude * (
            horizontalDistance * horizontalDistance / (2f * Mathf.Cos(45f * Mathf.Deg2Rad) * Mathf.Cos(45f * Mathf.Deg2Rad)) /
            (horizontalDistance * Mathf.Tan(45f * Mathf.Deg2Rad) - heightDifference)
        );

        return Mathf.Sqrt(minRequiredVelocitySquared) <= maxLaunchSpeed;
    }

    private void OnDrawGizmos() {
        if (showRadius) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        if (currentTarget != null && currentTarget.activeInHierarchy && showTrajectory) {
            DrawTrajectory(currentTarget.transform.position, Color.yellow);
            DrawTrajectory(PredictTargetPosition(), Color.red);
        }
    }

    private void DrawTrajectory(Vector3 targetPos, Color color) {
        if (!IsTargetReachable(targetPos)) return;

        Vector3 velocity = CalculateVelocity(targetPos);
        Vector3 position = firePoint.position;

        Gizmos.color = color;
        for (int i = 1; i < trajectorySteps; i++) {
            Vector3 nextPosition = position + velocity * trajectoryTimeStep;
            nextPosition.y = position.y + velocity.y * trajectoryTimeStep -
                             (Physics.gravity.magnitude * trajectoryTimeStep * trajectoryTimeStep) / 2f;

            Gizmos.DrawLine(position, nextPosition);

            position = nextPosition;
            velocity += Physics.gravity * trajectoryTimeStep;
        }
    }
}