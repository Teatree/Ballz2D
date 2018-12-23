using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public GameObject go;

    // Use this for initialization
    void Start() {
        var g = Instantiate(go, this.transform);
        g.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

    }

    // Update is called once per frame
    void Update() {

    }
}
