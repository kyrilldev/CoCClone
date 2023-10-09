using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DarkElixerStorage : StorageTower
{
    protected override void Start()
    {
        base.Start();
        AddYourself();
    }

    public override void AddYourself()
    {
        GameManager.Instance.DarkElixerBuildings.Add(this);
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
