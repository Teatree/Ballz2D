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
    }

    private void Update()
    {
        transform.Translate(dir * Time.deltaTime * moveSpeed);

        Ray2D ray = new Ray2D(transform.position, dir);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, .1f);

       // Debug.DrawRay(ray.origin, ray.direction, Color.red, 100);
        if (hit.collider != null ) {
            Debug.Log("colliding with " + hit.transform.gameObject.name);

            //var refLect = Vector2.Reflect(ray.origin, hit.normal);
            var refLect = Vector2.Reflect(transform.position, hit.normal);
            Debug.Log(">>> hit point: " + hit.point);
            Debug.Log(">>> position: " + transform.position);
            Debug.Log(">>> ray origin: " + ray.origin);
            Debug.Log(">>> reflect: " + refLect);

            RaycastHit2D ricochetHit = Physics2D.Raycast(transform.position, refLect + hit.normal);
            Debug.DrawRay(transform.position, refLect, Color.yellow, 100);

            dir = refLect;
        }

        //rigidbody2D.velocity = rigidbody2D.velocity.normalized * moveSpeed;
    }
}