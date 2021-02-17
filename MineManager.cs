using EasyMobile;
using Lean.Localization;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MineManager : MonoBehaviour
{
    [Header(" - 얘가 엑티브면 동굴 안에 들어와있다")]
    public GameObject SuperMomObject;
    [Space]
    public Text[] AmberBtnText;
    public Transform Pet_INFINITI;
    [Header(" - 안전 운행 뉴 팝업")]
    public GameObject[] innerNewPop;
    [Header(" - 잡다한거")]
    public GameObject SupplyBoxPos;
    public ScrollScript autoView;
    public Scrollbar scb;
    public Image HP_FILL;
    /// <summary>
    /// 채굴 남은 시간 
    /// </summary>
    public static float[] currentHPs;
    //
    public GameObject[] middleMoney;
    [Header(" - 공수교대1")]
    public Transform UI_TopBotCanvas;
    public Animator anim;
    [Header(" - 공수교대2")]
    public GameObject[] chractor;
    public GameObject[] bott;
    public GameObject[] backGnd;
    public GameObject[] middle;
    [Header(" -Item 부모")]
    public Transform InfiContents;
    [Header(" 머니 이미지")]
    public Sprite[] sprs;
    public Image moneyImg;
    [Header("- +100 관련 뭘로 바꿀래")]
    public Image[] btnImgs;
    public Sprite EnableBtn;
    public Sprite DisableBtn;
    [Header("-팝업 관련")]
    public Image AdsIcon;
    public Sprite[] AdsSprite;
    public Text popAmount;
    public Image popIconImg;
    public Sprite[] iconSprs;
    public GameObject[] btnSwich;
    public Text AdsDesc;
    [Header("-저장소 관련")]
    public Text[] stackDesc;
    public Image[] BtnStackImage;
    public Sprite[] BtnStackSprs;
    [Header("-곡괭이 관련")]
    public Text[] AxeLv;
    public Text[] AxeDesc;
    public GameObject[] AxeBtnMax;
    public Image[] BtnImage;
    public Sprite[] BtnSprs;


    public delegate void ChainFunc();       // 아웃라인 델리게이트
    public ChainFunc chain;                 // 체인 메서드


    Coroutine[] C_Routine;
    int[] HoBakCnt;

    /// <summary>
    /// true 이면 채굴 할래?
    /// false 이면 즉시 완료 할래?>
    /// </summary>
    /// <param name="_Trig"></param>
    public void PopWarnningUp(bool _Trig, int _AutoIndex)
    {
        /// 클릭한 아이템 인덱스 저장해주고
        _index = _AutoIndex;
        /// 채굴 아님 즉시 완료
        innerNewPop[0].SetActive(_Trig);
        innerNewPop[1].SetActive(!_Trig);
        PopUpManager.instance.ShowPopUP(29);
    }
    /// <summary>
    /// 뉴 팝업 클릭할때 내부 인덱스 임시 저장
    /// </summary>
    int _index;
    public int recentCoinSoMo;

    /// <summary>
    /// 채굴권 소모하여 채굴하시겠습니까? -> 확인
    /// </summary>
    public void Clicked_MinningStart()
    {
        if (PlayerInventory.mining < recentCoinSoMo) return;
        /// 채굴권 소모
        PlayerInventory.SetTicketCount("mining", -recentCoinSoMo);
        /// 광산 1회 진행
        if (PlayerPrefsManager.currentTutoIndex == 29) ListModel.Instance.TUTO_Update(29);
        /// 채굴 업적  완료 카운트
        ListModel.Instance.ALLlist_Update(8, 1);
        /// 채굴 업적
        ListModel.Instance.DAYlist_Update(9);
        /// 채굴 코루틴 시작.
        DieHardCoTimer(_index);
        /// 팝업 닫아줌
        PopUpManager.instance.HidePopUP(29);
    }
    public void Clicked_DiaComplete()
    {
        PlayerInventory.Money_Dia -= int.Parse(ListModel.Instance.mineCraft[_index].unlockDia);
        /// 채굴 코루틴 중단 시키고 즉시 완료
        CompForDia(_index);
        /// 팝업 닫아줌
        PopUpManager.instance.HidePopUP(29);
    }

    private void Awake()
    {
        C_Routine = new Coroutine[100];
        HoBakCnt = new int[100];
        currentHPs = new float[100];
        for (int i = 0; i < currentHPs.Length; i++)
        {
            currentHPs[i] = 0;
        }

    }

    /// <summary>
    /// 글로벌 채굴하는 코루틴 작동
    /// </summary>
    /// <param name="_id"></param>
    public void DieHardCoTimer(int _id)
    {
        if(C_Routine[_id] == null)
            C_Routine[_id] = StartCoroutine(OrignalWork(_id));
        /// ing 일때는 애니메이션
        anim.Play("Player_Mine", -1, 0f);
    }
    /// <summary>
    ///  즉시 완료
    /// </summary>
    public void CompForDia(int _index)
    {
        if (C_Routine[_index] != null)
        {
            StopCoroutine(C_Routine[_index]);
            C_Routine[_index] = null;
        }
        /// 진행도 초기화.
        currentHPs[_index] = 0;
        /// 채굴 완료 띄워주고
        ListModel.Instance.Mine_Unlock(_index, "COMP");
        /// 버튼 내용 새로고침
        RefleshAllItem();
        /// 빨간 점 띄우기
        RedDotManager.instance.RedDot[6].SetActive(true);
    }

    bool isMotherItemInit;
    /// <summary>
    /// 로컬에서 저장된 채굴 시간 불러올 때.
    /// </summary>
    public void InitTimeLoad()
    {
        if (isMotherItemInit)
            return;
        for (int i = 0; i < currentHPs.Length; i++)
        {
            if (ListModel.Instance.mineCraft[i].isEnable == "ING")
            {
                Debug.LogError("몇번 돌아가니!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                C_Routine[i] = StartCoroutine(OrignalWork(i));
            }
        }
        /// 최초 실행시만 실행
        isMotherItemInit = true;
    }


    /// <summary>
    /// 실제 채굴이 이루어 지는 코루틴
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    IEnumerator OrignalWork(int _index)
    {
        yield return null;
        /// 상태 0 이나 TRUE 일때 채굴 시도
        ListModel.Instance.Mine_Unlock(_index, "ING");
        /// 새로고침 -> 버튼 내용 바뀐다.
        RefleshAllItem();

        float MAX_HP = ListModel.Instance.mineCraft[_index].mine_hp;

        while (true)
        {
            /// 속도 만큼 대기
            yield return new WaitForSeconds(60.0f / (60.0f + GetMineStat(1)));
            /// 속도에 파워 만큼 진행도 올려줌
            currentHPs[_index] += GetMineStat(0);
            /// 진행도 올릴 때 마다 호박석 굴림
            HoBak(_index);
            /// 채굴 끝나면 브레이크
            if (currentHPs[_index] >= MAX_HP) 
                break;
        }
        /// 완료됨 상태 저장
        ListModel.Instance.Mine_Unlock(_index, "COMP");
        /// 새로고침 -> 버튼 내용 바뀐다.
        // 바깥에 나가있을때 오류 뿜는데?
        RefleshAllItem();
        /// 빨간 점 띄우기
        RedDotManager.instance.RedDot[6].SetActive(true);
        /// 초기화
        currentHPs[_index] = 0;
        C_Routine[_index] = null;
    }

    void HoBak(int _index)
    {
        if (++HoBakCnt[_index] < 10) return;
        HoBakCnt[_index] = 0;
        float randomseed = Random.Range(0, 100f);
        Debug.LogError("호박석 파밍 시도 : " + randomseed + " 1 프로");
        /// 호박석 확률 1프로
        if (randomseed < 1.1f)
        {
            ListModel.Instance.axeDataList[0].Stack_Amber++;
            Debug.LogError("호박석 파밍 성공");
        }
    }


    public void RefleshAllItem()
    {
        /// 지금 활성화된 자식만큼 배수 적용해줌. -> 배수 적용된 뒤에 새로고침.
        for (int i = 1; i < InfiContents.GetChild(0).childCount; i++)
        {
            InfiContents.GetChild(0).GetChild(i).GetComponent<AutoItem>()
                .BoxInfoUpdate(int.Parse(InfiContents.GetChild(0).GetChild(i).name));
        }
        RefreshStack();
    }



    /// <summary>
    /// TopView 전환 버튼
    /// 1 / 10 / 100
    /// </summary>
    public void DismissGrayBtn(int _multiple)
    {
        InfiContents.GetChild(0).gameObject.SetActive(false);
        InfiContents.GetChild(1).gameObject.SetActive(false);
        InfiContents.GetChild(2).gameObject.SetActive(false);
        //
        switch (_multiple)
        {
            case 1:
                InfiContents.GetChild(0).gameObject.SetActive(true);
                autoView.content = InfiContents.GetChild(0).GetComponent<RectTransform>();
                /// 스크롤 바 위로 쭉 올려줌.
                scb.value = 1f;
                middleMoney[0].SetActive(true);
                middleMoney[1].SetActive(false);
                btnImgs[0].sprite = EnableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = DisableBtn;
                break;

            case 10:
                InfiContents.GetChild(1).gameObject.SetActive(true);
                autoView.content = InfiContents.GetChild(1).GetComponent<RectTransform>();
                /// 스크롤 바 위로 쭉 올려줌.
                scb.value = 1f;
                middleMoney[0].SetActive(false);
                middleMoney[1].SetActive(true);
                /// 보관함 버튼 색 새로고침
                RefreshStack();
                btnImgs[0].sprite = DisableBtn;
                btnImgs[1].sprite = EnableBtn;
                btnImgs[2].sprite = DisableBtn;
                break;

            case 100:
                InfiContents.GetChild(2).gameObject.SetActive(true);
                autoView.content = InfiContents.GetChild(2).GetComponent<RectTransform>();
                /// 버튼에 호박석 필요 갯수 올려 줌.
                ShowAmberNeed();
                /// 스크롤 바 위로 쭉 올려줌.
                scb.value = 1f;
                middleMoney[0].SetActive(false);
                middleMoney[1].SetActive(true);
                /// 곡괭이 강화 페이지 버튼 색 새로고침
                RefreshAxe(-1);
                btnImgs[0].sprite = DisableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = EnableBtn;
                break;

            default:
                btnImgs[0].sprite = EnableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = DisableBtn;
                break;
        }
    }

    /// <summary>
    /// 곡괭이 3개에 호박석 필요 갯수 적어줌.
    /// </summary>
    private void ShowAmberNeed()
    {
        AmberBtnText[0].text = PlayerPrefsManager.instance.DoubleToStringNumber(ListModel.Instance.axeDataList[0].Axe_Power);
        AmberBtnText[1].text = PlayerPrefsManager.instance.DoubleToStringNumber(ListModel.Instance.axeDataList[0].Axe_Speed);
        AmberBtnText[2].text = PlayerPrefsManager.instance.DoubleToStringNumber(ListModel.Instance.axeDataList[0].Axe_Skill);
    }

    /// <summary>
    /// 광산 입장 하십니다.
    /// </summary>
    public void AddMineEnterCount()
    {
        /// 팝업 띄워주기
        PopUpManager.instance.ShowGrobalPopUP(4);
        /// 빨간 점 꺼주기
        RedDotManager.instance.RedDot[6].SetActive(false);
    }


    /// <summary>
    /// 채굴 중이면 곡괭이질 계속하고
    /// 아니라면 곡괭이질 멈춰
    /// </summary>
    void SelectPlayerAxeAnim()
    {
        for (int i = 0; i < ListModel.Instance.mineCraft.Count; i++)
        {
            /// 하나라도 채굴중 상태이면 채굴 애니메이션
            if (ListModel.Instance.mineCraft[i].isEnable == "ING")
            {
                anim.Play("Player_Mine", -1, 0f);
                return;
            }
        }
        //ListModel.Instance.axeDataList[0].Stack_Amber ++;
        /// 하나도 채굴중 아니면 곡괭 멈춰 
        anim.Play("Player_Mine_Idle", -1, 0f);
    }

    public void InvoMineEnter()
    {
        /// 멈춰!!
        anim.StopPlayback();
        anim.Play("Player_Mine_Idle", -1, 0f);
        // 공격 애니메이션 정지
        PlayerPrefsManager.isEnterTheMine = true;
        // 게임 오브젝트 덮기
        chractor[1].SetActive(true);
        chractor[0].SetActive(false);
        bott[1].SetActive(true);
        bott[0].SetActive(false);
        backGnd[1].SetActive(true);
        backGnd[0].SetActive(false);
        middle[1].SetActive(true);
        middle[0].SetActive(false);
        /// 곡괭이질 할까 말까?
        SelectPlayerAxeAnim();
        /// 수정 동굴 브금 재생
        AudioManager.instance.PlayAudio("Mine", "BGM");
        /// 클릭가능한 UI 숨김
        for (int i = 0; i < UI_TopBotCanvas.childCount; i++)
        {
            UI_TopBotCanvas.GetChild(i).gameObject.SetActive(false);
        }
        /// 곡괭이 강화 새로고침
        for (int i = 0; i < 3; i++)
        {
            RefreshAxe(i);
        }
        ///
        MoneyManager.instance.RefreshAllMoney();
        PopUpManager.instance.HidePopUP(23);
        /// 보급상자 임시 꺼주기
        SupplyBoxPos.SetActive(false);
        /// 광산 100개 리스트
        DismissGrayBtn(1);
    }

    /// <summary> 
    /// 나가기 버튼에 달려서 원래대로 되돌리기 #b71c1c
    /// </summary>
    public void ExitMine()
    {
        /// 나가기 팝업 알맹이 활성화.
        PopUpManager.instance.ShowGrobalPopUP(0);
    }
    /// <summary>
    /// [나가기] 버튼에 달리는 리얼 나가기
    /// </summary>
    public void InvoExitMine()
    {
        /// 빨간 점 꺼주기
        RedDotManager.instance.RedDot[6].SetActive(false);
        /// 멈춰!!
        anim.StopPlayback();
        anim.Play("Player_Mine_Idle", -1, 0f);
        /// 메인 브금 재생
        AudioManager.instance.PlayAudio("Main", "BGM");
        // 게임 오브젝트 덮기
        chractor[0].SetActive(true);
        chractor[1].SetActive(false);
        bott[0].SetActive(true);
        bott[1].SetActive(false);
        backGnd[0].SetActive(true);
        backGnd[1].SetActive(false);
        middle[0].SetActive(true);
        middle[1].SetActive(false);
        /// 본래 공격 몬스터로 돌려보내줌
        PlayerPrefsManager.isEnterTheMine = false;
        DistanceManager.instance.StopPlayer();
        /// 클릭가능한 UI 보여줌
        for (int i = 0; i < UI_TopBotCanvas.childCount; i++)
        {
            UI_TopBotCanvas.GetChild(i).gameObject.SetActive(true);
        }
        /// 튜토리얼 노란 박스 싹다 클리어했으면 표시 안함
        if (PlayerPrefsManager.isTutoAllClear)
        {
            UI_TopBotCanvas.GetChild(UI_TopBotCanvas.childCount - 1).gameObject.SetActive(false);
        }

        HP_FILL.fillAmount = 1.0f;
        PopUpManager.instance.HidePopUP(23);
        /// 보급상자 다시 켜준다
        SupplyBoxPos.SetActive(true);
        /// 펫 움직임 활성화.
        for (int i = 1; i < Pet_INFINITI.childCount; i++)
        {
            Pet_INFINITI.GetChild(i).GetComponent<PetItem>().BoxInfoUpdate(i - 1);
        }
    }

    private int adsIndex;
    private double adsAmount;

    /// <summary>
    /// 이지선다 팝업 이미지/텍스트 세팅
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_Amount"></param>
    void SetPopInSide(int _index, long _Amount)
    {
        /// 광고 스킵권 구매했음? 광고 아이콘 바꾸기
        if (PlayerInventory.isSuperUser != 0)
        {
            AdsIcon.sprite = AdsSprite[1];
        }
        else if (AdsIcon.sprite != AdsSprite[0])
        {
            AdsIcon.sprite = AdsSprite[0];
        }
        /// 2개 버튼 표시
        btnSwich[0].SetActive(true);
        btnSwich[1].SetActive(false);
        /// 광고 홍보 문기
        AdsDesc.text = LeanLocalization.GetTranslationText("StayReword_Description");
        /// 내용물 채우기
        adsIndex = _index;
        adsAmount = _Amount;
        popIconImg.sprite = iconSprs[_index];
        popAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(_Amount);
        PopUpManager.instance.ShowPopUP(20);
    }

    /// <summary>
    /// 광고 볼래? 그냥 받을래 버튼 2개 컨트롤
    /// </summary>
    /// <param name="_Moon"></param>
    public void ClickedEasySunDa(int _Moon)
    {
        if (_Moon == 0)
        {
            Ads_FreeDiaCanvas();
        }
        else
        {
            StackOverNormalGet(false);
            PopUpManager.instance.HidePopUP(20);
        }
    }


    #region <Rewarded Ads> 광산 보상 동영상 광고

    /// <summary>
    /// 실제 광고를 봤다면 true
    /// </summary>
    bool _AdsComp;
    void Ads_FreeDiaCanvas()
    {
        PlayerPrefsManager.instance.TEST_SaveJson();
        SystemPopUp.instance.LoopLoadingImg();
        Invoke(nameof(InvoStopLoop), 5.0f);

        if (PlayerInventory.isSuperUser != 0)
        {
            /// 광고 스킵
            _AdsComp = true;
            /// 2개 버튼 표시
            btnSwich[0].SetActive(false);
            btnSwich[1].SetActive(true);
            /// 광고 두배보상적용 ㅇㅋ
            AdsDesc.text = LeanLocalization.GetTranslationText("Config_Ads_Okay");
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
            /// 2개 버튼 표시
            btnSwich[0].SetActive(false);
            btnSwich[1].SetActive(true);
            /// 광고 없어도 ㅇㅋ
            AdsDesc.text = LeanLocalization.GetTranslationText("Config_Ads_Nope");
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
        /// 2개 버튼 표시
        btnSwich[0].SetActive(false);
        btnSwich[1].SetActive(true);
        /// 광고 두배보상적용 ㅇㅋ
        AdsDesc.text = LeanLocalization.GetTranslationText("Config_Ads_Okay");
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
    /// 보상 지급 메소드/
    /// </summary>
    void AdsInvo()
    {
        SystemPopUp.instance.StopLoopLoading();

        /// 2배 뻥튀기
        StackOverNormalGet(true);
        _AdsComp = false;

        if (PlayerInventory.isSuperUser != 0) return;

        ///  광고 1회 시청 완료 카운트
        ListModel.Instance.ALLlist_Update(0, 1);
        /// 광고 시청 일일 업적
        ListModel.Instance.DAYlist_Update(7);
    }

    #endregion




    /// <summary>
    /// 저장소 버튼 색깔 새로고침
    /// </summary>
    void RefreshStack()
    {
        /// 받기 가능 ? 불가능 ? 버튼 색 바꿔줌
        if (ListModel.Instance.axeDataList[0].Stack_EnStone < 1)
        {
            stackDesc[0].text = "0";
            BtnStackImage[0].sprite = BtnStackSprs[0];
        }
        else
        {
            stackDesc[0].text = PlayerPrefsManager.instance.DoubleToStringNumber(ListModel.Instance.axeDataList[0].Stack_EnStone);
            BtnStackImage[0].sprite = BtnStackSprs[1];
        }
        //
        if (ListModel.Instance.axeDataList[0].Stack_AmaCystal < 1)
        {
            stackDesc[1].text = "0";
            BtnStackImage[1].sprite = BtnStackSprs[0];
        }
        else
        {
            stackDesc[1].text = PlayerPrefsManager.instance.DoubleToStringNumber(ListModel.Instance.axeDataList[0].Stack_AmaCystal);
            BtnStackImage[1].sprite = BtnStackSprs[1];
        }
        //
        if (ListModel.Instance.axeDataList[0].Stack_Amber < 1)
        {
            stackDesc[2].text = "0";
            BtnStackImage[2].sprite = BtnStackSprs[0];
        }
        else
        {
            stackDesc[2].text = PlayerPrefsManager.instance.DoubleToStringNumber(ListModel.Instance.axeDataList[0].Stack_Amber);
            BtnStackImage[2].sprite = BtnStackSprs[1];
        }
    }
    /// <summary>
    /// 저장소에 보관된 자원 받기 버튼 눌렀을때
    /// </summary>
    /// <param name="_index"></param>
    public void ClickedStackOverFlow(int _index)
    {
        switch (_index)
        {
            case 0:
                if (ListModel.Instance.axeDataList[0].Stack_EnStone < 1) return;
                SetPopInSide(0, ListModel.Instance.axeDataList[0].Stack_EnStone);
                break;
            case 1:
                if (ListModel.Instance.axeDataList[0].Stack_AmaCystal < 1) return;
                SetPopInSide(1, ListModel.Instance.axeDataList[0].Stack_AmaCystal);
                break;
            case 2:
                if (ListModel.Instance.axeDataList[0].Stack_Amber < 1) return;
                SetPopInSide(2, ListModel.Instance.axeDataList[0].Stack_Amber);
                break;
        }
    }

    /// <summary>
    /// 동영상 안보고 그냥 받기 눌렀을 때
    /// </summary>
    void StackOverNormalGet(bool is2X)
    {
        switch (adsIndex)
        {
            case 0:
                if (is2X)
                {
                    PlayerInventory.Money_EnchantStone += (adsAmount * 2d);
                    ///  강화석 업적 카운트 올리기
                    ListModel.Instance.ALLlist_Update(5, adsAmount * 2d);
                    popAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(adsAmount * 2.0d);
                }
                else
                {
                    PlayerInventory.Money_EnchantStone += adsAmount;
                    ///  강화석 업적 카운트 올리기
                    ListModel.Instance.ALLlist_Update(5, adsAmount);
                }
                ListModel.Instance.axeDataList[0].Stack_EnStone = 0;
                break;
            case 1:
                if (is2X)
                {
                    //PlayerInventory.AmazonStoneCount += (adsAmount * 2);
                    ///// 결정조각  업적  카운트
                    //ListModel.Instance.ALLlist_Update(2, (adsAmount * 2));
                    /// 아마존 포션 추가
                    PlayerInventory.SetTicketCount("S_leaf_box", (int)(adsAmount * 2));
                    popAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(adsAmount * 2.0d);
                }
                else
                {
                    //PlayerInventory.AmazonStoneCount += adsAmount;
                    ///// 결정조각  업적  카운트
                    //ListModel.Instance.ALLlist_Update(2, adsAmount);
                    /// 아마존 포션 추가
                    PlayerInventory.SetTicketCount("S_leaf_box", (int)adsAmount);
                }
                ListModel.Instance.axeDataList[0].Stack_AmaCystal = 0;
                break;
            case 2:
                if (is2X)
                {
                    PlayerInventory.SetTicketCount("amber", (int)(adsAmount * 2));
                    popAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(adsAmount * 2.0d);
                }
                else PlayerInventory.SetTicketCount("amber", (int)adsAmount);
                ListModel.Instance.axeDataList[0].Stack_Amber = 0;
                break;
        }
        RefreshStack();
    }

    /// <summary>
    /// 저장소에 채굴한거 쌓아놓기.
    /// </summary>
    /// <param name="_index"></param>
    public void GetOverLoadReword(int _index)
    {
        /// 보상 -> 저장소에 저장
        float tmpES = float.Parse(ListModel.Instance.mineCraft[_index].reword_es) * (1.0f + (GetMineStat(2) * 0.005f) * (float)PlayerInventory.EnchantStone_Earned);
        ListModel.Instance.axeDataList[0].Stack_EnStone += Mathf.RoundToInt(tmpES);
        ListModel.Instance.axeDataList[0].Stack_AmaCystal += int.Parse(ListModel.Instance.mineCraft[_index].reword_ama);
        //
        ListModel.Instance.Mine_Unlock(_index, "TRUE");
        try
        {
            /// 아직 해금 상태가 아니라면 해금
            if (int.Parse(ListModel.Instance.mineCraft[_index + 1].isEnable) == 0) ListModel.Instance.Mine_Unlock(_index + 1, "TRUE");
        }
        catch (System.Exception Ex)
        {
            Debug.LogWarning("휴우~ 무사 통과 : " + Ex);
        }
        RefleshAllItem();
    }



    /// <summary>
    /// 외부에서 호출
    /// 0. 파워
    /// 1. 스피드
    /// 2. 스킬
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public float GetMineStat(int _index)
    {
        float result = 1.0f;
        switch (_index)
        {
            case 0: result = ListModel.Instance.axeDataList[0].Axe_Power; break;
            case 1: result = ListModel.Instance.axeDataList[0].Axe_Speed; break;
            case 2: result = ListModel.Instance.axeDataList[0].Axe_Skill; break;
        }
        return result;
    }



    void RefreshAxe(int _index)
    {
        /// 업그레이드 가능 불가능 버튼 색 바꿔주기
        if (_index == -1)
        {
            if (PlayerInventory.amber < (int)ListModel.Instance.axeDataList[0].Axe_Power)
            {
                BtnImage[0].sprite = BtnSprs[0];
            }
            else
            {
                BtnImage[0].sprite = BtnSprs[1];
            }

            if (PlayerInventory.amber < (int)ListModel.Instance.axeDataList[0].Axe_Speed)
            {
                BtnImage[1].sprite = BtnSprs[0];
            }
            else
            {
                BtnImage[1].sprite = BtnSprs[1];
            }

            if (PlayerInventory.amber < (int)ListModel.Instance.axeDataList[0].Axe_Skill)
            {
                BtnImage[2].sprite = BtnSprs[0];
            }
            else
            {
                BtnImage[2].sprite = BtnSprs[1];
            }
            return;
        }
        /// 각 인덱스 텍스트 새로고침
        AxeLv[_index].text = "Lv. " + GetMineStat(_index).ToString("F0");
        if (_index == 0)
        {
            AxeDesc[0].text = GetMineStat(0).ToString("F0");
        }
        else
        {
            AxeDesc[_index].text = (GetMineStat(_index) * 0.5f).ToString("F1") + "%";
        }
        /// 요구 호박석 수 갱신
        ShowAmberNeed();
    }

    /// <summary>
    /// <3번째탭> 곡괭이 강화 클릭시
    /// </summary>
    /// <param name="_index"></param>
    public void ClickedSkillUP(int _index)
    {
        /// 맥스버튼이거나 앰버 없으면 리턴
        if (AxeBtnMax[_index].activeSelf) return;
        int thisLevel;
        ///  엘릭서처럼 소모해
        switch (_index)
        {
            /// 파워 - 능률 증가
            case 0:
                thisLevel = (int)ListModel.Instance.axeDataList[0].Axe_Power;
                if (PlayerInventory.amber < thisLevel) return;
                PlayerInventory.SetTicketCount("amber", -thisLevel);
                //
                ListModel.Instance.axeDataList[0].Axe_Power++;
                if (ListModel.Instance.axeDataList[0].Axe_Power == 9999) AxeBtnMax[_index].SetActive(true);
                break;
            /// 스피드 - 속도 증가
            case 1:
                thisLevel = (int)ListModel.Instance.axeDataList[0].Axe_Speed;
                if (PlayerInventory.amber < thisLevel) return;
                PlayerInventory.SetTicketCount("amber", -thisLevel);

                ListModel.Instance.axeDataList[0].Axe_Speed++;
                if (ListModel.Instance.axeDataList[0].Axe_Speed == 9999) AxeBtnMax[_index].SetActive(true);
                break;
            /// 스킬 - 획득량 증가
            case 2:
                thisLevel = (int)ListModel.Instance.axeDataList[0].Axe_Skill;
                if (PlayerInventory.amber < thisLevel) return;
                PlayerInventory.SetTicketCount("amber", -thisLevel);

                ListModel.Instance.axeDataList[0].Axe_Skill++;
                if (ListModel.Instance.axeDataList[0].Axe_Skill == 9999) AxeBtnMax[_index].SetActive(true);
                break;
        }

        RefreshAxe(_index);
        RefreshAxe(-1);
    }




}
