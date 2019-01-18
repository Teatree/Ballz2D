using UnityEngine.UI;

public class Pause : IPopup<Pause> {
    public Text LevelText;

    void Start() {
        LevelText.text = "Level: " + (AllLevelsData.CurrentLevelIndex + 1).ToString();
    }

    public override void OnClick_Close() {
        LevelController.ResumeGame();
        base.OnClick_Close();
    }

    public void OnClick_Replay() {
        GameUIController.Instance.HandleRestart();
    }
}
