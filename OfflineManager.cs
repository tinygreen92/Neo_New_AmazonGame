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

    /// <summary>
    /// 페이크 로딩에서 나누 배너 오픈 해준다
    /// </summary>
    public void InitNanooBanner()
    {
        /// 배너 오픈
        nm.OpenBanner();
    }


    DateTime unbiasedRemaining;

    void SaveDateTime(DateTime dateTime)
    {
        string tmp = dateTime.ToString("yyyyMMddHHmmss");
        ObscuredPrefs.SetString("DateTime", tmp);
        ObscuredPrefs.Save();
        Debug.LogWarning("세이브 데이터 타임 " + tmp);
    }
    DateTime LoadDateTime()
    {
        /// if (!PlayerPrefsManager.isTutorialClear) return UnbiasedTime.Instance.Now();

        string data = ObscuredPrefs.GetString("DateTime", "19000101120000");
        DateTime saveDateTime = DateTime.ParseExact(data, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
        Debug.LogWarning("로드 데이터 타임 " + saveDateTime);
        /// 오프라인 시간 자동 저장 트리거 온! 얘 실행 되기 전까지 시간 강제저장 없음
        PlayerPrefsManager.isCheckOffline = true;
        return saveDateTime;
    }

    private bool isPaused = false; // 앱 일시정지
    /// <summary>
    /// 앱 일시정지 일때
    /// </summary>
    /// <param name="pause"></param>
    void OnApplicationPause(bool pause)
    {
        if (!PlayerPrefsManager.isLoadingComp) return;

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
                //OfflineInit();
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
        /// 안쓰는 거지만 유물 ListModel.Instance.Heart_myNeaf(heartIndexs[28] - 1, 1f) 갱신
        var tmppp = PlayerInventory.Offline_Earned;

        /// 거리 짧을때 혹은 서버 데이터 로드해서 재실행하면 오프라인 보상 패스. 
        if (PlayerInventory.RecentDistance < 1.0d || ObscuredPrefs.GetInt("isSeverDataLoad") != 0)
        {
            ObscuredPrefs.SetInt("isSeverDataLoad", 0);
            ObscuredPrefs.Save();
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
            /// 초단위
            int maxTime = (int)Math.Truncate(PlayerInventory.Offline_Time);
            Debug.LogWarning("maxTime : " + maxTime + " resultTime.TotalSeconds : " + resultTime.TotalSeconds);
            /// 3시간 넘거나 하루가 지나면 -> 최대치로 고정
            if (resultTime.TotalSeconds >= maxTime || resultTime.Days > 0)
            {
                /// 최대치 일때
                reHours = maxTime / 3600;
                reMinutes = maxTime % 3600;
                reSeconds = reMinutes % 60;
                //
                timerText.text = string.Format("{0:00}:{1:00}:{2:00}", reHours, reMinutes, reSeconds);
                /// 실제 지급
                CalOfflineReword(maxTime);
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
    double ltimeBae1 = 0;
    double ltimeBae2 = 0;
    long ltimeBae3 = 0;


    /// <summary>
    /// 최대 거리 보스 HP 계산
    /// 
    /// </summary>
    /// <param name="dtimeBae">이론상 최대거리</param>
    /// <returns></returns>
    double GetLastBossHp(double dtimeBae) => (0.1d * (dtimeBae * dtimeBae) + 0.1d * dtimeBae + 4.5d) * 3d * PlayerInventory.Monster_Boss_HP;

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
        applejack = (long)Math.Truncate(rainbowDash *0.1d);
        applejack *= 10;

        Debug.LogWarning("오프라인 초 : " + _input + " 최대 거리 :  " + rainbowDash  + "m" + " 애플 거리" + applejack  +"m");

        long bossDist = applejack;
        double playerDps = PlayerInventory.Player_DPS * 30.0d;
        double lastBossHp = GetLastBossHp(bossDist);
        // 최대거리 가는 동안 올 보스 킬 가능?
        if (playerDps < lastBossHp)
        {
            /// 최대거리 보스 못 잡았음
            for (long i = bossDist; i > tmpDistance; bossDist -= 10)
            {
                /// 어디까지 잡았니?
                if (playerDps > GetLastBossHp(bossDist))
                {
                    /// 잡은 보스 + 0.9km
                    Debug.LogWarning("플레이어 dps : " + playerDps + " 보스 거리 :  " + bossDist);
                    break;
                }
            }
            /// 보스 거리가 절삭 거리보다 크다면?
            if (bossDist >= applejack)
            {
                PlayerInventory.RecentDistance = Math.Truncate(bossDist + 9.0d);
            }
            /// 원래 거리가 더 크다면
            else
            {
                PlayerInventory.RecentDistance = Math.Truncate(tmpDistance + 9.0d);
            }
        }
        else
        {
            PlayerInventory.RecentDistance = Math.Truncate(rainbowDash);
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
        dtimeBae = (_input * 0.01d) * (3d * 1.15d * rainbowDash);
        dtimeBae *= PlayerInventory.Offline_Earned;
        PlayerInventory.Money_Gold += dtimeBae;
        ///  골드 업적 카운트 올리기
        ListModel.Instance.ALLlist_Update(3, dtimeBae);


        //나뭇잎
        ltimeBae1 = 0;

        /// 보스킬 0일 경우
        if (tmpDistance <= rainbowDash)
        {
            ltimeBae1 += 10.0d * (1.0d + (0.35d * (rainbowDash + 1d)));
        }
        else
        {
            /// 나뭇잎 기본 획득량 공식 (거리 비례)
            for (double i = tmpDistance; i < rainbowDash; i += 10.0d)
            {
                ltimeBae1 += (10.0d * (1.0d + (0.35d * i)));
            }
        }


        Debug.LogWarning("오프라인 이전 거리 : " + tmpDistance + " 보스 잡고 최대거리  :  " + rainbowDash + " 보스전 거리 " + applejack);

        /// 1.0키로 도달 못하면 예외처리
        if (rainbowDash < 10.0d) ltimeBae1 = 0;
        /// _input * 0.01d  = 잡은 몬스터 수 * 0.1
        ltimeBae1 = _input * 0.005d * ltimeBae1 * PlayerInventory.Offline_Earned;
        PlayerInventory.Money_Leaf += ltimeBae1;
        /// 나뭇잎 획득량 업적 올리기
        ListModel.Instance.ALLlist_Update(4, ltimeBae1);

        //강화석
        ltimeBae2 = _input / 600;
        ltimeBae2 = (ltimeBae2 * PlayerInventory.Offline_Earned);
        PlayerInventory.SetTicketCount("reinforce_box", (int)ltimeBae2);

        // 아마존 결정 조각
        ltimeBae3 = (long)Math.Truncate(_input / 500.0f);
        ltimeBae3 = (long)(ltimeBae3 * PlayerInventory.Offline_Earned);
        //PlayerInventory.AmazonStoneCount += ltimeBae3;
        ///// 결정조각  업적  카운트
        //ListModel.Instance.ALLlist_Update(2, ltimeBae3);
        /// 아마존 포션 추가
        PlayerInventory.SetTicketCount("S_leaf_box", (int)ltimeBae3);
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
        ///
        /// 이미 팝업때 1 번 더하고 광고보면 1더 더하는 거니까 1+1 = 2배 맞음 수정 X
        ///
        PlayerInventory.Money_Gold += dtimeBae;
        rewordText[0].text = PlayerPrefsManager.instance.DoubleToStringNumber(dtimeBae * 2.0d);
        ///  골드 업적 카운트 올리기
        ListModel.Instance.ALLlist_Update(3, dtimeBae * 2.0d);

        PlayerInventory.Money_Leaf += ltimeBae1;
        /// 나뭇잎 획득량 업적 올리기
        ListModel.Instance.ALLlist_Update(4, ltimeBae1);
        rewordText[1].text = PlayerPrefsManager.instance.DoubleToStringNumber(ltimeBae1 * 2d);

        PlayerInventory.Money_EnchantStone += ltimeBae2;
        ///  강화석 업적 카운트 올리기
        ListModel.Instance.ALLlist_Update(5, ltimeBae2);
        rewordText[2].text = (ltimeBae2 * 2d).ToString("N0");

        //PlayerInventory.AmazonStoneCount += ltimeBae3;
        ///// 결정조각  업적  카운트
        //ListModel.Instance.ALLlist_Update(2, ltimeBae3);
        /// 아마존 포션 추가
        PlayerInventory.SetTicketCount("S_leaf_box", (int)ltimeBae3);
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
        PlayerPrefsManager.instance.TEST_SaveJson();
        SystemPopUp.instance.LoopLoadingImg();
        Invoke(nameof(InvoStopLoop), 5.0f);

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

    void InvoStopLoop()
    {
        SystemPopUp.instance.StopLoopLoading();
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
        _AdsComp = false;
        PlayerPrefsManager.isGetOfflineReword = true;

        if (PlayerInventory.isSuperUser != 0) return;

        ///  광고 1회 시청 완료 카운트
        ListModel.Instance.ALLlist_Update(0, 1);
        /// 광고 시청 일일 업적
        ListModel.Instance.DAYlist_Update(7);
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
