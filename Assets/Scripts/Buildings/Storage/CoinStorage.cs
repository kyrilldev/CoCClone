using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinStorage : StorageTower
{
    protected override void Start()
    {
        base.Start();
        AddYourself();
    }

    public override void AddYourself()
    {
        GameManager.Instance.CoinBuildings.Add(this);
    }

    private void FixedUpdate()
    {
        if (ResourceAmount == maxCurrency && hasBeenPoppedUp)
        {
            PopUp();
        }
        else if (ResourceAmount != maxCurrency && hasBeenPoppedDown)
        {
            PopDown();
        }
    }
}
