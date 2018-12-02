using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public Transform LevelListParent;

    [SerializeField]
    private LevelUI levelUiElementPrefab;

    void Start () {
        AllLevelsData.allLevels = DataController.LoadLevels();
        PlayerInfo pi = DataController.LoadPlayer();
        pi.starsPerLvl[0] = 3;
        DataController.SavePlayer(pi);

        for (int i = 0; i< AllLevelsData.allLevels.Count; i++) {
            var lvl = Instantiate(levelUiElementPrefab);
            lvl.LevelNumber = i;
            lvl.transform.parent = LevelListParent.transform;
        }
	}
	
	void Update () {
		
	}
}
