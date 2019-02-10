﻿using System.Collections;
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
    public Sprite Star_Complete;
    public Sprite Star_InComplete;

    [Header("Popups")]
    public GameObject RevivePrefab;
    public GameObject GameOverPrefab;
    //public GameObject PreviewPrefab;
    public GameObject PausePrefab;
    public GameObject BoxPopupPrefab;
    public GameObject ShopPrefab;
 

    [Header("SpeedUPIcon")]
    public GameObject SpeedUpIcon;

    [Header ("Lasers")]
    public GameObject laserLine1;
    public GameObject laserLine2;
    public GameObject laserLine3;

    public GameObject Ad_Button;

    public static string currentLevelNumberLable;

    void Start() {
        SceneController.sceneController.UnloadMenu();
        HandlePreview();
        Ad_Button.SetActive(true);
        //LevelController.PauseGame();
        currentLevelNumberLable =  "" + (1 + AllLevelsData.CurrentLevelIndex);
        UpdateStars();

        Slider.maxValue = LevelController.ThirdStarScore;
    }

    void Update() {
        HandleInput();
        if (!LevelController.IsGameStopped()) {

        }
        UpdateBlocksAmount();
    }

    public void ShowShop() {
        LevelController.PauseGame();
        Instantiate(ShopPrefab, transform);
    }

    public void PauseGame() {
        LevelController.PauseGame();
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
        ScoreText.text = "" + endScore;
    }

    public void UpdateStars() {
        if (Slider.value >= 0) {
            Star1.sprite = Star_Complete;
            LevelController.LevelStarsAmount = 1;
        }
        if (Slider.value >= Slider.maxValue * 0.6f) {
            Star2.sprite = Star_Complete;
            LevelController.LevelStarsAmount = 2;
        }
        if (Slider.value >= Slider.maxValue) {
            Star3.sprite = Star_Complete;
            LevelController.LevelStarsAmount = 3;
        }
    }
    #endregion

    public void HandleGameOver() {
        if (Revive.available) {
            Instantiate(RevivePrefab, transform);
        } else {
            Instantiate(GameOverPrefab, transform);
        }
    }


    public void HideAdButtonFromTop() {
        if (Ad_Button.activeSelf) {
            Ad_Button.SetActive(false);
        }
    }

    public void ShowGameOver() {
       GameObject go =  Instantiate(GameOverPrefab, transform);
        GameOver g = go.transform.GetComponent<GameOver>();
        g._type = GameOver.GameOverType.Fail;
    }


    public void HandleWin() {
        LevelController.SubmitStars();
        GameObject go = Instantiate(GameOverPrefab, transform);
        GameOver g = go.transform.GetComponent<GameOver>();
        g._type = GameOver.GameOverType.Win;
        g.StarsAmount = LevelController.LevelStarsAmount;
        PlayerController.Instance.AddNewCompletedLevel(AllLevelsData.CurrentLevelIndex, LevelController.LevelStarsAmount);
    }

    public void HandlePreview() {
        //Instantiate(PreviewPrefab, transform);
    }

    public void HandleHomeButton() {
        SceneController.sceneController.LoadMenu();
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

    public void OpenBoxOpen() {
        Debug.Log("Opening Box");
        var Box = Instantiate(BoxPopupPrefab, transform);
        Box.GetComponent<BoxPopup>().itemToReceive = BoxOpener.Instance.GetBoxContents_BoxStars(PlayerController.player.numStarBoxesOpened);

        if (PlayerController.player.numStarBoxesOpened < PlayerController.Instance.starBoxes.Count) {
            PlayerController.player.numStarBoxesOpened += 1;
            PlayerController.player.progressTowardsNextStarBox = 0;
        }
    }

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
        LevelController.isGameOver = false;
    }

    public void TurnSpeedUpIcon_ON() {
        SpeedUpIcon.SetActive(true);
    }

    public void TurnSpeedUpIcon_OFF() {
        SpeedUpIcon.SetActive(false);
    }

}