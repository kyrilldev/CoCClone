using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Troop> SpawnedEnemies;
    public List<Troop> TotalEnemies;
    public Troop SelectedTroop;

    private void Awake()
    {
        Instance = this;
        SpawnedEnemies = new List<Troop>();
        TotalEnemies = new List<Troop>();
    }
    private void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Instantiate(SelectedTroop);
    }
}
