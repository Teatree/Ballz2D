using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyBoxTimer : MonoBehaviour {

    public Text timer;

    private int _countdownHour;
    private int _countdownMinute;
    private DateTime _countdown;

    void Update () {
        // if have a box don't count
        foreach (DateTime d in PlayerController.times) {
            if (DateTime.Now.Hour < d.Hour) {

                // if reached time
                if (_countdownHour <= 0 && _countdownMinute <= 0 && (59 - DateTime.Now.Second) <= 2) {
                    UIController.Instance.ShowDayBoxButton();
                }

                _countdownHour = d.Hour - DateTime.Now.Hour - 1;
                _countdownMinute = 59 - DateTime.Now.Minute;

                timer.text = string.Format("{0:00}h {1:00}m {2:00}s", _countdownHour, _countdownMinute, 59 - DateTime.Now.Second);

                PlayerController.player.giveBoxAt = d.ToString("yyyy-MM-dd HH:mm:ss");

                break;
            }
            
        }
    }
}
