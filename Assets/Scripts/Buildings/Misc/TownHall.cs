using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class TownHall : MonoBehaviour
{
    int lvl;

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
        lvl++;
    }
}
