using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallLauncher : MonoBehaviour {
    private static float base_y;

    [SerializeField]
    public int InitBallAmount;

    private Vector3 endDragPosition;
    private BlockSpawner blockSpawner;
    private LaunchPreview launchPreview;
    public List<Ball> balls = new List<Ball>();
    public GameObject ballVisual;

    public int BallsReadyToShoot;
    public static bool canShoot;
    private Vector3 newPos;

    [SerializeField]
    private Ball ballPrefab;

    public GameObject textCanvas;

    public static int ExtraBalls = 0;

    private void Awake() {
        base_y = transform.position.y;

        blockSpawner = FindObjectOfType<BlockSpawner>();
        launchPreview = GetComponent<LaunchPreview>();
        CreateBall(InitBallAmount);
        textCanvas.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "x" + BallsReadyToShoot;
    }

    private void Update() {
        if (!GameController.isGameOver) {
            CheckExtraBall();

            if (BallsReadyToShoot == balls.Count && canShoot) { // don't let the player launch until all balls are back.
                                                                //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.back * -10;
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                //controls 

                if (ShouldAim(worldPosition) && canShoot) {
                    if (Input.GetMouseButtonDown(0)) {
                        SetStartDrag();
                    }
                    else if (Input.GetMouseButton(0)) {
                        ContinueDrag(worldPosition);
                    }
                    else if (Input.GetMouseButtonUp(0)) {
                        EndDrag(worldPosition);
                    }
                }
                else {
                    //Reset launcher
                    
                    Input.ResetInputAxes();
                    HideGhosts();

                    //gameObject.SetActive(false);
                    //gameObject.SetActive(true);
                    //SetStartDrag();
                }
            }
        }
    }

    private void CheckExtraBall() {
        if (ExtraBalls > 0) {
            CreateBall(ExtraBalls);
            ExtraBalls = 0;
        }
    }

    public void ReturnBall(Ball b) {
        if (BallsReadyToShoot == 0) { 
            newPos = new Vector3(b.transform.position.x, base_y, 00f);
            ballVisual.gameObject.SetActive(true);
            ballVisual.gameObject.transform.position = newPos;

            textCanvas.gameObject.SetActive(true);
            textCanvas.gameObject.transform.position = newPos + new Vector3(0.27f, 0.17f);
            if (textCanvas.gameObject.transform.position.x > 2.55f) textCanvas.gameObject.transform.position = new Vector3(2.55f, textCanvas.gameObject.transform.position.y);
            //transform.position = new Vector3(b.transform.position.x, 0, 00f);
        }
        BallsReadyToShoot++;
        textCanvas.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "x" + BallsReadyToShoot;
        if (BallsReadyToShoot == balls.Count) {
            blockSpawner.SpawnRowOfBlocks();
            GameController.ResetScoreCoefficient();

            gameObject.transform.position = newPos;
            ballVisual.gameObject.transform.localPosition = new Vector2(0,0);
            textCanvas.gameObject.transform.localPosition = new Vector3(0.27f, 0.17f);
            if (textCanvas.gameObject.transform.position.x > 2.55f) textCanvas.gameObject.transform.position = new Vector3(2.55f, textCanvas.gameObject.transform.position.y);

            launchPreview.Init();
        }
        b.transform.position = new Vector2(-100, -100);
        b.moveSpeed = 0;
    }

    public void CreateBall(int ballsAmount) {
        for (int i = 0; i < ballsAmount; i++) {
            var ball = Instantiate(ballPrefab);
            ball.transform.position = new Vector3(transform.position.x, transform.position.y - 190);
            ball.ballId = i;
            balls.Add(ball);
            BallsReadyToShoot++;
        }
    }

    private void EndDrag(Vector3 worldPos) {
        //balls.ForEach(b => b.inTheField = false);
        textCanvas.SetActive(false);
        StartCoroutine(LaunchBalls());
        HideGhosts();
    }

    private IEnumerator LaunchBalls() {
        if (BallsReadyToShoot == balls.Count) {
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
                yield return new WaitForSeconds(0.015f);
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

    private void ContinueDrag(Vector3 worldPosition) {
        endDragPosition = worldPosition;
        if (ShouldAim(worldPosition)) {
            //Debug.Log(" endDragPosition " + endDragPosition);
            launchPreview.SetEndPoint(endDragPosition);
        }
    }

    private void SetStartDrag() {
        launchPreview.SetStartPoint(transform.position);
        launchPreview.active = true;
        ShowGhosts();
    }

    private bool ShouldAim(Vector3 worldPosition) {
        return worldPosition.y > (transform.position.y*0.9f);
    }
}
