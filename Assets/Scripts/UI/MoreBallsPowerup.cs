using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoreBallsPowerup : SceneSingleton<MoreBallsPowerup> {

    public GameObject Ani;
    public GameObject Button;
    public GameObject HC_cost;
    public int CostGems;

    public int ExtraBallsAmount;

    public void Start() {
        UpdateVisual();
    }

    public void GetMoreBalls() {
        BallLauncher.ExtraBalls += ExtraBallsAmount;
        TextCanvasUpdate();
        UpdateVisual();
    }

    public void TextCanvasUpdate() {
        BallLauncher.Instance.CheckExtraBalls();
        GameObject textCanvas = BallLauncher.Instance.textCanvas;
        textCanvas.gameObject.transform.localPosition = new Vector3(0.27f, 0.17f);
        if (textCanvas.gameObject.transform.position.x > 2.55f) textCanvas.gameObject.transform.position = new Vector3(2.55f, textCanvas.gameObject.transform.position.y);
        StartCoroutine(IncreaseScore(textCanvas.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>(), BallLauncher.Instance.BallsReadyToShoot, BallLauncher.Instance.BallsReadyToShoot + ExtraBallsAmount));
    }

    public IEnumerator IncreaseScore(Text textComponent, int wasBalls, int becomeBalls) {
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
    }

    public void DisableButton() {
        Button.SetActive(false);
    }

    public void EnableButton() {
        HC_cost.SetActive(hasExtraBallsItem() < 0);
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
            GetMoreBalls();
            if (PlayerController.player.items[hasItem].amount > 1) {
                PlayerController.player.items[hasItem].amount--;
            }
            else {
                PlayerController.player.items.RemoveAt(hasItem);
            }
        }
        else
         if (PlayerController.player.gems >= CostGems) {
            PlayerController.player.gems -= CostGems;
            GetMoreBalls();
        }
        else {
            GameUIController.Instance.ShowShop();
        }
        EnableButton();
    }

    private int hasExtraBallsItem() {
        int has = -1;
        for (int i = 0; i < PlayerController.player.items.Count; i++) {
            if (PlayerController.player.items[i].name.Equals("ExtraBalls")) {
                has = i;
            }
        }

        return has;
    }
}
