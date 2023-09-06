using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

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

        //if the added amount exceeds the max currency
        for (int i = 0; i < list.Count; i++)
        {
            //if the current resource amount in the tower is already maxed
            if (list[i].ResourceAmount == list[i].maxCurrency)
            {
                //we move onto next tower
                towerIndex++;
            }
            //if the resource amount in the selected tower is not full we don't count up
            else if (list[i].ResourceAmount != list[i].maxCurrency)
            {
                break;
            }
        }

        //kijken of de hele deposit past (zoland het deposit + resourceamount minder is dan maxcurrency)
        if ((list[towerIndex].ResourceAmount + deposit) <= list[towerIndex].maxCurrency)
        {
            UnityEngine.Debug.Log("hij komt hier1");
            //als het wel past dan tellen we het erbij op
            list[towerIndex].ResourceAmount += deposit;
        }
        //als het niet past
        else if ((list[towerIndex].ResourceAmount + deposit) > list[towerIndex].maxCurrency)
        {
            UnityEngine.Debug.Log("hij komt hier2");

            UnityEngine.Debug.Log("this is the deposit atm: " + deposit);
            UnityEngine.Debug.Log("this is the maxcurrency atm: " + list[towerIndex].maxCurrency);

            //vervolgens doen we een berekening om te kijken hoeveel torens er nodig zijn voor de deposit grootte
            //deposit / list[towerIndex].maxCurrency = hoeveel torens (oneven groot en deels van de tijd) oftewel afronden naar boven (heel getal)
            double AmountOfTowers = deposit / list[towerIndex].maxCurrency;
            UnityEngine.Debug.Log("the amount of towers needed are: " + AmountOfTowers);
            int TowersLeft = (int)RoundUpValue(AmountOfTowers, 2);
            //als de hoeveelheid towers nodig groter is dan de hoeveelheid beschikbaar
            if (TowersLeft > list.Count)
            {
                UnityEngine.Debug.Log("komt hij hier wel?");
                //skip naar de hoeveelheid towers
                //AKA het nummer van de toren die gevult kunnen worden
                towerIndex = TowersLeft - list.Count;

                //bereken hoeveel towers aan currency er niet past
                int currencyamount = list.Count * list[towerIndex].maxCurrency;
                //deposit minus the amount you can't store
                int excess = deposit -= currencyamount;
                //SHOWS MSG THAT CURRENCY WAS DELETED MUHAHAH (TODO)
                UnityEngine.Debug.Log("the amount of " + excess + " was deleted because you don't have enough storage space");
            }
        }

        //het geld werkelijk verdelen over de torens
        for (int i = 0; i < list.Count; i++)
        {
            if (deposit > list[i].maxCurrency)
            {
                deposit -= list[i].maxCurrency;
                list[i].ResourceAmount = list[i].maxCurrency;
            }
            //als de deposit inmiddels kleiner is dan de maxcurrency
            else if (deposit < list[i].maxCurrency)
            {
                //voeg het overige van de deposit toe aan de storage van de toren
                list[i].ResourceAmount += deposit;
            }
        }

        //vervolgens aan het einde runnen we een functie die de totale resources in de list bij elkaar optelt en het bijpassende variabele update
        UIManager.Instance.DisplayCountTotal(list);
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
        List<Building> result = new List<Building>();
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
            FillStorageTower(ref CoinBuildings, 5500);
        }
    }

}
