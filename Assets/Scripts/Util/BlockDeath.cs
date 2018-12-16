using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDeath : MonoBehaviour {

    public ParticleSystem part;

    public Color DeathColor;

    void Start () {
        if (part != null) {
            ParticleSystem.MainModule partmain = part.main;
            partmain.startColor = DeathColor;
            //Debug.Log(DeathColor)

            part.Play();
        }

        Destroy(gameObject, 1);
        //Debug.Log("should");
	}
}
