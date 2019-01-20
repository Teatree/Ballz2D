using UnityEngine;

public class BallReturn : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("BallReturn OnCollisionEnter2D ");
        collision.collider.gameObject.GetComponent<Ball>().OnFloorCollision(collision.collider);
    }
}
