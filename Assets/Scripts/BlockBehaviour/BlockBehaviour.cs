﻿using System.Collections;
using UnityEngine;

public class BlockBehaviour : IBehaviour {

    public override void setBlock(Block b) {
        this.block = b;
    }

    public override void OnDestroy() {
        int score = GameController.IncreseScore();
        block.CreateScoreFeedbacker(score);

        if (block.DeathParticle != null) block.CreateDeathParticle();
        GameUIController.Instance.UpdateScore(GameController.Score);

        block.DestroySelf();
    }

    public override void OnCollide(Ball ball) {
        block.Hit();
        block.UpdateVisualState();
    }

    public override void LooseOneLife() {
        block.hitsRemaining--;
        if (block.hitsRemaining > 0) {
            block.UpdateVisualState();
        }
        else {
            OnDestroy();
        }
    }

    public override void Update() {
        
    }
}
