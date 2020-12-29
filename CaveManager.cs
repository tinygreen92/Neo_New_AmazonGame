using EasyMobile;
using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaveManager : MonoBehaviour
{
    public HpBarManager HBM;
    public GameObject SupplyBoxPos;
    public GameObject EasySunDaPop;
    public Text easyRellyText;
    public GameObject GoldDropPos;
    public GameObject DamegeDropPos;
    public GameObject x_Btn;
    [Header(" - 타임 머신")]
    public GameObject swampTimeBar;
    public Image TimeFill;
    [Header(" - 보상 화면")]
    public Transform pop22GetSwampReword;
    public GameObject haeGumCanvas;
    public BattleGndManager bg;
    [Header(" - 단계 변경 버튼")]
    public Text thisDan;
    public Image[] BtnImgs;
    public Sprite[] BtnSprs;
    [Header(" - 공수교대1")]
    public Transform[] UI_TopBotCanvas;
    public Transform[] CaveDragonFire;
    public Text swampText;
    [Header(" - 공수교대2")]
    public GameObject[] bott;
    public GameObject[] middle;
    [Header(" - 내용물")]
    public Text[] moneyEnter;
    public Text[] moneySotang;
    public Text killLeaf;
    public Text killEnchant;
    public Text currentKillCount;
    public Text currentLeaf;
    public Text currentEnchant;
    public Text[] bestKillCount;
    public Text[] bestLeaf;
    public Text[] bestEnchant;
    [Header(" - 소탕 팝업 내용물")]
    public GameObject getpop;
    public Image AdsIcon;
    public Sprite[] AdsSprite;
    public Text sotangLeaf;
    public Text sotangEnchant;
    public Text sotangAdsText;
    public GameObject[] BtnLayoutSo;
    [Header(" - 토벌 팝업 내용물")]
    public Image AdsIcon2;
    public Text clearLeaf;
    public Text clearEnchant;
    public Text clearAdsDesc;
    public GameObject[] BtnLayoutTo;


    /// <summary>
    /// 10마리 잡으면 다음층 해금
    /// </summary>
    private const int CLEAR_KILL = 10;
    /// <summary>
    /// 최종 클리어 단계 몇? (10 마리 잡고 완클이면 다음 버튼색 노랑 / 아니면 회색)
    /// </summary>
    private int currentMyDan;
    /// <summary>
    /// 늪지 아이콘 클릭하면 팝업창 띄워 주기
    /// </summary>
    public void AddCaveRunCount()
    {
        /// 가장 높은 스테이지  currentMyDan 세팅
        GetCurrentTryStage();
        /// 이전 다음 버튼 세팅 + 팝업 내용물 세팅
        SetBtnNaverDaum(currentMyDan);
        /// 입장권 새로고침
        RefreshMoney();
        //
        PopUpManager.instance.ShowPopUP(21);
    }

    /// <summary>
    /// 나가기 버튼에 붙어있음
    /// </summary>
    public void ExitBtnClicked()
    {
        /// 배틀 씬에서 골드/대미지 다시 보여주기
        DamegeDropPos.SetActive(true);
        BugFixer();
        PopUpManager.instance.HidePopUP(21);
    }
    
    void RefreshMoney()
    {
        moneyEnter[0].text = PlayerInventory.ticket_cave_enter.ToString("N0");
        moneyEnter[1].text = PlayerInventory.ticket_cave_enter.ToString("N0");
        moneySotang[0].text = PlayerInventory.ticket_cave_clear.ToString("N0");
        moneySotang[1].text = PlayerInventory.ticket_cave_clear.ToString("N0");
    }

    /// <summary>
    /// 가장 높은 트라이 스테이지 (1마리 이상 킬 한 경우)
    /// </summary>
    void GetCurrentTryStage()
    {
        var scd = ListModel.Instance.swampCaveData;
        ///
        for (int i = 0; i < scd.Count; i++)
        {
            if (int.Parse(scd[i].killCount) < CLEAR_KILL)
            {
                if (i >0 && int.Parse(scd[i-1].killCount) >= CLEAR_KILL)
                {
                    currentMyDan = i + 1;
                    return;
                }
                else
                {
                    if(i > 0 && int.Parse(scd[i].killCount) == 0)
                    {
                        currentMyDan = i;
                        return;
                    }
                    else if(i == 0)
                    {
                        currentMyDan = 1;
                        return;
                    }
                }
            }
        }
        /// 100 스테이지까지 잡았음
        currentMyDan = 101;
    }

    /// <summary>
    /// 버튼 세팅에 내용물 세팅까지 전부
    /// </summary>
    /// <param name="_index"></param>
    private void SetBtnNaverDaum(int _index)
    {
        var scd = ListModel.Instance.swampCaveData;

        if (_index > 100)                                                 /// 마지막 스테이지 일때
        {
            BtnImgs[0].sprite = BtnSprs[1];
            BtnImgs[1].sprite = BtnSprs[0];
        }
        else
        {
            BtnImgs[0].sprite = _index == 1 ? BtnSprs[0] : BtnSprs[1];
            BtnImgs[1].sprite = GetKillMonsterCount(_index, "") < CLEAR_KILL ? BtnSprs[0] : BtnSprs[1];
        }

        /// 숨겨진 늪지 x 단계
        thisDan.text = LeanLocalization.GetTranslationText("Cave_Enter_Tiltle")
                                + " " + _index
                                + LeanLocalization.GetTranslationText("Cave_Enter_Tiltle_2");

        /// 내용물 채워주기
        killLeaf.text = "x " + PlayerPrefsManager.instance.DoubleToStringNumber(scd[_index - 1].rewordLeaf);
        killEnchant.text = "x " + PlayerPrefsManager.instance.DoubleToStringNumber(scd[_index - 1].rewordEnchant);
        //
        bestKillCount[0].text = GetKillMonsterCount(_index, "").ToString("N0") + LeanLocalization.GetTranslationText("Cave_Mari");
        bestKillCount[1].text = bestKillCount[0].text;
        bestLeaf[0].text = "x " + PlayerPrefsManager.instance.DoubleToStringNumber(GetKillMonsterCount(_index, "leaf"));
        bestLeaf[1].text = bestLeaf[0].text;
        bestEnchant[0].text = "x " + PlayerPrefsManager.instance.DoubleToStringNumber(GetKillMonsterCount(_index, "es"));
        bestEnchant[1].text = bestEnchant[0].text;
        //
        sotangLeaf.text = bestLeaf[0].text;
        sotangEnchant.text = bestEnchant[0].text;
    }

    /// <summary>
    /// 미들 캔버스에서 현재 진행중인 보상 실시간 갱신 _> 몬스터 1킬시 호출
    /// PlayerPrefsManager.swampMonKillCount++
    /// </summary>
    public void RefleshCurrentKill(int _kc)
    {
        //if (_kc >= CLEAR_KILL) ListModel.Instance.swampCaveData[currentMyDan - 1].killCount = "CLEAR_KILL";
        currentKillCount.text = _kc + LeanLocalization.GetTranslationText("Cave_Mari");
        currentLeaf.text = "x " + PlayerPrefsManager.instance.DoubleToStringNumber(ListModel.Instance.swampCaveData[currentMyDan - 1].rewordLeaf * (_kc * 1.0f * (float)PlayerInventory.Player_Leaf_Earned));
        currentEnchant.text = "x " + PlayerPrefsManager.instance.DoubleToStringNumber(ListModel.Instance.swampCaveData[currentMyDan - 1].rewordEnchant * (_kc * 1.0f * (float)PlayerInventory.EnchantStone_Earned));
    }

    /// <summary>
    /// 입력 스테이지의 최고 기록 불러오기 + 강화석 + 나뭇잎
    /// </summary>
    /// <param name="_stage"></param>
    float GetKillMonsterCount(int _stage, string _triger)
    {
        var kc = int.Parse(ListModel.Instance.swampCaveData[_stage - 1].killCount);
        switch (_triger)
        {
            case "leaf":
                return ListModel.Instance.swampCaveData[_stage - 1].rewordLeaf * (kc * 1.0f *(float)PlayerInventory.Player_Leaf_Earned);

            case "es":
                return ListModel.Instance.swampCaveData[_stage - 1].rewordEnchant * (kc * 1.0f * (float)PlayerInventory.EnchantStone_Earned);

            default:
                return kc;
        }
    }

    /// <summary>
    /// 네이버 다음에 붙는 메서드
    /// </summary>
    /// <param name="_isRight"></param>
    public void ClickedNaverDaum(bool _isRight)
    {
        if (!_isRight)
        {
            if (BtnImgs[0].sprite == BtnSprs[0]) return;
            /// 이전 버튼 누르면 이전 스테이지로 돌아감 (단, 1스테이지가 아닐때)
            currentMyDan--;
        }
        else
        {
            if (BtnImgs[1].sprite == BtnSprs[0])
            {
                /// 해금 안내 문구 표기
                haeGumCanvas.SetActive(true);
                return;
            }
            /// 다음 버튼 누르면 다음 스테이지로
            currentMyDan++;
        }
        /// 내용물 새로 고침
        SetBtnNaverDaum(currentMyDan);
    }

    /// <summary>
    /// [토벌] 입장할래 토벌할래 팝업에서 [입장] 누르면 해당 스테이지에 입장
    /// </summary>
    void ClickedEnterSwamp()
    {
        /// 팝업 꺼줌
        EasySunDaPop.SetActive(false);
        /// 보스 코루틴 꺼줌.
        PlayerPrefsManager.isEnterTheMine = false;
        HBM.CheatGetOut();
        /// 입장권 소모
        PlayerInventory.SetTicketCount("cave_enter", -1);
        /// 입장권 새로고침
        RefreshMoney();
        /// 보급상자 꺼준다
        SupplyBoxPos.SetActive(false);
        ///  숨겨진 늪지 1회 진행
        if (PlayerPrefsManager.currentTutoIndex == 14) ListModel.Instance.TUTO_Update(14);
        /// 현재 스테이지 킬 카운터 초기화
        PlayerPrefsManager.swampMonKillCount = 0;
        RefleshCurrentKill(0);
        /// 입장권 새로고침
        RefreshMoney();
        /// 배경 그림 바꿔주기
        bg.ChageBG_Render(2);
        /// 늪지 브금 재생
        AudioManager.instance.PlayAudio("Cave", "BGM");
        ///  동굴 입장 카운터 1 올리기
        ListModel.Instance.DAYlist_Update(2);
        ///  업적  완료 카운트
        ListModel.Instance.ALLlist_Update(7, 1);
        /// 게임 오브젝트 덮기
        bott[1].SetActive(true);
        bott[0].SetActive(false);
        middle[1].SetActive(true);
        middle[0].SetActive(false);
        /// 클릭 아이콘들 다 꺼져
        for (int i = 0; i < UI_TopBotCanvas.Length; i++)
        {
            UI_TopBotCanvas[i].gameObject.SetActive(false);
        }
        CaveDragonFire[0].GetChild(0).gameObject.SetActive(false);
        CaveDragonFire[0].GetChild(1).gameObject.SetActive(true);
        for (int i = 1; i < CaveDragonFire[1].childCount-1; i++)
        {
            CaveDragonFire[1].GetChild(i).gameObject.SetActive(false);
        }
        /// 몇 단계? 
        swampText.text = LeanLocalization.GetTranslationText("Cave_Enter_Tiltle") 
                                        + " " + currentMyDan 
                                        + LeanLocalization.GetTranslationText("Cave_Enter_Tiltle_2");
        swampText.transform.parent.gameObject.SetActive(true);
        /// 배틀 씬에서 골드/대미지 없애주기
        GoldDropPos.SetActive(false);
        PlayerPrefsManager.isGoldposOnAir = true;
        DamegeDropPos.SetActive(false);
        /// 입장 팝업 꺼주고 입장
        x_Btn.SetActive(true);
        PopUpManager.instance.HidePopUP(21);
        /// 현재 몬스터 날리고 늪지 몬스터 소환
        PlayerPrefsManager.currentMyStage = currentMyDan - 1;
        DistanceManager.instance.SwampDelay();
        /// 타이머 켜주고 늪지 전투 시작
        Invoke(nameof(InvoEnterCave), 1.5f);
    }

    void InvoEnterCave()
    {
        SystemPopUp.instance.StopLoopLoading();
        PlayerPrefsManager.isEnterTheSwamp = false;
        // 대미지 보여쥼
        DamegeDropPos.SetActive(true);
        /// 타이머 트리거 초기화
        PlayerPrefsManager.isSwampTimeOver = false;
        /// 타이머 세팅
        swampTimeBar.SetActive(true);
        TimeFill.fillAmount = 1.0f;
        /// 타이머 코루틴 시작
        c_time = StartCoroutine(CaveTimer());
        /// 몹 등장
        DistanceManager.instance.SwampStart();
    }


    /// <summary>
    /// [소탕] 입장할래 토벌할래 팝업에서 [소탕] 누르면 작동
    /// </summary>
    void ClickedSoTangSwamp()
    {
        if (PlayerInventory.ticket_cave_clear < 1)
        {
            PopUpManager.instance.ShowGrobalPopUP(3);
            return;
        }
        /// 소탕권 소모
        PlayerInventory.SetTicketCount("cave_clear", -1);
        /// 입장권 새로고침
        RefreshMoney();
        /// 확인 버튼으로 바꿔줌
        BtnLayoutSo[0].SetActive(true);
        BtnLayoutSo[1].SetActive(false);
        /// 지금 스테이지 몇이지? currentMyDan
        /// currentMyDan
        /// 광고시청 홍보
        sotangAdsText.text = LeanLocalization.GetTranslationText("StayReword_Description");
        /// 광고 스킵권 구매했음? 광고 아이콘 바꾸기
        if (PlayerInventory.isSuperUser != 0)
        {
            AdsIcon.sprite = AdsSprite[1];
        }
        else if(AdsIcon.sprite != AdsSprite[0])
        {
            AdsIcon.sprite = AdsSprite[0];
        }
        ///팝업 표시
        getpop.SetActive(true);
    }
    /// <summary>
    /// [소탕] 광고 안보고 받기
    /// </summary>000000
    public void NoAdsSotang()
    {
        /// 숨겨진 늪지 소탕하기
        if (PlayerPrefsManager.currentTutoIndex == 26)
        {
            ListModel.Instance.TUTO_Update(26);
        }

        /// 광고 안보고 소탕 1배
        GetSotangReword(false);
    }

    Coroutine c_time;

    /// <summary>
    /// 나가기 버튼 눌렀을때
    /// </summary>
    public void ExitCaveRun()
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
        /// 늪지 토벌을 종료하시겠습니까?
        pop22GetSwampReword.GetChild(0).gameObject.SetActive(false);
        pop22GetSwampReword.GetChild(1).gameObject.SetActive(true);
        pop22GetSwampReword.gameObject.SetActive(true);
    }

    int _KC;
    /// <summary>
    /// 늪지 토벌을 종료하시겠습니까? 확인 누르면 
    /// </summary>
    public void ClickedRealExit()
    {
        /// 광고 스킵권 구매했음? 광고 아이콘 바꾸기
        if (PlayerInventory.isSuperUser != 0)
        {
            AdsIcon2.sprite = AdsSprite[1];
        }
        else if (AdsIcon2.sprite != AdsSprite[0])
        {
            AdsIcon2.sprite = AdsSprite[0];
        }
        /// 광고시청 홍보
        clearAdsDesc.text = LeanLocalization.GetTranslationText("StayReword_Description");
        /// 킬 카운터
        _KC = PlayerPrefsManager.swampMonKillCount;
        ///최고 기록 갱신
        if (_KC > int.Parse(ListModel.Instance.swampCaveData[currentMyDan - 1].killCount))
        {
            ListModel.Instance.swampCaveData[currentMyDan - 1].killCount = _KC.ToString("F0");
        }
        /// 현재까지 보상 지급 텍스트 세팅
        clearLeaf.text = "x " + PlayerPrefsManager.instance.DoubleToStringNumber(ListModel.Instance.swampCaveData[currentMyDan - 1].rewordLeaf * (_KC * 1.0f * (float)PlayerInventory.Player_Leaf_Earned));
        clearEnchant.text = "x " + PlayerPrefsManager.instance.DoubleToStringNumber(ListModel.Instance.swampCaveData[currentMyDan - 1].rewordEnchant * (_KC * 1.0f * (float)PlayerInventory.EnchantStone_Earned));
        ///
        BtnLayoutTo[0].SetActive(true);
        BtnLayoutTo[1].SetActive(false);
        pop22GetSwampReword.GetChild(0).gameObject.SetActive(true);
        pop22GetSwampReword.GetChild(1).gameObject.SetActive(false);
        /// 타이머 코루틴 정지
        if (c_time != null) StopCoroutine(c_time);
        c_time = null;
        /// 팝업 상자 x 없애기
        x_Btn.SetActive(false);
        /// 공격 정지
        PlayerPrefsManager.isSwampTimeOver = true;
        PlayerPrefsManager.isEnterTheMine = true;
        DistanceManager.instance.SwampStopPlayer();
        /// 메인 브금 재생
        AudioManager.instance.PlayAudio("Main", "BGM");
    }

    /// <summary>
    /// 토벌 보상 받을거야? [받기] 누르면 배틀 그라운드로 돌아가기
    /// </summary>
    public void ClickedBackBattle()
    {
        /// 배경 바꿔주기
        bg.ChageBG_Render(0);
        /// 게임 오브젝트 덮기
        bott[0].SetActive(true);
        bott[1].SetActive(false);
        middle[0].SetActive(true);
        middle[1].SetActive(false);
        /// 본래 공격 몬스터로 돌려보내줌
        PlayerPrefsManager.isEnterTheMine = false;
        DistanceManager.instance.StopPlayer();
        /// 클릭 아이콘들 다시 보여줘
        for (int i = 0; i < UI_TopBotCanvas.Length; i++)
        {
            UI_TopBotCanvas[i].gameObject.SetActive(true);
        }
        /// 튜토리얼 노란 박스 싹다 클리어했으면 표시 안함
        if (PlayerPrefsManager.isTutoAllClear)
        {
            UI_TopBotCanvas[3].gameObject.SetActive(false);
        }
        ///
        CaveDragonFire[0].GetChild(0).gameObject.SetActive(true);
        CaveDragonFire[0].GetChild(1).gameObject.SetActive(false);
        CaveDragonFire[1].GetChild(1).gameObject.SetActive(true);
        /// 보스 버튼이 살아 있니?
        if (PlayerPrefsManager.isBossBtnAlive)
        {
            CaveDragonFire[1].GetChild(3).gameObject.SetActive(true);
        }
        swampTimeBar.SetActive(false);
        /// 실제 보상 지급
        GetClearReword(false);
        /// 보급상자 다시 켜준다
        SupplyBoxPos.SetActive(true);
        Debug.LogWarning("늪지 나가면 현재거리" + PlayerInventory.RecentDistance);
    }


    public void BugFixer()
    {
        PlayerPrefsManager.isEnterTheSwamp = false;
        DistanceManager.instance.StopPlayer();
    }


    bool _isEasySunDaLeft;
    /// <summary>
    /// 정말로 할거냐 물어보는 팝업에 붙어
    /// </summary>
    /// <param name="_isLeft"></param>
    public void EasySunDa(bool _isLeft)
    {
        _isEasySunDaLeft = _isLeft;
        /// 팝업 호출
        if (_isLeft)
        {
            easyRellyText.text = LeanLocalization.GetTranslationText("Enter_The_Swapm_Enter");
            PlayerPrefsManager.isEnterTheSwamp = true;
        }
        else
        {
            /// 소탕 기록이 있으면 소탕권 사용하실? 메세지
            if (int.Parse(ListModel.Instance.swampCaveData[currentMyDan - 1].killCount) > 0)
            {
                easyRellyText.text = LeanLocalization.GetTranslationText("Enter_The_Swapm_Sotang");
            }
            /// 소탕 기록 없다 = 킬카운터 0 이다 기록없음 팝업.
            else
            {
                easyRellyText.text = LeanLocalization.GetTranslationText("Enter_The_Swapm_Norecord");
            }
        }
        EasySunDaPop.SetActive(true);
    }
    public void InvoEasySun()
    {

        if(_isEasySunDaLeft)
        {
            if (PlayerInventory.ticket_cave_enter < 1)
            {
                PopUpManager.instance.ShowGrobalPopUP(2);
            }
            else
            {
                SystemPopUp.instance.LoopLoadingImg();
                /// 입장한다
                PlayerPrefsManager.isEnterTheMine = true;
                ClickedEnterSwamp();
                return;
            }
        }
        else
        {
            /// 소탕 기록이 있으면 소탕권 사용하실? 메세지
            if (int.Parse(ListModel.Instance.swampCaveData[currentMyDan - 1].killCount) < 1)
            {
                /// 팝업 꺼줌
            }
            else
            {
                ClickedSoTangSwamp();
            }
        }
        /// 팝업 꺼줌
        EasySunDaPop.SetActive(false);

    }


    /// <summary>
    /// 재화 더해주는 메서드 (광고 보면 2배 지급 / 안보면 1배 )
    /// </summary>
    /// <param name="_isAdsWatch"></param>
    public void GetClearReword(bool _isAdsWatch)
    {
        var tmpLeaf = Mathf.RoundToInt(ListModel.Instance.swampCaveData[currentMyDan - 1].rewordLeaf * (_KC * (float)PlayerInventory.Player_Leaf_Earned));
        var tmpES = Mathf.RoundToInt(ListModel.Instance.swampCaveData[currentMyDan - 1].rewordEnchant * (_KC * (float)PlayerInventory.EnchantStone_Earned));
        //
        if (_isAdsWatch)
        {
            PlayerInventory.Money_Leaf += tmpLeaf  * 2 ;
            PlayerInventory.Money_EnchantStone += tmpES * 2;
            /// 나뭇잎 획득량 업적 올리기
            ListModel.Instance.ALLlist_Update(4, tmpLeaf * 2);
            ///  강화석 업적 카운트 올리기
            ListModel.Instance.ALLlist_Update(5, tmpES * 2);
        }
        else
        {
            PlayerInventory.Money_Leaf += tmpLeaf;
            PlayerInventory.Money_EnchantStone += tmpES;
            /// 나뭇잎 획득량 업적 올리기
            ListModel.Instance.ALLlist_Update(4, tmpLeaf);
            ///  강화석 업적 카운트 올리기
            ListModel.Instance.ALLlist_Update(5, tmpES);
        }
        /// 배틀 씬에서 골드/대미지 다시 보여주기
        DamegeDropPos.SetActive(true);
        PlayerPrefsManager.isGoldposOnAir = false;
    }
    /// <summary>
    /// 재화 더해주는 메서드 (광고 보면 2배 지급 / 안보면 1배 )
    /// </summary>
    /// <param name="_isAdsWatch"></param>
    public void GetSotangReword(bool _isAdsWatch)
    {
        float _KC = float.Parse(ListModel.Instance.swampCaveData[currentMyDan - 1].killCount);
        //
        var tmpLeaf = Mathf.RoundToInt(ListModel.Instance.swampCaveData[currentMyDan - 1].rewordLeaf * (_KC * (float)PlayerInventory.Player_Leaf_Earned));
        var tmpES = Mathf.RoundToInt(ListModel.Instance.swampCaveData[currentMyDan - 1].rewordEnchant * (_KC * (float)PlayerInventory.EnchantStone_Earned));
        //
        if (_isAdsWatch)
        {
            PlayerInventory.Money_Leaf += tmpLeaf * 2;
            PlayerInventory.Money_EnchantStone += tmpES * 2;
            /// 나뭇잎 획득량 업적 올리기
            ListModel.Instance.ALLlist_Update(4, tmpLeaf * 2);
            ///  강화석 업적 카운트 올리기
            ListModel.Instance.ALLlist_Update(5, tmpES * 2);
        }
        else
        {
            PlayerInventory.Money_Leaf += tmpLeaf;
            PlayerInventory.Money_EnchantStone += tmpES;
            /// 나뭇잎 획득량 업적 올리기
            ListModel.Instance.ALLlist_Update(4, tmpLeaf);
            ///  강화석 업적 카운트 올리기
            ListModel.Instance.ALLlist_Update(5, tmpES);
        }
    }


    float currentTime;
    IEnumerator CaveTimer()
    {
        yield return new WaitForSeconds(1f / PlayerInventory.Player_Move_Speed);

        float Maxcnt = (float)PlayerInventory.Cave_Time;
        currentTime = Maxcnt;
        //TimerFillamount.text = "남은 시간 : " + string.Format("{0:f1}", Maxcnt);
        while (currentTime > 0)
        {
            yield return new WaitForFixedUpdate();
            /// 타이머는 정지 전까지 계속 돌아가
            currentTime -= Time.deltaTime;
            Debug.Log("남은 시간 : " + currentTime.ToString("F1"));
            TimeFill.fillAmount = currentTime / Maxcnt;
        }
        /// 팝업 상자 x 없애기
        x_Btn.SetActive(false);
        /// 광고 스킵권 구매했음? 광고 아이콘 바꾸기
        if (PlayerInventory.isSuperUser != 0)
        {
            AdsIcon.sprite = AdsSprite[1];
        }
        else if (AdsIcon.sprite != AdsSprite[0])
        {
            AdsIcon.sprite = AdsSprite[0];
        }
        pop22GetSwampReword.gameObject.SetActive(true);
        /// 시간 지나면 무조건 타임오버
        PlayerPrefsManager.isSwampTimeOver = true;
        /// 몬스터 죽어
        DistanceManager.instance.SwampStopPlayer();
        if (c_time != null) StopCoroutine(c_time);
        c_time = null;
        /// 보상지급
        ClickedRealExit();
    }





    #region <Rewarded Ads> 숨겨진 늪지 동영상 광고
    bool isSotangAds;
    public void Ads_Sotang()
    {
        SystemPopUp.instance.LoopLoadingImg();
        isSotangAds = true;
        if (PlayerInventory.isSuperUser != 0)
        {
            /// 광고 스킵
            _AdsComp = true;
            /// 광고 문구 집어 넣기 Config_Ads_Okay
            sotangAdsText.text = LeanLocalization.GetTranslationText("Config_Ads_Okay");
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
            /// 광고 없음 문구 집어 넣기 Config_Ads_Nope
            sotangAdsText.text = LeanLocalization.GetTranslationText("Config_Ads_Nope");
            Invoke(nameof(AdsInvo), 0.5f);
        }
    }


    /// <summary>
    /// 실제 광고를 봤다면 true
    /// </summary>
    bool _AdsComp;
    public void Ads_FreeDiaCanvas()
    {
        SystemPopUp.instance.LoopLoadingImg();

        if (PlayerInventory.isSuperUser != 0)
        {
            /// 광고 스킵
            _AdsComp = true;
            /// 광고 문구 집어 넣기 Config_Ads_Okay
            clearAdsDesc.text = LeanLocalization.GetTranslationText("Config_Ads_Okay");
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
            /// 광고 없음 문구 집어 넣기 Config_Ads_Nope
            clearAdsDesc.text = LeanLocalization.GetTranslationText("Config_Ads_Nope");
            Invoke(nameof(AdsInvo), 0.5f);
        }

    }
    // Event handler called when a rewarded ad has completed
    void AdsCompleated(RewardedAdNetwork network, AdPlacement location)
    {
        _AdsComp = true;
        /// 광고 문구 집어 넣기 Config_Ads_Okay
        clearAdsDesc.text = LeanLocalization.GetTranslationText("Config_Ads_Okay");
        sotangAdsText.text = LeanLocalization.GetTranslationText("Config_Ads_Okay");
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
    /// 광고 보상 지급 메소드
    /// </summary>
    void AdsInvo()
    {
        ///  광고 1회 시청 완료 카운트
        ListModel.Instance.ALLlist_Update(0, 1);
        /// 광고 시청 일일 업적
        ListModel.Instance.DAYlist_Update(7);

        if (isSotangAds)
        {
            /// 현재까지 보상 지급 2배 텍스트 세팅
            sotangLeaf.text = "x " + PlayerPrefsManager.instance.DoubleToStringNumber(ListModel.Instance.swampCaveData[currentMyDan - 1].rewordLeaf * (_KC * 2.0f * (float)PlayerInventory.Player_Leaf_Earned));
            sotangEnchant.text = "x " + PlayerPrefsManager.instance.DoubleToStringNumber(ListModel.Instance.swampCaveData[currentMyDan - 1].rewordEnchant * (_KC * 2.0f * (float)PlayerInventory.EnchantStone_Earned));
            /// 2배로 지급
            GetSotangReword(true);
            /// 확인 버튼으로 바꿔줌
            BtnLayoutSo[0].SetActive(false);
            BtnLayoutSo[1].SetActive(true);
        }
        else
        {
            /// 현재까지 보상 지급 2배 텍스트 세팅
            clearLeaf.text = "x " + PlayerPrefsManager.instance.DoubleToStringNumber(ListModel.Instance.swampCaveData[currentMyDan - 1].rewordLeaf * (_KC * 2.0f * (float)PlayerInventory.Player_Leaf_Earned));
            clearEnchant.text = "x " + PlayerPrefsManager.instance.DoubleToStringNumber(ListModel.Instance.swampCaveData[currentMyDan - 1].rewordEnchant * (_KC * 2.0f * (float)PlayerInventory.EnchantStone_Earned));
            /// 2배로 지급
            GetClearReword(true);
            /// 확인 버튼으로 바꿔줌
            BtnLayoutTo[0].SetActive(false);
            BtnLayoutTo[1].SetActive(true);
        }

        _AdsComp = false;
        isSotangAds = false;
        SystemPopUp.instance.StopLoopLoading();
    }

    #endregion


}
