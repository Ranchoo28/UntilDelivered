using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField] private Button timeButton; // Riferimento al bottone
    [SerializeField] private Text buttonText; // Testo opzionale per il bottone

    [Header("Icone")]
    [SerializeField] private RawImage buttonImage; // Componente RawImage del bottone
    [SerializeField] private Texture2D playTexture; // Texture per il play
    [SerializeField] private Texture2D pauseTexture; // Texture per la pausa

    private bool isTimePaused = false;

    private void Start()
    {
        // Verifica che il bottone sia assegnato
        if (timeButton != null)
        {
            // Aggiunge il listener per il click del bottone
            timeButton.onClick.AddListener(ToggleTime);
        }
        else
        {
            Debug.LogWarning("TimeController: Nessun bottone assegnato!");
        }

        // Verifica che le texture siano assegnate
        if (buttonImage == null || playTexture == null || pauseTexture == null)
        {
            Debug.LogWarning("TimeController: Componenti delle texture mancanti!");
        }

        UpdateButtonState();
    }

    private void ToggleTime()
    {
        isTimePaused = !isTimePaused;

        // Imposta il timeScale a 0 per fermare il tempo, 1 per farlo scorrere normalmente
        Time.timeScale = isTimePaused ? 0f : 1f;

        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        // Aggiorna il testo se presente
        if (buttonText != null)
        {
            buttonText.text = isTimePaused ? "Riprendi Tempo" : "Pausa Tempo";
        }

        // Aggiorna la texture se tutti i componenti sono presenti
        if (buttonImage != null && playTexture != null && pauseTexture != null)
        {
            buttonImage.texture = isTimePaused ? playTexture : pauseTexture;

            // Opzionalmente, puoi regolare l'UV Rect per controllare come viene visualizzata la texture
            // buttonImage.uvRect = new Rect(0, 0, 1, 1);
        }
    }

    private void OnDisable()
    {
        // Assicurati che il tempo torni normale se il componente viene disabilitato
        Time.timeScale = 1f;
    }
}