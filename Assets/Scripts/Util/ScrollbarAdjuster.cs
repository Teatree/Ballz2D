using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarAdjuster : MonoBehaviour {

    public Scrollbar scrollbar;

	void Start () 
    {
        StartCoroutine(LateStart(0.2f));
    }

    IEnumerator LateStart(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        float targetVal = 0;
        int totalLvls = transform.childCount;
        for (int i = 0; i < AllLevelsData.allLevels.Count; i++) {
            int curLvl = AllLevelsData.CurrentLevelIndex;
            targetVal = (float)curLvl / (float)totalLvls;
            scrollbar.value = targetVal;
        }
        yield return null;
    }
}
