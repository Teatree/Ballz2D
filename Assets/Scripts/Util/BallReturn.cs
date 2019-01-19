using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReturn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        collision.collider.gameObject.GetComponent<Ball>().OnFloorCollision(collision.collider);
    }
}
