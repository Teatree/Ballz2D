using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SubscriptionPopup : IPopup<SubscriptionPopup> {
    public Text freeCost;
    public Text monthlySubCost;
    public Text yearlySubCost;

    public void Start()
    {
        freeCost.text = Purchaser.purchaser.GetLocalPrice(Purchaser.WEEK_SUB);
        monthlySubCost.text = Purchaser.purchaser.GetLocalPrice(Purchaser.MONTH_SUB);
        yearlySubCost.text = Purchaser.purchaser.GetLocalPrice(Purchaser.YEAR_SUB);
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
