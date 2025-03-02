using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject comands;
    [SerializeField] private GameObject levels;
    [SerializeField] private GameObject shop;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private List<GameObject> items;
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject popupError;

    private bool isThereAnError = false;
    private string itemName = "";

    private Dictionary<string, string> descriptions = new Dictionary<string, string>
    {
        { "1-Mortar" , Const.MORTAR_DESCRIPTION},
        { "1-ATM" , Const.ATM_DESCRIPTION},
        { "1-FragileLauncher", Const.FRAGILE_DESCRIPTION},
        { "0-Kamikaze" ,Const.KAMIKAZE_DESCRIPTION },
        { "0-Drone", Const.DRONE_DESCRIPTION}
    };

    private Dictionary<int, string> error = new Dictionary<int, string>
    {
        { 1 , "You don't have enough diamond."},
        { 2 , "Cheater... Here some diamond..."}
    };

    public void Start() {
        SaveSystem.GetInstance().LoadGame();
    }

    public void PlayLevels(int level)
    {
        Debug.Log("Level: " + level);
        
        switch (level)
        {
            case 1:
                SceneManager.LoadScene("LevelOne");
                break;
            case 2:
                SceneManager.LoadScene("LevelTwo");
                break;
            case 3:
                SceneManager.LoadScene("LevelThree");
                break;
            default:
                SceneManager.LoadScene("Game");
                break;
        }
    }

    public void DeleteSaves()
    {
        SaveSystem.GetInstance().DeleteSaves();
    }

    public void OpenSettings()
    {
        activateCanvas(0);
    }

    public void OpenComands()
    {
        activateCanvas(2);
    }

    public void OpenLevels()
    {
        activateCanvas(3);
    }

    public void OpenShop()
    {
        activateCanvas(4);
        text.text = PlayerInfo.GetInstance().getDiamond().ToString();
    }

    public void ShowCredits()
    {
        activateCanvas(1);
    }

    public void OpenPopUp(string name)
    {
        popup.SetActive(true);
        foreach (var textComponent in popup.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (textComponent.gameObject.name != "TextCost")
            {
                textComponent.text = descriptions[name];
                break;
            }
        }
        itemName = name;
    }
    public void ClosePopUp()
    {
        itemName = "";
        foreach (var textComponent in popup.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (textComponent.gameObject.name != "TextCost")
            {
                textComponent.text = "";
                break;
            }
        }
        popup.SetActive(false);
    }

    public void ManagePopUpError()
    {
        popupError.GetComponentInChildren<TextMeshProUGUI>().text = "";
        popupError.SetActive(false);
    }

    public void BuySomething()
    {
        bool abilityOrTower = false; // false ability true tower
        if (itemName.StartsWith('0'))
        {
            itemName = itemName.Substring(2);
        }
        else
        {
            itemName = itemName.Substring(2);
            abilityOrTower = true;
        }
        if (PlayerInfo.GetInstance().getDiamond() > 2) //Controllare se gli altri sono d'accordo nel mettere il costo a 3 
        {
            PlayerInfo.GetInstance().addDiamond(-3);
            if (abilityOrTower)
            {
                PlayerInfo.GetInstance().UnlockTower(itemName);
            }
            else
            {
                PlayerInfo.GetInstance().UnlockAbility(itemName);
            }
            foreach (GameObject i in items)
            {
                if (i.name == itemName)
                {
                    Destroy(i);
                    items.Remove(i);
                    break;
                }
            }
            text.text = PlayerInfo.GetInstance().getDiamond().ToString();
            SaveSystem.GetInstance().SaveGame();
            //Debug.Log(PlayerInfo.GetInstance().StampaItem());
        }
        else
        {
            Debug.Log("Not enough diamond");
            isThereAnError = true;
            
        }
        popup.SetActive(false);
        itemName = "";
        if (isThereAnError)
        {
            popupError.SetActive(true);
            popupError.GetComponentInChildren<TextMeshProUGUI>().text = error[1];
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Exited"); // Funziona solo in build, non in Editor
    }

    public void GoBack()
    {
       activateCanvas(10);
    }

    private void activateCanvas(int c)
    {
        switch(c)
        {
            case 0:
                start.SetActive(false);
                settings.SetActive(true);
                credits.SetActive(false);
                comands.SetActive(false);
                levels.SetActive(false);
                shop.SetActive(false);
                break;
            case 1:
                start.SetActive(false);
                settings.SetActive(false);
                credits.SetActive(true);
                comands.SetActive(false);
                levels.SetActive(false);
                shop.SetActive(false);
                break;
            case 2:
                start.SetActive(false);
                settings.SetActive(false);
                credits.SetActive(false);
                comands.SetActive(true);
                levels.SetActive(false);
                shop.SetActive(false);
                break;
            case 3:
                start.SetActive(false);
                settings.SetActive(false);
                credits.SetActive(false);
                comands.SetActive(false);
                levels.SetActive(true);
                shop.SetActive(false);
                break;
            case 4:
                start.SetActive(false);
                settings.SetActive(false);
                credits.SetActive(false);
                comands.SetActive(false);
                levels.SetActive(false);
                shop.SetActive(true);
                break;
            default:
                start.SetActive(true);
                settings.SetActive(false);
                credits.SetActive(false);
                comands.SetActive(false);
                levels.SetActive(false);
                shop.SetActive(false);
                break;
        } 
    }


    public void CheatButton()
    {
        PlayerInfo.GetInstance().addDiamond(20);
        popupError.SetActive(true);
        popupError.GetComponentInChildren<TextMeshProUGUI>().text = error[2];
        SaveSystem.GetInstance().SaveDiamond();
    }

}
