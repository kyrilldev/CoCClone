using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

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

    private void Awake()
    {
        Instance = this;
    }

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
    public void UpdateCurrencies()
    {
        TextBuilders.text = GameManager.Instance.TotalBuilders().ToString() + " of " + GameManager.Instance.TotalMaxBuilders().ToString();
        TextDarkElixir.text = GameManager.Instance.TotalDarkElixir().ToString();
        TextElixir.text = GameManager.Instance.TotalElixir().ToString();
        TextCoins.text = GameManager.Instance.TotalCoins().ToString();
    }


    #region Panel Functions
    public void ShowBattlePanel() { ForLooping(3); }

    public void ShowLevelPanel() { ForLooping(1); }

    public void ShowSettingsPanel() { ForLooping(2); }

    public void ShowGUI() { ForLooping(0); }

    #endregion
}