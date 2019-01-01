using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour {

    public GameObject TabContent;
    public GameObject TabContentPanel;
    public GameObject TabPanel;
    public bool initialB;

    private Color color;

    private void Start() {
        color = GetComponent<Image>().color;
        if (initialB) {
            TabContent.SetActive(initialB);
            GetComponent<Image>().color = new Color(color.r, 255, color.b);
        }
    }

    public void SetContentActive() {
        foreach(Transform content in TabContentPanel.transform) {
            content.gameObject.SetActive(false);
        }
        foreach (Transform tab in TabPanel.transform) {
            tab.gameObject.GetComponent<Image>().color = color;
        }
        TabContent.SetActive(true);
        GetComponent<Image>().color = new Color(color.r, 255, color.b);
    }
}
