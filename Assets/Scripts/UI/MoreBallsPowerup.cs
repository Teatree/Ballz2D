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
        bool showHC = true;
        foreach (ItemData i in PlayerController.player.items) {
            if (i.name.Equals("ExtraBalls")) {
                showHC = false;
            }
        }
        HC_cost.SetActive(showHC);
        Button.SetActive(true);
    }

    public void UpdateVisual() { 
        if (PlayerController.player.gems < CostGems) {
            DisableButton();
        }
    }

    public void OnClick_GetMoreBalls() {
        if (PlayerController.player.gems >= CostGems) {
            PlayerController.player.gems -= CostGems;
            GetMoreBalls();
        }
        else {
            GameUIController.Instance.ShowShop();
        }
    }
}
