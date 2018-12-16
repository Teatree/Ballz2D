using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDeath : MonoBehaviour {

    public ParticleSystem part;

    public Color DeathColor;

    void Start () {
        if (part != null) {
            part.startColor = Color.white;
            part.Play();
        }

        Destroy(gameObject, 1);
        //Debug.Log("should");
	}
}
