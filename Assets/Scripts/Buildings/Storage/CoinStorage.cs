using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinStorage : StorageTower
{
    public override void AddYourself()
    {
        GameManager.Instance.CoinBuildings.Add(this);
    }
}
