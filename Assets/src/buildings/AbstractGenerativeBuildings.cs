using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGenerativeBuildings : MonoBehaviour
{
    public int Id { get; set; } = 1;
    public abstract int Price { get; set; }
    //Quanto deve generare
    protected abstract int QuantityToGenerate{ get; set; }
    //Tempo da aspettare
    protected abstract float Delay { get; set; }
    //Coroutine per generare
    protected abstract IEnumerator Generate();
}
