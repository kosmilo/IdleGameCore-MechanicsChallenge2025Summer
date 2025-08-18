using System;
using UnityEngine;
using UnityEngine.UI;

public class Tabs : MonoBehaviour
{
    [Serializable]
    public struct Tab
    {
        public Button TabButton;
        public GameObject TabPanel;
    }
    [SerializeField] private Tab[] _tabs;

    private void Awake()
    {
        foreach (Tab tab in _tabs)
        {
            tab.TabButton.onClick.AddListener(() => { OpenTab(tab); });
            tab.TabPanel.SetActive(false);
        }
        _tabs[0].TabPanel.SetActive(true);
    }

    private void OpenTab(Tab openTab)
    {
        foreach (Tab tab in _tabs)
        {
            tab.TabPanel.SetActive(false);
        }
        openTab.TabPanel.SetActive(true);
    }
}
