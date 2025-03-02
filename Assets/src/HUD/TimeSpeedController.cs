using UnityEngine;
using UnityEngine.UI;

public class TimeSpeedController : MonoBehaviour
{
    [SerializeField] private Button speedButton;
    [SerializeField] private Text speedText;

    [Header("Impostazioni Velocit�")]
    [SerializeField] private float[] speedMultipliers = { 1f, 2f, 3f, 5f }; // Valori di velocit� disponibili
    [SerializeField] private float maxAllowedSpeed = 5f; // Velocit� massima consentita

    [Header("Icone")]
    [SerializeField] private RawImage buttonImage;
    [SerializeField] private Texture2D[] speedTextures; // Array di texture per ogni velocit�

    private int currentSpeedIndex = 0;

    private void Start()
    {
        if (speedButton != null)
        {
            speedButton.onClick.AddListener(CycleSpeed);
        }
        else
        {
            Debug.LogWarning("TimeSpeedController: Nessun bottone assegnato!");
        }

        // Verifica che ci siano texture per ogni velocit�
        if (speedTextures != null && speedTextures.Length != speedMultipliers.Length)
        {
            Debug.LogWarning("TimeSpeedController: Il numero di texture non corrisponde al numero di velocit�!");
        }

        UpdateSpeedState();
    }

    private void CycleSpeed()
    {
        // Passa alla prossima velocit�
        currentSpeedIndex = (currentSpeedIndex + 1) % speedMultipliers.Length;
        UpdateSpeedState();
    }

    private void UpdateSpeedState()
    {
        // Imposta la nuova velocit�
        float newSpeed = speedMultipliers[currentSpeedIndex];

        // Verifica che non superi la velocit� massima
        if (newSpeed <= maxAllowedSpeed)
        {
            Time.timeScale = newSpeed;
        }
        else
        {
            Debug.LogWarning($"TimeSpeedController: Velocit� {newSpeed}x supera il limite massimo di {maxAllowedSpeed}x");
            Time.timeScale = maxAllowedSpeed;
        }

        // Aggiorna il testo
        if (speedText != null)
        {
            speedText.text = $"{Time.timeScale}x";
        }

        // Aggiorna l'icona
        if (buttonImage != null && speedTextures != null && speedTextures.Length > currentSpeedIndex)
        {
            buttonImage.texture = speedTextures[currentSpeedIndex];
        }
    }

    private void OnDisable()
    {
        // Ripristina la velocit� normale quando il componente viene disabilitato
        Time.timeScale = 1f;
    }

    // Metodo pubblico per impostare una velocit� specifica
    public void SetSpeed(float speed)
    {
        // Trova l'indice della velocit� pi� vicina
        float minDiff = float.MaxValue;
        int targetIndex = 0;

        for (int i = 0; i < speedMultipliers.Length; i++)
        {
            float diff = Mathf.Abs(speedMultipliers[i] - speed);
            if (diff < minDiff)
            {
                minDiff = diff;
                targetIndex = i;
            }
        }

        currentSpeedIndex = targetIndex;
        UpdateSpeedState();
    }

    // Metodo per ottenere la velocit� attuale
    public float GetCurrentSpeed()
    {
        return Time.timeScale;
    }
}