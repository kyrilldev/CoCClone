using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<TroopBase> SpawnedEnemies;
    public List<TroopBase> TotalEnemies;
    public TroopBase SelectedTroop;

    private void Awake()
    {
        Instance = this;
        SpawnedEnemies = new List<TroopBase>();
        TotalEnemies = new List<TroopBase>();
    }
    private void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Vector3 MousePos = Input.mousePosition;

        Vector3 objectPos = Camera.current.ScreenToWorldPoint(MousePos);

        Instantiate(SelectedTroop, objectPos, Quaternion.identity);
    }

}
