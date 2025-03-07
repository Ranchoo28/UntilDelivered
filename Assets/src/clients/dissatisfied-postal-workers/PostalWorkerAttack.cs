using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostalWorkerAttack : StateMachineBehaviour
{

    [SerializeField] private GameObject projectile;
    private Transform spawnPoint;
    private int animationFPS = 11;
    private int shootFrame = 1;
    private int lastFrame = -1;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform postalWorker = animator.transform;
        PostalWorkersCombatIA enemy = postalWorker.GetComponent<PostalWorkersCombatIA>();
        Transform spawnPoint = postalWorker.transform.Find("SpawnPointProjectile");
        Debug.Log(animator.speed);
        if (enemy.isFuryMode())
        {
            animator.speed = 2f;
        }
        else
        {
            animator.speed = 1f;
        }

        float animationDuration = stateInfo.length;
        int totalFrames = Mathf.RoundToInt(animationDuration * animationFPS);
        int currentFrame = Mathf.FloorToInt(Mathf.Repeat(stateInfo.normalizedTime, 1f) * totalFrames);

        // Se siamo nel frame di sparo e il frame è diverso dal precedente, spara
        if (currentFrame == shootFrame && lastFrame != shootFrame)
        {
            projectile.GetComponent<ProjectileScript>().CreateProjectile(projectile, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), Quaternion.identity, enemy.damage);
        }

        lastFrame = currentFrame; // Aggiorna il frame precedente
    }

}
