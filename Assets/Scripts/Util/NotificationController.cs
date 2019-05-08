using UnityEngine;
using System;
using Assets.SimpleAndroidNotifications;

public class NotificationController : SceneSingleton<NotificationController> {

    private static bool notificationSet;

	void Start () {
        //ScheduleSimple();
    }

    public void ScheduleSimple() {
        NotificationManager.Send(TimeSpan.FromSeconds(5), "Simple notification", "Come back please, omg come back", new Color(1, 0.3f, 0.15f));
    }

    public void ScheduleNormal() {
        NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(15), "Notification!", "Hello world :3", new Color(0, 0.6f, 1), NotificationIcon.Heart);
    }

    public void ScheduleBoxNotification(DateTime notifyAt) {
        TimeSpan waitTime = notifyAt - DateTime.Now; 
        var notificationParams = new NotificationParams {
            Id = UnityEngine.Random.Range(0, int.MaxValue),
            Delay = TimeSpan.FromSeconds(waitTime.TotalSeconds),
            Title = "It's time!",
            Message = "Get the box!",
            Ticker = "Ticker",
            Sound = true,
            Vibrate = false,
            Light = true,
            SmallIcon = NotificationIcon.Heart,
            SmallIconColor = new Color(0.7f, 0.5f, 0),
            LargeIcon = "app_icon"
        };

        NotificationManager.SendCustom(notificationParams);
    }

    public void ScheduleComeback(DateTime notifyAt) {
        if (!notificationSet) {
            TimeSpan waitTime = notifyAt - DateTime.Now;
            var notificationParams = new NotificationParams {
                Id = UnityEngine.Random.Range(0, int.MaxValue),
                Delay = TimeSpan.FromHours(waitTime.Hours),
                Title = "We miss you",
                Message = "Pease come back",
                Ticker = "Ticker",
                Sound = true,
                Vibrate = false,
                Light = true,
                SmallIcon = NotificationIcon.Heart,
                SmallIconColor = new Color(0.7f, 0.5f, 0),
                LargeIcon = "app_icon"
            };

            NotificationManager.SendCustom(notificationParams);
            notificationSet = true;
        }
    }

    public void ScheduleCustom() {
        var notificationParams = new NotificationParams {
            Id = UnityEngine.Random.Range(0, int.MaxValue),
            Delay = TimeSpan.FromSeconds(5),
            Title = "Custom notification",
            Message = "Message",
            Ticker = "Ticker",
            Sound = true,
            Vibrate = true,
            Light = true,
            SmallIcon = NotificationIcon.Heart,
            SmallIconColor = new Color(0, 0.5f, 0),
            LargeIcon = "app_icon"
        };

        NotificationManager.SendCustom(notificationParams);
    }

    public void CancelAll() {
        NotificationManager.CancelAll();
    }
}
