using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownChildTemplate : MonoBehaviour
{
    public Text Content;
    public Text CountdownText;
    public Button StartBtn;
    public Button HideBtn;

    public RectTransform Rebuild;

    private float maxTime = 0;
    private float curTime;
    private float preTime;

    private int frameCount = 0;
    private int showCount = 4;

    private bool running = false;

    private void Awake()
    {
        StartBtn.onClick.AddListener(StartCountdown);
        HideBtn.onClick.AddListener(HideBtnClicked);
    }

    private void OnDestroy()
    {
        StartBtn.onClick.RemoveListener(StartCountdown);
        HideBtn.onClick.RemoveListener(HideBtnClicked);
    }

    void Update()
    {
        if (running)
        {
            if (frameCount == showCount)
            {
                frameCount = 0;
                float rt = Time.realtimeSinceStartup;
                curTime = curTime - (rt - preTime);
                preTime = rt;
                ShowCountdownText(curTime);
                if (curTime <= 0)
                {
                    curTime = maxTime + curTime;
                }
            }
            else
            {
                frameCount++;
            }
        }
    }

    public void SetData(string content, float time)
    {
        Content.text = content;
        ShowCountdownText(time);
        maxTime = time;
    }

    public void StartCountdown()
    {
        curTime = maxTime;
        preTime = Time.realtimeSinceStartup;
        ShowCountdownText(curTime);
        running = true;
    }

    public void HideBtnClicked()
    {
        gameObject.SetActive(false);
        //LayoutRebuilder.ForceRebuildLayoutImmediate(Rebuild);
    }

    public void ShowCountdownText(float time)
    {
        if (time > 5)
        {
            CountdownText.color = Color.black;
        }
        else
        {
            CountdownText.color = Color.red;
        }

        CountdownText.text = time.ToString("#0.0");
    }

}
