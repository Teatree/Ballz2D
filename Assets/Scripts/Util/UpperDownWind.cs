using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperDownWind : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        Debug.Log("wind says: " + other.gameObject.name);
    }
}
