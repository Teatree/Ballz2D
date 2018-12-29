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

    public static string currentLevelNumberLable;

    void Start() {
        SceneController.sceneController.UnloadMenu();

        HandlePreview();
        GameController.PauseGame();
        currentLevelNumberLable =  "" + (1 + AllLevelsData.CurrentLevelIndex);
        UpdateStars();
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
        GameController.PauseGame();
        Instantiate(PausePrefab, transform);
    }

    #region stars
    public void UpdateScore(int newScore) {
        StopCoroutine("IncreaseScore");

        //Debug.Log("Increase The Score!");
        int curScore = int.Parse(ScoreText.text);
        int dif = newScore - curScore;
        StartCoroutine(IncreaseScore(curScore, newScore, dif));
    }

    public void UpdateBlocksAmount() {
        BlockText.text = "" + GridController.BlocksAmount;
    }

    public IEnumerator IncreaseScore(int curScore, int endScore, int dif) {
        //  Debug.Log("curScore = " + curScore + " endScore = " + endScore);
        while (curScore < endScore) {
            int increment = Mathf.RoundToInt((float)dif / 20) + 1;
            curScore += increment;
            Slider.value = curScore;
            ScoreText.text = curScore.ToString();
            yield return null;
            
        }
        curScore = endScore;
    }

    public void UpdateStars() {
        if (Slider.value >= 0) {
            Star1.color = Color.white;
            GameController.LevelStarsAmount = 1;
        }
        if (Slider.value >= Slider.maxValue * 0.6f) {
            Star2.color = Color.white;
            GameController.LevelStarsAmount = 2;
        }
        if (Slider.value >= Slider.maxValue) {
            Star3.color = Color.white;
            GameController.LevelStarsAmount = 3;
        }
    }
    #endregion

    public void HandleGameOver() {
        if (Revive.available) {
            Debug.LogWarning("Revive!");
            Instantiate(RevivePrefab, transform);
        } else {
            Instantiate(GameOverPrefab, transform);
        }
    }


    public void ShowGameOver() {
       GameObject go =  Instantiate(GameOverPrefab, transform);
        GameOver g = go.transform.GetComponent<GameOver>();
        g._type = GameOver.GameOverType.Fail;
    }


    public void HandleWin() {
        GameObject go = Instantiate(GameOverPrefab, transform);
        GameOver g = go.transform.GetComponent<GameOver>();
        g._type = GameOver.GameOverType.Fail;
        g.StarsAmount = GameController.LevelStarsAmount;
    }

    public void HandleRestart() {
        Debug.Log("AllLevelsData.CurrentLevelIndex " + AllLevelsData.CurrentLevelIndex);
        GameController.ResetScore();
        SceneController.sceneController.LoadGame();
    }

    public void HandlePreview() {
        //Show UI
        Instantiate(PreviewPrefab, transform);

    }
    public void LoadNextLevel() {
        AllLevelsData.CurrentLevelIndex++;
        GameController.ResetScore();
        SceneController.sceneController.LoadGame();
    }

    public void HandleHomeButton() {
        //Show UI
        SceneController.sceneController.LoadMenu();
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

            // Warning.Instance.ShowWarning();
            // ShowGameOver();


            Debug.Log(" > BallsReadyToShoot >  " + BallLauncher.Instance.BallsReadyToShoot);
            Debug.Log(" > balls.Count  >  " + BallLauncher.Instance.balls.Count);
            Debug.Log(" > canShoot > " + BallLauncher.canShoot);

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
        for (int i = 0; i < 50; i++) {
            Color c = laserLine.material.color;
            c.a = c.a - 0.02f;
            laserLine.material.color = c;
            yield return null;
        }
        GameController.isGameOver = false;
    }
}