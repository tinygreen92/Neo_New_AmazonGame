using EasyMobile;
using Lean.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.Storage;

public class AmazonShopManager : MonoBehaviour
{
    [Header("- 갯수 조정 팝업 매니저")]
    public CountBuyManager cbm;
    [Header("-나누 랑 룬")]
    public NanooManager nm;
    public RuneManager rm;
    [Header( "아이템 스프라이트")]
    public Sprite[] Icons;
    public Sprite[] BtnsSprs;
    [Header("알림 정보")]
    public GameObject giftBoxPop;
    public Image iconImg;
    public Text iconDesc;
    [Header("탑 뷰 클릭 관련")]
    public Image[] topBtn;
    public Sprite[] topBtnSpr;
    public GameObject[] MiddleEarth;
    [Header("아마존 결정 내부 내용")]
    public Image[] AcceptBtn;
    public Sprite[] AcceptSpr;
    public Text InnerSlideText;
    public Text ZogarkText;
    public Text TimerText;
    public Slider InnerSlide;
    [Header("광고 시청하고 결정 먹기 팝업")]
    public Text AdsAmount;
    //public Text AdsDesc;

    private static int Zogark_LV;
    private static string Zogark_STR;
    private static string HEAD_STR = " ( ";
    private static string MID_STR = " / ";
    private static string TAIL_STR = " )";


    // 조작 불가 타이머 스탬프
    private DateTime dailyEndTimestamp;
    private TimeSpan dailydRemaining;
    private bool isTimerOn;

    void SaveDateTime(DateTime dateTime)
    {
        string tmp = dateTime.ToString("yyyyMMddHHmmss");
        ObscuredPrefs.SetString("AmazonShop", tmp);
        ObscuredPrefs.Save();
    }

    private DateTime LoadDateTime()
    {
        if (!ObscuredPrefs.HasKey("AmazonShop"))
        {
            return UnbiasedTime.Instance.Now();
        }

        string data = ObscuredPrefs.GetString("AmazonShop");
        return DateTime.ParseExact(data, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
    }
    private void Update()
    {
        if (!isTimerOn) return;

        dailydRemaining = dailyEndTimestamp - UnbiasedTime.Instance.Now();

        ///카운트가 계속 돌아가는 상태
        if (dailydRemaining.TotalSeconds > 0)
        {
            TimerText.text = string.Format("{0:00}:{1:00}:{2:00}", dailydRemaining.Hours, dailydRemaining.Minutes, dailydRemaining.Seconds);
        }
        else // 카운트 0미만 이면 하루 지났다 / 아님 출석 안했다.
        {
            /// 활성화 블루
            AcceptBtn[1].sprite = AcceptSpr[1];
            TimerText.text = LeanLocalization.GetTranslationText("BtnText_NormalAccept");
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
            /// 활성화 블루
            AcceptBtn[1].sprite = AcceptSpr[1];
            // 타이머 숫자 숨겨준다.
            TimerText.text = LeanLocalization.GetTranslationText("BtnText_NormalAccept");
        }
    }


    public void ShowAMAMA()
    {
        /// 타이머 돌리기
        FreeDiaTimer();
        /// 현재 아마존 결정 레벨
        Zogark_LV = PlayerInventory.CurrentAmaLV;
        /// 아마존 결정 맥스 카운터 += 5 단위로 상승
        if (PlayerPrefsManager.ZogarkMissionCnt == 0)
        {
            PlayerPrefsManager.ZogarkMissionCnt += 5;
        }
        /// 아마존 결정 레벨 불러오기
        Zogark_STR = LeanLocalization.GetTranslationText("AMA_Free_Front")
            + Zogark_LV + MID_STR
            + PlayerPrefsManager.ZogarkMissionCnt + TAIL_STR;
        /// 달성 보상 수령 가능 트리거
        if (Zogark_LV >= PlayerPrefsManager.ZogarkMissionCnt)
        {
            isGetTime = true;
        }
        /// 창 켜지면 [아마존 결정] 탭으로 가야한다
        TopViewClicked(true);
    }

    /// <summary>
    /// 5레벨이 되어서 달성 보상 받을 수 있어짐.
    /// </summary>
    bool isGetTime;
    void RefreshSlide()
    {
        /// 슬라이더 텍스트
        InnerSlideText.text = HEAD_STR + Zogark_LV + MID_STR + PlayerPrefsManager.ZogarkMissionCnt + TAIL_STR;
        /// 슬라이더 움직임
        if (Zogark_LV > 0 && Zogark_LV % 5 == 0)
        {
            if (isGetTime)
            {
                InnerSlide.value = 5;
                AcceptBtn[0].sprite = AcceptSpr[1];         /// 활성화 블루
            }
            else
            {
                InnerSlide.value = 0;
                AcceptBtn[0].sprite = AcceptSpr[0];         /// 비활성화 그레이
            }

        }
        else if(Zogark_LV > PlayerPrefsManager.ZogarkMissionCnt)
        {
            InnerSlide.value = 5;
            AcceptBtn[0].sprite = AcceptSpr[1];         /// 활성화 블루
        }
        else
        {
            InnerSlide.value = Zogark_LV % 5;
            AcceptBtn[0].sprite = AcceptSpr[0];         /// 비활성화 그레이
        }
    }

    void RefreshTimer()
    {
        if (PlayerPrefsManager.AmaAdsTimer < 10)
        {
            /// 타이머 돌면
            if (isTimerOn)
            {
                AcceptBtn[1].sprite = AcceptSpr[0];         /// 비활성화 그레이
            }
            else
            {
                AcceptBtn[1].sprite = AcceptSpr[1];         /// 활성화 블루
            }
            /// 광고 텍스트 10회 제한 텍스트 갱신
            ZogarkText.text = LeanLocalization.GetTranslationText("AMA_Free_Front") + HEAD_STR
                + PlayerPrefsManager.AmaAdsTimer.ToString("D2") + MID_STR
                + "10" + TAIL_STR;
        }
        else
        {
            ZogarkText.text = LeanLocalization.GetTranslationText("AMA_Free_Front") + "( 10 / 10 )";
            AcceptBtn[1].sprite = AcceptSpr[0];         /// 비활성화 그레이
        }
    }

    /// <summary>
    /// 아마존 샵 탑 뷰 누르면?
    /// </summary>
    public void TopViewClicked(bool _IsLeft)
    {
        /// 체크한건 왼쪽
        if (_IsLeft)
        {
            /// 슬라이더 갱신
            RefreshSlide();
            /// 타이머 텍스트 갱신
            RefreshTimer();

            topBtn[0].sprite = topBtnSpr[1];
            topBtn[1].sprite = topBtnSpr[0];
            //
            MiddleEarth[0].SetActive(true);
            MiddleEarth[1].SetActive(false);
        }
        else
        {
            topBtn[0].sprite = topBtnSpr[0];
            topBtn[1].sprite = topBtnSpr[1];
            //
            MiddleEarth[0].SetActive(false);
            MiddleEarth[1].SetActive(true);
        }
        /// 본 화면 팝업
        PopUpManager.instance.ShowPopUP(14);
    }




    /// <summary>
    /// 룬 상점에서 아이템 구입시 우편함으로 들어가
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_Amount"></param>
    public void SetGiftBoxDesc(int _index, int _Amount)
    {
        iconImg.sprite = Icons[_index];
        iconDesc.text = "x" + _Amount.ToString("N0");
        ///
        switch (_index)
        {
            //case 0: nm.PostboxDailySend("L_box", 1); break;
            //case 1: nm.PostboxDailySend("S_box", 1); break;.

            case 0: nm.CouponCheak("L_box", _Amount.ToString()) ; break;
            case 1: nm.CouponCheak("S_box", _Amount.ToString()); break;

            //case 2: nm.PostboxDailySend("Crazy_dia", 1); break;
            //case 3: nm.PostboxDailySend("S_reinforce_box", 1); break;
            //case 4: nm.PostboxDailySend("Crazy_elixr", 1); break;
            //case 5: nm.PostboxDailySend("S_leaf_box", 1); break;

            /// 룬 뽑기 실행하면 리턴
            case 6:
                /// 아마존 상점에서 룬 구매하기시 튜토리얼 통과
                if (PlayerPrefsManager.currentTutoIndex == 32) 
                    ListModel.Instance.TUTO_Update(32);
                /// 룬가챠
                rm.GatchaRune(); 
                gameObject.SetActive(false); 

                return;
            default: return;
        }
        /// 
        giftBoxPop.SetActive(true);
    }

    /// <summary>
    /// 위쪽에 붙는거 [받기] 버튼
    /// </summary>
    public void ClickedHEADAccept()
    {
        /// 그레이색이면 리턴
        if (AcceptBtn[0].sprite == AcceptSpr[0]) return;
        /// 아마존 레벨 달성 보상
        PlayerInventory.Money_AmazonCoin += PlayerPrefsManager.ZogarkMissionCnt;
        /// 아마존 결정 조각 획득 팝업
        PopUpManager.instance.ShowGetPop(10, PlayerPrefsManager.ZogarkMissionCnt.ToString());
        /// 미션 카운터 늘려줌
        PlayerPrefsManager.ZogarkMissionCnt += 5;
        /// 새로고침
        isGetTime = false;
        TopViewClicked(true);
    }

    /// <summary>
    /// 아래쪽에 붙는거 [받기] 버튼 -> 동영상 광고 팝업
    /// </summary>
    public void ClickedTAILAccept()
    {
        /// 그레이색이면 리턴 || 결정 카운터 9 이상이면 리턴
        if (AcceptBtn[1].sprite == AcceptSpr[0] || PlayerPrefsManager.AmaAdsTimer > 9) return;
        /// 팝업 내용 채우기
        AdsAmount.text = "x" + (10 * (PlayerPrefsManager.AmaAdsTimer+1)).ToString();
        /// 프리 아마존 팝업
        PopUpManager.instance.ShowPopUP(26);
    }







    #region <Rewarded Ads> 아마존 결정 상점 동영상 광고

    /// <summary>
    /// 실제 광고를 봤다면 true
    /// </summary>
    bool _AdsComp;
    public void Ads_FreeDiaCanvas()
    {
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
            /// 광고 없으면 안돼
            //Invoke(nameof(AdsInvo), 0.5f);
            SystemPopUp.instance.StopLoopLoading();
            /// 프리 아마존 팝업 꺼주기
            PopUpManager.instance.HidePopUP(26);
            /// 15초 타이머
            PopUpManager.instance.fwm.AdsHolding20s();
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
        /// 광고 두배보상적용 ㅇㅋ
        //AdsDesc.text = LeanLocalization.GetTranslationText("Config_Ads_Okay");
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
        ///
        int tmpReword = 10 * ++PlayerPrefsManager.AmaAdsTimer;
        // 타이머 저장
        dailyEndTimestamp = UnbiasedTime.Instance.Now().AddSeconds(600);
        SaveDateTime(dailyEndTimestamp);
        isTimerOn = true;
        ///  광고 1회 시청 완료 카운트
        ListModel.Instance.ALLlist_Update(0, 1);
        /// 광고 시청 일일 업적
        ListModel.Instance.DAYlist_Update(7);
        /// 아마존 포션 추가
        nm.CouponCheak("S_leaf_box", tmpReword.ToString());
        _AdsComp = false;
        /// 결정 조각 팝업 
        PopUpManager.instance.ShowGetPop(10, tmpReword.ToString());
        /// 팝업 꺼주기
        TopViewClicked(true);
        PopUpManager.instance.HidePopUP(26);
    }

    #endregion

}
