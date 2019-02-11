using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DayBoxTimer : SceneSingleton<DayBoxTimer>{

    public Text timerText;
    public GameObject dayBoxWaitButtton;


    public float timeLeftMin = 0; //6h
    private float timeLeftSec = 0; //6h
    public static bool stop = true;

    private float minutes;
    private float hours;

    void Start() {
        if (timeLeftMin > 0) {
            startTimer(timeLeftMin);
            UIController.Instance.ShowDayBoxWaitButton();
        }
    }

    void Update() {
        if (stop) {
            if (dayBoxWaitButtton.activeSelf) {
                UIController.Instance.ShowDayBoxButton();
            }
            return;
        }

        timeLeftSec -= Time.deltaTime;
        timeLeftMin = Mathf.Floor(timeLeftSec / 60);

        hours = Mathf.Floor(timeLeftMin / 60);
        minutes = timeLeftMin % 60;
        if (minutes > 59) minutes = 59;
        if (hours < 0) {
            stop = true;
            hours = 0;
            minutes = 0;
        }
    }

    public void startTimer(float fromInMin) {
        dayBoxWaitButtton.SetActive(true);
        stop = false;
        timeLeftMin = fromInMin;
        timeLeftSec = fromInMin * 60;
        Update();
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown() {
        while (!stop) {
            timerText.text = string.Format("{0:0}:{1:00}", hours, minutes);
            yield return new WaitForSeconds(1f);
        }
    }

    public void showWaitForItPopup() {
        Debug.Log(">>>> Wait for it");
        UIController.Instance.showWaitForItPopup();
    }

    public void SetCountDownTo(DateTime to) {
        TimeSpan t = DateTime.Now - to;
        timeLeftMin = (float) t.TotalMinutes;
        Debug.Log(">>>> timespan " + t.TotalMinutes);
    }
}