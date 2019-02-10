using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DayBoxTimer : MonoBehaviour {

    public Text timerText;
    public GameObject dayBoxWaitButtton;


    public float timeLeftMin = 360; //6h
    public static bool stop = true;

    private float minutes;
    private float hours;

    void Start() {
        startTimer(10);
    }

    void Update() {
        if (stop) {
            if (dayBoxWaitButtton.activeSelf) {
                dayBoxWaitButtton.SetActive(false);
            }
            return;
        }


        timeLeftMin -= Time.deltaTime;

        hours = Mathf.Floor(timeLeftMin / 60);
        minutes = timeLeftMin % 60;
        if (minutes > 59) minutes = 59;
        if (minutes < 0) {
            stop = true;
            minutes = 0;
            minutes = 0;
        }
    }

    public void startTimer(float fromInMin) {
        dayBoxWaitButtton.SetActive(true);
        stop = false;
        timeLeftMin = fromInMin;
        Update();
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown() {
        while (!stop) {
            timerText.text = string.Format("{0:0}:{1:00}", hours, minutes);
            yield return new WaitForSeconds(60f);
        }
    }

    public void showWaitForItPopup() {
        Debug.Log(">>>> Wait for it");
        UIController.Instance.showWaitForItPopup();
    }

}