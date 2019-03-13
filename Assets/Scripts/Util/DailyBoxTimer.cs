using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyBoxTimer : MonoBehaviour {

    public Text timer;
    public Text popupTimer; 

    private int _countdownHour;
    private int _countdownMinute;
    private DateTime _countdown;

    void Update () {
        foreach (DateTime d in PlayerController.times) {
            if (DateTime.Now.Hour < d.Hour) {
                // if reached time
                if (_countdownHour <= 0 && _countdownMinute <= 0 && (59 - DateTime.Now.Second) <= 2) {
                    UIController.Instance.ShowDayBoxButton();
                }

                _countdownHour = d.Hour - DateTime.Now.Hour - 1;
                _countdownMinute = 59 - DateTime.Now.Minute;

                PlayerController.player.giveBoxAt = d.ToString("yyyy-MM-dd HH:mm:ss");

                break;
            } 
        }
        if(DateTime.Now.Hour > PlayerController.times[PlayerController.times.Count - 1].Hour) {
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
}
