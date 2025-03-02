using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuController : MonoBehaviour
{
    [SerializeField] private Button[] towerButtons; // Array di bottoni per le torrette
    [SerializeField] private Button[] abilityButtons; // Array di bottoni per le abilità

    // Start is called before the first frame update
    void Start()
    {
        UpdateBuildingMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateBuildingMenu()
    {
        ArrayList unlockedItems = PlayerInfo.GetInstance().ItemUnlocked(); // Otteniamo le torrette sbloccate

        foreach (Button button in towerButtons)
        {
            string towerName = button.name; // assumiamo che il nome del bottone corrisponda al nome della torre 
            bool isUnlocked = unlockedItems.Contains(towerName);
            button.interactable = isUnlocked;

            // Trova l'immagine del lucchetto dentro il bottone e attivala/disattivala
            Transform lockImage = button.transform.Find("LockedIcon");
            if (lockImage != null)
            {
                lockImage.gameObject.SetActive(!isUnlocked);
            }
        }

        foreach(Button button in abilityButtons)
        {
            string abilityName = button.name;
            bool isUnlocked = unlockedItems.Contains(abilityName);
            button.interactable = isUnlocked;
            Transform lockImage = button.transform.Find("LockedIcon");
            if (lockImage != null)
            {
                lockImage.gameObject.SetActive(!isUnlocked);
            }
        }

        AbilityMenuController abilityMenu = FindObjectOfType<AbilityMenuController>();

        if (abilityMenu != null)
        {
            abilityMenu.UpdateUnlockedStates();
        }
    }
}
