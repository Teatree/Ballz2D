using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoreBallsPowerup : SceneSingleton<MoreBallsPowerup> {

    public GameObject Ani;
    public GameObject Button;
    public GameObject Text;

    private Button _buttonComponent;
    private int CostGems;

    public int ExtraBallsAmount;

    public void Start() {
        _buttonComponent = Button.transform.GetComponent<Button>();
        UpdateVisual();
    }

    public void GetMoreBalls() {
        Debug.Log("More balls!");
        if (GameController.Gems >= CostGems) {
            PlayAni();
            BallLauncher.ExtraBalls += ExtraBallsAmount;
            GameController.Gems -= CostGems;
            CostGems += 100;
        }
       TextCanvasUpdate();
        UpdateVisual();
    }

    public void TextCanvasUpdate() {
        BallLauncher.Instance.CheckExtraBalls();
        GameObject textCanvas = BallLauncher.Instance.textCanvas;
      //  textCanvas.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "x" + BallLauncher.Instance.BallsReadyToShoot;
        textCanvas.gameObject.transform.localPosition = new Vector3(0.27f, 0.17f);
        if (textCanvas.gameObject.transform.position.x > 2.55f) textCanvas.gameObject.transform.position = new Vector3(2.55f, textCanvas.gameObject.transform.position.y);
        StartCoroutine(IncreaseScore(textCanvas.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>(), BallLauncher.Instance.BallsReadyToShoot, BallLauncher.Instance.BallsReadyToShoot + ExtraBallsAmount));
    }

    public IEnumerator IncreaseScore(Text textComponent, int wasBalls, int becomeBalls) {
        int i = (becomeBalls - wasBalls)/2; //halfway
        int _initialFontSize = textComponent.fontSize;
        bool _changeSize = false;

        while (wasBalls < becomeBalls) {
            i--;
            if (i > 0 && _changeSize) {
                textComponent.fontSize ++;
            } else if (_changeSize){
                textComponent.fontSize--;
            }
            _changeSize = !_changeSize;
         
            wasBalls++;
            textComponent.text = "x"+ wasBalls.ToString();
            yield return new WaitForSeconds(0.02f);
        }
        textComponent.fontSize = _initialFontSize;
    }

    public void DisableButton() {
        if (_buttonComponent != null) {
            _buttonComponent.interactable = false;
        }
    }

    public void EnableButton() {
        if (_buttonComponent != null && GameController.Gems > CostGems) {
            _buttonComponent.interactable = true;
        }
    }

    public void UpdateVisual() {
        if (_buttonComponent != null) {
            Text.GetComponent<Text>().text = CostGems > 0 ? "" + CostGems : "";
        }

        if (GameController.Gems < CostGems) {
            DisableButton();
        }
    }

    private void PlayAni() {

    }
}
