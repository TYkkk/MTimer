using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTemplate : MonoBehaviour
{
    public Text Content;
    public Button AllStartBtn;
    public GameObject CountdownChildTemplate;

    private List<CountdownChildTemplate> loadedList = new List<CountdownChildTemplate>();

    private void Awake()
    {
        AllStartBtn.onClick.AddListener(AllStartBtnClicked);
    }

    private void OnDestroy()
    {
        AllStartBtn.onClick.RemoveListener(AllStartBtnClicked);
        loadedList = null;
    }

    private void AllStartBtnClicked()
    {
        foreach (var child in loadedList)
        {
            child.StartCountdown();
        }
    }

    public void SetData(TimeConfig config)
    {
        Content.text = config.Content;

        AllStartBtn.gameObject.SetActive(config.CountdownName.Length > 1);

        for (int i = 0; i < config.CountdownName.Length; i++)
        {
            GameObject create = Instantiate(CountdownChildTemplate);
            create.transform.SetParent(transform, false);
            var template = create.GetComponent<CountdownChildTemplate>();
            template.SetData(config.CountdownName[i], config.Countdown[i]);
            loadedList.Add(template);
        }
    }
}
