using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PacksBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject health;
    private Camera camera;
    private Slider lifeSlider;
    private float delay;
    private int life = 20;
    private void Start()
    {
        camera = Camera.main;
        lifeSlider = health.GetComponentInChildren<Slider>();
        lifeSlider.maxValue = life;
    }

    public void setDelay(float d) { delay = d; }

    public void TakeDamage(float damage)
    {
        if ((lifeSlider.value -= damage) < 0)
            Destroy(gameObject);
        else
            lifeSlider.value -= damage;
    }

    void LateUpdate()
    {
        health.transform.LookAt(transform.position + camera.transform.forward);
    }
}
