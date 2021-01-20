using EasyMobile;
using Lean.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.Storage;

public class StayRewordManager : MonoBehaviour
{
    public Text textStaytext;
    [Header("-1시간 이면 3600")]
    public float delayTime = 3600f;
    public NanooManager nm;
    [Header("-공통 부분")]
    public Sprite[] mainSprs;

    [Header("-메인 팝업 부분.1")]
    public Image iconMainImg;
    public Text[] mainAmount;

    [Header("-획득 팝업 부분")]
    public Image AdsIcon;
    public Sprite[] AdsSprite;
    public GameObject getPopUp;
    public Image iconImg;
    public Text getItemAmount;
    public Text get2XText;

    private bool isGetMainItem;
    private bool isSecondChan;

    Coroutine StayRm;


    public void TEST_RewordCoolTime()
    {
        stayTime = 0.1f;
    }

    /// <summary>
    /// 데일리 매니저에서 호출해서 12시 지나면 초기화.
    /// </summary>
    public void ResetRewordStack()
    {
        ObscuredPrefs.SetInt("rewordCharge", 0);
        /// 레드닷 꺼줌
        RedDotManager.instance.RedDot[0].SetActive(false);
        /// 새로 코루틴 돌리기
        if (StayRm != null)
        {
            StopCoroutine(OnTimePass());
        }
        StayRm = StartCoroutine(OnTimePass());
    }

    private void Start()
    {
        textStaytext.text = LeanLocalization.GetTranslationText("Side_Left_StayReword");
        StayRm = StartCoroutine(OnTimePass());
    }
    float stayTime = 0;
    void CheckTime()
    {
        // 3600 초 = 1시간 대기후 호출 갱신
        StartCoroutine(OnTimePass());
    }
    /// <summary>
    /// 대기 시간 후 스택 쌓기 -> 쌓기 이후에 보상 받을 때 까지 대기
    /// </summary>
    IEnumerator OnTimePass()
    {
        yield return null;

        stayTime = delayTime;
        for (;;)
        {
            yield return new WaitForFixedUpdate();
            stayTime -= Time.deltaTime;
            textStaytext.text = string.Format("{0:00}:{1:00}", (stayTime/60.0f), (stayTime % 60.0f));
            // 탈출
            if (stayTime < 0) break;
        }
        /// 빨간 점 띄우기
        RedDotManager.instance.RedDot[0].SetActive(true);
        textStaytext.text = LeanLocalization.GetTranslationText("Side_Left_StayReword");
        /// 플레이팹에 상태 저장
        PlayerPrefsManager.instance.JObjectSave(true);
    }

    /// <summary>
    ///  접속 보상 버튼에 달아서 팝업 호출 // 안 받기 있음.
    /// </summary>
    public void ShowSRpopUp()
    {
        /// 빨간 점 안 들어와있으면 리턴
        if (!RedDotManager.instance.RedDot[0].activeSelf)
        {
            return;
        }
        /// 5개 보상 다 받으면 0으로 초기화.
        if (ObscuredPrefs.GetInt("rewordCharge", 0) > 4)
        {
            ObscuredPrefs.SetInt("rewordCharge", 0);
        }
        /// 빨간 점 표시시 팝업 표기
        switch (ObscuredPrefs.GetInt("rewordCharge", 0))
        {
            case 0: iconMainImg.sprite = mainSprs[0]; break;
            case 1: iconMainImg.sprite = mainSprs[1]; break;
            case 2: iconMainImg.sprite = mainSprs[2]; break;
            case 3: iconMainImg.sprite = mainSprs[3]; break;
            case 4: iconMainImg.sprite = mainSprs[4]; break;
            //case 5: iconMainImg.sprite = mainSprs[5]; break;
            default: return;
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
        /// 내용물 다 채우면 팝업
        PopUpManager.instance.ShowPopUP(1);
    }



    /// <summary>
    ///  오른쪽  [받기] 클릭시 호출
    /// </summary>
    public void ClickedFirst()
    {
        _AdsComp = false;
        /// 두번째 보상 트리거 초기화.
        isGetMainItem = false;
        isSecondChan = false;
        /// 획득 팝업 부분 -> 인덱스 6은 아마존 결정 고정
        iconImg.sprite = mainSprs[ObscuredPrefs.GetInt("rewordCharge", 0)];
        getItemAmount.text = "x1";
        /// 미니 팝업  show
        getPopUp.SetActive(true);
        /// 해당 스택 보상 지급
        get2XText.text = LeanLocalization.GetTranslationText("Config_Gift_Desc");
        GetRealPresent();
    }

    void GetRealPresent()
    {
        /// 5개 보상 다 받으면 0으로 초기화.
        if (ObscuredPrefs.GetInt("rewordCharge", 0) > 4)
        {
            ObscuredPrefs.SetInt("rewordCharge", 0);
        }
        int reInt = ObscuredPrefs.GetInt("rewordCharge", 0);
        switch (reInt)
        {
            //case 0: nm.PostboxItemSend("E_box", 1, ""); break;
            //case 1: nm.PostboxItemSend("D_box", 1, ""); break;
            //case 2: nm.PostboxItemSend("C_box", 1, ""); break;
            //case 3: nm.PostboxItemSend("B_box", 1, ""); break;
            //case 4: nm.PostboxItemSend("A_box", 1, ""); break;

            case 0: nm.CouponCheak("E_box", "1"); break;
            case 1: nm.CouponCheak("D_box", "1"); break;
            case 2: nm.CouponCheak("C_box", "1"); break;
            case 3: nm.CouponCheak("B_box", "1"); break;
            case 4: nm.CouponCheak("A_box", "1"); break;


            default: return;
        }
        /// 보상 받았으면 접속 보상 +1스택
        ObscuredPrefs.SetInt("rewordCharge", ++reInt);
        /// 보상 받았으면 다시 코루틴 돌려주기
        CheckTime();
        /// 레드닷 꺼줌
        RedDotManager.instance.RedDot[0].SetActive(false);
    }
    /// <summary>
    /// 두번째 팝업 외부에서 클릭
    /// </summary>
    public void ClickedSecond()
    {
        /// 두번째 아이템까지 받았으면 꺼져
        if (isGetMainItem)
        {
            getPopUp.SetActive(false);
            PopUpManager.instance.HidePopUP(1);
        }
        /// 미니팝업 첫번째 받기 -> 누르면 아마존 조각으로 바꿔줌
        else
        {
            iconImg.sprite = mainSprs[6];
            /// 일반 받기
            if (!_AdsComp)
            {
                getItemAmount.text = "x10";
                //nm.PostboxItemSend("crystal", 10, "");
                nm.CouponCheak("crystal", "10");
            }
            /// 광고 보았나?
            else
            {
                getItemAmount.text = "x20";
                //nm.PostboxItemSend("crystal", 20, "");
                nm.CouponCheak("crystal", "20");
            }

            /// 보상 더 남았나?
            isGetMainItem = true;
        }
    }


    #region <Rewarded Ads> 접속 보상 1시간 마다

    /// <summary>
    /// 실제 광고를 봤다면 true
    /// </summary>
    bool _AdsComp;
    /// <summary>
    /// 2배 받기 버튼에 붙어
    /// </summary>
    public void Ads_FreeDiaCanvas()
    {
        PlayerPrefsManager.instance.TEST_SaveJson();
        SystemPopUp.instance.LoopLoadingImg();
        Invoke(nameof(InvoStopLoop), 5.0f);

        if (PlayerInventory.isSuperUser != 0)
        {
            /// 2배 적용 ㅇㅋ
            _AdsComp = true;
            //get2XText.text = LeanLocalization.GetTranslationText("Config_Ads_Okay");
            Invoke(nameof(AdsInvo), 0.5f);
            return;
        }

        if (Advertising.IsRewardedAdReady(RewardedAdNetwork.MoPub, AdPlacement.Default))
        {
            Advertising.RewardedAdCompleted += AdsCompleated;
            Advertising.RewardedAdSkipped += AdsSkipped;
            Advertising.ShowRewardedAd(RewardedAdNetwork.MoPub, AdPlacement.Default);
        }
        else
        {
            /// 광고 없어도 ㅇㅋ
            _AdsComp = false;
            //get2XText.text = LeanLocalization.GetTranslationText("Config_Ads_Nope");
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
        //get2XText.text = LeanLocalization.GetTranslationText("Config_Ads_Okay");
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
        /// 보상 지급
        AdsClickedFirst();

        if (PlayerInventory.isSuperUser != 0) return;

        ///  광고 1회 시청 완료 카운트
        ListModel.Instance.ALLlist_Update(0, 1);
        /// 광고 시청 일일 업적
        ListModel.Instance.DAYlist_Update(7);
    }

    #endregion

    /// <summary>
    ///  광고 보면 이거 호출
    /// </summary>
    void AdsClickedFirst()
    {
        /// 5개 보상 다 받으면 0으로 초기화.
        if (ObscuredPrefs.GetInt("rewordCharge", 0) > 4)
        {
            ObscuredPrefs.SetInt("rewordCharge", 0);
        }
        int reInt = ObscuredPrefs.GetInt("rewordCharge", 0);
        isGetMainItem = false;
        isSecondChan = false;
        /// 획득 팝업 부분 -> 인덱스 6은 아마존 결정 고정
        iconImg.sprite = mainSprs[reInt];
        getItemAmount.text = "x2";
        /// 미니 팝업
        getPopUp.SetActive(true);
        /// 해당 스택 보상 지급
        switch (reInt)
        {
            //case 0: nm.PostboxItemSend("E_box", 2, ""); break;
            //case 1: nm.PostboxItemSend("D_box", 2, ""); break;
            //case 2: nm.PostboxItemSend("C_box", 2, ""); break;
            //case 3: nm.PostboxItemSend("B_box", 2, ""); break;
            //case 4: nm.PostboxItemSend("A_box", 2, ""); break;

            case 0: nm.CouponCheak("E_box", "2"); break;
            case 1: nm.CouponCheak("D_box", "2"); break;
            case 2: nm.CouponCheak("C_box", "2"); break;
            case 3: nm.CouponCheak("B_box", "2"); break;
            case 4: nm.CouponCheak("A_box", "2"); break;

            default: return;
        }
        /// 보상 받았으면 접속 보상 +1스택
        ObscuredPrefs.SetInt("rewordCharge", ++reInt);        
        /// 보상 받았으면 다시 코루틴 돌려주기
        CheckTime();
        /// 레드닷 꺼줌
        RedDotManager.instance.RedDot[0].SetActive(false);
    }




}
