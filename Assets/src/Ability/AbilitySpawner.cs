using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CameraController;

public class AbilitySpawner : MonoBehaviour
{
    private bool isSpawned = false;

    private Dictionary<string, int> ability = new Dictionary<string, int>
    {
        { "CamionRotated", 30 },
        { "Drone", 50 },
        { "", 1 }
    };

    public void Spawn(GameObject prefab)
    {
        if (!isSpawned && PlayerInfo.GetInstance().getGold() >= ability[prefab.name])
        {
            Instantiate(prefab, transform.position, new Quaternion(0, 0, 0, 0));
            PlayerInfo.GetInstance().addGold(-ability[prefab.name]);
        }
    }
}