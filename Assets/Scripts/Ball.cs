using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 dir;
    private Rigidbody2D rb;

    private float moveSpeed;
    public int ballId;
    public bool active;

    [SerializeField]
    public float moveSpeedNorm;
    [SerializeField]
    public float moveSpeedFast;

    private void Start() {
       rb = GetComponent<Rigidbody2D>();
        moveSpeed = moveSpeedNorm;
    }

    public void SetDir(Vector3 newDir) {
        dir = newDir;
        dir = dir.normalized;
    }

    private void Update()
    {
        transform.position += dir * Time.deltaTime * moveSpeed;

        if (rb.isKinematic && transform.position.y < 0.01f) { //enable collision when ball is close to the ground
            EnableCollision();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        foreach(ContactPoint2D contact in collision.contacts) {
            dir = 2 * (Vector3.Dot(dir, Vector3.Normalize(contact.normal))) * Vector3.Normalize(contact.normal) - dir;
            dir *= -1;
        }
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