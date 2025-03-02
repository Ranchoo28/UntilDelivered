using UnityEngine;
using TMPro; // Assicurati di avere TextMeshPro importato nel progetto

public class GoldCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI goldText; // Riferimento al componente TextMeshPro che mostrerà il gold

    private PlayerInfo playerInfo;

    private void Start()
    {
        // Ottieni l'istanza del singleton
        playerInfo = PlayerInfo.GetInstance();

        //Debug.Log(playerInfo.getGold());

        // Aggiorna il testo iniziale
        UpdateGoldText();
    }

    private void Update()
    {
        // Aggiorna il testo ogni frame per mostrare sempre il valore corrente
        UpdateGoldText();
    }

    private void UpdateGoldText()
    {
        if (goldText != null)
        {
            goldText.text = $"{playerInfo.getGold()}";
        }
    }

    // Metodo opzionale per aggiungere gold (può essere chiamato da altri script o eventi)
    public void AddGold(int amount)
    {
        playerInfo.addGold(amount);
        UpdateGoldText();
    }
}