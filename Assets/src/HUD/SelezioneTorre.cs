using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public GameObject tooltipPanel; // Il riquadro che contiene la torre
    [SerializeField] private Vector3 tooltipOffset = new Vector3(0, 50, 0); // Offset per posizionare il tooltip sopra il quadrato

    private void Start()
    {
        // Assicuriamoci che il tooltip sia nascosto all'inizio
        tooltipPanel.SetActive(false);
        
    }

    // Quando il mouse entra nel quadrato
    public void OnPointerEnter(PointerEventData eventData)
    {

            ShowTooltip();
        
    }

    // Quando il mouse esce dal quadrato
    public void OnPointerExit(PointerEventData eventData)
    {

            HideTooltip();
        
    }

   
    private void ShowTooltip()
    {
         tooltipPanel.SetActive(true);
        tooltipPanel.transform.position = transform.position + tooltipOffset;
    }

    private void HideTooltip()
    {
         tooltipPanel.SetActive(false);
    }
}