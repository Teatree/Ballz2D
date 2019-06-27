using System;
using UnityEngine;
using UnityEngine.UI;

public class SubscriptionPopup : IPopup<SubscriptionPopup> {
    public Text freeCost;
    public Text monthlySubCost;
    public Text yearlySubCost;

    public GameObject freeBtn;
    public GameObject monthlySubBtn;
    public GameObject yearlySubBtn;

    [Header("Claiming")]
    public Text receivedItemsDescText;
    public Sprite hcIcon;
    public Sprite boosterLightningIcon;
    public Sprite boosterMoreBallsIcon;
    public GameObject claimButton;

    //timer
    public Text timer;
    public Text popupTimer;

    private int _countdownHour;
    private int _countdownMinute;

    void Update() {
        // timer
        if (DateTime.Now.Hour > PlayerController.times[PlayerController.times.Count - 1].Hour) {
            var tempDate = PlayerController.times[0].AddDays(1);
            _countdownHour = tempDate.Hour - DateTime.Now.Hour + 23;
            _countdownMinute = 59 - DateTime.Now.Minute;

            PlayerController.player.giveBoxAt = tempDate.ToString("yyyy-MM-dd HH:mm:ss");
        }

        timer.text = string.Format("{0:00}h {1:00}m {2:00}s", _countdownHour, _countdownMinute, 59 - DateTime.Now.Second);
        //Debug.Log(">>>>> timerPopo > " + (popupTimer == null));
        if (popupTimer != null) {
            popupTimer.text = string.Format("{0:00}h {1:00}m {2:00}s", _countdownHour, _countdownMinute, 59 - DateTime.Now.Second);
        }
    }


    public bool isOnStart;

    public void Start()
    {
        InitPopup();
    }

    public void ClaimRewards() {
        int boosterType = (int)UnityEngine.Random.Range(0, 1);
        int boosterAmount = (int)UnityEngine.Random.Range(1, 2);
        if (boosterType == 0) { 
            // give lighting 

            // show lightning
            UIController.Instance.Show2ItemReceived(400, boosterAmount, hcIcon, boosterLightningIcon);
            SetItemsClaimed();
        }
        else {
            
            // give moreb 
            // show moreb
            UIController.Instance.Show2ItemReceived(400, boosterAmount, hcIcon, boosterMoreBallsIcon);
            SetItemsClaimed();
        }
        
    }

    public void SetItemsClaimed() {
        receivedItemsDescText.text = "✔ 1  random booster \n" + "✔ 400 Gems \n" + "✔ No ADS ";
        SetTimer();
    }

    // set timer
    public void SetTimer() {
        claimButton.GetComponent<Button>().interactable = false;
        claimButton.transform.GetChild(0).GetComponent<Text>().text = "timer...";
    }

    public void InitPopup() {
        if (monthlySubBtn != null) {
            if (isOnStart) {
                monthlySubBtn.SetActive(false);
                yearlySubBtn.SetActive(false);
            }
            else {
                monthlySubBtn.SetActive(true);
                yearlySubBtn.SetActive(true);
                monthlySubCost.text = Purchaser.purchaser.GetLocalPrice(Purchaser.MONTH_SUB);
                yearlySubCost.text = Purchaser.purchaser.GetLocalPrice(Purchaser.YEAR_SUB);
            }
        }
    }

    public void Update() {
       // freeCost.text = Purchaser.advertiseSubs + " / " + Purchaser.debugbs;
    }



    public void buyTryWeekSub()
    {
        Purchaser.purchaser.BuyWeekSub();
    }
    public void buyMonthSub()
    {
        Purchaser.purchaser.BuyMonthSub();
    }
    public void buyYearSub()
    {
        Purchaser.purchaser.BuyYearSub();
    }
}
