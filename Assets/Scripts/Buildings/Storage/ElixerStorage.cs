using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElixerStorage : StorageTower
{
    protected override void Start()
    {
        base.Start();
        AddYourself();
    }

    public override void AddYourself()
    {
        GameManager.Instance.ElixerBuildings.Add(this);
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
