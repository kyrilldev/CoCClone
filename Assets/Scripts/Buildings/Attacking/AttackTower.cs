using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum Currency
{
    Coins,
    DarkElixer,
    Elixer
}

public class AttackTower : MonoBehaviour
{

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// Initialize the building
    /// </summary>
    private void Init()
    {
        
    }
    /// <summary>
    /// WIP need to add json functionality
    /// </summary>
    public virtual async void GetCurrency()
    {
        await Task.Delay(100);
        //Debug.Log("Getting currency");
        ////get the currency from a json
        //int remoteCurrency = 0;//use this var

        //ResourceAmount = remoteCurrency;
    }
}
