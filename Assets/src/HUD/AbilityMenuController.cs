using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenuController : MonoBehaviour
{

    [SerializeField] private Animator menuAnimator; 
    [SerializeField] private Button[] abilityButtons;
    [SerializeField] private Image[] cooldownOverlays; // Overlay nero da attivare
    public Text cooldownText;

    [SerializeField] private float cooldownTime = 5f;
    private float currentCooldownTime = 0f; 
    private bool isCooldownActive = false;
    private bool isOpen = false;

    // Aggiungi un dizionario per tenere traccia dello stato di sblocco
    private Dictionary<Button, bool> unlockedStates = new Dictionary<Button, bool>();

    // Start is called before the first frame update
    void Start()
    {
        menuAnimator = GetComponent<Animator>();

        foreach (Image overlay in cooldownOverlays)
        {
            overlay.gameObject.SetActive(false);
        }

        UpdateUnlockedStates(); // Assicurati che lo stato iniziale sia corretto
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooldownActive)
        {
            currentCooldownTime -= Time.deltaTime; 
            cooldownText.text = Mathf.Ceil(currentCooldownTime).ToString(); 
        }
        
    }

    private IEnumerator Cooldown()
    {
        isCooldownActive = true;
        currentCooldownTime = cooldownTime;

        // Disabilita solo i bottoni che erano sbloccati
        foreach (Button button in abilityButtons)
        {
            if (unlockedStates[button])
            {
                button.interactable = false;
            }
        }

        foreach (Image overlay in cooldownOverlays)
        {
            overlay.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(cooldownTime);
        
        isCooldownActive = false;
        
        // Ripristina lo stato originale dei bottoni
        foreach (Button button in abilityButtons)
        {
            button.interactable = unlockedStates[button];
        }

        foreach (Image overlay in cooldownOverlays)
        {
            overlay.gameObject.SetActive(false);
        }

        cooldownText.text = "";
    }

    // Aggiungi questo metodo per aggiornare gli stati di sblocco
    public void UpdateUnlockedStates()
    {
        foreach (Button button in abilityButtons)
        {
            unlockedStates[button] = button.interactable;

            // Rimuovi eventuali listener esistenti
            button.onClick.RemoveListener(OnAbilityButtonClicked);

            // Aggiungi il listener solo se il bottone è sbloccato
            if (button.interactable)
            {
                button.onClick.AddListener(OnAbilityButtonClicked);
            }
        }
    }


    // Funzione chiamata quando si clicca il pulsante
    public void OnAbilityButtonClicked()
    {
        if (!isCooldownActive) // Se il cooldown non è attivo
        {
            //StartCooldown(); // Avvia il cooldown
            StartCoroutine(Cooldown());
        }
    }

    private void StartCooldown()
    {
        isCooldownActive = true; // Attiva il cooldown
        currentCooldownTime = cooldownTime; // Reset del tempo del cooldown
        foreach (Button button in abilityButtons)
        {
            button.interactable = false; // Disabilita tutti i pulsanti durante il cooldown
            Debug.Log("Pulsante disabilitato: " + button.name); // Log per confermare che il pulsante è disabilitato
        }
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
        menuAnimator.SetBool("isOpen", isOpen);
    }
}
