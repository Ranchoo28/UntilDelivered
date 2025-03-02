using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> buttons = new List<GameObject>();
    private int turretPlaced, maxTurretPlaceable;
    [SerializeField] private TextMeshProUGUI text;
    void Start()
    {
        maxTurretPlaceable = 10;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = GameObject.FindGameObjectsWithTag("Turret").Length + " / " + maxTurretPlaceable.ToString();
        if (CountTurretPlaced() >= maxTurretPlaceable)
        {
            DeactivateAllButtons();
        }
    }

    private int CountTurretPlaced()
    {
        return GameObject.FindGameObjectsWithTag("Turret").Length;
    }



    private void DeactivateAllButtons()
    {
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().enabled = false; //non va bene, ma funziona. Dovrebbe cambiare il colore in uno che indichi che è disabled
        }
    }
}
