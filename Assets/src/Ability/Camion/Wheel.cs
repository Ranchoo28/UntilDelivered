using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    private CamionMovement truck; // Il Rigidbody del camion
    private float rotationSpeedMultiplier = 10f; // Moltiplicatore per la velocità di rotazione

    private void Start()
    {
        
        truck = GetComponentInParent<CamionMovement>();
    }

    void Update()
    {
        if (truck!= null)
        {
            transform.Rotate(Vector3.up, 5 * rotationSpeedMultiplier * Time.deltaTime);
        }
    }
}
