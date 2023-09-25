using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<TroopBase> SpawnedEnemies;
    public List<TroopBase> TotalEnemies;
    public TroopBase SelectedTroop;

    //need to be stored in database
    [SerializeField] private int TotalAmountBuilders;
    [SerializeField] private int TotalCoinAmount;
    [SerializeField] private int TotalDarkElixirAmount;
    [SerializeField] private int TotalElixirAmount;

    [SerializeField] private int MaxBuilders = 3;

    public List<Building> CoinBuildings;
    public List<Building> ElixerBuildings;
    public List<Building> DarkElixerBuildings;

    private void Awake()
    {
        Instance = this;
        SpawnedEnemies = new List<TroopBase>();
        TotalEnemies = new List<TroopBase>();

        CoinBuildings = new();
        ElixerBuildings = new();
        DarkElixerBuildings = new();
    }

    private void Start()
    {
        GetCurrency();
    }

    private void OnMouseDown()
    {
        Vector3 MousePos = Input.mousePosition;

        Vector3 objectPos = Camera.current.ScreenToWorldPoint(MousePos);

        Instantiate(SelectedTroop, objectPos, Quaternion.identity);
    }

    #region Currency Saving

    private void OnApplicationQuit()
    {
        SetCurrency();
    }

    /// <summary>
    /// replace with sql database later on
    /// </summary>
    private void GetCurrency()
    {
        TotalAmountBuilders = PlayerPrefs.GetInt("TotalAmountBuilders");
        TotalCoinAmount = PlayerPrefs.GetInt("TotalCoinAmount");
        TotalDarkElixirAmount = PlayerPrefs.GetInt("TotalDarkElixirAmount");
        TotalElixirAmount = PlayerPrefs.GetInt("TotalElixirAmount");
    }
    /// <summary>
    /// replace with sql database later on
    /// </summary>
    private void SetCurrency()
    {
        PlayerPrefs.SetInt("TotalAmountBuilders", TotalAmountBuilders);
        PlayerPrefs.SetInt("TotalCoinAmount", TotalCoinAmount);
        PlayerPrefs.SetInt("TotalDarkElixirAmount", TotalDarkElixirAmount);
        PlayerPrefs.SetInt("TotalElixirAmount", TotalElixirAmount);
    }

    #endregion

    public int TotalCoins() => TotalCoinAmount;
    public int TotalDarkElixir() => TotalDarkElixirAmount;
    public int TotalElixir() => TotalElixirAmount;
    public int TotalBuilders() => TotalAmountBuilders;
    public int TotalMaxBuilders() => MaxBuilders;

    /// <summary>
    /// fills the resource amount given across the 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="deposit"></param>
    public void FillStorageTower(ref List<Building> list, int deposit)
    {
        int towerIndex = 0;

        List<int> maxes = new();
        for (int i = 0; i < list.Count; i++)
        {
            maxes.Add(list[i].maxCurrency);
        }
        List<int> resources = new();
        for (int i = 0; i < list.Count; i++)
        {
            resources.Add(list[i].ResourceAmount);
        }

        int totalResourceRoom = 0;
        for (int i = 0; i < list.Count; i++)
        {
            totalResourceRoom += list[i].maxCurrency;
        }
        print("total amount of room is: " + totalResourceRoom);

        int totalResources = 0;
        for (int i = 0; i < list.Count; i++)
        {
            totalResources += list[i].ResourceAmount;
        }
        print("total amount of resources stored are: " + totalResources);

        int resourceRoomLeft = totalResourceRoom - totalResources;

        List<int> roomLeftPerTower = new();

        foreach (Building b in list)
        {
            int roomLeft = b.maxCurrency - b.ResourceAmount;
            roomLeftPerTower.Add(roomLeft);
        }

        if (deposit > totalResourceRoom)
        {
            int overFlow = deposit -= totalResourceRoom;
            print("the amount of: " + overFlow + " was deleted due to lack of storage");
        }

        //filling the actual towers
        for (int i = 0; i < roomLeftPerTower.Count; i++)
        {
            //als er geen ruimte meer is in de betrefte ruimte of de deposit leeg is
            if (roomLeftPerTower[i] <= 0 || deposit <= 0)
            {
                //breek uit de for-loop
                break;
            }
            //als er niet genoeg deposit is de een tower te vullen
            else if (roomLeftPerTower[i] >= deposit)
            {
                int fillAmount = deposit;
                deposit = 0;
                list[i].ResourceAmount += fillAmount;
            }
            //als er te veel deposit is voor deze tower
            else
            {
                //indicate how much room there is in the tower
                int roomLeft = roomLeftPerTower[i];
                //subtract the amount were gonna deposit from the deposit
                deposit -= roomLeft;
                //top up the tower
                list[i].ResourceAmount += roomLeft;
            }
        }

        CalculateMaxCurrency(list);

        //vervolgens aan het einde runnen we een functie die de totale resources in de list bij elkaar optelt en het bijpassende variabele update
        //UIManager.Instance.DisplayCountTotal(list);
    }

    private void CalculateMaxCurrency(List<Building> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            TotalCoinAmount += list[i].ResourceAmount;
        }
    }

    /// <summary>
    /// fill the building that gets given in the parameter
    /// </summary>
    public void FillBuilding(ref List<Building> list, int towerfillindex)
    {
        list[towerfillindex].ResourceAmount = list[towerfillindex].maxCurrency;
    }

    public void FillAllBuildings(List<Building> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].ResourceAmount = list[i].maxCurrency;
        }
    }

    /// <summary>
    /// returns the amount of towers that are not filled all the way
    /// </summary>
    /// <returns></returns>
    public int TowersFilled(ref List<Building> list)
    {
        int filledAmount = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].TowerFull())
            {
                filledAmount++;
            }
        }

        return filledAmount;
    }

    public double RoundUpValue(double value, int decimalpoint)
    {
        var result = Math.Round(value, decimalpoint);
        if (result < value)
        {
            result += Math.Pow(10, -decimalpoint);
        }
        return result;
    }

    public int CountTotal(List<Building> list)
    {
        List<Building> result;
        result = CountTotalListDecider(list[0]);

        int totalDisplayCurrency = 0;

        foreach (Building building in result)
        {
            //count up the amount of resources per building
            totalDisplayCurrency += building.ResourceAmount;
        }

        return totalDisplayCurrency;
    }

    private List<Building> CountTotalListDecider(Building list)
    {
        if (list.currency == Currency.Coins)
        {
            return CoinBuildings;
        }
        else if (list.currency == Currency.DarkElixer)
        {
            return DarkElixerBuildings;
        }
        else
        {
            return ElixerBuildings;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            FillStorageTower(ref CoinBuildings, 4500);
        }
    }

}
