using UnityEngine;
using UnityEngine.UI;

public class TimeSpeedController : MonoBehaviour
{
    [SerializeField] private Button speedButton;
    [SerializeField] private Text speedText;

    [Header("Impostazioni Velocità")]
    [SerializeField] private float[] speedMultipliers = { 1f, 2f, 3f, 5f }; // Valori di velocità disponibili
    [SerializeField] private float maxAllowedSpeed = 5f; // Velocità massima consentita

    [Header("Icone")]
    [SerializeField] private RawImage buttonImage;
    [SerializeField] private Texture2D[] speedTextures; // Array di texture per ogni velocità

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

        // Verifica che ci siano texture per ogni velocità
        if (speedTextures != null && speedTextures.Length != speedMultipliers.Length)
        {
            Debug.LogWarning("TimeSpeedController: Il numero di texture non corrisponde al numero di velocità!");
        }

        UpdateSpeedState();
    }

    private void CycleSpeed()
    {
        // Passa alla prossima velocità
        currentSpeedIndex = (currentSpeedIndex + 1) % speedMultipliers.Length;
        UpdateSpeedState();
    }

    private void UpdateSpeedState()
    {
        // Imposta la nuova velocità
        float newSpeed = speedMultipliers[currentSpeedIndex];

        // Verifica che non superi la velocità massima
        if (newSpeed <= maxAllowedSpeed)
        {
            Time.timeScale = newSpeed;
        }
        else
        {
            Debug.LogWarning($"TimeSpeedController: Velocità {newSpeed}x supera il limite massimo di {maxAllowedSpeed}x");
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
        // Ripristina la velocità normale quando il componente viene disabilitato
        Time.timeScale = 1f;
    }

    // Metodo pubblico per impostare una velocità specifica
    public void SetSpeed(float speed)
    {
        // Trova l'indice della velocità più vicina
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

    // Metodo per ottenere la velocità attuale
    public float GetCurrentSpeed()
    {
        return Time.timeScale;
    }
}