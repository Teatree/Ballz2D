using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    private Vector3 startDragPosition;
    private Vector3 endDragPosition;
    private BlockSpawner blockSpawner;
    private LaunchPreview launchPreview;
    private List<Ball> balls = new List<Ball>();
    private int ballsReady;

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
        ballsReady++;
        if (ballsReady == balls.Count)
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
            ballsReady++;
        }
    }

    private void Update()
    {
        if (ballsReady != balls.Count) // don't let the player launch until all balls are back.
            return;

        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.back * -10;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //controls 
        if (Input.GetMouseButtonDown(0))
        {
            StartDrag(worldPosition);
        }
        else if (Input.GetMouseButton(0))
        {
            ContinueDrag(worldPosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    private void EndDrag()
    {
        StartCoroutine(LaunchBalls());
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
            //ball.GetComponent<Rigidbody2D>().AddForce(direction);

            yield return new WaitForSeconds(0.03f);
        }
        ballsReady = 0;
    }

    private void ContinueDrag(Vector3 worldPosition)
    {
        endDragPosition = worldPosition;

        //Vector3 direction = endDragPosition - startDragPosition;

        //launchPreview.SetEndPoint(transform.position - direction);
        launchPreview.SetEndPoint(endDragPosition);
    }

    private void StartDrag(Vector3 worldPosition)
    {
        startDragPosition = worldPosition;
        launchPreview.SetStartPoint(transform.position);
    }
}
