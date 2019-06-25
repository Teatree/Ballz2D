using UnityEngine;
using UnityEngine.UI;

public class SubscriptionPopup : IPopup<SubscriptionPopup> {
    public Text freeCost;
    public Text monthlySubCost;
    public Text yearlySubCost;

    public GameObject freeBtn;
    public GameObject monthlySubBtn;
    public GameObject yearlySubBtn;

    public bool isOnStart;

    public void Start()
    {
        InitPopup();
    }


    public void InitPopup() {
        if (isOnStart) {
            monthlySubBtn.SetActive(false);
            yearlySubBtn.SetActive(false);
        }
        else {
            monthlySubCost.text = Purchaser.purchaser.GetLocalPrice(Purchaser.MONTH_SUB);
            yearlySubCost.text = Purchaser.purchaser.GetLocalPrice(Purchaser.YEAR_SUB);
        }
    }

    public void Update() {
        freeCost.text = Purchaser.advertiseSubs + " / " + Purchaser.debugbs;
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
