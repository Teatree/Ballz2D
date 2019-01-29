using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour {

    CircleCollider2D circleCollider;

    void Start () {
        circleCollider = GetComponent<CircleCollider2D>();
    }
	
	void Update () {
		
	}

    public void isCollidingNonCollidable(Vector3 pos) {
        if (circleCollider.bounds.Contains(pos)) {
            Debug.Log("well done!");
        }
    }
}
