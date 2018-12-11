using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour {
    private Vector3 dir;
    //public Rigidbody2D rb;
    private bool isCollidable;

    private float moveSpeed;
    public int ballId;
    private BallLauncher ballLauncher;
    public bool ignoreCollision;

    [SerializeField]
    public float moveSpeedNorm;
    [SerializeField]
    public float moveSpeedFast;

    private float timeToConsiderBeingStuck = 15f;
    private float timer = 0;
    private Vector3 baseOffsetDirection = Vector3.down * 0.1f;



    private void Start() {
        ballLauncher = FindObjectOfType<BallLauncher>();
        moveSpeed = moveSpeedNorm;
    }

    private void OnEnable() {
        isCollidable = true;
    }

    public void SetDir(Vector3 newDir) {
        dir = newDir;
        dir = dir.normalized;
    }

    private void FixedUpdate() {
        transform.position += dir * Time.deltaTime * moveSpeed;

        timer += Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, moveSpeed * Time.deltaTime * 1.2f, ~(1 << 8));
        if (!hit)
            return;


        if (!ignoreCollision &&
            (hit.collider.gameObject.GetComponent<Block>() == null || hit.collider.gameObject.GetComponent<Block>()._type.isCollidable)
            && !"BallReturn".Equals(hit.collider.gameObject.tag)
            ) {
            timer = 0; //Do not collide
        }
        else {
            if ("BallReturn".Equals(hit.collider.gameObject.tag)) {
                OnFloorCollision(hit.collider);
            }
            if (hit.collider.gameObject.GetComponent<Block>() != null && !hit.collider.gameObject.GetComponent<Block>()._type.isCollidable) {
                hit.collider.gameObject.GetComponent<Block>().interactWithBall();
            }
            return;
        }

        if (hit.collider.gameObject.GetComponent<Block>() != null && hit.collider.gameObject.GetComponent<Block>()._type.isCollidable) {
          hit.collider.gameObject.GetComponent<Block>().interactWithBall();
        }

        //Reflect
        Vector3 offsetDirection = Vector3.zero;
        if (timer >= timeToConsiderBeingStuck) {
            timer = 0;
            offsetDirection = baseOffsetDirection;
        }

        dir = Vector3.Reflect(dir, hit.normal) + offsetDirection;
    }

    private void OnFloorCollision(Collider2D collider) {
      //  Debug.Log("OnFloorCollision");
        //  Ball launcher where the first ball fell
        if (ballLauncher.BallsReadyToShoot == 0) {
            GameController.ResetScoreCoefficient();
            ballLauncher.gameObject.transform.position = new Vector3(transform.position.x, 0, 01f);
            ballLauncher.gameObject.SetActive(true);
        }
        EnableCollision();
        ballLauncher.ReturnBall(this);
    }

    public void DisableCollision() {
        Debug.Log("Disable collision");
        ignoreCollision = true;
        moveSpeed = moveSpeedFast;
    }

    public void EnableCollision() {
        ignoreCollision = false;
        moveSpeed = moveSpeedNorm;
    }


}