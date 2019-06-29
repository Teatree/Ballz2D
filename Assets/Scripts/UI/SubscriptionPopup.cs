﻿using System;
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

    private int _countdownHour;
    private int _countdownMinute;

    public ItemObject boosterLightningItem;
    public ItemObject boosterMoreBallsItem;

    void Update() {

        // timer
        if (claimButton != null && claimButton.GetComponent<Button>().interactable == false) {
            _countdownHour = 23 - DateTime.Now.Hour;
            _countdownMinute = 59 - DateTime.Now.Minute;

            timer.text = string.Format("{0:00}h {1:00}m {2:00}s", _countdownHour, _countdownMinute, 59 - DateTime.Now.Second);
            //Debug.Log(">>>>> timerPopo > " + (popupTimer == null));
        }
    }

    public bool isOnStart;

    public void Start()
    {
        InitPopup();

        // find an kill
        if(Purchaser.giveSubsStuff){
            var IamToBeKilled = GameObject.Find("Subscription(Clone)");
            Destroy(IamToBeKilled);
        }
    }

    public void ClaimRewards() {
        int boosterType = (int)UnityEngine.Random.Range(0, 1);
        int boosterAmount = 1;

        PlayerController.player.gems += 400;
        if (boosterType == 0) {
            // give lighting 
            ItemsController.getItem(boosterLightningItem, true);
            // show lightning
            UIController.Instance.Show2ItemReceived(400, boosterAmount, hcIcon, boosterLightningIcon);
            SetItemsClaimed();
        }
        else {
            // give moreb 
            ItemsController.getItem(boosterMoreBallsItem, true);
            // show moreb
            UIController.Instance.Show2ItemReceived(400, boosterAmount, hcIcon, boosterMoreBallsIcon);
            SetItemsClaimed();
        }
    }

    public void SetItemsClaimed() {
        receivedItemsDescText.text = "✔ 1  random booster \n" + "✔ 400 Gems \n" + "✔ No ADS ";
        PlayerController.player.SubBonusReceivedDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
        if(claimButton != null) claimButton.GetComponent<Button>().interactable = false;
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

        DateTime dt = PlayerController.player.SubBonusReceivedDate != null && PlayerController.player.SubBonusReceivedDate != ""
                                        ? DateTime.Parse(PlayerController.player.SubBonusReceivedDate) : DateTime.MinValue;
        
        if (dt.Day >= DateTime.Now.Day) {
            if (claimButton != null) claimButton.GetComponent<Button>().interactable = false;
        }
        else {
            if (claimButton != null) claimButton.GetComponent<Button>().interactable = true;
        }
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
