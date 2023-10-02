using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public enum Currency
{
    Coins,
    DarkElixer,
    Elixer
}

public class Building : MonoBehaviour
{
    public int ResourceAmount;
    public float InitTime;
    public float AttackTime;
    public bool TownHall;

    [SerializeField] private bool defenseTower;
    public int minCurrency = 0;
    public int maxCurrency = 5000;

    public Currency currency;

    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// Initialize the building
    /// </summary>
    private void Init()
    {
        AddYourself();
    }

    /// <summary>
    /// Throw or shoot for a attacking building (Override-able)
    /// </summary>
    public virtual void Shoot()
    {

    }

    /// <summary>
    /// unnecessary but cool
    /// </summary>
    /// <returns></returns>
    public bool IsDefenseTower() => defenseTower;

    /// <summary>
    /// generates a currency amount between min and max
    /// </summary>
    /// <returns></returns>
    //public int CurrencyAmount() { int amount = Random.Range(minCurrency, maxCurrency); return amount; }

    public void AddYourself()
    {
        switch (currency)
        {
            case Currency.Coins:
                GameManager.Instance.CoinBuildings.Add(this);
                break;
            case Currency.DarkElixer:
                GameManager.Instance.DarkElixerBuildings.Add(this);
                break;
            case Currency.Elixer:
                GameManager.Instance.ElixerBuildings.Add(this);
                break;
        }
    }
    /// <summary>
    /// WIP need to add json functionality
    /// </summary>
    private async void GetCurrency()
    {
        await Task.Delay(100);
        Debug.Log("Getting currency");
        //get the currency from a json
        int remoteCurrency = 0;//use this var

        ResourceAmount = remoteCurrency;
    }


}
