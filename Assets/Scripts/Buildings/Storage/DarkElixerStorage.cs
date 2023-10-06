using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkElixerStorage : StorageTower
{
    public override void AddYourself()
    {
        GameManager.Instance.DarkElixerBuildings.Add(this);
    }
}
