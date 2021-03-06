﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BallLauncher : SceneSingleton<BallLauncher> {
    private static float base_y;

    public Transform scalingParent;
    [SerializeField]
    [Header("Ball Settings")]
    public int InitBallAmount;
    public float ballScale = 1;
    public Color ballColor = Color.white;
    public GameObject ballVisual;

    [Header("UI")]
    public GameObject textCanvas;
    public Slider Slider;

    private Vector3 endDragPosition;
    private LaunchPreview launchPreview;

    public static int ExtraBalls = 0;
    public static int ExtraAdBalls = 0;

    [HideInInspector]
    public List<Ball> balls = new List<Ball>();
    [HideInInspector]
    public int BallsReadyToShoot;
    public static bool canShoot;
    public int shotCount;
    private Vector3 newPos;
    public bool _slider;

    [SerializeField]
    private Ball ballPrefab;

    public Coroutine launcherBallRoutine;

    private float outTime = 0;
    private float outTimeLimit = 40;

    public GameObject upperDownWindGO;

    public static bool aimCanceled = false;

    private void Awake() {
        SpeedUP_remove();
        outTime = 0; 
        base_y = transform.position.y;
        launchPreview = GetComponent<LaunchPreview>();
        CreateBall(InitBallAmount);
        SetBallsUIText();
        canShoot = true;
    }

    private void Update() {
        if (!LevelController.IsGameStopped()) {
            //Debug.Log(" >>>>>> BallsReadyToShoot " + BallsReadyToShoot);
            //Debug.Log(" >>>>>> balls.Count " + balls.Count);
            if (BallsReadyToShoot == balls.Count && canShoot) { // don't let the player launch until all balls are back.
                //GameUIController.Instance.SetDebugText("Balls Are back!");
                LightningPowerup.Instance.EnableButton();
                MoreBallsPowerup.Instance.EnableButton();
                CancelPowerup.Instance.DisableButton();

                CheckExtraBalls();                                           //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.back * -10;
                Vector3 worldPosition;
                if (_slider) {
                    worldPosition = new Vector3(Slider.value, 1f);
                }
                else {
                    worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //Debug.Log("worldPosition: " + worldPosition);
                }

                //controls
                if (!_slider) {
                    if (canShoot && !IsPointerOverUIObject()) {
                        //GameUIController.Instance.SetDebugText(" I AM NOT OVER ANY OBJECT OR NOTHING !");

                        if (Input.GetMouseButtonDown(0)) {

                            if (PlayerController.player.completedLvls != null && PlayerController.player.completedLvls.Count == 0 && AllLevelsData.CurrentLevelIndex == 0) {
                                GameUIController.Instance.RemoveFTUE();
                            }

                            SetStartDrag(worldPosition);
                        }
                        else if (Input.GetMouseButton(0)) {

                            if (worldPosition.y > 0.65) {
                                ContinueDrag(worldPosition);
                            }
                            else {
                                ContinueDrag(new Vector3(worldPosition.x, 0.65f, worldPosition.z));
                            }
                            //Debug.Log("y - " + worldPosition.y);
                        }
                        else if (Input.GetMouseButtonUp(0)) {
                            EndDrag();
                        }
                    }
                    else {
                        HideGhosts();
                    }
                }
            }

            // Check whether balls were out for too long and speed up
            if (BallsReadyToShoot != balls.Count && outTime > outTimeLimit) {
                //Debug.Log(">>> BallsReadyToShoot > " + BallsReadyToShoot);
                SpeedUP(2);
                outTime += Time.deltaTime * 10;
                if (outTime > outTimeLimit * 2) {
                    SpeedUP(5);
                  //  Debug.Log(" --------- increaseing speed ");
                }
            }
            else if (BallsReadyToShoot != balls.Count && outTime <= outTimeLimit && Time.deltaTime != 2f) {
                //  Debug.Log("outTime = " + outTime);
                outTime += Time.deltaTime * 10;
            }
        }
    }

    public void CheckExtraBalls() {
        if (ExtraBalls > 0) {
            CreateBall(ExtraBalls);

            ExtraBalls = 0;
            SetBallsUIText();
        }
    }

    public void ReturnBall(Ball b) {
        if (BallsReadyToShoot == 0 || (b.ignoreCollision && newPos.y == 0)) {
            UpdateVisualsFirstBall(b);
        }

        BallsReadyToShoot++;

        SetBallsUIText();

        if (BallsReadyToShoot == balls.Count) {
            GridController.Instance.SpawnRowOfBlocks(false);
            LevelController.ResetScoreCoefficient();

            UpdateVisualsLastBall();
            GridController.Instance.DidIwin();

            // resetting the speedup
            SpeedUP_remove();
            outTime = 0;
            upperDownWindGO.active = false;
        }

        b.transform.position = new Vector2(-100, -100);
        b.moveSpeed = 0;
    }

    public void SetBallsUIText() {
        if (ExtraAdBalls > 0 && BallsReadyToShoot >= balls.Count - ExtraAdBalls) {
            if (BallsReadyToShoot < balls.Count) {
                textCanvas.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "x" + (balls.Count - ExtraAdBalls) + "+"+(balls.Count - BallsReadyToShoot);
            }
            else {
                textCanvas.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "x" + (BallsReadyToShoot - ExtraAdBalls) + "+" + ExtraAdBalls;
            }
        }
        else {
            textCanvas.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "x" + BallsReadyToShoot;
        }
    }

    private void UpdateVisualsFirstBall(Ball b) {
        newPos = b.ignoreCollision ? transform.position : new Vector3(b.transform.position.x, base_y, 0f);
        //Debug.Log("newPos: " + newPos);

        ballVisual.gameObject.SetActive(true);
        ballVisual.gameObject.transform.position = newPos;

        textCanvas.gameObject.SetActive(true);
        textCanvas.gameObject.transform.position = newPos + new Vector3(0.27f, 0.17f);
        if (textCanvas.gameObject.transform.position.x > 2.55f) textCanvas.gameObject.transform.position = new Vector3(2.55f, textCanvas.gameObject.transform.position.y);
        //transform.position = new Vector3(b.transform.position.x, 0, 00f);
    }

    public void UpdateVisualsLastBall() {
        gameObject.transform.position = newPos;
        ballVisual.gameObject.transform.localPosition = new Vector2(0, 0);
        textCanvas.gameObject.transform.localPosition = new Vector3(0.27f, 0.17f);
        if (textCanvas.gameObject.transform.position.x > 2.55f) textCanvas.gameObject.transform.position = new Vector3(2.55f, textCanvas.gameObject.transform.position.y);

        launchPreview.Init();
        Slider.value = 0;
        Slider.gameObject.transform.parent.gameObject.SetActive(true);
        newPos = new Vector3();
    }

    public void CreateBall(int ballsAmount) {
        for (int i = 0; i < ballsAmount; i++) {
            var ball = Instantiate(ballPrefab, scalingParent);

            UPdateBallSprite(ball);

            ball.transform.position = new Vector3(transform.position.x, transform.position.y - 190, 100);
            ball.ballId = i;
            balls.Add(ball);
            BallsReadyToShoot++;
        }
    }

    public void EndDrag() {
        if (!aimCanceled && launchPreview.active) {
            //GameUIController.Instance.SetDebugText("Shooting! ");
            Slider.gameObject.transform.parent.gameObject.SetActive(false);
            textCanvas.SetActive(false);
            launcherBallRoutine = StartCoroutine(LaunchBalls());
            HideGhosts();
            upperDownWindGO.active = true;
            shotCount++;
        }
    }

    public void shootBallPart() {
        //ballShooter.Emit(1);
    }

    private IEnumerator LaunchBalls() {
        GameUIController.Instance.BlockAdButtonFromTop();
        _slider = false;
        if (BallsReadyToShoot == balls.Count) {
            GridController.doNotMoveRowDown = false;

            LightningPowerup.Instance.DisableButton();
            MoreBallsPowerup.Instance.DisableButton();
            CancelPowerup.Instance.EnableButton();

            canShoot = false;
            Vector3 direction = endDragPosition - transform.position;
            direction.Normalize();
            Debug.Log(" >>> Really? ");
            BallsReadyToShoot = 0;
            foreach (var ball in balls) {
                ball.transform.position = transform.position;
                ball.moveSpeed = ball.moveSpeedNorm;
                ball.gameObject.SetActive(true);
                ball.AddForceBall(LaunchPreview.launchDirection);
                //shootBallPart();
                ball.EnableCollision();
                ball.active = true;

                float waitingTime = 2f * Time.fixedUnscaledDeltaTime;
                //Debug.Log(waitingTime);
                yield return new WaitForSeconds(waitingTime);
            }
            HideGhosts();
            _slider = false;
            //gameObject.SetActive(false);           
        }
    }

    public void HideGhosts() {
        launchPreview.active = false;
        foreach (Transform gos in transform) {
            if (gos.gameObject.name.Contains("Ghost")) {
                gos.position = new Vector3(-100, -100, 0);
                gos.gameObject.SetActive(false);
            }
        }
    }

    private void ShowGhosts() {
        foreach (Transform ghost in transform) {
            ghost.gameObject.SetActive(true);
        }
    }

    public void ContinueDrag(Vector3 worldPosition) {
        endDragPosition = worldPosition;

       // Debug.Log(" endDragPosition: " + endDragPosition);
        launchPreview.SetEndPoint(endDragPosition);
    }

    public void ContinueSliderDrag() {
        endDragPosition = new Vector3(transform.position.x + Slider.value, 1f - Mathf.Abs(Slider.value * 0.7f));
        endDragPosition = new Vector3(endDragPosition.x, endDragPosition.y, -10);

       // Debug.Log(" endDragPosition slider: " + endDragPosition);
        launchPreview.SetEndPoint(endDragPosition);
    }

    public void SetStartDrag(Vector3 worldPosition) {
        //Debug.Log("starting drag");
        launchPreview.SetStartPoint(transform.position);

        endDragPosition = worldPosition;

        // Debug.Log(" endDragPosition: " + endDragPosition);
        launchPreview.SetEndPoint(endDragPosition);

        launchPreview.active = true;
        ShowGhosts();
    }

    public void SetSlider(bool r) {
        _slider = r;
    }

    public void SummonAllBalls() {
        if (launcherBallRoutine != null)
        StopCoroutine(launcherBallRoutine);
        foreach (Ball b in balls) {
            if (b.active) {
                b.AddForceBall(ballVisual.transform.position - b.transform.position);
                b.DisableCollision();
            }
        }
        int ballsAlreadyThere = 0;
        foreach (Ball b in balls) {
            if (!b.active) {
                ballsAlreadyThere++;
            }
        }
        BallsReadyToShoot = ballsAlreadyThere;
    }

    public void SpeedUP(float s) {
        if (Time.timeScale == 1f && !LevelController.isGamePaused && !BallLauncher.canShoot) {
          //  Debug.Log("Speeding Up!");
            GameUIController.Instance.TurnSpeedUpIcon_ON();
            Time.timeScale = s;
        }
    }

    public void SpeedUP_remove() {
        if (Time.timeScale == 2f) {
        //    Debug.Log("Speeding Down!");
            GameUIController.Instance.TurnSpeedUpIcon_OFF();
            Time.timeScale = 1f;
        }
    }

    // Checks if it's over UI object, a workaround fro mobile
    private bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private static void UPdateBallSprite(Ball ball) {
        if (LevelController.SpecialBall != null) {
            Sprite s = Resources.Load<Sprite>("balls/" + LevelController.SpecialBall);
            ball.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = s;
        }
    }
}
