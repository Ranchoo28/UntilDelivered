using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Apple;

public class CamionMovement : MonoBehaviour
{
    private float movementSpeed = 12f; // Velocità del camion
    private float dropDelay;

    [SerializeField] private GameObject objectToDrop;
    private GameObject objectFromDrop;

    private GameObject destination;
    private NavMeshAgent agent;

    private bool isDropping = true;
    private Vector3 position;

    private void Start()
    {
        destination = GameObject.FindWithTag("Spawner");
        agent = GetComponentInParent<NavMeshAgent>();
        agent.speed = movementSpeed;
        agent.SetDestination(destination.transform.position);
        dropDelay = Random.Range(2f, 4f);
        StartCoroutine(DropObject());
        StartCoroutine(Destroyer());
    }

    private void Update()
    {
        if (isDropping)
        {
            transform.position = position;
        }
        if(agent.remainingDistance < 1f)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DropObject()
    {
        isDropping = false;
        
        yield return new WaitForSeconds(dropDelay);
        position = transform.position;
        agent.speed = 0;
        isDropping = true;
        objectFromDrop = Instantiate(objectToDrop, transform.position+new Vector3(0, 2, -5), transform.rotation);
        objectFromDrop.GetComponentInChildren<PacksBehaviour>().setDelay(dropDelay);

        yield return new WaitForSeconds(dropDelay);
        agent.speed = movementSpeed;
        isDropping = false;
        movementSpeed *= 5;
    }

    private IEnumerator Destroyer()
    { 
        yield return new WaitForSeconds(60f);
        Destroy(gameObject);
    }

}
