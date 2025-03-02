using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetritoScript : MonoBehaviour
{
   void OnTriggerEnter(Collider other) {
       if (other.gameObject.tag == Const.ENEMY_TAG) { 
           other.gameObject.GetComponentInChildren<HealthScript>().TakeDamage(3);
           Destroy(gameObject);

       }
       StartCoroutine(DestroyDetrito());
   }

    IEnumerator DestroyDetrito() {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
