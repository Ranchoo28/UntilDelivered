using System;
using System.Collections;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] private GameObject projectile;

    private float flightSpeed = 40f;
    private float stoppingDistance = 0.5f;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool canMoveAgain = true;
    private bool canStartMoving = false;
    private bool isDelivering = false; 
    private int packCount = 3;
    private float height = 10;
    private GameObject proj;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (!canStartMoving)
        {
            MoveToHeight(height);
        }

        if (Input.GetMouseButtonDown(0) && packCount > 0 && canMoveAgain &&
            canStartMoving && !isDelivering)
        {
            int layerToAvoid = LayerMask.GetMask("Towers");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~layerToAvoid))
            {
                targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                isMoving = true;
                canMoveAgain = false;
                isDelivering = true; 
            }
        }
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > stoppingDistance)
        {
            transform.position += direction * flightSpeed * Time.deltaTime;
        }
        else
        {
            proj = Instantiate(projectile, transform.position - new Vector3(0, 3.5f, 0), Quaternion.identity);
            isMoving = false;
            canMoveAgain = true;
            StartCoroutine(WaitForDeliveryComplete());
        }
    }

    private IEnumerator WaitForDeliveryComplete()
    {
        if (packCount > 0)
        {
            DroneProjectile projectileScript = proj.GetComponentInChildren<DroneProjectile>();
            projectileScript.Release();

            while (projectileScript != null && !projectileScript.HasLanded)
            {
                yield return null;
            }

            packCount--;
            Debug.Log("Pacco rilasciato! Rimangono " + packCount + " pacchi.");
        }

        isDelivering = false; 

        if (packCount == 0)
        {
            Debug.Log("Tutti i pacchi sono stati rilasciati. Fine operazioni.");
            StartCoroutine(Destroyer());
        }
    }

    private IEnumerator Destroyer()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    public void MoveToHeight(float targetHeight)
    {
        StartCoroutine(MoveVertically(targetHeight));
    }

    private IEnumerator MoveVertically(float targetHeight)
    {
        while (Mathf.Abs(transform.position.y - targetHeight) > 0.1f)
        {
            float step = Time.deltaTime / 4;
            transform.position = new Vector3(
                transform.position.x,
                Mathf.MoveTowards(transform.position.y, targetHeight, step),
                transform.position.z
            );
            yield return null;
        }

        canStartMoving = true;
    }
}