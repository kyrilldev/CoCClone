using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [Header("Volume sliders")]
    public AudioSource Music;
    public AudioSource Alerts;
    public AudioSource SFX;

    [Header("Dropdowns")]
    public TMP_Dropdown _Vsync;
    public TMP_Dropdown _Quality;
    public TMP_Dropdown _MipMap;
    public TMP_Dropdown _Aaliasing;

    [Header("Sliders")]
    public Slider _music;
    public Slider _alerts;
    public Slider _sfx;

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

    public void Vsync(TMP_Dropdown dropdown)
    {
        QualitySettings.vSyncCount = dropdown.value;
        PlayerPrefs.SetInt("Vsync", QualitySettings.vSyncCount);
    }

    public void Quality(TMP_Dropdown dropdown)
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("Quality", QualitySettings.GetQualityLevel());
    }

    public void AntiAliasing(TMP_Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            case 0:
                QualitySettings.antiAliasing = dropdown.value;
                break;
            case 1:
                QualitySettings.antiAliasing = dropdown.value * 2;
                break;
            case 2:
                QualitySettings.antiAliasing = dropdown.value * 2;
                break;
            case 3:
                QualitySettings.antiAliasing = 8;
                break;
        }
        PlayerPrefs.SetInt("AntiAliasing", QualitySettings.antiAliasing);
    }

    public void Mipmap(TMP_Dropdown dropdown)
    {
        QualitySettings.globalTextureMipmapLimit = dropdown.value;
        PlayerPrefs.SetInt("MipMap", QualitySettings.globalTextureMipmapLimit);
    }

    public void SetSettings()
    {
        _Aaliasing.value = QualitySettings.antiAliasing;
        _MipMap.value = QualitySettings.globalTextureMipmapLimit;
        _Quality.value = QualitySettings.GetQualityLevel();
        _Vsync.value = QualitySettings.vSyncCount;

        Music.volume = PlayerPrefs.GetFloat("Music");
        Alerts.volume = PlayerPrefs.GetFloat("Alerts");
        SFX.volume = PlayerPrefs.GetFloat("SFX");

        _music.value = Music.volume;
        _alerts.value = Alerts.volume;
        _sfx.value = SFX.volume;
    }

    public void VolumeMusic(Slider source)
    {
        Music.volume = source.value;
    }

    public void VolumeAlerts(Slider source)
    {
        if (Alerts != null)
            Alerts.volume = source.value;
    }

    public void VolumeFX(Slider source)
    {
        SFX.volume = source.value;
    }
}