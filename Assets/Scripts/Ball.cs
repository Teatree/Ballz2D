using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour {
    private Vector3 dir;
    public Rigidbody2D rb;
    private bool isCollidable;

    private float moveSpeed;
    public int ballId;
    public bool active;

    [SerializeField]
    public float moveSpeedNorm;
    [SerializeField]
    public float moveSpeedFast;

    private float timeToConsiderBeingStuck = 15f;
    private float timer = 0;
    private Vector3 baseOffsetDirection = Vector3.down * 0.1f;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
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

        if (rb.isKinematic && transform.position.y < 0.01f) { //enable collision when ball is close to the ground
            EnableCollision();
        }

        timer += Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, moveSpeed * Time.deltaTime * 1.2f, ~(1 << 8));
        if (!hit)
            return;

        // Checking for what kind of collision is it??
        if ((hit.collider.gameObject.GetComponent<Block>() == null || hit.collider.gameObject.GetComponent<Block>()._type.Family != "Laser") && hit.collider.gameObject.GetComponent<BallReturn>() == null) {
            timer = 0;
        }
        else {
            return;
        }
        // Add Floor

        Vector3 offsetDirection = Vector3.zero;
        if (timer >= timeToConsiderBeingStuck) {
            timer = 0;
            offsetDirection = baseOffsetDirection;
        }

        dir = Vector3.Reflect(dir, hit.normal) + offsetDirection;

    }

    //private void OnCollisionEnter2D(Collision2D collision) {
    //    if (isCollidable) {
    //        foreach (ContactPoint2D contact in collision.contacts) {
    //            if (contact.collider.gameObject.GetComponent<Block>() == null || contact.collider.gameObject.GetComponent<Block>()._type.Family != "Laser") {
    //                dir = 2 * (Vector3.Dot(dir, Vector3.Normalize(contact.normal))) * Vector3.Normalize(contact.normal) - dir;
    //                dir *= -1;

    //                isCollidable = false;
    //                StartCoroutine(Countdown(0.002f));
    //            }
    //        }
    //    }
    //}

    private IEnumerator Countdown(float dur) {
        float normalizedTime = 0;

        while (normalizedTime <= 1f) {
            normalizedTime += Time.deltaTime / dur;
            yield return null;
        }

        Debug.Log("done with coroutine!");
        isCollidable = true;
    }

    public void DisableCollision() {
        rb.isKinematic = true;
        moveSpeed = moveSpeedFast;
    }

    public void EnableCollision() {
        rb.isKinematic = false;
        moveSpeed = moveSpeedNorm;
    }
}