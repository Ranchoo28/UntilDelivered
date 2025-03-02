using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public sealed class SaveSystem {

    private static SaveSystem instance;
    private static readonly object lockSingleton = new object();


    private SaveSystem() { }
    public static SaveSystem GetInstance() {
        if (instance == null) {
            lock (lockSingleton) {
                if (instance == null) {
                    instance = new SaveSystem();
                }
            }
        }
        return instance;

    }

    public void SaveGame() {
        SavePlayerInfo();
        SaveAbilityUnlocked();
        SaveTowerUnlocked();
        SaveMissionCompleted();
        Debug.Log("Game Saved");
    }

    public void LoadGame() {
        LoadPlayerInfo();
        LoadAbilityUnlocked();
        LoadTowerUnlocked();
        LoadMissionCompleted();
        Debug.Log("Game Loaded");
    }

    private void SavePlayerInfo() {
        PlayerPrefs.SetInt(Const.EXP_STRING, PlayerInfo.GetInstance().getExp());
        PlayerPrefs.SetInt(Const.GOLD_STRING, PlayerInfo.GetInstance().getGold());
        PlayerPrefs.SetInt(Const.DIAMOND_STRING, PlayerInfo.GetInstance().getDiamond());
        PlayerPrefs.SetString(Const.USERNAME_STRING, PlayerInfo.GetInstance().getUsername());
    }

    private void SaveMissionCompleted()
    {

    }

    private void SaveAbilityUnlocked() {
        foreach (var keyValuePair in PlayerInfo.GetInstance().getAbilityUnlocked()) {
            PlayerPrefs.SetInt(keyValuePair.Key.ToString(), keyValuePair.Value ? 1 : 0);
        }
    }

    private void SaveTowerUnlocked() {
        foreach (var keyValuePair in PlayerInfo.GetInstance().getTowerUnlocked()) {
            PlayerPrefs.SetInt(keyValuePair.Key.ToString(), keyValuePair.Value ? 1 : 0);
        }
    }

    public void SaveDiamond() {
        PlayerPrefs.SetInt(Const.DIAMOND_STRING, PlayerInfo.GetInstance().getDiamond());
    }


    private void LoadPlayerInfo() {
        PlayerInfo.GetInstance().addExp(PlayerPrefs.GetInt(Const.EXP_STRING));
        PlayerInfo.GetInstance().addGold(PlayerPrefs.GetInt(Const.GOLD_STRING));
        PlayerInfo.GetInstance().addDiamond(PlayerPrefs.GetInt(Const.DIAMOND_STRING));
        PlayerInfo.GetInstance().setUsername(PlayerPrefs.GetString(Const.USERNAME_STRING));
    }

    private void LoadMissionCompleted() 
    {
    }

    private void LoadAbilityUnlocked() {
        Dictionary<string, bool> abilities = new Dictionary<string, bool>();
        foreach (var ability in PlayerInfo.GetInstance().getAbilityUnlocked()) {
            abilities.Add(ability.Key, PlayerPrefs.GetInt(ability.Key.ToString(), 0) == 1);
        }

        PlayerInfo.GetInstance().getAbilityUnlocked().Clear();

        foreach (var ability in abilities) {
            PlayerInfo.GetInstance().getAbilityUnlocked().Add(ability.Key, ability.Value);
        }
    }

    
    private void LoadTowerUnlocked() {
        Dictionary<string, bool> towers = new Dictionary<string, bool>();
        foreach (var tower in PlayerInfo.GetInstance().getTowerUnlocked()) {
            towers.Add(tower.Key, PlayerPrefs.GetInt(tower.Key.ToString(), 0) == 1);
        }

        PlayerInfo.GetInstance().getTowerUnlocked().Clear();

        foreach (var tower in towers) {
            PlayerInfo.GetInstance().getTowerUnlocked().Add(tower.Key, tower.Value);
        }
    }

    public void DeleteSaves() {
        PlayerPrefs.DeleteAll();
        PlayerInfo.GetInstance().Reset();
        Debug.Log("Game Reset");
    }

}
