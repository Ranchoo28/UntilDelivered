using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private bool debug = true;

    public virtual void OnDrawGizmos() {
        if (!debug) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1.5f);
    }
}
