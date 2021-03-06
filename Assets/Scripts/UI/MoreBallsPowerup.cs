﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoreBallsPowerup : SceneSingleton<MoreBallsPowerup> {

    public GameObject Ani;
    public GameObject Button;
    public GameObject HC_cost;
    public Text AmountText;
    public int CostGems;
    public int minExtraBalls = 1;
    public int maxExtraBalls = 10;

    public int ExtraBallsAmount;

    public void Start() {
        UpdateVisual();
    }

    public void GetMoreBalls(int ExtraBallsAmount) {
        BallLauncher.ExtraBalls += ExtraBallsAmount;
        BallsAmountTextCanvasUpdate(BallLauncher.Instance.BallsReadyToShoot, ExtraBallsAmount);
    }

    public int getRandomBallsAmount() {
        int res = Random.Range(minExtraBalls, maxExtraBalls);
        return res;
    }

    public void BallsAmountTextCanvasUpdate(int ballsAmount, int ExtraBallsAmount) {
        BallLauncher.Instance.CheckExtraBalls();
        GameObject textCanvas = BallLauncher.Instance.textCanvas;
        textCanvas.gameObject.transform.localPosition = new Vector3(0.27f, 0.17f);
        if (textCanvas.gameObject.transform.position.x > 2.55f) textCanvas.gameObject.transform.position = new Vector3(2.55f, textCanvas.gameObject.transform.position.y);
        
        StartCoroutine(IncreaseBalls(textCanvas.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>(), ballsAmount, ballsAmount + ExtraBallsAmount));
    }

    public IEnumerator IncreaseBalls(Text textComponent, int wasBalls, int becomeBalls) {
        int i = (becomeBalls - wasBalls) / 2; //halfway
        int _initialFontSize = textComponent.fontSize;
        bool _changeSize = false;

        while (wasBalls < becomeBalls) {
            i--;
            if (i > 0 && _changeSize) {
                textComponent.fontSize++;
            }
            else if (_changeSize) {
                textComponent.fontSize--;
            }
            _changeSize = !_changeSize;

            wasBalls++;
            textComponent.text = "x" + wasBalls.ToString();
            yield return new WaitForSeconds(0.02f);
        }
        textComponent.fontSize = _initialFontSize;
        BallLauncher.Instance.SetBallsUIText();
    }

    public void DisableButton() {
        Button.SetActive(false);
    }

    public void EnableButton() {
        int has = hasExtraBallsItem();
        HC_cost.SetActive(has < 0);
        AmountText.text = has >= 0 ? "x" + PlayerController.player.items[has].amount.ToString() : "";
        Button.SetActive(true);
    }

    public void UpdateVisual() {
        if (PlayerController.player.gems < CostGems) {
            DisableButton();
        }
    }

    public void OnClick_GetMoreBalls() {
        int hasItem = hasExtraBallsItem();
        
        if (hasItem >= 0) {
            GetMoreBalls(ExtraBallsAmount);
            UpdateVisual();
            if (PlayerController.player.items[hasItem].amount > 1) {
                PlayerController.player.items[hasItem].amount--;
            }
            else {
                PlayerController.player.items.RemoveAt(hasItem);
            }
            AnalyticsController.Instance.LogLevelBoostsUsedEvent("Level " + AllLevelsData.CurrentLevelIndex, "MoreBalls", BallLauncher.Instance.shotCount);
        }
        else
         if (PlayerController.player.gems >= CostGems) {
            PlayerController.player.gems -= CostGems;

            AnalyticsController.Instance.LogSpendCreditsEvent("MoreBalls", "Power Up", CostGems);
            GetMoreBalls(ExtraBallsAmount);
        }
        else {
            GameUIController.Instance.ShowRedirectPoor();
        }
        EnableButton();
    }

    private int hasExtraBallsItem() {
        int has = -1;
        for (int i = 0; i < PlayerController.player.items.Count; i++) {
            if (PlayerController.player.items[i].name.Equals("Booster Balls")) {
                has = i;
            }
        }

        return has;
    }
}
