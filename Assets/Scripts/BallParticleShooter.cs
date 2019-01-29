using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallParticleShooter : MonoBehaviour {

    public ParticleSystem ps;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    private ParticleSystem.Particle[] m_particles = new ParticleSystem.Particle[10]; // amount SE T THE AMOUTN
    public TEST t;

    // Use this for initialization
    void Start() {
        
    }
	
    void Update() {
        int numParticlesAlive = ps.GetParticles(m_particles);

        //Debug.Log("m_particles = " + m_particles);
        for (int i = 0; i < numParticlesAlive; i++) {
            //t.isCollidingNonCollidable(m_particles[i].position);

            foreach (Block b in GridController.blocksSpawned) {
                //Debug.Log("Particle Pos: " + m_particles[i].position);
                b.isCollidingNonCollidable(m_particles[i].position);
            }
        }

        ps.SetParticles(m_particles, numParticlesAlive);
    }

    void OnParticleTrigger() {
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        for (int i = 0; i < numEnter; i++) {
            ParticleSystem.Particle p = enter[i];
            //take damage
            enter[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        Debug.Log("OnTrigger CallBack From Enter!");
    }

    private void OnParticleCollision(GameObject other) {
        ProcessBlockCOllision(other.GetComponent<Collider2D>());
    }

    private void ProcessBlockCOllision(Collider2D collider) {
        if (collider.gameObject.GetComponent<Block>() != null) { //If collision is not ignored and collided with block 
            collider.gameObject.GetComponent<Block>().interactWithBall(); //Interact with a block 

            if (!collider.gameObject.GetComponent<Block>()._type.isCollidable) { // If the block is not collidable -> return
                return;
            }
        }
        //else {
        //    timer = 0;
        //}
    }
}
