using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ConfigTemplate : MonoBehaviour
{
    private string configName;
    private string configPath;

    public Text NameText;
    public Button LoadBtn;
    public MainUI Main;

    private TimeConfig[] timeConfigs;

    public void SetData(Config config)
    {
        configName = config.ConfigName;
        configPath = config.ConfigPath;
        NameText.text = configName;
    }

    private void Awake()
    {
        LoadBtn.onClick.AddListener(LoadBtnClicked);
    }

    private void OnDestroy()
    {
        LoadBtn.onClick.RemoveListener(LoadBtnClicked);
    }

    private void LoadBtnClicked()
    {
        if (timeConfigs == null)
        {
            string path = $"{Global.ConfigDirectoryName}/Config/{configPath}.txt";
            if (!File.Exists(path))
            {
                return;
            }
            string s = File.ReadAllText(path);
            timeConfigs = JsonConvert.DeserializeObject<TimeConfig[]>(s);
        }

        Main.LoadTimeConfig(timeConfigs);
    }
}
