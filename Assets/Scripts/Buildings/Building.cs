using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int ResourceAmount;
    public float InitTime;
    public float AttackTime;
    public bool TownHall;

    /// <summary>
    /// Initialize the building
    /// </summary>
    public IEnumerator Init()
    {
        yield return new WaitForSeconds(InitTime);
    }

    /// <summary>
    /// Throw or shoot for a attacking building (Override-able)
    /// </summary>
    public virtual void Shoot()
    {

    }
}
