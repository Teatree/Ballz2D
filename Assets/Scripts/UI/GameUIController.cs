using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : SceneSingleton<GameUIController> {

    [Header("Score")]
    public Text ScoreText;
    public Text BlockText; // text containing the number of blocks left for player to kill to win the level (most likely necessary for calculating req. score for stars) only cunts blocks that have lives
    public Slider Slider;
    public Image Star1;
    public Image Star2;
    public Image Star3;

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

    public void UpdateScore(int newScore) {
        StopCoroutine("IncreaseScore");
        Debug.Log("Increase The Score!");
        int curScore = int.Parse(ScoreText.text);
        StartCoroutine(IncreaseScore(curScore, curScore + newScore));
    }

    public IEnumerator IncreaseScore(int curScore, int endScore) {
        Debug.Log("curScore = " + curScore + " endScore = " + endScore);
        while (curScore < endScore) {
            curScore += 2;
            Slider.value = curScore;
            ScoreText.text = curScore.ToString();
            yield return null;
        }
    }

    public void UpdateStars() {
        if (Slider.value >= 0) {
            Star1.color = Color.white;
        }
        if (Slider.value >= Slider.maxValue * 0.6f) {
            Star2.color = Color.white;
        }
        if (Slider.value >= Slider.maxValue) {
            Star3.color = Color.white;
        }
    }
}
