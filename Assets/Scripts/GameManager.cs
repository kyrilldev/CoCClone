using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
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

        int totalResources = 0;
        for (int i = 0; i < list.Count; i++)
        {
            totalResources += list[i].ResourceAmount;
        }

        int resourceRoomLeft = totalResourceRoom - totalResources;

        if (TotalCoinAmount < totalResourceRoom)
        {
            //init
            List<int> roomLeftPerTower = new();

            foreach (Building b in list)
            {
                int roomLeft = b.maxCurrency - b.ResourceAmount;
                roomLeftPerTower.Add(roomLeft);
            }

            //als de deposit groter is dan de hoeveelheid ruimte over
            if (deposit > resourceRoomLeft)
            {
                //calculate overflow
                int overFlow = deposit - resourceRoomLeft;
                deposit = deposit - overFlow;
            }

            //filling the actual towers
            for (int i = 0; i < roomLeftPerTower.Count; i++)
            {
                //als er geen ruimte meer is in de betrefte ruimte of de deposit leeg is
                if (deposit <= 0)
                {
                    //breek uit de for-loop
                    break;
                }
                //als de deposit kleiner is dan de ruimte in de toren AKA (als het past in deze ene toren)
                else if (roomLeftPerTower[i] >= deposit)
                {
                    int amountDeposited = list[i].ResourceAmount += deposit;
                    deposit -= amountDeposited;
                }
                //als er te veel deposit is voor deze tower AKA (als de deposit te groot is voor de tower)
                else if (roomLeftPerTower[i] < deposit)
                {
                    //set how much room there is in the tower
                    int roomLeft = roomLeftPerTower[i];
                    //subtract the amount were gonna deposit from the deposit
                    deposit -= roomLeft;
                    //top up the tower
                    list[i].ResourceAmount += roomLeft;
                }
            }

            CalculateMaxCurrency(list);
            UIManager.Instance.UpdateCurrencies();
        }
    }

    private void CalculateMaxCurrency(List<Building> list)
    {
        TotalCoinAmount = 0;
        for (int i = 0; i < list.Count; i++)
        {
            TotalCoinAmount += list[i].ResourceAmount;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            FillStorageTower(ref CoinBuildings, 5000);
        }
    }
}