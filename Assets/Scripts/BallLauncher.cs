using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour {
    private static float MIN_AIM_HEIGHT = 0.2f;

    [SerializeField]
    public int InitBallAmount;

    private Vector3 endDragPosition;
    private BlockSpawner blockSpawner;
    private LaunchPreview launchPreview;
    public List<Ball> balls = new List<Ball>();


    public int BallsReadyToShoot;
    public int BallsShot;
    public static bool canShoot;

    [SerializeField]
    private Ball ballPrefab;

    private void Awake() {
        blockSpawner = FindObjectOfType<BlockSpawner>();
        launchPreview = GetComponent<LaunchPreview>();
        CreateBall(InitBallAmount);
    }

    private void Update() {
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
                HideGhosts();
                gameObject.SetActive(false);
                gameObject.SetActive(true);
                //SetStartDrag();
            }
        }
    }

    public void ReturnBall(Ball b) {

        Debug.Log(">>>> " + BallsReadyToShoot);
        if (BallsReadyToShoot == 0) {
            //gameObject.SetActive(true);
            transform.position = new Vector3(b.transform.position.x, 0, 00f);
        }
        BallsReadyToShoot++;
        if (BallsReadyToShoot == balls.Count) {
            blockSpawner.SpawnRowOfBlocks();
            GameController.ResetScoreCoefficient();
            gameObject.SetActive(true);
        }
        b.transform.position = new Vector2(-100, -100);
        b.moveSpeed = 0;
    }

    private void CreateBall(int ballsAmount) {
        for (int i = 0; i < ballsAmount; i++) {
            var ball = Instantiate(ballPrefab);
            ball.transform.position = new Vector3(transform.position.x, transform.position.y-190);
            ball.ballId = i;
            balls.Add(ball);
            BallsReadyToShoot++;
        }
    }

    private void EndDrag(Vector3 worldPos) {
        StartCoroutine(LaunchBalls());
    }

    private IEnumerator LaunchBalls() {
        Debug.Log("LaunchBalls");
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
                yield return new WaitForSeconds(0.03f);
            }
            HideGhosts();
            gameObject.SetActive(false);
        }
    }

    private void HideGhosts() {
        GetComponent<LineRenderer>().positionCount = 0;
        launchPreview.active = false;
        foreach (Transform ghost in transform) {
            ghost.position = new Vector3(-100, -100, 0);
            ghost.gameObject.SetActive(false);
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
        return worldPosition.y > (transform.position.y + MIN_AIM_HEIGHT);
    }
}
