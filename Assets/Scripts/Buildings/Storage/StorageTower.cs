using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StorageTower : MonoBehaviour
{
    public int ResourceAmount;

    public int minCurrency = 0;
    public int maxCurrency = 5000;

    //start at lvl 1
    public int lvl = 1;

    public bool hasBeenPoppedUp;
    public bool hasBeenPoppedDown = true;
    public GameObject popUp;

    protected virtual void Start()
    {
        if (ResourceAmount >= maxCurrency)
        {
            PopUp();
        }
        else
        {
            PopDown();
        }
    }

    public virtual void AddYourself()
    {
        
    }

    public void Upgrade()
    {
        if (CanUpgrade())
        {
            lvl++;
        }
    }

    public bool CanUpgrade()
    {
        if (lvl < TownHall.instance.lvl)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PopUp()
    {
        hasBeenPoppedUp = true; hasBeenPoppedDown = false;
        if (ResourceAmount >= maxCurrency)
            popUp.GetComponentInChildren<TextMeshPro>().text = "Full";

        popUp.GetComponent<Animator>().Play("FadeIn");
    }

    public void PopDown()
    {
        hasBeenPoppedUp = false; hasBeenPoppedDown = true;
        popUp.GetComponent<Animator>().Play("FadeOut");
    }
}
