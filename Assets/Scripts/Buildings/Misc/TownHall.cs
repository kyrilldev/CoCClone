using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class TownHall : MonoBehaviour
{
    public static TownHall instance;
    public int lvl;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        lvl = GetLvl();
    }

    private int GetLvl()
    {
        //implement getting data from server
        return Random.Range(1, 5);
    }

    public void Upgrade()
    {
        if (CanUpgrade())
        {
            lvl++;
        }
    }

    public bool CanUpgrade()
    {
        if (GameManager.Instance.TotalCurrency >= GameManager.Instance.TotalCurrency * (lvl / 10))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}