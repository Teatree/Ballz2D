using UnityEngine;

public class Ball : MonoBehaviour {
    public Vector3 dir;
   
    public float moveSpeed;
    public int ballId;
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
    //RaycastHit2D hit;
    public bool ignoreCollision;
    public Rigidbody2D rb;

    private void Start() {
        moveSpeed = moveSpeedNorm;
    }

    public void AddForceBall(Vector3 newDir) {
        gameObject.SetActive(true);
        dir = newDir;
        //ballSprite.transform.LookAt(dir);
        dir = dir.normalized;
        rb.velocity = new Vector2();
        rb.AddForce(dir * 750);
        //Debug.Log("force: " + dir * 750);
    }

    private void Update() {
        if (LevelController.IsGameStopped()) {
            return;
        }
        var localVelocity = transform.InverseTransformDirection(rb.velocity);

        timer += Time.deltaTime;

        // Trying to make the egg look in the direction it's going
        var lookDir = new Vector2(transform.position.x, transform.position.y) + rb.velocity;
        var lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(lookAngle, Vector3.forward);
        //transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z-100, transform.rotation.w);
        //rb.velocity = rb.velocity.normalized * moveSpeed;

        //sorry,no
        if (transform.position.y < 0) {
            if (this.active) {
                EnableCollision();
                BallLauncher.Instance.ReturnBall(this);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        ProcessBlockCOllision(collision.collider);

        rb.velocity = ClampMagnitude(rb.velocity, 12.525f, 12.520f);
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        ProcessBlockCOllision(collider);
    }

    private void ProcessBlockCOllision(Collider2D collider) {
        if (collider.gameObject.GetComponent<UpperDownWind>() != null) {
            //collider.gameObject.GetComponent<UpperDownWind>().BlowWind(this.GetComponent<Rigidbody2D>());

            //rb.velocity = new Vector2();
        }

        if (collider.gameObject.GetComponent<Block>() != null) { //If collision is not ignored and collided with block 
            collider.gameObject.GetComponent<Block>().interactWithBall(this); //Interact with a block 

            if (!collider.gameObject.GetComponent<Block>()._type.isCollidable) { // If the block is not collidable -> return
                return;
            }
        }
        else {
            timer = 0;
        }
    }

    public void OnFloorCollision(Collider2D collider) {
        BallLauncher.Instance.ReturnBall(this);
        //EnableCollision();
        active = false;
        gameObject.SetActive(false);
    }

    public void DisableCollision() {
        ignoreCollision = true;
        gameObject.layer = 10;
        moveSpeed = moveSpeedFast;
    }

    public void EnableCollision() {
        ignoreCollision = false;
        gameObject.layer = 8;
        moveSpeed = moveSpeedNorm;
    }

    private void Rotating() {
        ballSprite.transform.Rotate(new Vector3(0, 0, 18));
    }

    private Vector3 ClampMagnitude(Vector3 v, float max, float min) {
        double sm = v.sqrMagnitude;
        if (sm > (double)max * (double)max) return v.normalized * max;
        else if (sm < (double)min * (double)min) return v.normalized * min;
        return v;
    }
}