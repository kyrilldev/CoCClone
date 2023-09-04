using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Fields")]
    public TextMeshProUGUI TextBuilders;
    public TextMeshProUGUI TextDarkElixir;
    public TextMeshProUGUI TextElixir;
    public TextMeshProUGUI TextCoins;

    [Header("Edit")]
    public Button ButtonEditMode;
    public bool EditMode;

    [Header("Battle")]
    public Button ButtonBattle;
    public bool Battle;

    [Header("Panels")]
    [SerializeField] private GameObject BattlePanel;
    [SerializeField] private GameObject LevelPanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject GUI;

    [SerializeField] private List<GameObject> PanelList;
    [SerializeField] private int PanelIndex;

    [Header("Misc")]  
    public Vector3 increasedScale;

    public void TestFunction() { Debug.Log("this is working"); }

    private void Start()
    {
        PanelList = new List<GameObject>
        {
            GUI,
            LevelPanel,
            SettingsPanel,
            BattlePanel
        };
    }

    private void FixedUpdate()
    {
        UpdateCurrencies();

        switch (PanelIndex)
        {
            case 1:
                ForLooping(0);
                break;
            case 2:
                ForLooping(1);
                break;
            case 3:
                ForLooping(2);
                break;
            case 4:
                ForLooping(3);
                break;
        }
    }

    private void ForLooping(int active)
    {
        for (int i = 0; i < PanelList.Count; i++)
        {
            if (i == active)
            {
                PanelList[i].SetActive(true);
            }
            else if (i != active)
            {
                PanelList[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// sets the vars in the Gamemanager.cs to TMProUGUI types
    /// </summary>
    private void UpdateCurrencies()
    {
        TextBuilders.text = GameManager.Instance.TotalBuilders().ToString() + " of " + GameManager.Instance.TotalMaxBuilders().ToString();
        TextDarkElixir.text = GameManager.Instance.TotalDarkElixir().ToString();
        TextElixir.text = GameManager.Instance.TotalElixir().ToString();
        TextCoins.text = GameManager.Instance.TotalCoins().ToString();
    }

    #region Panel Functions
    public void ShowBattlePanel() { PanelIndex = 4; }

    public void ShowLevelPanel() { PanelIndex = 2; }

    public void ShowSettingsPanel() { PanelIndex = 3; }

    public void ShowGUI() { PanelIndex = 1; }

    #endregion
}