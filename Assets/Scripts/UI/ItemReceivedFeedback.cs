using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemReceivedFeedback : MonoBehaviour {

    public Text amount;
    public Image icon;
    public bool active;
    public bool two;
    public Text amount2_1;
    public Image icon2_1;
    public Text amount2_2;
    public Image icon2_2;
    public GameObject itemHolder;
    public GameObject itemHolder2;

    public void SetTwo(bool two) {
        this.two = two;
        itemHolder.gameObject.SetActive(!two);
        itemHolder2.gameObject.SetActive(two);
    }

    public void setAmount(int amount) {
        this.amount.text = "x " + amount;
    }

    public void setIcon(Sprite iconSprite) {
        this.icon.sprite = iconSprite;
        this.icon.SetNativeSize();
    }

    public void setAmount2(int amount, int amount2) {
        this.amount2_1.text = "x" + amount;
        this.amount2_2.text = "x" + amount2;
    }

    public void setIcon2(Sprite iconSprite, Sprite iconSprite2) {
        this.icon2_1.sprite = iconSprite;
        this.icon2_1.SetNativeSize();
        this.icon2_2.sprite = iconSprite2;
        this.icon2_2.SetNativeSize();
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
        if (active && !two) {
            MoreBallsPowerup.Instance.UpdateVisual();
            StartCoroutine(waitBeforeShoot());

            BallLauncher.ExtraAdBalls = int.Parse(amount.text);
            BallLauncher.Instance.SetBallsUIText();
        }
        else {
            Destroy(gameObject);
        }
    }

    protected IEnumerator waitBeforeShoot() {
        for (int i = 0; i < 10; i++) {
            BallLauncher.Instance.BallsReadyToShoot = BallLauncher.Instance.balls.Count;
            BallLauncher.canShoot = true;

            active = false;
            
            yield return null;
        }
        transform.gameObject.SetActive(false);
    }
}
