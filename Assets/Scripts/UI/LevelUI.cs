using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour {

    public int LevelNumber;
    public bool IsCompleted;
    public int StarsNumber;

    public Button buttonComponent;

    [Header("UI")]
    public Text levelNumText;
    public Text Star1;
    public Text Star2;
    public Text Star3;

    private void Start() {
        levelNumText.text = (LevelNumber + 1).ToString();
        switch (StarsNumber) {
            case 1: {
                    Star1.color = Color.yellow;
                    break;
                }
            case 2: {
                    Star1.color = Color.yellow;
                    Star2.color = Color.yellow;
                    break;
                }
            case 3: {
                    Star1.color = Color.yellow;
                    Star2.color = Color.yellow;
                    Star3.color = Color.yellow;
                    break;
                }
            default: {
                    break;
                }
        }
    }

    public void StartGameAtLevel() {
        AllLevelsData.CurrentLevelIndex = LevelNumber;
        GameController.ResetScore();
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}
