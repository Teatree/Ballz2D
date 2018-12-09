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
        SummonBalls();
    }


    private void SummonBalls() {
        foreach (Ball b in ballLauncher.balls) {
            ballLauncher.gameObject.SetActive(true);
            b.SetDir(ballLauncher.transform.position - b.transform.position);
            b.DisableCollision();
        }
    }
}
