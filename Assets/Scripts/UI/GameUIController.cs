using UnityEngine;

public class GameUIController : MonoBehaviour {

    void Start() {

    }

    void Update() {

    }

    public void Shop() {
        Debug.LogWarning("Shop!");
    }

    public void Pause() {
        if (GameController.IsGameStopped()) {
            GameController.ResumeGame();
        }
        else {
            Debug.LogWarning("Pause!");
            GameController.PauseGame();
        }
    }
}
