using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
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

        int excess = 0;

        int AmountOfTowersInt = 0;
        double AmountOfTowers = 0;
        //does the deposit fit in the first building?
        if (!list[towerIndex].TowerFull() && (list[towerIndex].ResourceAmount + deposit) < list[towerIndex].maxCurrency)
        {
            list[towerIndex].ResourceAmount += deposit;
        }
        else if (list[towerIndex].TowerFull())
        {
            //move onto next tower in the list
            towerIndex++;
        }
        else if (!list[towerIndex].TowerFull() && (list[towerIndex].ResourceAmount + deposit) >= list[towerIndex].maxCurrency)
        {
            //calculate the amount that does fit
            int am = list[towerIndex].maxCurrency = list[towerIndex].ResourceAmount;
            //deposit the amount that does fit
            list[towerIndex].ResourceAmount += am;
            //calculate excess not deposited
            excess = deposit - am;
            //move to next tower
            towerIndex++;
        }

        //als de resourceamount in de current tower met de excess amount kleiner is dan de maxcurrency
        if ((list[towerIndex].ResourceAmount + excess) < list[towerIndex].maxCurrency)
        {
            //deposit the excess;
            list[towerIndex].ResourceAmount += excess;
        }
        else {
            //calculate how many buildings we need to fit the deposit
            AmountOfTowers = excess / list[towerIndex].maxCurrency;
            AmountOfTowersInt = (int)RoundUpValue(AmountOfTowers, 2);
            UnityEngine.Debug.Log("the amount of towers needed are: " + AmountOfTowersInt + "rounded");
        }

        //do have the amount of towers needed?
        int AmountOfTowersLeft = TowersFilled(ref list);
        UnityEngine.Debug.Log("the amount of towers filled are: " + AmountOfTowersLeft);
        //als we genoeg torens hebben
        if (AmountOfTowersInt <= AmountOfTowersLeft)
        {
            //vul alle torens die gevuld kunnen worden
            //for example hier komt 3.2 uit
            decimal fillableTowers = (decimal)(excess / AmountOfTowers);
            //als het getal heel is
            if (fillableTowers % 1 == 0)
            {
                //vul elk gebouw tot de max
                for (int i = towerIndex; i < list.Count - towerIndex; i++)
                {
                    FillBuilding(ref list, i);
                }
            }
            else
            {
                //als het 3.2 is
                int wholeTowers = (int)Decimal.Truncate(fillableTowers);
                //fill whole towers

                //get the decimalnumber

                //calculate how much the decimalnumber means in currency

                //towersindex++;
                //deposit the remaining amount in the final tower
            }
        }
        //als we niet genoeg torens hebben
        else if (AmountOfTowersInt > AmountOfTowersLeft)
        {
            //hoeveel gebouwen hebben we?
            int filleableBuildings = AmountOfTowersLeft;
            //begin bij de towerindex, ga tot het einde van de lijst (kan nog vervangen worden door filleableBuildings)
            for (int i = towerIndex; i < list.Count; i++)
            {
                //use fill function to fill the buildings that we do have
            }

            //calculate how many buildings were not able to filled

            //let's say it's 0.7f
            int wastedDeposit = 0;
            //wastedDeposit = excess - the amount used to fill the other builings;

            UnityEngine.Debug.Log("the amount of:" + wastedDeposit);
        }

        //vervolgens aan het einde runnen we een functie die de totale resources in de list bij elkaar optelt en het bijpassende variabele update
        UIManager.Instance.DisplayCountTotal(list);
    }

    /// <summary>
    /// fill the building that gets given in the parameter
    /// </summary>
    public void FillBuilding(ref List<Building> list, int towerfillindex)
    {
        list[towerfillindex].ResourceAmount = list[towerfillindex].maxCurrency;
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
