using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Transform LevelListParent;

    [SerializeField]
    private LevelUI levelUiElementPrefab;

    public SpriteState sprState = new SpriteState();

    void Start() {
        //Load data
        AllLevelsData.allLevels = DataController.LoadLevels();
        AllLevelsData.playerInfo = DataController.LoadPlayer();

        //Create level buttons
        for (int i = 0; i < AllLevelsData.allLevels.Count; i++) {
            var lvl = Instantiate(levelUiElementPrefab);
            lvl.LevelNumber = i;
            lvl.transform.parent = LevelListParent.transform;
            if (i <= AllLevelsData.playerInfo.starsPerLvl.Count) {
                lvl.buttonComponent.interactable = true;
                lvl.StarsNumber = (i < AllLevelsData.playerInfo.starsPerLvl.Count) ? AllLevelsData.playerInfo.starsPerLvl[i] : 0;
            }
        }
    }

    void Update() {

    }
}
