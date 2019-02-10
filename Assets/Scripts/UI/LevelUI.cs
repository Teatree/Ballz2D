using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour {

    public int LevelNumber;
    public bool IsCompleted;
    public int StarsNumber;

    public Camera c;

    public Button buttonComponent;
    public GameObject Text;
    public GameObject Stars;
    public GameObject LockImage;

    [Header("UI")]
    public Text levelNumText;
    public Image Star1;
    public Image Star2;
    public Image Star3;

    [Header("Art")]
    public Sprite Star_Complete;
    public Sprite Star_InComplete;
    public Sprite Level_Complete;
    public Sprite Level_InComplete;

    private void Start() {
        SceneController.sceneController.UnloadGame();

        c = Camera.main;
        c.transform.position = transform.position;
       // Debug.Log("c " + c + "c.pos: " + c.transform.position);

        levelNumText.text = (LevelNumber + 1).ToString();
        switch (StarsNumber) {
            case 1: {
                    Star1.sprite = Star_Complete;
                    break;
                }
            case 2: {
                    Star1.sprite = Star_Complete;
                    Star2.sprite = Star_Complete;
                    break;
                }
            case 3: {
                    Star1.sprite = Star_Complete;
                    Star2.sprite = Star_Complete;
                    Star3.sprite = Star_Complete;
                    break;
                }
            default: {
                    break;
                }
        }
    }

    public void UpdateButtonVisuals(int starNum) {
        var img = GetComponent<Image>();
        if (buttonComponent.interactable == false) {
            img.sprite = Level_InComplete;
            LockImage.SetActive(true);
            Text.SetActive(false);
            Stars.SetActive(false);
        }
        else {
            //Debug.Log("level: " + LevelNumber + " starNum: " + starNum);
            if (starNum == 0) {
                img.sprite = Level_InComplete;
            }
            else {
                img.sprite = Level_Complete;
            }
            LockImage.SetActive(false);
            Text.SetActive(true);
            Stars.SetActive(true);
        }
    }
    
    public void StartGameAtLevel() {
        AllLevelsData.CurrentLevelIndex = LevelNumber;
        LevelController.ResetScore();
       // AdmobController.Instance.ShowIterstitial();
        SceneController.sceneController.LoadGame();
    }
}
