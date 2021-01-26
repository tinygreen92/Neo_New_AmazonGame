using EasyMobile;
using Firebase.Analytics;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                /// 파이어베이스 로그인 하고  FCM 세팅
                Login();
                //Notifications.Init();
                //Notifications.LocalNotificationOpened += OnLocalNotificationOpened;
                //Notifications.RemoteNotificationOpened += OnRemoteNotificationOpened;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    #region 이지모바일 노티피케이션 (미적용)

    /// <summary>
    /// 로컬 알람 테스트
    ///     // Schedule a notification to be delivered by 08:08AM, 08 August 2018.
    /// </summary>
    void ScheduleLocalNotification()
    {
        // Prepare the notification content (see the above section).
        NotificationContent content = PrepareNotificationContent();

        // Set the delivery time.
        DateTime triggerDate = new DateTime(2020, 12, 29, 15, 00, 08);

        // Schedule the notification.
        Notifications.ScheduleLocalNotification(triggerDate, content);

        // Set the delay time as a TimeSpan.
        TimeSpan delay = new TimeSpan(00, 02, 30);
        Notifications.ScheduleLocalNotification(delay, content);


    }

    // Construct the content of a new notification for scheduling.
    NotificationContent PrepareNotificationContent()
    {
        NotificationContent content = new NotificationContent();

        // Provide the notification title.
        content.title = "Demo Notification";

        // You can optionally provide the notification subtitle, which is visible on iOS only.
        content.subtitle = "Demo Subtitle";

        // Provide the notification message.
        content.body = "This is a demo notification.";

        // You can optionally attach custom user information to the notification
        // in form of a key-value dictionary.
        content.userInfo = new Dictionary<string, object>();
        content.userInfo.Add("string", "OK");
        content.userInfo.Add("number", 3);
        content.userInfo.Add("bool", true);

        // You can optionally assign this notification to a category using the category ID.
        // If you don't specify any category, the default one will be used.
        // Note that it's recommended to use the category ID constants from the EM_NotificationsConstants class
        // if it has been generated before. In this example, UserCategory_notification_category_test is the
        // generated constant of the category ID "notification.category.test".
        ///content.categoryId = EM_NotificationsConstants.UserCategory_notification_category_test;

        // If you want to use default small icon and large icon (on Android),
        // don't set the smallIcon and largeIcon fields of the content.
        // If you want to use custom icons instead, simply specify their names here (without file extensions).
        ///content.smallIcon = "YOUR_CUSTOM_SMALL_ICON";
        ///content.largeIcon = "YOUR_CUSTOM_LARGE_ICON";

        return content;
    }


    // This handler will be called when a local notification is opened.
    void OnLocalNotificationOpened(EasyMobile.LocalNotification delivered)
    {
        // The actionId will be empty if the notification was opened with the default action.
        // Otherwise it contains the ID of the selected action button.
        if (!string.IsNullOrEmpty(delivered.actionId))
        {
            Debug.Log("Action ID: " + delivered.actionId);
        }

        // Whether the notification is delivered when the app is in foreground.
        Debug.Log("Is app in foreground: " + delivered.isAppInForeground.ToString());

        // Gets the notification content.
        NotificationContent content = delivered.content;

        // Take further actions if needed...
    }

    // This handler will be called when a remote notification is opened.
    void OnRemoteNotificationOpened(EasyMobile.RemoteNotification delivered)
    {
        // The actionId will be empty if the notification was opened with the default action.
        // Otherwise it contains the ID of the selected action button.
        if (!string.IsNullOrEmpty(delivered.actionId))
        {
            Debug.Log("Action ID: " + delivered.actionId);
        }

        // Whether the notification is delivered when the app is in foreground.
        Debug.Log("Is app in foreground: " + delivered.isAppInForeground.ToString());

        // Gets the notification content.
        NotificationContent content = delivered.content;

        // If OneSignal service is in use you can access the original OneSignal payload like below.
        // If OneSignal is not in use this will be null.
        OneSignalNotificationPayload osPayload = delivered.oneSignalPayload;

        // If Firebase Messaging service is in use you can access the original Firebase
        // payload like below. If Firebase is not in use this will be null.
        FirebaseMessage fcmPayload = delivered.firebasePayload;

        // Take further actions if needed...
    }

    #endregion

    /// <summary>
    /// ----------------------------------- 이지 모바일 초기화로 써보는 중 / 파베 제공 함수 일단 대기.
    /// </summary>

    void Login()
    {
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        // Log an event with no parameters.
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
        /// 이지 모바일에서 해주지 않을까? 테스트 필요
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {

        ////string notification = e.Message.Notification.Body;
        //string notification = e.Message.Notification.Body;
        ////var notification = e.Message.Data;
        ////var eData = e.Message.Data;

        //if (e.Message.Data.Count > 0)
        //{
        //    Debug.Log("data:");
        //    foreach (System.Collections.Generic.KeyValuePair<string, string> iter in e.Message.Data)
        //    {
        //        Debug.Log("  " + iter.Key + ": " + iter.Value);
        //        notification = iter.Value;
        //    }
        //}
        //else
        //{
        //    return;
        //}

        //// 예시) 펭수 기념 *국밥*100*그릇 드립니다.
        //Debug.LogWarning("파이어 베이스 몸통 메시지: " + notification);

        //string[] sDataArray = notification.Split('*');

        //// 아이템 + 수량 조합으로 끊어 먹을 것
        //int cutIndex = 0;
        //string itemCode = "";

        //// 출력할 스트링
        //string targetString = "";

        //for (int i = 0; i < sDataArray.Length; i++)
        //{
        //    if (sDataArray[i] == "국밥")
        //    {
        //        itemCode = "gupbap";
        //        cutIndex = i;
        //        break;
        //    }
        //    else if (sDataArray[i] == "열쇠")
        //    {
        //        itemCode = "key";
        //        cutIndex = i;
        //        break;
        //    }
        //    else if (sDataArray[i] == "쌀밥")
        //    {
        //        itemCode = "ssal";
        //        cutIndex = i;
        //        break;
        //    }
        //    else if (sDataArray[i] == "다이아")
        //    {
        //        itemCode = "diamond";
        //        cutIndex = i;
        //        break;
        //    }
        //}


        //for (int j = 0; j < cutIndex; j++)
        //{
        //    targetString += sDataArray[j];
        //}

        //targetString += sDataArray[cutIndex] + " ";
        //targetString += sDataArray[cutIndex + 1];

        //for (int j = cutIndex + 2; j < sDataArray.Length; j++)
        //{
        //    targetString += sDataArray[j];
        //}

        //Debug.LogWarning("분해후 재조합 문자 : " + targetString);
        //Debug.LogWarning("아이템 코드 : " + sDataArray[cutIndex]);
        //Debug.LogWarning("수량 : " + sDataArray[cutIndex + 1]);

        //// PostboxItemSend(string _code, int _amount, string _msg)

        //GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().PostboxItemSend(itemCode, int.Parse(sDataArray[cutIndex + 1]), targetString);

    }


}
