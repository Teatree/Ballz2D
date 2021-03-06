﻿using UnityEngine;
using System.Collections;

public class LaserCrossBehaviour : IBehaviour {

    private LineRenderer laserLine;

    public override void setBlock(Block b) {

        this.block = b;
        laserLine = block.gameObject.GetComponent<LineRenderer>();
    }

    public override void OnDestroy() {
        block.DestroySelf();
    }

    public override void OnCollide(Ball ball) {
        UpdateSavedBlocks();
        ShootLasers();
        if (!activated) {
            foreach (Block b in GridController.blocksSpawned) {
                if (!b.destroyed && (b.row == block.row || b.col == block.col)) {
                    b.Hit();
                }
            }
            block.wasHit = true;
        }
        activated = false;
    }

    // shoot them pretty lasers
    public void ShootLasers() {
        //if (fadeRoutine != null) {
        //    StopFadeRoutine();
        //}
       // Debug.Log("pew pew ");
        
        Color c = laserLine.material.color;
        c.a = 1f;
        laserLine.material.color = c;

        RaycastHit2D hitLeft = Physics2D.Raycast(block.transform.position, Vector2.left, 50f, block.WallsMask);
        RaycastHit2D hitRight = Physics2D.Raycast(block.transform.position, Vector2.right, 50f, block.WallsMask);
        RaycastHit2D hitUp = Physics2D.Raycast(block.transform.position, Vector2.up, 50f, block.WallsMask);
        RaycastHit2D hitDown = Physics2D.Raycast(block.transform.position, Vector2.down, 50f, block.WallsMask);

        laserLine.positionCount = 5;
        laserLine.SetPosition(0, new Vector2(hitRight.point.x, block.transform.position.y));
        laserLine.SetPosition(1, new Vector2(hitLeft.point.x, block.transform.position.y));
        laserLine.SetPosition(2, new Vector3(block.transform.position.x, block.transform.position.y, -100));

        laserLine.SetPosition(3, new Vector2(block.transform.position.x, hitDown.point.y));
        laserLine.SetPosition(4, new Vector2(block.transform.position.x, hitUp.point.y));

        fadeRoutine = block.StartCoroutine(LaserFade(laserLine));
    }

    public override void LooseOneLife() {
        //Do something 
    }

    public override void Update() { }
}
