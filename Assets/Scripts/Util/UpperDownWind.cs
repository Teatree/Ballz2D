using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperDownWind : MonoBehaviour {

    int counter = -800;

    public void BlowWind(Rigidbody2D rb) {
        StartCoroutine(Woooof(rb));
        
    }

    public IEnumerator Woooof(Rigidbody2D rb) {
        while (rb != null) {

            counter++;
            if (counter > 200) {
                rb.AddForce(new Vector2(rb.velocity.x+0.05f, (rb.velocity.y+0.01f) * 100));
                Debug.Log("Woooof vel: " + rb.velocity);
                counter = 0;
            }

            yield return null;
        }
    }
}
