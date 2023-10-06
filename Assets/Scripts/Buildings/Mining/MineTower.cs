using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MineTower: MonoBehaviour
{
    public int ResourceAmount;

    public int minCurrency = 0;
    public int maxCurrency = 500;

    public int LvL;

    public virtual void Init()
    {
        //get level onLoad
        LvL = GetLvl();
    }

    public virtual IEnumerator Mine()
    {
        yield return null;
    }

    private int GetLvl()
    {
        //implement getting data from server
        return Random.Range(1,5);
    }

    public virtual void SendStoredResources() { }
}
