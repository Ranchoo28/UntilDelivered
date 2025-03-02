using UnityEngine;

public class DroneProjectile : MonoBehaviour
{
    private float fallSpeed = 25;
    private float damage = 7f;

    private bool isFalling = false;
    private Rigidbody rb;

    public bool HasLanded { get; private set; } = false; 

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Update()
    {
        if (!rb.isKinematic)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }

    public void Release()
    {
        isFalling = true;
        rb.useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Pacco colpito da un nemico! Distruggendo il pacco.");
            HealthScript enemy = other.gameObject.GetComponentInChildren<HealthScript>();
            enemy.TakeDamage(damage);
            HasLanded = true;
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Turret"))
        {
            isFalling = false;
            rb.useGravity = false;
            rb.isKinematic = true;
            HasLanded = true; 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CanPlaceTower"))
        {
            isFalling = false;
            rb.useGravity = false;
            rb.isKinematic = true;
            HasLanded = true;
        }
    }
}