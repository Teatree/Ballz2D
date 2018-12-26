using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameUIController : SceneSingleton<GameUIController> {

    [Header("Score")]
    public Text ScoreText;
    public Text BlockText; // text containing the number of blocks left for player to kill to win the level (most likely necessary for calculating req. score for stars) only cunts blocks that have lives
    public Slider Slider;
    public Image Star1;
    public Image Star2;
    public Image Star3;

    [Header("Popups")]
    public GameObject RevivePrefab;
    public GameObject GameOverPrefab;
    public GameObject PreviewPrefab;
    public GameObject PausePrefab;


    [Header ("Lasers")]
    public GameObject laserLine1;
    public GameObject laserLine2;
    public GameObject laserLine3;


    void Start() {
        HandlePreview();
        GameController.PauseGame();
    }

    void Update() {
        HandleInput();
        if (!GameController.IsGameStopped()) {

        }
        UpdateBlocksAmount();

    }

    public void ShowShop() {
        Debug.LogWarning("Shop!");
    }

    public void PauseGame() {
        //Show UI
        Instantiate(PausePrefab, transform);

        GameController.PauseGame();
    }

    #region stars
    public void UpdateScore(int newScore) {
        StopCoroutine("IncreaseScore");

        //Debug.Log("Increase The Score!");
        int curScore = int.Parse(ScoreText.text);
        StartCoroutine(IncreaseScore(curScore, newScore));
    }

    public void UpdateBlocksAmount() {
        BlockText.text = "" + GridController.BlocksAmount;
    }

    public IEnumerator IncreaseScore(int curScore, int endScore) {
        //  Debug.Log("curScore = " + curScore + " endScore = " + endScore);
        while (curScore < endScore) {
            curScore += 5;
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
    #endregion

    public void HandleGameOver() {
        //Show UI
        Instantiate(RevivePrefab, transform);
    }

    public void HandleWin() {
        //Show UI
        Debug.Log("Trying to add prefab");
        Instantiate(GameOverPrefab, transform);
        
    }

    public void HandleRestart() {
        Debug.Log("AllLevelsData.CurrentLevelIndex " + AllLevelsData.CurrentLevelIndex);
        GameController.ResetScore();
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void HandlePreview() {
        //Show UI
        Instantiate(PreviewPrefab, transform);

    }

    public void HandleHomeButton() {
        //Show UI
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    public void HandleWarning() {
        Warning.Instance.ShowWarning();
    }

    public void ShootLightning() {
        LightningPowerup.Instance.ShootLightning();
    }

    private void HandleInput() {
        if (Input.GetKeyDown("space")) {
            //LightningPowerup.Instance.ShootLightning();
            //GetMoreBalls();
            //HandlePreview();
            //GameController.PauseGame();

            Warning.Instance.ShowWarning();

        }

        //Android
        if (Application.platform == RuntimePlatform.Android) {
            if (Input.GetKey(KeyCode.Escape)) {
                PauseGame();
                return;
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer) {
            if (Input.GetKey(KeyCode.Escape)) {
                PauseGame();
                return;
            }
        }
        else {
            if (Input.GetKeyDown("escape")) {
                PauseGame();
            }
        }
    }

    #region slider 
    public void ContinueSliderDrag() {
        BallLauncher.Instance.SetSlider(true);
        BallLauncher.Instance.ContinueSliderDrag();
    }

    public void EndDrag() {
        BallLauncher.Instance.SetSlider(true);
        BallLauncher.Instance.EndDrag();
    }

    public void SetStartDrag() {
        BallLauncher.Instance.SetSlider(true);
        BallLauncher.Instance.SetStartDrag();
    }


    public void SetSlider(bool r) {
        BallLauncher.Instance.SetSlider(r);
        // Debug.Log("slider = " + _slider);
    }
    #endregion

    public void FadingLasers(LineRenderer laserLine) {
        StartCoroutine(BombLaserFade(laserLine));
    }

    private IEnumerator BombLaserFade(LineRenderer laserLine) {
        Debug.Log("Laser fade");
        for (int i = 0; i < 50; i++) {
            Color c = laserLine.material.color;
            c.a = c.a - 0.02f;
            Debug.Log("Laser fade");
            laserLine.material.color = c;
            yield return null;
        }
        GameController.isGameOver = false;
    }
}