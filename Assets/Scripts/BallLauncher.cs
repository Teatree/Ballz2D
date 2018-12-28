using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    [HideInInspector]
    public List<Ball> balls = new List<Ball>();
    [HideInInspector]
    public int BallsReadyToShoot;
    public static bool canShoot;
    private Vector3 newPos;
    public bool _slider;

    [SerializeField]
    private Ball ballPrefab;

    public Coroutine launcherBallRoutine;


    public static int ExtraBalls = 0;

    private void Awake() {
        base_y = transform.position.y;

        launchPreview = GetComponent<LaunchPreview>();
        CreateBall(InitBallAmount);
        textCanvas.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "x" + BallsReadyToShoot;
    }

    private void Update() {
        if (!GameController.IsGameStopped()) {
            if (BallsReadyToShoot == balls.Count && canShoot) { // don't let the player launch until all balls are back.
                LightningPowerup.Instance.EnableButton();
                MoreBallsPowerup.Instance.EnableButton();

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
                if (canShoot && !EventSystem.current.IsPointerOverGameObject()) {
                    if (Input.GetMouseButtonDown(0)) {
                        SetStartDrag();
                    }
                    else if (Input.GetMouseButton(0)) {
                        ContinueDrag(worldPosition);
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
    }

    public void CheckExtraBalls() {
        if (ExtraBalls > 0) {
            CreateBall(ExtraBalls);
            ExtraBalls = 0;
        }
    }

    public void ReturnBall(Ball b) {
        if (BallsReadyToShoot == 0) {
            UpdateVisualsFirstBall(b);
        }

        BallsReadyToShoot++;
        textCanvas.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "x" + BallsReadyToShoot;
        if (BallsReadyToShoot == balls.Count) {
            GridController.Instance.SpawnRowOfBlocks(false);
            GameController.ResetScoreCoefficient();

            UpdateVisualsLastBall();
            GridController.Instance.DidIwin();
        }
        b.transform.position = new Vector2(-100, -100);
        b.moveSpeed = 0;
    }

    private void UpdateVisualsFirstBall(Ball b) {
        newPos = new Vector3(b.transform.position.x, base_y, 00f);
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
    }

    public void CreateBall(int ballsAmount) {
        for (int i = 0; i < ballsAmount; i++) {
            var ball = Instantiate(ballPrefab, scalingParent);
            ball.transform.position = new Vector3(transform.position.x, transform.position.y - 190);
            ball.ballId = i;
            balls.Add(ball);
            BallsReadyToShoot++;
        }
    }

    public void EndDrag() {
        Slider.gameObject.transform.parent.gameObject.SetActive(false);
        textCanvas.SetActive(false);
        launcherBallRoutine = StartCoroutine(LaunchBalls());
        HideGhosts();
    }

    private IEnumerator LaunchBalls() {

        if (BallsReadyToShoot == balls.Count) {
            LightningPowerup.Instance.DisableButton();
            MoreBallsPowerup.Instance.DisableButton();

            canShoot = false;
            Vector3 direction = endDragPosition - transform.position;
            direction.Normalize();
            BallsReadyToShoot = 0;
            foreach (var ball in balls) {
                ball.transform.position = transform.position;
                ball.moveSpeed = ball.moveSpeedNorm;
                ball.gameObject.SetActive(true);
                ball.SetDir(LaunchPreview.launchDirection);
                ball.EnableCollision();
                ball.active = true;
                yield return new WaitForSeconds(0.03f);
            }

            HideGhosts();
            //gameObject.SetActive(false);           
        }
    }

    private void HideGhosts() {
        //GetComponent<LineRenderer>().positionCount = 0;

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

        if (ShouldAim(worldPosition)) {
            //Debug.Log(" endDragPosition " + endDragPosition);
            launchPreview.SetEndPoint(endDragPosition);
        }
        // else {
        //    //Reset launcher
        // Debug.Log("Input.ResetInputAxes()");
        //HideGhosts();
        //  }
    }

    public void ContinueSliderDrag() {
        endDragPosition = new Vector3(Slider.value, 1f - Mathf.Abs(Slider.value * 0.7f));

        if (ShouldAim(endDragPosition)) {
            //Debug.Log(" endDragPosition " + endDragPosition);
            launchPreview.SetEndPoint(endDragPosition);
        }
    }

    public void SetStartDrag() {
        launchPreview.SetStartPoint(transform.position);
        launchPreview.active = true;
        ShowGhosts();
    }

    private bool ShouldAim(Vector3 worldPosition) {
        return worldPosition.y > (transform.position.y * 0.9f);
    }

    public void SetSlider(bool r) {
        _slider = r;
        // Debug.Log("slider = " + _slider);
    }

    public void SummonAllBalls() {
        StopCoroutine(launcherBallRoutine);

        //gameObject.SetActive(true);
        foreach (Ball b in balls) {
            b.SetDir(transform.position - b.transform.position);
            b.DisableCollision();
        }
        int ballsAlreadyThere = 0;
        foreach (Ball b in balls) {
            if (!b.active) {
                ballsAlreadyThere++;
            }
        }
        BallsReadyToShoot = ballsAlreadyThere;
    }
}
