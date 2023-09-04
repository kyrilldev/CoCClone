using System.Collections;
using System.Collections.Generic;
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
    private int minCurrency = 500;
    private int maxCurrency = 5000;
    public int StoredCurrency;

    public Currency currency; 

    /// <summary>
    /// Initialize the building
    /// </summary>
    public IEnumerator Init()
    {
        StoredCurrency = CurrencyAmount();

        if (!IsDefenseTower())
            ResourceAmount = minCurrency;
        yield return new WaitForSeconds(InitTime);
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
    public int CurrencyAmount() { int amount = Random.Range(minCurrency, maxCurrency); return amount; }


}
