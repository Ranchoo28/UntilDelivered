using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmBehaviour : AbstractGenerativeBuildings
{
    private bool generating = false;
    [SerializeField] GameObject coin;

    private int quantity = 10;
    private float delay = 7;
    private int price = 75;
    public override int Price
    {
        get => price;
        set => price = value;
    }

    protected override int QuantityToGenerate
    {
        get => quantity;
        set => quantity = value;
    }

    protected override float Delay
    {
        get => delay;
        set => delay = value;
    }

    void Update()
    {
        if (!generating)
        {
            StartCoroutine(Generate());
        }
    }

    protected override IEnumerator Generate()
    {
        generating = true;
        yield return new WaitForSeconds(delay);
        PlayerInfo.GetInstance().addGold(quantity);
        Instantiate(coin, transform.position + new Vector3(0f, 9f, 0f), Quaternion.identity);
        generating = false;
    }
}
