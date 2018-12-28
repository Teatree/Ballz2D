
using UnityEngine;

public class Ball : MonoBehaviour {
    public Vector3 dir;

    public float moveSpeed;
    public int ballId;
    public bool ignoreCollision;
    public bool active;

    [SerializeField]
    public float moveSpeedNorm;
    [SerializeField]
    public float moveSpeedFast;

    private float timeToConsiderBeingStuck = 15f;
    private float timer = 0;
    private Vector3 baseOffsetDirection = Vector3.down * 0.1f;

    private void Start() {
        moveSpeed = moveSpeedNorm;
    }

    private void OnEnable() {
    }

    public void SetDir(Vector3 newDir) {
        dir = newDir;
        dir = dir.normalized;
    }

    private void FixedUpdate() {
        if (GameController.IsGameStopped()) {
            return;
        }
        transform.position += dir * Time.deltaTime * moveSpeed;

        timer += Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, moveSpeed * Time.deltaTime * 1.2f, ~(1 << 8));

        if (!hit) {
            return;
        }

        if ("BallReturn".Equals(hit.collider.gameObject.tag)) { //Interaction with the floor does not depend on  the ignoreCollision, and reflecrt calc are not needed. Ball returned back to launcher
            OnFloorCollision(hit.collider);
            return;
        }

        if (ignoreCollision) { //if ignoreCollision return, and skip reflect calc
            return;
        }
        else {
            if (hit.collider.gameObject.GetComponent<Block>() != null) { //If collision is not ignored and collided with block 
                hit.collider.gameObject.GetComponent<Block>().interactWithBall(this); //Interact with a block 

                if (!hit.collider.gameObject.GetComponent<Block>()._type.isCollidable) { // If the block is not collidable -> return
                    return;
                } 
            }
            else {
                timer = 0;
            }
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
        BallLauncher.Instance.ReturnBall(this);
        EnableCollision();
        active = false;
    }

    public void DisableCollision() {
        ignoreCollision = true;
        moveSpeed = moveSpeedFast;
    }

    public void EnableCollision() {
        ignoreCollision = false;
        moveSpeed = moveSpeedNorm;
    }


}