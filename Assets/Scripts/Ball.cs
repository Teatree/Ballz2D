using UnityEngine;

public class Ball : MonoBehaviour
{
    //private new Rigidbody2D rigidbody2D;
    private Vector3 dir;

    [SerializeField]
    private float moveSpeed;

    private void Start() {
        //dir = LaunchPreview.launchDirection;

        //rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void SetDir(Vector3 newDir) {
        dir = newDir;
        dir = dir.normalized;
        Debug.Log("dir = " + dir);
    }

    private void Update()
    {
        transform.position += dir * Time.deltaTime * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        //Debug.Log("collision detected");
        foreach(ContactPoint2D contact in collision.contacts) {
            dir = 2 * (Vector3.Dot(dir, Vector3.Normalize(contact.normal))) * Vector3.Normalize(contact.normal) - dir;
            dir *= -1;


        }
    }
}