using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour {

    public int LevelNumber;
    public bool IsCompleted;
    public int StarsNumber;

    public Camera c;

    public Button buttonComponent;

    [Header("UI")]
    public Text levelNumText;
    public Image Star1;
    public Image Star2;
    public Image Star3;

    private void Start() {
        SceneController.sceneController.UnloadGame();

        c = Camera.main;
        c.transform.position = transform.position;
       // Debug.Log("c " + c + "c.pos: " + c.transform.position);

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
        LevelController.ResetScore();
        //SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        SceneController.sceneController.LoadGame();
    }
}
