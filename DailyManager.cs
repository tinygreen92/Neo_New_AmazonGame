using System;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.Storage;

public class DailyManager : MonoBehaviour
{
    public StayRewordManager SRM;
    public NanooManager nm;
    [Header("-획득 팝업 부분")]
    public GameObject getPopUp;
    public Image iconImg;
    public Text getItemAmount;
    public Sprite[] mainSprs;

    [Header("-메인 팝업 부분")]
    public GameObject invisibleDragon;
    public Text cheackCountText;
    public Transform[] btnPanelChild;
    /// <summary>
    /// 앱 껐다가 다시 켰을때 체크
    /// </summary>
    private bool isFristRun;
    private string sigan;

    // 조작 불가 타이머 스탬프
    private DateTime dailyEndTimestamp;
    private TimeSpan dailydRemaining;
    void SaveDateTime(DateTime dateTime)
    {
        string tmp = dateTime.ToString("yyyyMMddHHmmss");
        ObscuredPrefs.SetString("Check_Daily", tmp);
        ObscuredPrefs.Save();
    }

    private DateTime LoadDateTime()
    {
        if (!ObscuredPrefs.HasKey("Check_Daily"))
        {
            /// 처음이면 
            var tmpTime = UnbiasedTime.Instance.Now().Date.AddDays(1);
            /// 늪지 / 채굴권 2장 충전
            PlayerInventory.SetTicketCount("cave_enter", 2);
            PlayerInventory.SetTicketCount("mining", 2);
            /// 저장해주구려
            SaveDateTime(tmpTime);
            return tmpTime; //내일 0시 반환.
        }
        string data = ObscuredPrefs.GetString("Check_Daily");
        return DateTime.ParseExact(data, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 플레이 팹 로그인 확인되면 호출 - > 출석판 초기화.
    /// </summary>
    public void CheckDailyInit()
    {
        /// "남은 시간" <- 번역
        sigan = Lean.Localization.LeanLocalization.GetTranslationText("DailyReword_Timer");
        /// 게임 처음 키면우편함 체크
        nm.PostboxCheck();
        /// 뺑뺑이 종료
        SystemPopUp.instance.StopLoopLoading();
        // 마지막 종료 시간
        dailyEndTimestamp = LoadDateTime();
        // 값 없으면 다음날 0시 시간. / 저장된 값
        dailydRemaining = dailyEndTimestamp - UnbiasedTime.Instance.Now();

        if (dailydRemaining.TotalSeconds < 0)
        {
            /// 최신값이 바뀌었다 = 날짜가 바뀌었다 = 출석체크 가능
            ResetDailyQuest();
        }
        else
        {
            /// 출석판 키면 레드닷 OFF
            if (!RedDotManager.instance.RedDot[1].activeSelf)
            {
                RedDotManager.instance.RedDot[2].SetActive(false);
            }
            RedDotManager.instance.RedDot[4].SetActive(false);
        }
        /// ------------------------- 출석체크 -------------------------------
        //아직 출석 안함 - >  1. 출석창 호출 안함 2. 출석 회색처리
        if (!PlayerPrefsManager.isDailyCheak)
        {
            /// 출석판 캔버스 드러내기.
            PopUpManager.instance.ShowPopUP(3);
            /// 클릭 버튼 활성화.
            invisibleDragon.SetActive(true);
        }
        else if (PlayerPrefsManager.DailyCount_Cheak >= 25) // 마지막 출석한지 하루 지나면?
        {
            PlayerPrefsManager.DailyCount_Cheak = 0;
        }

        /// 버튼 오브젝트에 GO 배열 만들고 ALL OFF
        for (int i = PlayerPrefsManager.DailyCount_Cheak; i < btnPanelChild.Length; i++)
        {
            btnPanelChild[i].GetChild(1).gameObject.SetActive(false); // 출첵완료 이미지 벗기기
        }
        /// ture 면 update문 실행
        isFristRun = true;

    }

    bool isSave;
    private void Update()
    {
        if (!isFristRun) return;

        dailydRemaining = dailyEndTimestamp - UnbiasedTime.Instance.Now();
        /// 3600초마다 저장 = 정각 마다 저장
        if (!isSave && dailydRemaining.Minutes == 59)
        {
            PlayerPrefsManager.instance.TEST_SaveJson();
            Debug.LogError("3600초 저장! : " + dailydRemaining.Minutes);
            isSave = true;
            Invoke(nameof(InvoSaveTrigger), 10.0f);
        }

        /// 24시간 카운트가 계속 돌아가는 상태 = 아직 날짜 안지남. / 출첵도 함.
        if (dailydRemaining.TotalSeconds > 0)
        {
            cheackCountText.text = sigan + " : " + string.Format("{0:00}:{1:00}:{2:00}", dailydRemaining.Hours, dailydRemaining.Minutes, dailydRemaining.Seconds);
        }
        else // 카운트 0미만 이면 하루 지났다 / 아님 출석 안했다.
        {
            // 날짜 바뀌면 체크.
            ResetDailyQuest();
            /// 업데이트 문 탈출
            //isFristRun = false;
        }
    }

    void InvoSaveTrigger()
    {
        isSave = false;
    }


    bool isGetMainItem;

    /// <summary>
    /// 해당 아이템 갯수 반환
    /// </summary>
    /// <returns></returns>
    private string[] GetItemAmountTxt(int _index)
    {
        string[] result = new string[2];
        switch (_index)
        {
            case 0: result[0] = "x2"; result[1] = "x1"; break;
            case 1: result[0] = "x2"; result[1] = "x1"; break;
            case 2: result[0] = "x1"; result[1] = "x1"; break;
            case 3: result[0] = "x2"; result[1] = "x1"; break;
            case 4: result[0] = "x100"; result[1] = "x1"; break;

            case 5: result[0] = "x2"; result[1] = "x1"; break;
            case 6: result[0] = "x2"; result[1] = "x1"; break;
            case 7: result[0] = "x1"; result[1] = "x1"; break;
            case 8: result[0] = "x2"; result[1] = "x1"; break;
            case 9: result[0] = "x100"; result[1] = "x1"; break;

            case 10: result[0] = "x2"; result[1] = "x1"; break;
            case 11: result[0] = "x2"; result[1] = "x1"; break;
            case 12: result[0] = "x1"; result[1] = "x1"; break;
            case 13: result[0] = "x2"; result[1] = "x1"; break;
            case 14: result[0] = "x100"; result[1] = "x1"; break;

            case 15: result[0] = "x2"; result[1] = "x1"; break;
            case 16: result[0] = "x2"; result[1] = "x1"; break;
            case 17: result[0] = "x1"; result[1] = "x1"; break;
            case 18: result[0] = "x2"; result[1] = "x1"; break;
            case 19: result[0] = "x100"; result[1] = "x1"; break;

            case 20: result[0] = "x5"; result[1] = "x1"; break;
            case 21: result[0] = "x4"; result[1] = "x5"; break;
            case 22: result[0] = "x2"; result[1] = "x5"; break;
            case 23: result[0] = "x4"; result[1] = "x5"; break;
            case 24: result[0] = "x200"; result[1] = "x5"; break;

            default:break;
        }

        return result;
    }

    private void GetRealPresent(int _value)
    {
        ///아마존 결정 지급
        if (_value < 20) nm.PostboxDailySend("stone", 1);
        else nm.PostboxDailySend("stone", 5);
        /// 나머지 지급
        switch (_value)
        {
            case 0: nm.PostboxDailySend("weapon_coupon", 2); break;
            case 1: nm.PostboxDailySend("cave", 2); break;
            case 2: nm.PostboxDailySend("reinforce_box", 1); break;
            case 3: nm.PostboxDailySend("cave_clear", 2); break;
            case 4: nm.PostboxDailySend("diamond", 100); break;

            case 5: nm.PostboxDailySend("weapon_coupon", 2); break;
            case 6: nm.PostboxDailySend("cave", 2); break;
            case 7: nm.PostboxDailySend("leaf_box", 1); break;
            case 8: nm.PostboxDailySend("cave_clear", 2); break;
            case 9: nm.PostboxDailySend("diamond", 100); break;

            case 10: nm.PostboxDailySend("weapon_coupon", 2); break;
            case 11: nm.PostboxDailySend("cave", 2); break;
            case 12: nm.PostboxDailySend("reinforce_box", 1); break;
            case 13: nm.PostboxDailySend("cave_clear", 2); break;
            case 14: nm.PostboxDailySend("diamond", 100); break;

            case 15: nm.PostboxDailySend("weapon_coupon", 2); break;
            case 16: nm.PostboxDailySend("cave", 2); break;
            case 17: nm.PostboxDailySend("leaf_box", 1); break;
            case 18: nm.PostboxDailySend("cave_clear", 2); break;
            case 19: nm.PostboxDailySend("diamond", 100); break;

            case 20: nm.PostboxDailySend("weapon_coupon", 5); break;
            case 21: nm.PostboxDailySend("cave", 4); break;
            case 22: nm.PostboxDailySend("elixr", 2); break;
            case 23: nm.PostboxDailySend("cave_clear", 4); break;
            case 24: nm.PostboxDailySend("diamond", 200); break;

            default: break;
        }
    }

    /// <summary>
    ///  판 다 덮은 투명한 버튼 클릭하면 자동으로 출석체크되게 한다. 
    /// </summary>
    public void ClickedInvisibleBtn()
    {
        btnPanelChild[PlayerPrefsManager.DailyCount_Cheak].GetChild(1).gameObject.SetActive(true);
        
        ObscuredPrefs.SetInt("DailyCount_Cheak", ++PlayerPrefsManager.DailyCount_Cheak);
        ObscuredPrefs.SetInt("isDailyCheak", 525);
        PlayerPrefsManager.isDailyCheak = true;
        ObscuredPrefs.Save();
        //
        invisibleDragon.SetActive(false);
        /// 두번째 아이템까지 받는 트리거 초기화
        isGetMainItem = false;
        ///

        /// 획득 팝업 부분 -> 인덱스 0은 아마존 결정 고정
        iconImg.sprite = mainSprs[PlayerPrefsManager.DailyCount_Cheak];
        getItemAmount.text = GetItemAmountTxt(PlayerPrefsManager.DailyCount_Cheak - 1)[0];
        getPopUp.SetActive(true);
        /// 해당 날짜 보상 지급
        GetRealPresent(PlayerPrefsManager.DailyCount_Cheak - 1);
    }

    public void ClickedGetPopOk()
    {
        /// 두번째 아이템까지 받았으면 꺼져
        if (isGetMainItem) getPopUp.SetActive(false);
        //
        iconImg.sprite = mainSprs[0];
        getItemAmount.text = GetItemAmountTxt(PlayerPrefsManager.DailyCount_Cheak - 1)[1];
        isGetMainItem = true;
    }


    /// <summary>
    /// 출석판 클릭할 때도 호출하기
    /// </summary>
    public void ChageLang()
    {
        sigan = Lean.Localization.LeanLocalization.GetTranslationText("DailyReword_Timer");
        /// 날짜 지나고 빨간 색 True이면 ??
        if (RedDotManager.instance.RedDot[4].activeSelf)
        {
            CheckDailyInit();
        }
        else
        {
            PopUpManager.instance.ShowPopUP(3);
        }

    }

    /// <summary>
    /// 날짜 바뀌었을때 체크 해준다.
    /// </summary>
    void ResetDailyQuest()
    {
        Debug.LogError("!!!!! Quest Reset !!!!!");
        // 다음날 0시 시간. /  최신값
        DateTime currentTime = UnbiasedTime.Instance.Now().Date.AddDays(1);
        // 최신값 세이브
        SaveDateTime(currentTime);
        dailyEndTimestamp = currentTime;
        /// 늪지 / 채굴권 2장 충전
        PlayerInventory.SetTicketCount("cave_enter", 2);
        PlayerInventory.SetTicketCount("mining", 2);

        ///  TODO : 일일 퀘스트 초기화. 
        for (int i = 0; i < 6; i++)
        {
            ListModel.Instance.DAYlist_Update(i, 0);
        }
        /// 출석체크 판 초기화.
        if (PlayerPrefsManager.DailyCount_Cheak >= 25)
        {
            PlayerPrefsManager.DailyCount_Cheak = 0;
            for (int i = 0; i < btnPanelChild.Length; i++)
            {
                btnPanelChild[i].GetChild(1).gameObject.SetActive(false); // 출첵완료 이미지
            }
        }
        /// 접속시간 보상 스택 0으로 초기화.
        SRM.ResetRewordStack();
        PlayerPrefsManager.AmaAdsTimer = 0;
        PlayerPrefsManager.FreeDiaCnt = 0;
        PlayerPrefsManager.FreeWeaponCnt = 0;
        //
        ObscuredPrefs.SetInt("isDailyCheak", 0);
        PlayerPrefsManager.isDailyCheak = false;
        ObscuredPrefs.Save();
        /// 날짜 지나면 레드닷 활성화
        RedDotManager.instance.RedDot[2].SetActive(true);
        RedDotManager.instance.RedDot[4].SetActive(true);
        /////-> 다시 24시간 카운터 재시작
        //Invoke(nameof(CheckDailyInit), 0.5f);
    }



    /// <summary>
    /// TEST TEST 다음 날짜로 옮기는 클릭
    /// </summary>
    public void NextTimeBaby()
    {
        ResetDailyQuest();
    }


}
