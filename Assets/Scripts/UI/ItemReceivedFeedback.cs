﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemReceivedFeedback : MonoBehaviour {

    public Text amount;
    public Image icon;
    public bool active;

    public void setAmount(int amount) {
        this.amount.text = "x " + amount;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable() {
        active = true;
    }
    public void OnClick() {
        if (active) {
            MoreBallsPowerup.Instance.UpdateVisual();
            StartCoroutine(waitBeforeShoot());
        }
    }

    protected IEnumerator waitBeforeShoot() {
        for (int i = 0; i < 10; i++) {
            BallLauncher.Instance.BallsReadyToShoot = BallLauncher.Instance.balls.Count;
            BallLauncher.canShoot = true;
            Debug.Log(">>>>>  BallLauncher.Instance.BallsReadyToShoot " + BallLauncher.Instance.BallsReadyToShoot);
            Debug.Log(">>>>>  BallLauncher.Instance.balls.Count " + BallLauncher.Instance.balls.Count);

            active = false;
            
            yield return null;
        }
        transform.gameObject.SetActive(false);
    }
}
