using UnityEngine;

public class BallReturn : MonoBehaviour
{
    private BallLauncher ballLauncher;

    private void Awake()
    {
        ballLauncher = FindObjectOfType<BallLauncher>();
    }


    private void Update() 
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Ball launcher where the first ball fell
        if (ballLauncher.BallsReadyToShoot == 0) {
            ballLauncher.gameObject.transform.position = new Vector3 (collision.collider.transform.position.x, 0,01f);
            ballLauncher.gameObject.SetActive(true);
        }
        ballLauncher.ReturnBall(collision.collider.GetComponent<Ball>());
        collision.collider.gameObject.SetActive(false);
    }

}