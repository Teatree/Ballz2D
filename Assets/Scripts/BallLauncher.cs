using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    private static float MIN_AIM_HEIGHT = 0.2f;

    //private Vector3 startDragPosition
    private Vector3 endDragPosition;
    private BlockSpawner blockSpawner;
    private LaunchPreview launchPreview;
    private List<Ball> balls = new List<Ball>();


    public int BallsReady;

    [SerializeField]
    private Ball ballPrefab;

    private void Awake()
    {
        blockSpawner = FindObjectOfType<BlockSpawner>();
        launchPreview = GetComponent<LaunchPreview>();
        CreateBall(13);
    }

    public void ReturnBall()
    {
        BallsReady++;
        if (BallsReady == balls.Count)
        {
            blockSpawner.SpawnRowOfBlocks();
            //CreateBall();
        }
    }

    private void CreateBall(int ballsAmount)
    {
        for (int i = 0; i < ballsAmount; i++) {
            var ball = Instantiate(ballPrefab);
            balls.Add(ball);
            BallsReady++;
        }
    }

    private void Update()
    {
        if (BallsReady != balls.Count) // don't let the player launch until all balls are back.
            return;

        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.back * -10;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //controls 
        if (ShouldAim(worldPosition)) {
            if (Input.GetMouseButtonDown(0)) {
                StartDrag(worldPosition);
            }
            else if (Input.GetMouseButton(0)) {
                ContinueDrag(worldPosition);
            }
            else if (Input.GetMouseButtonUp(0)) {
                EndDrag(worldPosition);
            }
        } else {
            EndDrag(worldPosition);
        }
    }

    private void EndDrag(Vector3 worldPos)
    {
        if (ShouldAim(worldPos)) {
            StartCoroutine(LaunchBalls());
        } else  {
            HideGhosts();
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }

    private IEnumerator LaunchBalls()
    {
        Vector3 direction = endDragPosition - transform.position;
        direction.Normalize();

        foreach (var ball in balls)
        {
            ball.transform.position = transform.position;
            ball.gameObject.SetActive(true);
            ball.SetDir(LaunchPreview.launchDirection);
            yield return new WaitForSeconds(0.03f);
        }
        BallsReady = 0;

        HideGhosts();
        gameObject.SetActive(false);
    }

    private void HideGhosts() {
        GetComponent<LineRenderer>().positionCount = 0;
        foreach (Transform ghost in transform) {
            ghost.position = new Vector3(-100, -100, 0);
            //Debug.Log("ghost pos = " + ghost.position)
        }
    }

    private void ContinueDrag(Vector3 worldPosition)
    {
        endDragPosition = worldPosition;
        if (ShouldAim(worldPosition)) {
            launchPreview.SetEndPoint(endDragPosition);
        }
    }

    private void StartDrag(Vector3 worldPosition)
    {
   //     startDragPosition = worldPosition;
        launchPreview.SetStartPoint(transform.position);
    }

    private bool ShouldAim (Vector3 worldPosition) {
        return worldPosition.y > (transform.position.y + MIN_AIM_HEIGHT);
    }
}
