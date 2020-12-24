using CodeStage.AntiCheat.Storage;
using EasyMobile;
using System;
using UnityEngine;
using UnityEngine.UI;

public class OfflineManager : MonoBehaviour
{
    public NanooManager nm;
    [Header("-메인 오프라인 팝업")]
    public Image AdsIcon;
    public Sprite[] AdsSprite;
    public GameObject offlinePop;
    public Text[] rewordText;                                               /// 0 골드 1 나뭇잎 2 강화석 3 아마존 조각
    public Text distanceText;
    public Text monsterKillText;
    public Text timerText;

    [Header("-메인 0// 보상 1 버튼 전환")]
    public GameObject[] BtnLayouts;

    [Header("-보상 텍스트 -> 동영상 두배 문구 전환 ")]
    public Text rewordDesc;


    DateTime unbiasedRemaining;

    void SaveDateTime(DateTime dateTime)
    {
        string tmp = dateTime.ToString("yyyyMMddHHmmss");
        ObscuredPrefs.SetString("DateTime", tmp);
        Debug.LogWarning("세이브 데이터 타임 " + tmp);
    }
    DateTime LoadDateTime()
    {
        /// if (!PlayerPrefsManager.isTutorialClear) return UnbiasedTime.Instance.Now();

        string data = ObscuredPrefs.GetString("DateTime", "19000101120000");
        DateTime saveDateTime = DateTime.ParseExact(data, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
        Debug.LogWarning("로드 데이터 타임 " + saveDateTime);
        return saveDateTime;
    }

    private bool isPaused = false; // 앱 일시정지
    /// <summary>
    /// 앱 일시정지 일때
    /// </summary>
    /// <param name="pause"></param>
    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isPaused = true;
            /* 앱이 비활성화 되었을 때 처리 */
            unbiasedRemaining = UnbiasedTime.Instance.Now();
            SaveDateTime(unbiasedRemaining);

        }
        else
        {
            if (isPaused)
            {
                isPaused = false;
                /* 앱이 활성화 되었을 때 처리 */
                /// TODO : 오프라인 보상 처리
                OfflineInit();
            }
        }
    }
    /// <summary>
    /// 앱이 완전히 종료 될때.
    /// </summary>
    void OnApplicationQuit()
    {
        /* 앱이 종료 될 때 처리 */
        unbiasedRemaining = UnbiasedTime.Instance.Now();
        SaveDateTime(unbiasedRemaining);
    }

    public void OfflineInit()
    {
        if (PlayerInventory.RecentDistance < 1.0d)
        {
            PlayerPrefsManager.isGetOfflineReword = true;
            return;
        }

        DateTime dateTime = LoadDateTime();
        /// 5분 이상 떠나있었다면
        if (dateTime.AddSeconds(302) < UnbiasedTime.Instance.Now())
        {
            Debug.LogWarning("UnbiasedTime.Instance.Now() : " + UnbiasedTime.Instance.Now());
            rewordDesc.text = Lean.Localization.LeanLocalization.GetTranslationText("StayReword_Description");
            /// 오프라인 시간 계산
            TimeSpan resultTime = (UnbiasedTime.Instance.Now() - dateTime);
            int reHours = resultTime.Hours;
            int reMinutes = resultTime.Minutes;
            int reSeconds = resultTime.Seconds;
            int maxTime = Mathf.RoundToInt((float)PlayerInventory.Offline_Time);
            /// 3시간 넘거나 하루가 지나면 최대치
            if ((reHours * 60) + reMinutes >= maxTime || resultTime.Days > 0)
            {
                /// 최대치 일때
                reHours = maxTime / 60;
                reMinutes = maxTime % 60;
                reSeconds = 0;
                //
                timerText.text = string.Format("{0:00}:{1:00}:{2:00}", reHours, reMinutes, reSeconds);
                /// 실제 지급
                CalOfflineReword(reHours * 3600);
            }
            else
            {
                /// 최대치 미만일 때
                timerText.text = string.Format("{0:00}:{1:00}:{2:00}", reHours, reMinutes, reSeconds);

                /// 실제 지급
                CalOfflineReword((reHours * 3600) + (reMinutes * 60) + reSeconds);
            }
            /// 광고 스킵권 구매했음? 광고 아이콘 바꾸기
            if (PlayerInventory.isSuperUser != 0)
            {
                AdsIcon.sprite = AdsSprite[1];
            }
            else if (AdsIcon.sprite != AdsSprite[0])
            {
                AdsIcon.sprite = AdsSprite[0];
            }
            /// 팝업
            offlinePop.SetActive(true);
        }
        else
        {
            PlayerPrefsManager.isGetOfflineReword = true;
        }

    }


    double dtimeBae = 0;
    double rainbowDash = 0;
    long applejack = 0;
    long ltimeBae1 = 0;
    long ltimeBae2 = 0;
    long ltimeBae3 = 0;


    /// <summary>
    /// 최대 거리 보스 HP 계산
    /// </summary>
    /// <param name="dtimeBae">이론상 최대거리</param>
    /// <returns></returns>
    double GetLastBossHp(double dtimeBae) => 5d * (5d* 1.55d * dtimeBae) * PlayerInventory.Monster_Boss_HP;

    /// <summary>
    /// 보상지급과 텍스트 표시 동시에 1분 단위 지급
    /// </summary>
    /// <param name="_input">오프라인 경과 [초]</param>
    void CalOfflineReword(int _input)
    {
        /// 임시 오프라인 전 거리 저장
        double tmpDistance = PlayerInventory.RecentDistance;
        // 이론상 최대거리 계산
        rainbowDash = (_input * 0.1d) + tmpDistance;
        /// 0.1km 단위 절삭
        applejack = Mathf.FloorToInt((float)(rainbowDash *0.1d));
        applejack *= 10;

        Debug.LogWarning("오프라인 초 : " + _input + " 최대 거리 :  " + rainbowDash  + "m" + " 애플 거리" + applejack  +"m");

        double playerDps = PlayerInventory.Player_DPS * 30.0d;
        double lastBossHp = GetLastBossHp(applejack);
        // 최대거리 가는 동안 올 보스 킬 가능?
        if (playerDps < lastBossHp)
        {
            /// 최대거리 보스 못 잡았음
            for (long i = applejack; i > tmpDistance; applejack -= 10)
            {
                /// 어디까지 잡았니?
                if (playerDps > GetLastBossHp(applejack))
                {
                    /// 잡은 보스 + 0.9km
                    Debug.LogWarning("플레이어 dps : " + playerDps + " 보스 거리 :  " + applejack);
                    break;
                }
            }
            /// 거리 갱신
            PlayerInventory.RecentDistance = applejack + 19.0d;
        }
        else
        {
            /// 거리 갱신
            PlayerInventory.RecentDistance = Mathf.RoundToInt((float)rainbowDash);
        }
        // 거리 표기
        DistanceManager.instance.TextDistDisplay(PlayerInventory.RecentDistance);
        PlayerPrefsManager.isGetOfflineReword = true;
        /// 새로운 거리 다시 세팅
        rainbowDash = PlayerInventory.RecentDistance;
        /// 나머지 팝업 채우기
        if (rainbowDash - tmpDistance < 1)
        {
            distanceText.text = "0km";
        }
        else
        {
            distanceText.text = ((rainbowDash - tmpDistance) * 0.1d).ToString("N1") + "km";
        }
        monsterKillText.text = (_input * 0.1d).ToString("N0") + "마리";

        // 골드 계산
        dtimeBae = (_input * 0.01d) * (3d * 1.15d * rainbowDash) * PlayerInventory.Player_Gold_Earned;
        dtimeBae *= PlayerInventory.Offline_Earned;
        PlayerInventory.Money_Gold += dtimeBae;
        ///  골드 업적 카운트 올리기
        ListModel.Instance.ALLlist_Update(3, dtimeBae);


        //나뭇잎
        ltimeBae1 = 0;
        /// 나뭇잎 기본 획득량 공식 (거리 비례)
        for (double i = tmpDistance; i < rainbowDash; i+= 10.0d)
        {
            ltimeBae1 += Mathf.CeilToInt((float)(PlayerInventory.Player_Leaf_Earned * 10.0d * (1.0d + (0.35d * applejack))));
        }
        Debug.LogWarning("오프라인 이전 거리 : " + tmpDistance + " 보스 잡고 최대거리  :  " + rainbowDash + " 보스전 거리 " + applejack);

        if (rainbowDash < 11.0d) ltimeBae1 = 0;              /// 보스킬 0 이면 예외처리
        ltimeBae1 = (long)(ltimeBae1 * PlayerInventory.Offline_Earned);
        PlayerInventory.Money_Leaf += ltimeBae1;
        /// 나뭇잎 획득량 업적 올리기
        ListModel.Instance.ALLlist_Update(4, ltimeBae1);

        //강화석
        ltimeBae2 = _input / 600;
        ltimeBae2 = (long)(ltimeBae2 * PlayerInventory.Offline_Earned);
        PlayerInventory.SetTicketCount("reinforce_box", (int)ltimeBae2);

        // 아마존 결정 조각
        ltimeBae3 = Mathf.CeilToInt(_input / 500.0f);
        ltimeBae3 = (long)(ltimeBae3 * PlayerInventory.Offline_Earned);
        PlayerInventory.AmazonStoneCount += ltimeBae3;
        /// 결정조각  업적  카운트
        ListModel.Instance.ALLlist_Update(2, ltimeBae3);
        /// 텍스트 표기
        rewordText[0].text = PlayerPrefsManager.instance.DoubleToStringNumber(dtimeBae);
        rewordText[1].text = PlayerPrefsManager.instance.DoubleToStringNumber(ltimeBae1);
        rewordText[2].text = ltimeBae2.ToString("N0");
        rewordText[3].text = ltimeBae3.ToString("N0");

    }

    ///// <summary>
    ///// 백업용 입니다.
    ///// </summary>
    ///// <param name="_input"></param>
    //void BACKCalOfflineReword(int _input)
    //{
    //    //골드
    //    if (PlayerInventory.RecentDistance < 1.0d)
    //    {
    //        dtimeBae = 1.0d;
    //    }
    //    else
    //    {
    //        dtimeBae = PlayerInventory.RecentDistance;
    //    }

    //    dtimeBae = 2d * (1.07d * dtimeBae * PlayerInventory.Player_Gold_Earned);
    //    dtimeBae *= _input * 1.0d < 1.0d ? 1 : _input * 1.0d;
    //    dtimeBae *= PlayerInventory.Offline_Earned;
    //    PlayerInventory.Money_Gold += dtimeBae;
    //    //나뭇잎
    //    /// 나뭇잎 기본 획득량 공식 (거리 비례)
    //    double currentLeaf = 10.0d * (1.0d + (0.35d * PlayerInventory.RecentDistance));
    //    ltimeBae1 = Mathf.CeilToInt((float)(PlayerInventory.Player_Leaf_Earned * currentLeaf) / 2.0f);
    //    ltimeBae1 *= _input / 10 == 0 ? 1 : _input / 10;
    //    if (PlayerInventory.RecentDistance < 11.0d) ltimeBae1 = 0;              /// 보스킬 0 이면 예외처리
    //    ltimeBae1 = (long)(ltimeBae1 * PlayerInventory.Offline_Earned);
    //    PlayerInventory.Money_Leaf += ltimeBae1;
    //    /// 나뭇잎 획득량 업적 올리기
    //    ListModel.Instance.ALLlist_Update(4, ltimeBae1);
    //    //강화석
    //    ltimeBae2 = ltimeBae1 / 10;
    //    ltimeBae2 = (long)(ltimeBae2 * PlayerInventory.Offline_Earned);
    //    PlayerInventory.Money_EnchantStone += ltimeBae2;
    //    ///  강화석 업적 카운트 올리기
    //    ListModel.Instance.ALLlist_Update(5, ltimeBae2);

    //    // 아마존 결정 조각
    //    ltimeBae3 = _input / 10;
    //    ltimeBae3 = (long)(ltimeBae3 * PlayerInventory.Offline_Earned);
    //    PlayerInventory.AmazonStoneCount += ltimeBae3;
    //    /// 결정조각  업적  카운트
    //    ListModel.Instance.ALLlist_Update(2, ltimeBae3);
    //    /// 텍스트 표기
    //    rewordText[0].text = PlayerPrefsManager.instance.DoubleToStringNumber(dtimeBae);
    //    rewordText[1].text = ltimeBae1.ToString("N0");
    //    rewordText[2].text = ltimeBae2.ToString("N0");
    //    rewordText[3].text = ltimeBae3.ToString("N0");

    //}


    /// <summary>
    /// 광고 동영상 보면 2배로 뿔려주기
    /// </summary>
    void AdsOfflineReword()
    {
        PlayerInventory.Money_Gold += dtimeBae;
        rewordText[0].text = PlayerPrefsManager.instance.DoubleToStringNumber(dtimeBae * 2.0d);
        ///  골드 업적 카운트 올리기
        ListModel.Instance.ALLlist_Update(3, dtimeBae * 2.0d);

        PlayerInventory.Money_Leaf += ltimeBae1;
        /// 나뭇잎 획득량 업적 올리기
        ListModel.Instance.ALLlist_Update(4, ltimeBae1);
        rewordText[1].text = PlayerPrefsManager.instance.DoubleToStringNumber(ltimeBae2 * 2);

        PlayerInventory.Money_EnchantStone += ltimeBae2;
        ///  강화석 업적 카운트 올리기
        ListModel.Instance.ALLlist_Update(5, ltimeBae2);
        rewordText[2].text = (ltimeBae2 * 2).ToString("N0");

        PlayerInventory.AmazonStoneCount += ltimeBae3;
        /// 결정조각  업적  카운트
        ListModel.Instance.ALLlist_Update(2, ltimeBae3);
        rewordText[3].text = (ltimeBae3 * 2).ToString("N0");
    }

    /// <summary>
    /// 광고 안보고 일반 받기 누를게요
    /// </summary>
    public void ClickedNormalGetBtn()
    {
        PlayerPrefsManager.isGetOfflineReword = true;
        offlinePop.SetActive(false);
    }


    #region <Rewarded Ads> 오프라인  동영상 광고

    bool _AdsComp;
    public void Ads_OffCanvas()
    {
        SystemPopUp.instance.LoopLoadingImg();

        if (PlayerInventory.isSuperUser != 0)
        {
            /// 광고 스킵
            _AdsComp = true;
            Invoke(nameof(AdsOffInvo), 0.5f);
            return;
        }

        if (Advertising.IsRewardedAdReady(RewardedAdNetwork.MoPub, AdPlacement.Default))
        {
            Advertising.RewardedAdCompleted += AdsOffCompleated;
            Advertising.RewardedAdSkipped += AdsOffSkipped;
            Advertising.ShowRewardedAd(RewardedAdNetwork.MoPub, AdPlacement.Default);
        }
        else
        {
            _AdsComp = false;
            Invoke(nameof(AdsOffInvo), 0.5f);
        }

    }
    // Event handler called when a rewarded ad has completed
    void AdsOffCompleated(RewardedAdNetwork network, AdPlacement location)
    {
        _AdsComp = true;
        Invoke(nameof(AdsOffInvo), 0.5f);
        Advertising.RewardedAdCompleted -= AdsOffCompleated;
        Advertising.RewardedAdSkipped -= AdsOffSkipped;
    }

    // Event handler called when a rewarded ad has been skipped
    void AdsOffSkipped(RewardedAdNetwork network, AdPlacement location)
    {
        Advertising.RewardedAdCompleted -= AdsOffCompleated;
        Advertising.RewardedAdSkipped -= AdsOffSkipped;
        SystemPopUp.instance.StopLoopLoading();
    }
    /// <summary>
    /// 보상 지급 메소드
    /// </summary>
    void AdsOffInvo()
    {
        SystemPopUp.instance.StopLoopLoading();
        /// 광고 보상 2배 문구 변경
        if (_AdsComp) rewordDesc.text = Lean.Localization.LeanLocalization.GetTranslationText("Config_Ads_Okay");
        else rewordDesc.text = Lean.Localization.LeanLocalization.GetTranslationText("Config_Ads_Nope");

        /// 보상 지급
        AdsOfflineReword();

        /// 버튼 바꿔줌
        BtnLayouts[0].SetActive(false);
        BtnLayouts[1].SetActive(true);
        ///  광고 1회 시청 완료 카운트
        ListModel.Instance.ALLlist_Update(0, 1);
        /// 광고 시청 일일 업적
        ListModel.Instance.DAYlist_Update(7);
        _AdsComp = false;
    }

    #endregion


    public void ComplateReword()
    {
        /// 버튼 바꿔줌
        BtnLayouts[0].SetActive(true);
        BtnLayouts[1].SetActive(false);
        /// 오프라인 창 꺼줌
        PlayerPrefsManager.isGetOfflineReword = true;
        offlinePop.SetActive(false);
    }

    public void ExitOffpopOff()
    {
        /// 오프라인 창 꺼줌
        PlayerPrefsManager.isGetOfflineReword = true;
        offlinePop.SetActive(false);
    }

}
