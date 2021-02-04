using EasyMobile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.Storage;
using Lean.Localization;

public class PopUpManager : MonoBehaviour
{
    [Header("- 글로벌 획득 팝업")]
    public AutoDisposePopUp adp;
    public NanooManager nm;
    public static PopUpManager instance;
    [Header("- 프리다이아몬드 남은시간")]
    public Text FreeDiaRemainBox;
    public GameObject FreeDiaRewordPop;
    [Space]
    public GameObject[] GuardImgs;
    public Text[] guardText;
    [Header("- 팝업 게임 오브젝트 배열")]
    [SerializeField]
    private GameObject[] Pops;
    public GameObject[] grobalPop;
    [Header("- 닉네임 설정 자식 원투")]
    public GameObject[] NickPops;
    public Text nickCheckTxt;




    // 조작 불가 타이머 스탬프
    private DateTime dailyEndTimestamp;
    private TimeSpan dailydRemaining;
    private bool isTimerOn;

    void SaveDateTime(DateTime dateTime)
    {
        string tmp = dateTime.ToString("yyyyMMddHHmmss");
        ObscuredPrefs.SetString("FreeDia", tmp);
        ObscuredPrefs.Save();
    }

    private DateTime LoadDateTime()
    {
        if (!ObscuredPrefs.HasKey("FreeDia"))
        {
            return UnbiasedTime.Instance.Now();
        }

        string data = ObscuredPrefs.GetString("FreeDia");
        return DateTime.ParseExact(data, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
    }
    private void Update()
    {
        if (!isTimerOn) return;

        dailydRemaining = dailyEndTimestamp - UnbiasedTime.Instance.Now();

        /// 24시간 카운트가 계속 돌아가는 상태 = 아직 날짜 안지남. / 출첵도 함.
        if (dailydRemaining.TotalSeconds > 0)
        {
            FreeDiaRemainBox.text = string.Format("{0:00}:{1:00}", dailydRemaining.Minutes, dailydRemaining.Seconds);
        }
        else // 카운트 0미만 이면 하루 지났다 / 아님 출석 안했다.
        {
            GuardImgs[0].gameObject.SetActive(false);

            FreeDiaRemainBox.text = "FREE";
            /// 업데이트 문 탈출
            isTimerOn = false;
        }
    }


    /// <summary>
    /// 무료 타이머 돌려라 돌려
    /// </summary>
    void FreeDiaTimer()
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
            GuardImgs[0].gameObject.SetActive(false);
            // 타이머 숫자 숨겨준다.
            FreeDiaRemainBox.text = "FREE";
        }
    }


    /// <summary>
    /// 팝업 매니저로 관리될 캔버스 스캔
    /// </summary>
    private void Awake()
    {
        instance = this;
        Pops = new GameObject[transform.childCount];
        /// 팝업창 갯수 파악해서 배열에 저장
        for (int i = 0; i < Pops.Length; i++)
        {
            Pops[i] = transform.GetChild(i).gameObject;
        }
        /// 무료 다이아 타이머
        FreeDiaTimer();
    }
    /// <summary>
    /// 알맹이 인덱스 켜주고 팝업 켜줌
    /// </summary>
    /// <param name="_ie"></param>
    public void ShowGrobalPopUP(int _ie)
    {
        for (int i = 0; i < grobalPop.Length; i++)
        {
            grobalPop[i].SetActive(false);
        }
        grobalPop[_ie].SetActive(true);
        ///
        Pops[23].SetActive(true);

        /// 스트링[] 몽땅 저장
        PlayerPrefsManager.instance.TEST_SaveJson();
    }

    /// <summary>
    /// 이미지와 갯수 지정해서 팝업
    /// </summary>
    /// <param name="_ie"></param>
    public void ShowGetPop(int _imgIndex, string _amount)
    {
        adp.iconImg.sprite = adp.itemSprs[_imgIndex];
        adp.itemAmount.text = "x"+ _amount;
        ///
        Pops[25].SetActive(true);

        /// 스트링[] 몽땅 저장
        PlayerPrefsManager.instance.TEST_SaveJson();
    }

    public void ShowPopUP(int _ie)
    {
        if (isTimerOn && _ie == 2) return;
        Pops[_ie].SetActive(true);
    }

    public void HidePopUP(int _ind)
    {
        Pops[_ind].SetActive(false);
    }

    /// <summary>
    /// 1.ErrorPanel 과 2.CheackPanel 를 호출.
    /// </summary>
    /// <param name="_index"></param>
    public void ShowNickPanel(int _index)
    {
        NickPops[_index - 1].SetActive(true);
    }

    public void SetNickCheckTxt(string _name)
    {
        nickCheckTxt.text = "\""+ _name +"\"" + "(으)로 생성하시겠습니까?";
    }

    public void ShowPopUPdiaFree()
    {
        if (isTimerOn || GuardImgs[0].activeSelf)
        {
            return;
        }
        /// 타이머 안돌면  팝업
        ShowPopUP(2);
    }

    #region <Rewarded Ads> 2.FreeDiaCanvas 동영상 광고

    bool _AdsComp;
    public void Ads_FreeDiaCanvas()
    {
        // 하루 10회 제한
        if (PlayerPrefsManager.FreeDiaCnt > 9) return;

        PlayerPrefsManager.instance.TEST_SaveJson();
        SystemPopUp.instance.LoopLoadingImg();
        Invoke(nameof(InvoStopLoop), 5.0f);

        if (Advertising.IsRewardedAdReady(RewardedAdNetwork.AdMob, AdPlacement.Default))
        {
            Advertising.RewardedAdCompleted += AdsCompleated;
            Advertising.RewardedAdSkipped += AdsSkipped;
            Advertising.ShowRewardedAd(RewardedAdNetwork.AdMob, AdPlacement.Default);
        }
        else
        {
            _AdsComp = false;
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
        SaveDateTime( dailyEndTimestamp);
        isTimerOn = true;
        /// 보상 지급
        //nm.PostboxDailySend("diamond", 100);
        nm.CouponCheak("diamond", "100");
        ///  광고 1회 시청 완료 카운트
        ListModel.Instance.ALLlist_Update(0, 1);
        /// 광고 시청 일일 업적
        ListModel.Instance.DAYlist_Update(7);
        _AdsComp = false;
        /// 다이아 획득 팝업
        PlayerPrefsManager.FreeDiaCnt++;
        FreeDiaRewordPop.SetActive(true);
    }

    #endregion

    



}
