using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElixerMine : MineTower
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        StartCoroutine(Mine());
    }

    public void OnMouseDown()
    {
        SendStoredResources();
    }

    public override void SendStoredResources()
    {
        if (ResourceAmount != minCurrency || ResourceAmount >= maxCurrency)
        {
            GameManager.Instance.FillStorageTower(ResourceAmount, GameManager.Instance.ElixerBuildings);
        }
        else
        {
            Debug.Log("empty tower");
        }
    }

    public override IEnumerator Mine()
    {
        //always mining
        ResourceAmount += (1 * LvL);
        yield return new WaitForSeconds(1);
        StartCoroutine(Mine());
    }
}
