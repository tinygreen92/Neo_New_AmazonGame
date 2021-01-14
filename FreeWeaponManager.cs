using CodeStage.AntiCheat.Storage;
using EasyMobile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreeWeaponManager : MonoBehaviour
{
    public NanooManager nm;
    public Text TimerText;
    public GameObject FreeDiaRewordPop;
    [Space]
    public GameObject[] GuardImgs;
    public Text[] guardText;
    // 조작 불가 타이머 스탬프
    private DateTime dailyEndTimestamp;
    private TimeSpan dailydRemaining;
    private bool isTimerOn;

    void SaveDateTime(DateTime dateTime)
    {
        string tmp = dateTime.ToString("yyyyMMddHHmmss");
        ObscuredPrefs.SetString("FreeWeapon", tmp);
        ObscuredPrefs.Save();
    }

    private DateTime LoadDateTime()
    {
        if (!ObscuredPrefs.HasKey("FreeWeapon"))
        {
            return UnbiasedTime.Instance.Now();
        }

        string data = ObscuredPrefs.GetString("FreeWeapon");
        return DateTime.ParseExact(data, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
    }
    private void Update()
    {
        if (!isTimerOn) return;

        dailydRemaining = dailyEndTimestamp - UnbiasedTime.Instance.Now();

        ///카운트가 계속 돌아가는 상태
        if (dailydRemaining.TotalSeconds > 0)
        {
            TimerText.text = string.Format("{0:00}:{1:00}", dailydRemaining.Minutes, dailydRemaining.Seconds);
        }
        else // 카운트 0미만 이면 하루 지났다 / 아님 출석 안했다.
        {
            TimerText.text = "FREE";
            /// 업데이트 문 탈출
            isTimerOn = false;
        }
    }

    public void ShowPopUP()
    {
        if (isTimerOn || GuardImgs[1].activeSelf)
        {
            return;
        }
        /// 타이머 안돌면  팝업
        PopUpManager.instance.ShowPopUP(27);
    }

    /// <summary>
    /// 무료 타이머 돌려라 돌려
    /// </summary>
    void FreeWeaponTimer()
    {
        dailyEndTimestamp = LoadDateTime();
        dailydRemaining = dailyEndTimestamp - UnbiasedTime.Instance.Now();

        // 타이머 시간이 남아있다?
        if (dailydRemaining.TotalSeconds > 0)
        {
            // 타이머 실행
            isTimerOn = true;
        }
        else /// 타이머 끝남.
        {
            // 타이머 숫자 숨겨준다.
            TimerText.text = "FREE";
        }
    }

    private void Awake()
    {
        /// 무료 타이머
        FreeWeaponTimer();
    }










    #region <Rewarded Ads> 무료 무기 뽑기 광고

    /// <summary>
    /// 실제 광고를 봤다면 true
    /// </summary>
    bool _AdsComp;
    /// <summary>
    /// 클릭해서 웨폰 뽑기 들어가는 버튼
    /// </summary>
    public void Ads_FreeWeaponBtnClicked()
    {
        if (PlayerPrefsManager.FreeWeaponCnt > 9) return;

        PlayerPrefsManager.instance.TEST_SaveJson();
        SystemPopUp.instance.LoopLoadingImg();
        Invoke(nameof(InvoStopLoop), 5.0f);

        if (Advertising.IsRewardedAdReady(RewardedAdNetwork.MoPub, AdPlacement.Default))
        {
            Advertising.RewardedAdCompleted += AdsCompleated;
            Advertising.RewardedAdSkipped += AdsSkipped;
            Advertising.ShowRewardedAd(RewardedAdNetwork.MoPub, AdPlacement.Default);
        }
        else
        {
            _AdsComp = false;
            /// 광고 없어도 ㅇㅋ
            //AdsDesc.text = LeanLocalization.GetTranslationText("Config_Ads_Nope");
            Invoke(nameof(AdsInvo), 0.5f);
        }

    }

    void InvoStopLoop()
    {
        SystemPopUp.instance.StopLoopLoading();
    }

    // Event handler called when a rewarded ad has completed
    void AdsCompleated(RewardedAdNetwork network, AdPlacement location)
    {
        _AdsComp = true;
        Invoke(nameof(AdsInvo), 0.5f);
        Advertising.RewardedAdCompleted -= AdsCompleated;
        Advertising.RewardedAdSkipped -= AdsSkipped;
    }

    // Event handler called when a rewarded ad has been skipped
    void AdsSkipped(RewardedAdNetwork network, AdPlacement location)
    {
        Advertising.RewardedAdCompleted -= AdsCompleated;
        Advertising.RewardedAdSkipped -= AdsSkipped;
        SystemPopUp.instance.StopLoopLoading();
    }
    /// <summary>
    /// 보상 지급 메소드
    /// </summary>
    void AdsInvo()
    {
        SystemPopUp.instance.StopLoopLoading();
        // 타이머 저장
        dailyEndTimestamp = UnbiasedTime.Instance.Now().AddSeconds(1800);
        SaveDateTime(dailyEndTimestamp);
        // 트리거 초히과
        isTimerOn = true;
        _AdsComp = false;
        ///  광고 1회 시청 완료 카운트
        ListModel.Instance.ALLlist_Update(0, 1);
        /// 광고 시청 일일 업적
        ListModel.Instance.DAYlist_Update(7);
        /// 우편에 무기 뽑기권 지급
        PlayerPrefsManager.FreeWeaponCnt++;
        nm.PostboxDailySend("weapon_coupon", 3);
        /// 보상 팝업
        FreeDiaRewordPop.SetActive(true);
    }

    /// <summary>
    /// 무료 다이아 / 무료 뽑기 / 무료 포션 받기 하면 20초 대기.
    /// </summary>
    public void AdsHolding20s()
    {
        GuardImgs[0].SetActive(true);
        GuardImgs[1].SetActive(true);
        //
        StartCoroutine(hold20());
    }

    IEnumerator hold20()
    {
        yield return null;
        int currntTime = 15;
        WaitForSeconds delay = new WaitForSeconds(1f);
        guardText[0].text = currntTime.ToString();
        guardText[1].text = currntTime.ToString();
        while (currntTime > 1)
        {
            yield return delay;
            currntTime -= 1;
            guardText[0].text = currntTime.ToString();
            guardText[1].text = currntTime.ToString();
        }

        GuardImgs[0].SetActive(false);
        GuardImgs[1].SetActive(false);
    }


    #endregion

}
