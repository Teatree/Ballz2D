﻿using System.Collections;
using UnityEngine;

public class LaserHorisontalBehaviour : IBehaviour {

    private LineRenderer laserLine;

    public override void setBlock(Block b) {

        this.block = b;
        laserLine = block.gameObject.GetComponent<LineRenderer>();
    }

    public override void OnDestroy() {
        block.DestroySelf();
    }

    public override void OnCollide(Ball ball) {
        if (!activated) {
            ShootLasers();
            foreach (Block b in GridController.blocksSpawned) {
                if (!b.destroyed && b.row == block.row) {
                    b.Hit();
                }
            }
            block.wasHit = true;
            activated = true;
        }
    }

    // shoot them pretty lasers
    public void ShootLasers() {
        Color c = laserLine.material.color;
        c.a = 1f;
        laserLine.material.color = c;

        laserLine.positionCount = 2;
        laserLine.SetPosition(0, new Vector2(3.15f, block.transform.position.y));
        laserLine.SetPosition(1, new Vector2(-3.15f, block.transform.position.y));

        block.StartCoroutine(LaserFade(laserLine));
    }

    public override void LooseOneLife() {
        //Do something 
    }

    public override void Update() { }
}