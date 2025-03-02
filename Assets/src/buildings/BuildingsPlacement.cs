using UnityEngine;
using UnityEngine.UI;

public class BuildingsPlacement : MonoBehaviour
{
    [SerializeField] private LayerMask PlayerCheckMask; // LayerMask per controllare la presenza di giocatori o altri oggetti che impediscono il piazzamento della torre.
    [SerializeField] private LayerMask PlacementCollideMask; // LayerMask per determinare le superfici valide per il piazzamento della torre.
    [SerializeField] private Camera PlayerCamera; // La telecamera del giocatore usata per ottenere la posizione del cursore.
    [SerializeField] private Material canPlace;
    [SerializeField] private Material cantPlace;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private GameObject hud; // Riferimento al GameObject dell'HUD
    [SerializeField] private Text errorMessageText; // Riferimento al testo dell'errore

    [SerializeField] private GameObject placementHintPrefab; // Prefab per il testo con i comandi

    [SerializeField] private Material materialCanPlace; 
    [SerializeField] private Material materialCantPlace; 

    private Button towerButton;
    private GameObject CurrentPlacingTower; // La torre attualmente in fase di piazzamento.
    private int typeOfBuildings;
    private int price;


    void Start() {
        towerButton = GameObject.FindWithTag(Const.TOWER_NAME_BTN).GetComponent<Button>();
        errorMessageText.gameObject.SetActive(false); // Nasconde il messaggio di errore all'avvio
    }

    void Update()
    {
        if (CurrentPlacingTower == null)
        {

            // Riabilita l'HUD quando non si sta piazzando una torre
            if (hud != null && !hud.activeSelf)
            {
                hud.SetActive(true);
            }

            towerButton.interactable = true;
        }
        if (CurrentPlacingTower != null)
        {
            // Disabilita l'HUD quando si sta piazzando una torre
            if (hud != null && hud.activeSelf)
            {
                hud.SetActive(false);
            }
            towerButton.interactable = false;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                CancelBuildingPlacement();
                return;
            }

            Ray camray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit HitInfo;

            if (Physics.Raycast(camray, out HitInfo, 300f, PlacementCollideMask))
            {
                CurrentPlacingTower.transform.position = HitInfo.point;

                placementHintPrefab.transform.position = new Vector3(Input.mousePosition.x + 50, Input.mousePosition.y - 50, Input.mousePosition.z);
                placementHintPrefab.SetActive(true);
                
                if (
                    !HitInfo.collider.gameObject.CompareTag(Const.CANT_PLACE_TOWER_TAG) &&
                    !HitInfo.collider.gameObject.CompareTag(Const.POSTA_TARGET_TAG) &&
                    !HitInfo.collider.gameObject.CompareTag(Const.TURRET_TAG) &&
                    !HitInfo.collider.gameObject.CompareTag(Const.ENEMY_TAG))
                {

                    BoxCollider TowerCollider = CurrentPlacingTower.gameObject.GetComponent<BoxCollider>();
                    TowerCollider.isTrigger = true;

                    Vector3 BoxCenter = CurrentPlacingTower.transform.TransformPoint(TowerCollider.center);
                    Vector3 HalfExtents = Vector3.Scale(TowerCollider.size * 0.5f, CurrentPlacingTower.transform.lossyScale);

                    if (!Physics.CheckBox(BoxCenter, HalfExtents, Quaternion.identity, PlayerCheckMask, QueryTriggerInteraction.Ignore))
                    {
                        ColorTower(canPlace);
                        ShowBuildingRange(true, true); // Mostra l'anteprima del range
                    }
                    else
                    {
                        
                        ColorTower(cantPlace);
                        ShowBuildingRange(true, false); // Nasconde l'anteprima se non è piazzabile
                    }
                }
                else
                {
                    ColorTower(cantPlace);
                    ShowBuildingRange(true, false); // Nasconde l'anteprima se non è piazzabile
                }
            }

            if (Input.GetMouseButtonDown(0) && HitInfo.collider.gameObject != null)
            {
                if (!HitInfo.collider.gameObject.CompareTag("CantPlaceTower") &&
                    !HitInfo.collider.gameObject.CompareTag("PostaTarget") &&
                    !HitInfo.collider.gameObject.CompareTag("Turret") &&
                    !HitInfo.collider.gameObject.CompareTag("Enemy"))
                {
                    BoxCollider TowerCollider = CurrentPlacingTower.gameObject.GetComponent<BoxCollider>();
                    TowerCollider.isTrigger = true;

                    Vector3 BoxCenter = CurrentPlacingTower.transform.TransformPoint(TowerCollider.center);
                    Vector3 HalfExtents = Vector3.Scale(TowerCollider.size * 0.5f, CurrentPlacingTower.transform.lossyScale);

                    if (!Physics.CheckBox(BoxCenter, HalfExtents, Quaternion.identity, PlayerCheckMask, QueryTriggerInteraction.Ignore))
                    {
                        ColorTower(defaultMaterial);
                        PlayerInfo.GetInstance().addGold(-price);
                        switch (typeOfBuildings)
                        {
                            case 0:
                                CurrentPlacingTower.GetComponent<AbstractAttackBuilding>().enabled = true;
                                break;
                            case 1:
                                CurrentPlacingTower.GetComponent<AbstractGenerativeBuildings>().enabled = true;
                                break;
                        }
                        
                        TowerCollider.isTrigger = false;
                        ShowBuildingRange(false, false); // Rimuove l'anteprima del range
                        ShowErrorMessage("Turret placed!", "blue"); // Messaggio di conferma
                        placementHintPrefab.SetActive(false);
                        CurrentPlacingTower = null;                      
                    }
                    else
                    {
                        ShowErrorMessage("Turret cannot be placed here!", "red"); // Mostra il messaggio di errore
                    }
                }
                else
                {
                    ShowErrorMessage("Turret cannot be placed here!", "red"); // Mostra il messaggio di errore
                }
            }
        }
    }

    public void CancelBuildingPlacement()
    {
        if (CurrentPlacingTower != null)
        {
            Destroy(CurrentPlacingTower);
            placementHintPrefab.SetActive(false);
            CurrentPlacingTower = null;
            hud.SetActive(true);
            towerButton.interactable = true;
            ShowErrorMessage("Turret placement canceled", "red");
            Invoke("HideErrorMessage", 2f); // Nasconde il messaggio dopo 2 secondi;
        }
    }

    public void ShowErrorMessage(string errorMessage, string color)
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = "<b><color=" + color + ">" + errorMessage + "</color></b>"; // Testo bold e colorato
            errorMessageText.gameObject.SetActive(true);
            Invoke("HideErrorMessage", 2f); // Nasconde il messaggio dopo 2 secondi;
        }
    }

    public void HideErrorMessage()
    {
        if(errorMessageText != null)
        {
            errorMessageText.gameObject.SetActive(false);
        }
    }   

    public void ColorTower(Material material)
    {
        Transform[] childTransforms = CurrentPlacingTower.GetComponentsInChildren<Transform>();

        foreach (Transform child in childTransforms)
        {
            if (child.CompareTag("CanBeColored"))
            {
                MeshRenderer renderer = child.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material = material;
                }
            }
        }
    }

    public void SetBuildingToPlace(GameObject tower)
    {
        if (tower.GetComponent<AbstractAttackBuilding>() != null)   
        {
            typeOfBuildings = tower.GetComponent<AbstractAttackBuilding>().Id;
            price = tower.GetComponent<AbstractAttackBuilding>().Price;
        }
        else if (tower.GetComponent<AbstractGenerativeBuildings>() != null)
        {
            typeOfBuildings = tower.GetComponent<AbstractGenerativeBuildings>().Id;
            price = tower.GetComponent<AbstractGenerativeBuildings>().Price;
        }
        if (PlayerInfo.GetInstance().getGold() >= price) {
            CurrentPlacingTower = Instantiate(tower, Vector3.zero, Quaternion.identity);
            ShowBuildingRange(true, true);
        }
        else
        {
            Debug.Log("Sei povero");
        }
            
    }

    public void ShowBuildingRange(bool show, bool canPlace)
    {
        if(CurrentPlacingTower != null)
        {
            // Ottieni il componente SphereCollider usato per il range
            SphereCollider rangeCollider = CurrentPlacingTower.GetComponent<SphereCollider>();
            if(rangeCollider != null)
            {
                // Cerca o crea un oggetto visivo per rappresentare il range
                Transform rangePreview = CurrentPlacingTower.transform.Find("RangePreview");
                if(rangePreview == null)
                {
                    // Crea un oggetto sfera trasparente per mostrare il range
                    GameObject rangeVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    rangeVisualizer.name = "RangePreview";
                    rangeVisualizer.transform.SetParent(CurrentPlacingTower.transform);
                    rangeVisualizer.transform.localPosition = rangeCollider.center; // Centra sul collider
                    //rangeVisualizer.transform.localScale = Vector3.one * rangeCollider.radius * 2;  // Scala in base al raggio(SFERA)
                    rangeVisualizer.transform.localScale = new Vector3(rangeCollider.radius * 2, 0.1f, rangeCollider.radius * 2); // PIATTA
                    rangeVisualizer.GetComponent<Collider>().enabled = false; // Disabilita il collider della sfera

                    rangePreview = rangeVisualizer.transform;

                }

                // Modifica il colore in base a se può essere piazzato o no

                if (canPlace)
                {
                    rangePreview.GetComponent<Renderer>().material = materialCanPlace; // Azzurro trasparente
                }
                else
                {
                    rangePreview.GetComponent<Renderer>().material = materialCantPlace; // Rosso trasparente
                }
                    
                // Mostra o nascondi l'anteprima
                rangePreview.gameObject.SetActive(show);

            }
        }
    }

}
