using UnityEngine;

public class BallReturn : MonoBehaviour
{
    private BallLauncher ballLauncher;

    private void Awake()
    {
        ballLauncher = FindObjectOfType<BallLauncher>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ballLauncher.BallsReady == 0) {
            ballLauncher.gameObject.transform.position = new Vector2 (collision.collider.transform.position.x, 0);
            ballLauncher.gameObject.SetActive(true);
            Debug.Log("this does indeed happen");
        }

        ballLauncher.ReturnBall();
        collision.collider.gameObject.SetActive(false);
    }
}