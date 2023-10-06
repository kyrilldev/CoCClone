using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElixerStorage : StorageTower
{
    public override void AddYourself()
    {
        GameManager.Instance.ElixerBuildings.Add(this);
    }
}
