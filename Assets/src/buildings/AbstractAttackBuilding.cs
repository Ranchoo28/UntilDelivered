using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAttackBuilding : MonoBehaviour
{
    public int Id { get; set; } = 0;
    public abstract int Price { get; set; }

    protected GameObject posta;
}
