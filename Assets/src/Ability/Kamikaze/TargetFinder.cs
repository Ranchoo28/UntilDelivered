using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    // Find the target inside the trigger
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {   // Set the target
            transform.parent.GetComponent<KamikazeMovement>().SetTarget(other.transform);
            disableTrigger();
        }
    }

    public void disableTrigger()
    {
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public void enableTrigger()
    {
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
