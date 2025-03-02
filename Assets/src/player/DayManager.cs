using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Directional Light Settings")]
    [SerializeField] private Light directionalLight; // La luce principale della scena

    [Header("Skybox Shader Settings")]
    [SerializeField] private Material skyboxMaterial; // Lo skybox che usa la tua shader
    [SerializeField] private Color dayTint = new Color(1f, 1f, 1f, 1f); // Colore del giorno
    [SerializeField] private Color nightTint = new Color(0.1f, 0.1f, 0.3f, 1f); // Colore della notte
    [SerializeField] private float dayExposure = 1.3f; // Esposizione diurna
    [SerializeField] private float nightExposure = 0.3f; // Esposizione notturna

    [Header("Cycle Settings")]
    [SerializeField] private float cycleDuration = 300f; // Durata del ciclo giorno-notte in secondi (5 minuti)

    private float cycleTimer = 0f;

    void Start()
    {
        if (directionalLight == null)
        {
            Debug.LogError("Directional Light non assegnata!");
            enabled = false;
        }

        if (skyboxMaterial == null)
        {
            Debug.LogError("Materiale dello Skybox non assegnato!");
            enabled = false;
        }
    }

    void Update()
    {
        cycleTimer += Time.deltaTime;
        float phase = (cycleTimer % cycleDuration) / cycleDuration;

        float sunAngle = Mathf.Lerp(0, 180, phase);
        directionalLight.transform.rotation = Quaternion.Euler(sunAngle, 90f, 0f);
        float blendFactor = Mathf.Sin(phase * Mathf.PI);
        skyboxMaterial.SetFloat("_Blend", blendFactor);
        skyboxMaterial.SetColor("_Tint1", Color.Lerp(dayTint, nightTint, blendFactor));
        skyboxMaterial.SetColor("_Tint2", Color.Lerp(nightTint, dayTint, blendFactor));
        skyboxMaterial.SetFloat("_Exposure1", Mathf.Lerp(dayExposure, nightExposure, blendFactor));
        skyboxMaterial.SetFloat("_Exposure2", Mathf.Lerp(nightExposure, dayExposure, blendFactor));
        DynamicGI.UpdateEnvironment(); 
    }
}
