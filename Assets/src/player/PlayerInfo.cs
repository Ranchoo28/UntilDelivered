using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.AI;
using static CameraController;

public sealed class PlayerInfo
{
    private static PlayerInfo instance;
    private static readonly object lockObject = new object();

    private int exp;
    private int gold;
    private int diamond;
    private string username;
    
    private Dictionary<string, bool> towerUnlocked = new Dictionary<string, bool>
    {
        { Const.MORTAIO_STRING , false},
        { Const.ATM_STRING , false},
        { Const.FRAGILELAUNCHER_STRING, false}
    };

    private Dictionary<string, bool> abilityUnlocked = new Dictionary<string, bool>
    {
        { Const.KAMIKAZE_STRING , false},
        { Const.DRONE_STRING, false}
    };

    private Dictionary<string, bool> levelCompleted = new Dictionary<string, bool>
    {
        { Const.LEVEL_ONE, false},
        { Const.LEVEL_TWO, false},
        { Const.LEVEL_THREE, false}
    };


    private PlayerInfo(){
        exp = 0; 
        gold = 100;
        diamond = 0;
        username = null;
    }

    public static PlayerInfo GetInstance()
    {
        if (instance == null)
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = new PlayerInfo();
                }
            }
        }
        return instance;
    }

    public int getExp(){ return exp; }

    public void addExp(int exp){ this.exp += exp; }

    public int getGold(){ return gold; }

    public void addGold(int gold){ this.gold += gold; }

    public int getDiamond() { return diamond; }

    public void addDiamond(int d) { this.diamond += d; }

    public string getUsername(){ return username; }

    public void setUsername(string n){ this.username = n; }

    public Dictionary<string, bool> getTowerUnlocked() { return towerUnlocked; }
    public Dictionary<string, bool> getAbilityUnlocked() { return abilityUnlocked; }

    public Dictionary<string, bool> getLevelCompleted() { return levelCompleted; }

    public void Reset() {
        exp = 0;
        gold = 100;
        username = null;
        diamond = 0;
    }

    public void ResetGold()
    {
        gold = 100;
    }

    public void UnlockTower(string tower)
    {
        towerUnlocked[tower] = true;
    }

    public void UnlockAbility(string ability)
    {
        abilityUnlocked[ability] = true;
    }

    public bool checkTowerUnlocked(string tower)
    {
        return towerUnlocked[tower];
    }

    public bool checkAbilityUnlocked(string ability)
    {
        return abilityUnlocked[ability];
    }

    public ArrayList ItemUnlocked()
    {
        ArrayList items = new ArrayList();
        foreach (var item in towerUnlocked)
        {
            //items+= "\nKey: " + item.Key + " Value: " + item.Value;
            if (item.Value)
            {
                items.Add(item.Key);
            }
        }
        foreach (var item in abilityUnlocked)
        {
            //items += "\nKey: " + item.Key + " Value: " + item.Value;
            if (item.Value)
            {
                items.Add(item.Key);
            }
        }
        return items;
    }

    public void CompleteLevel(string level)
    {
        levelCompleted[level] = true;
    }
}
