using UnityEngine;

public class Ball : MonoBehaviour {
    public Vector3 dir;

   
    public float moveSpeed;
    public int ballId;
    public bool ignoreCollision;
    public bool active;

    public string personalLog;

    [SerializeField]
    public float moveSpeedNorm;
    [SerializeField]
    public float moveSpeedFast;

    public GameObject ballSprite;

    private float timeToConsiderBeingStuck = 0.5f;
    private float timer = 0;
    private Vector3 baseOffsetDirection = Vector3.down * 0.1f;

    // I think declaring some of these as fields helps with performance
    RaycastHit2D hit;

    private void Start() {
        moveSpeed = moveSpeedNorm;
    }

    public void SetDir(Vector3 newDir) {
        gameObject.SetActive(true);
        dir = newDir;
        dir = dir.normalized;
    }

    private void Update() {
        if (LevelController.IsGameStopped()) {
            return;
        }
        Rotating();
        transform.position += dir * Time.deltaTime * moveSpeed;

        timer += Time.deltaTime;

        hit = Physics2D.Raycast(transform.position, dir, moveSpeed * Time.deltaTime * 1.2f, ~(1 << 8));

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
        Vector2 offsetDirection = Vector2.zero;
        if (timer >= timeToConsiderBeingStuck) {
            timer = 0;
            offsetDirection = baseOffsetDirection;
        }

        personalLog += "\n dir = " + dir;
        Vector2 v = Vector2.Reflect(dir, hit.normal) + offsetDirection;
        dir = new Vector3(v.x, v.y, 0);
        personalLog += "\n reflecting from " + hit.collider.name + " hit.point = " + hit.point + " hit.normal = " + hit.normal;
    }

    private void OnFloorCollision(Collider2D collider) {
        BallLauncher.Instance.ReturnBall(this);
        EnableCollision();
        active = false;
        gameObject.SetActive(false);
    }

    public void DisableCollision() {
        ignoreCollision = true;
        moveSpeed = moveSpeedFast;
    }

    public void EnableCollision() {
        ignoreCollision = false;
        moveSpeed = moveSpeedNorm;
    }

    private void Rotating() {
        ballSprite.transform.Rotate(new Vector3(0, 0, 18));
    }
    //public void AddForceBall(Vector2 dir) {
    //    GetComponent<Rigidbody2D>().AddForce(dir * moveSpeed * 20);
    //}
}