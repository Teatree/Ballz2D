using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOver : IPopup<GameOver> {

    public GameOverType _type;
    public int StarsAmount;

    public GameObject Btn_Group_Single;
    public GameObject Btn_Group_Ads;
    public GameObject Btn_Group_Fail;

    public GameObject Star1;
    public GameObject Star2;
    public GameObject Star3;

    public Slider StarsSlider;

    public GameObject StatusText;
    public GameObject levelText;

    public void Start() {
        StarsSlider.maxValue = PlayerController.Instance.starBoxes[PlayerController.player.numStarBoxesOpened].starCost;

        Debug.Log("GridController.oldStarsNumForGameOver = " + GridController.oldStarsNumForGameOver + " LevelController.LevelStarsAmount = " + LevelController.LevelStarsAmount);

        //if (GridController.oldStarsNumForGameOver < LevelController.LevelStarsAmount) {
        //    StarsSlider.value = PlayerController.player.stars - Mathf.Abs(GridController.oldStarsNumForGameOver - LevelController.LevelStarsAmount);
        //    Debug.Log("PlayerController.player.stars - (GridController.oldStarsNumForGameOver - LevelController.LevelStarsAmount); " +
        //        (PlayerController.player.stars - (GridController.oldStarsNumForGameOver - LevelController.LevelStarsAmount)));
        //}
        //else {
            StarsSlider.value = PlayerController.player.progressTowardsNextStarBox;
        //}

        if (PlayerController.player.stars >= StarsSlider.maxValue) {
            //_type = GameOverType.WinBox;
            _type = GameOverType.Win;
        }

        InitThings();
    }

    private void InitThings() {
        switch (_type) {
            case GameOverType.Fail: {
                    InitFail();
                    break;
                }
            case GameOverType.Win: {
                    InitWin();
                    break;
                }

            case GameOverType.WinBox: {
                    InitWinBox();
                    break;
                }
            default: {
                    throw new System.ArgumentException("Init for Popup " + _type + "is not defined ");
                }
        }
    }

    private void InitFail() {
        StatusText.transform.GetComponent<Text>().text = "FAILED";
        StatusText.transform.GetComponent<Text>().color = new Color(0.89f, 0.47f, 0.2f, 1);
        levelText.transform.GetComponent<Text>().text = "Level : " + GameUIController.currentLevelNumberLable;

        Btn_Group_Single.SetActive(false);
        Btn_Group_Ads.SetActive(false);
        Btn_Group_Fail.SetActive(true);

        Star1.transform.GetComponent<Image>().color = Color.grey;
        Star2.transform.GetComponent<Image>().color = Color.grey;
        Star3.transform.GetComponent<Image>().color = Color.grey;
    }

    public void InitWin() {
        InitWinText();
        InitWinSatrs();

        Btn_Group_Ads.SetActive(false);
        Btn_Group_Fail.SetActive(false);
        Btn_Group_Single.SetActive(true);

        Window_Confetti.Instance.ReleaseConfetti();
        StartCoroutine(stopConfetti());
    }

    public void InitWinBox() {
        InitWinText();
        InitWinSatrs();

        Btn_Group_Fail.SetActive(false);
        Btn_Group_Single.SetActive(false);
        Btn_Group_Ads.SetActive(true);

        Debug.Log("I reached this!");
    }

    private void InitWinText() {
        StatusText.transform.GetComponent<Text>().text = "NICE!";
        StatusText.transform.GetComponent<Text>().color = new Color(0, 0.93f, 0, 1);
        levelText.transform.GetComponent<Text>().text = "Level : " + GameUIController.currentLevelNumberLable;
    }

    private void InitWinSatrs() {
        StartCoroutine(InitStars());
    }

    public IEnumerator InitStars() {
        switch (StarsAmount) {
            case 1: {
                    Star1.transform.GetComponent<Animator>().SetTrigger("TurnIntoGold");
                    if (GridController.oldStarsNumForGameOver < LevelController.LevelStarsAmount) {
                        StarsSlider.value += 1;
                        PlayerController.player.progressTowardsNextStarBox += 1;
                    }
                    yield return new WaitForSeconds(0.3f);
                    Star2.transform.GetComponent<Image>().color = Color.grey;
                    Star3.transform.GetComponent<Image>().color = Color.grey;
                    break;
                }
            case 2: {
                    Star1.transform.GetComponent<Animator>().SetTrigger("TurnIntoGold");
                    if (GridController.oldStarsNumForGameOver < LevelController.LevelStarsAmount) {
                        StarsSlider.value += 1;
                        PlayerController.player.progressTowardsNextStarBox += 1;
                    }
                    yield return new WaitForSeconds(0.3f);
                    Star2.transform.GetComponent<Animator>().SetTrigger("TurnIntoGold");
                    if (GridController.oldStarsNumForGameOver < LevelController.LevelStarsAmount) {
                        StarsSlider.value += 1;
                        PlayerController.player.progressTowardsNextStarBox += 1;
                    }
                    Star3.transform.GetComponent<Image>().color = Color.grey;
                    break;
                }
            case 3: {
                    Star1.transform.GetComponent<Animator>().SetTrigger("TurnIntoGold");
                    if (GridController.oldStarsNumForGameOver < LevelController.LevelStarsAmount) {
                        StarsSlider.value += 1;
                        PlayerController.player.progressTowardsNextStarBox += 1;
                    }
                    yield return new WaitForSeconds(0.3f);
                    Star2.transform.GetComponent<Animator>().SetTrigger("TurnIntoGold");
                    if (GridController.oldStarsNumForGameOver < LevelController.LevelStarsAmount) {
                        StarsSlider.value += 1;
                        PlayerController.player.progressTowardsNextStarBox += 1;
                    }
                    yield return new WaitForSeconds(0.3f);
                    Star3.transform.GetComponent<Animator>().SetTrigger("TurnIntoGold");
                    if (GridController.oldStarsNumForGameOver < LevelController.LevelStarsAmount) {
                        StarsSlider.value += 1;
                        PlayerController.player.progressTowardsNextStarBox += 1;
                    }
                    break;
                }
        }
        yield return null;
        Debug.Log("reached this");
        if (PlayerController.player.progressTowardsNextStarBox >= PlayerController.Instance.starBoxes[PlayerController.player.numStarBoxesOpened].starCost) {
            GameUIController.Instance.OpenBoxOpen();
        }
    }

    public enum GameOverType {
        Win, Fail, WinBox
    }

    public void OnClick_Replay() {
        ReloadGameScene();
    }

    private static void ReloadGameScene() {
        LevelController.ResetScore();
        SceneController.sceneController.UnloadGame();
        SceneController.sceneController.RestartGame();
    }

    public void OnClick_Next() {
        Window_Confetti.Instance.active = false;
        AllLevelsData.CurrentLevelIndex++;
        ReloadGameScene();
    }

    public override void OnClick_Close() {
        SceneController.sceneController.LoadMenu();
        Destroy(gameObject); // kill self
    }

    private IEnumerator stopConfetti() {
        yield return new WaitForSeconds(2);
        Window_Confetti.Instance.active = false;
    }
}
