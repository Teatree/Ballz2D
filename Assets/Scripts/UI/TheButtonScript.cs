using UnityEngine;
using UnityEngine.UI;

public class TheButtonScript : MonoBehaviour {
    public Button buttonComponent;
    private BallLauncher ballLauncher;

    void Start() {
        buttonComponent.onClick.AddListener(HandleClick);
        ballLauncher = FindObjectOfType<BallLauncher>();
    }

    public void HandleClick() {
        ballLauncher.StopAllCoroutines();
        ballLauncher.SummonAllBalls();
    }



}
