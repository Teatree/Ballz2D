public class Pause : IPopup<Pause> {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnClick_Close() {
        LevelController.ResumeGame();
        base.OnClick_Close();
    }

    public void OnClick_Replay() {
        GameUIController.Instance.HandleRestart();
    }
}
