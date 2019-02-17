using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour {

    public GameObject TabContent;
    public GameObject TabContentPanel;
    public GameObject TabPanel;
    public bool initialB;

    private Color activeCol;
    private Color disableCol;

    private void Start() {
        ColorUtility.TryParseHtmlString("#4E1C3A", out activeCol);
        ColorUtility.TryParseHtmlString("#180611", out disableCol);

        if (initialB) {
            TabContent.SetActive(initialB);
            GetComponent<Image>().color = activeCol;
        }
    }

    public void SetContentActive() {
        foreach(Transform content in TabContentPanel.transform) {
            content.gameObject.SetActive(false);
        }
        foreach (Transform tab in TabPanel.transform) {
            tab.gameObject.GetComponent<Image>().color = disableCol;
        }
        TabContent.SetActive(true);
        GetComponent<Image>().color = activeCol;
    }
}
