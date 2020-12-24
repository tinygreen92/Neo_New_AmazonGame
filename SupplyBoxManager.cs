using DG.Tweening;
using EasyMobile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupplyBoxManager : MonoBehaviour
{
    public float supplyDelay;
    [Header("- 좌표")]
    public Transform StartPos;
    public Transform TargetPos;
    [Header("- 프리팹 붙여줄 것")]
    public Transform Normal;
    public Transform Super;
    [Header("-메인 팝업 부분.1")]
    public Image AdsIcon;
    public Sprite[] AdsSprite;
    public Image iconMainImg;
    public Sprite[] iconSpr;
    public Text mainAmount;

    [Header("-획득 팝업 부분")]
    public GameObject getPopUp;
    public Image iconImg;
    public Text getItemAmount;

    private Animator anim;
    private int superCont;
    /// <summary>
    /// 뽑은 확률
    /// </summary>
    private float returnValue;
    private double tmpGold;
    private bool isSuperBox;


    /// <summary>
    /// 보급상자 획득하기 미션 나오면 5초 후에 임시로 내리기
    /// </summary>
    public void MissionSupplySex()
    {
        Invoke(nameof(CreateNormalBox), 5.0f);
    }



    #region <Rewarded Ads> 보급상자 동영상 광고

    /// <summary>
    /// 실제 광고를 봤다면 true
    /// </summary>
    bool _AdsComp;
    void Ads_FreeDiaCanvas()
    {
        SystemPopUp.instance.LoopLoadingImg();

        if (PlayerInventory.isSuperUser != 0)
        {
            /// 광고 스킵
            _AdsComp = true;
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
            _AdsComp = false;
            Invoke(nameof(AdsInvo), 0.5f);
        }

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
        ///  광고 1회 시청 완료 카운트
        ListModel.Instance.ALLlist_Update(0, 1);
        /// 광고 시청 일일 업적
        ListModel.Instance.DAYlist_Update(7);
        /// 10배 보급 상자인가?
        if (isSuperBox) AcceptSuperBox(1);
        else AcceptNormalBox(1);
        _AdsComp = false;
    }

    #endregion



    /// <summary>
    /// 공통 메인 팝업에 붙일 것
    /// </summary>
    public void EasySunDa(int _moon)
    {
        /// 동영상
        if (_moon == 0)
        {
            Ads_FreeDiaCanvas();
        }
        /// 일반
        else
        {
            if (isSuperBox) AcceptSuperBox(0);
            else AcceptNormalBox(0);
        }
    }


    public void ClickedNormalBox()
    {
        /// 보급 상자 획득하기
        if (PlayerPrefsManager.currentTutoIndex == 25) ListModel.Instance.TUTO_Update(25);
        ///  업적  완료 카운트
        ListModel.Instance.ALLlist_Update(9, 1);
        //
        isSuperBox = false;
        tmpGold = 5d * (3d * 1.15d * PlayerInventory.RecentDistance);
        Normal.gameObject.SetActive(false);
        System.Random seedRnd = new System.Random();
        int startIndex = seedRnd.Next();
        float[] probs_L = new float[] { 70f, 15f, 10f, 5f };
        returnValue = PlayerPrefsManager.instance.GetRandom(probs_L, startIndex);
        switch (returnValue)
        {
            case 70f: iconMainImg.sprite = iconSpr[0];
                mainAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(tmpGold);
                break;
            case 15f: iconMainImg.sprite = iconSpr[1];
                mainAmount.text = "x50";
                break;
            case 10f: iconMainImg.sprite = iconSpr[2]; 
                mainAmount.text = "x5";
                break;
            case 5f: iconMainImg.sprite = iconSpr[3];
                mainAmount.text = "x3";
                break;
            default: iconMainImg.sprite = iconSpr[0]; 
                break;
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
        //팝업
        PopUpManager.instance.ShowPopUP(19);
        /// supplyIdle
         anim.Play("supplyIdle", -1, 0f);
    }
    void AcceptNormalBox(int _iSwitch)
    {
        /// 보상 1배
        if (_iSwitch == 0)
        {
            switch (returnValue)
            {
                case 70f: iconImg.sprite = iconSpr[0]; 
                    PlayerInventory.Money_Gold += tmpGold;
                    ///  골드 업적 카운트 올리기
                    ListModel.Instance.ALLlist_Update(3, tmpGold);
                    getItemAmount.text =  "x"+ PlayerPrefsManager.instance.DoubleToStringNumber(tmpGold);
                    break;
                case 15f: iconImg.sprite = iconSpr[1]; 
                    PlayerInventory.Money_Dia += 50;
                    getItemAmount.text = "x50";
                    break;
                case 10f: iconImg.sprite = iconSpr[2]; 
                    PlayerInventory.Money_Elixir += 5;
                    getItemAmount.text = "x5";
                    break;
                case 5f: iconImg.sprite = iconSpr[3]; 
                    PlayerInventory.Money_AmazonCoin += 3; 
                    getItemAmount.text = "x3";
                    break;
                default: iconImg.sprite = iconSpr[0]; break;
            }

            getPopUp.SetActive(true);
        }
        /// 동영상 보고 보상 2배
        else
        {
            switch (returnValue)
            {
                case 70f:
                    iconImg.sprite = iconSpr[0];
                    PlayerInventory.Money_Gold += tmpGold * 2.0d;
                    ///  골드 업적 카운트 올리기
                    ListModel.Instance.ALLlist_Update(3, tmpGold * 2.0d);
                    getItemAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(tmpGold *2.0d);
                    break;
                case 15f:
                    iconImg.sprite = iconSpr[1];
                    PlayerInventory.Money_Dia += 100;
                    getItemAmount.text = "x100";
                    break;
                case 10f:
                    iconImg.sprite = iconSpr[2];
                    PlayerInventory.Money_Elixir += 10;
                    getItemAmount.text = "x10";
                    break;
                case 5f:
                    iconImg.sprite = iconSpr[3];
                    PlayerInventory.Money_AmazonCoin += 6;
                    getItemAmount.text = "x6";
                    break;
                default: iconImg.sprite = iconSpr[0]; break;
            }

            getPopUp.SetActive(true);
        }
    }
    


    public void ClickedSuperBox()
    {
        /// 보급 상자 획득하기
        if (PlayerPrefsManager.currentTutoIndex == 25) ListModel.Instance.TUTO_Update(25);
        ///  업적  완료 카운트
        ListModel.Instance.ALLlist_Update(9, 1);
        //
        isSuperBox = true;
        tmpGold = 10d * (3d * 1.15d * PlayerInventory.RecentDistance);
        Super.gameObject.SetActive(false);
        System.Random seedRnd = new System.Random();
        int startIndex = seedRnd.Next();
        float[] probs_L = new float[] { 70f, 15f, 10f, 5f };
        returnValue = PlayerPrefsManager.instance.GetRandom(probs_L, startIndex);
        switch (returnValue)
        {
            case 70f:
                iconMainImg.sprite = iconSpr[0];
                mainAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(tmpGold);
                break;
            case 15f:
                iconMainImg.sprite = iconSpr[1];
                mainAmount.text = "x100";
                break;
            case 10f:
                iconMainImg.sprite = iconSpr[2];
                mainAmount.text = "x10";
                break;
            case 5f:
                iconMainImg.sprite = iconSpr[3];
                mainAmount.text = "x5";
                break;
            default:
                iconMainImg.sprite = iconSpr[0];
                break;
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
        PopUpManager.instance.ShowPopUP(19);
        /// supplyIdle
        anim.Play("supplyIdle", -1, 0f);
    }
    void AcceptSuperBox(int _iSwitch)
    {
        /// 보상 1배
        if (_iSwitch == 0)
        {
            switch (returnValue)
            {
                case 70f:
                    iconImg.sprite = iconSpr[0];
                    PlayerInventory.Money_Gold += tmpGold;
                    ///  골드 업적 카운트 올리기
                    ListModel.Instance.ALLlist_Update(3, tmpGold);
                    getItemAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(tmpGold);
                    break;
                case 15f:
                    iconImg.sprite = iconSpr[1];
                    PlayerInventory.Money_Dia += 100;
                    getItemAmount.text = "x100";
                    break;
                case 10f:
                    iconImg.sprite = iconSpr[2];
                    PlayerInventory.Money_Elixir += 10;
                    getItemAmount.text = "x10";
                    break;
                case 5f:
                    iconImg.sprite = iconSpr[3];
                    PlayerInventory.Money_AmazonCoin += 5;
                    getItemAmount.text = "x5";
                    break;
                default: iconImg.sprite = iconSpr[0]; break;
            }

            getPopUp.SetActive(true);
        }
        /// 동영상 보고 보상 2배
        else
        {
            switch (returnValue)
            {
                case 70f:
                    iconImg.sprite = iconSpr[0];
                    PlayerInventory.Money_Gold += tmpGold * 2.0d;
                    ///  골드 업적 카운트 올리기
                    ListModel.Instance.ALLlist_Update(3, tmpGold*2.0d);
                    getItemAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(tmpGold * 2.0d);
                    break;
                case 15f:
                    iconImg.sprite = iconSpr[1];
                    PlayerInventory.Money_Dia += 200;
                    getItemAmount.text = "x200";
                    break;
                case 10f:
                    iconImg.sprite = iconSpr[2];
                    PlayerInventory.Money_Elixir += 20;
                    getItemAmount.text = "x20";
                    break;
                case 5f:
                    iconImg.sprite = iconSpr[3];
                    PlayerInventory.Money_AmazonCoin += 10;
                    getItemAmount.text = "x10";
                    break;
                default: iconImg.sprite = iconSpr[0]; break;
            }

            getPopUp.SetActive(true);
        }
    }

    private void Start()
    {
        Normal.gameObject.SetActive(false);
        Super.gameObject.SetActive(false);
        StartCoroutine(AutoSupply());
    }

    IEnumerator AutoSupply()
    {
        yield return null;
        yield return new WaitForSeconds(supplyDelay);
        /// 보급상자 쿨 마다 로컬 데이터 저장 - > 후에 서버 저장까지?
        PlayerPrefsManager.instance.TEST_SaveJson();
        /// 광산/늪지 입장 아닐때만 정상 카운트 되고 보급품 떨어짐.
        if(!PlayerPrefsManager.isEnterTheMine)
        {
            if (superCont >= 10)
            {
                superCont = 0;
                CreateSuperBox();
            }
            else
            {
                superCont++;
                CreateNormalBox();
            }
        }

        StartCoroutine(AutoSupply());
    }

    void CreateNormalBox()
    {
        anim = Normal.GetComponent<Animator>();
        Normal.gameObject.SetActive(true);
        Normal.DOMove(TargetPos.position, 5f).OnComplete(CallBackGround);
    }

    void CreateSuperBox()
    {
        anim = Super.GetComponent<Animator>();
        Super.gameObject.SetActive(true);
        Super.DOMove(TargetPos.position, 5f).OnComplete(CallBackGround);
    }

    /// <summary>
    /// DOMove 완료되면 애니메이션 재생
    /// </summary>
    void CallBackGround()
    {
        if(Normal.gameObject.activeSelf || Super.gameObject.activeSelf)
        {
            anim.Play("TopDown", -1, 0f);
            Invoke(nameof(InvoDismiss), 5f);
        }
        else
        {
            InvoDismiss();
        }
    }

    void InvoDismiss()
    {
        Normal.position = StartPos.position;
        Normal.gameObject.SetActive(false);
        Super.position = StartPos.position;
        Super.gameObject.SetActive(false);
    }







}
