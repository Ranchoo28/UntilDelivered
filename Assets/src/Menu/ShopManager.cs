using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> items;

    void Start()
    {
        ArrayList list = PlayerInfo.GetInstance().ItemUnlocked();
        foreach (GameObject i in items)
        {
            if (list.Contains(i.name))
            {
                i.SetActive(false);
            }
        }
    }

}
