using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public GameObject TopObject;

    public Button LoadConfigBtn;
    public RectTransform ConfigPanel;
    public GameObject ConfigTemplate;

    public RectTransform CountdownPanel;
    public Transform CountdownTemplateRoot;
    public GameObject CountdownTemplate;

    public RectTransform TopPanel;

    public Button ControlBtn;

    private List<GameObject> loadedCountdownTemplates = new List<GameObject>();

    private void Awake()
    {
        Screen.SetResolution(200, 320, false);
        Application.targetFrameRate = -1;
        LoadConfigBtn.onClick.AddListener(LoadConfigBtnClicked);
        ControlBtn.onClick.AddListener(ControlBtnClicked);
    }

    private void OnDestroy()
    {
        LoadConfigBtn.onClick.RemoveListener(LoadConfigBtnClicked);
        ControlBtn.onClick.RemoveListener(ControlBtnClicked);
        loadedCountdownTemplates = null;
    }

    private void LoadConfigBtnClicked()
    {
        string filePath = OpenFile();
        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            StartCoroutine(LoadConfig(filePath));
        }
    }

    IEnumerator LoadConfig(string configFilePath)
    {
        string s = File.ReadAllText(configFilePath);
        Config[] configs = JsonConvert.DeserializeObject<Config[]>(s);
        foreach (var child in configs)
        {
            GameObject create = Instantiate(ConfigTemplate);
            create.transform.SetParent(ConfigPanel, false);
            create.GetComponent<ConfigTemplate>().SetData(child);
        }

        TopObject.SetActive(false);

        yield return new WaitForEndOfFrame();

        LayoutRebuilder.ForceRebuildLayoutImmediate(TopPanel);
    }

    private void ControlBtnClicked()
    {
        ConfigPanel.gameObject.SetActive(!ConfigPanel.gameObject.activeSelf);
    }

    private string OpenFile()
    {
        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "文本文件(*.txt;*.TD)\0*.txt;*.TD\0\0";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
        openFileName.title = "选择文件";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;


        if (LocalDialog.GetOpenFileName(openFileName))
        {

            FileInfo file = new FileInfo(openFileName.file);
            Global.ConfigDirectoryName = file.DirectoryName;
            #region
            //TimeConfig[] timeConfigs = new TimeConfig[] {
            //    new TimeConfig(){ Content="H1古蛇:仇恨开始 15s小群/20s大群",CountdownName=new string[]{ "古蛇小群","古蛇大群"},Countdown=new float[]{ 15f,20f} },
            //    new TimeConfig(){ Content="H2猴子:仇恨开始 45s转身大群",CountdownName=new string[]{ "猴子大群"},Countdown=new float[]{ 45f} },
            //    new TimeConfig(){ Content="H2十方:250W血开始 45s转身流血",CountdownName=new string[]{ "十方流血"},Countdown=new float[]{ 45f} },
            //};

            //string s = JsonConvert.SerializeObject(timeConfigs);
            //File.WriteAllText($"{file.DirectoryName}/H12.txt", s);
            #endregion

            return openFileName.file;
        };

        return null;
    }

    public void LoadTimeConfig(TimeConfig[] config)
    {
        for (int i = 0; i < loadedCountdownTemplates.Count; i++)
        {
            if (loadedCountdownTemplates[i] != null)
            {
                Destroy(loadedCountdownTemplates[i]);
            }
        }

        loadedCountdownTemplates.Clear();

        if (config != null && config.Length > 0)
        {
            foreach (var child in config)
            {
                GameObject create = Instantiate(CountdownTemplate);
                create.transform.SetParent(CountdownTemplateRoot, false);
                create.GetComponent<CountdownTemplate>().SetData(child);

                loadedCountdownTemplates.Add(create);
            }
        }
    }
}

public class Config
{
    public string ConfigName;
    public string ConfigPath;

    public Config()
    {

    }

    public Config(string name, string path)
    {
        ConfigName = name;
        ConfigPath = path;
    }
}

public class TimeConfig
{
    public string Content;
    public string[] CountdownName;
    public float[] Countdown;

    public TimeConfig()
    {

    }
}

public static class Global
{
    public static string ConfigDirectoryName;
}