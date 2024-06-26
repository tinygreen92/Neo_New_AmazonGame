﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using System.Collections;

enum EnumNumber
{
    num_A = 4, // 네자리 수 부터
    num_B = 7,
    num_C = 10,
    num_D = 13,
    num_E = 16,
    num_F = 19,
    num_G = 22,
    num_H = 25,
    num_I = 28,
    num_J = 31,
    num_K = 34,
    num_L = 37,
    num_M = 40,
    num_N = 43,
    num_O = 46,
    num_P = 49,
    num_Q = 52,
    num_R = 55,
    num_S = 58,
    num_T = 61,
    num_U = 64,
    num_V = 67,
    num_W = 70,
    num_X = 73,
    num_Y = 76,
    num_Z = 79,
    num_AA = 82,
    num_AB = 85,
    num_AC = 88,
    num_AD = 91,
    num_AE = 94,
    num_AF = 97,
    num_AG = 100,
    num_AH = 103,
    num_AI = 106,
    num_AJ = 109,
    num_AK = 112,
    num_AL = 115,
    num_AM = 118,
    num_AN = 121,
    num_AO = 124,
    num_AP = 127,
    num_AQ = 130,
    num_AR = 133,
    num_AS = 136,
    num_AT = 139,
    num_AU = 142,
    num_AV = 145,
    num_AW = 148,
    num_AX = 151,
    num_AY = 154,
    num_AZ = 157,
    num_BA = 160,
    num_BB = 163,
    num_BC = 166,
    num_BD = 169,
    num_BE = 172,
    num_BF = 175,
    num_BG = 178,
    num_BH = 181,
    num_BI = 184,
    num_BJ = 187,
    num_BK = 190,
    num_BL = 193,
    num_BM = 196,
    num_BN = 199,
    num_BO = 202,
    num_BP = 205,
    num_BQ = 208,
    num_BR = 211,
    num_BS = 214,
    num_BT = 217,
    num_BU = 220,
    num_BV = 223,
    num_BW = 226,
    num_BX = 229,
    num_BY = 232,
    num_BZ = 235,
    num_CA = 238,
    num_CB = 241,
    num_CC = 244,
    num_CD = 247,
    num_CE = 250,
    num_CF = 253,
    num_CG = 256,
    num_CH = 259,
    num_CI = 262,
    num_CJ = 265,
    num_CK = 268,
    num_CL = 271,
    num_CM = 274,
    num_CN = 277,
    num_CO = 280,
    num_CP = 283,
    num_CQ = 286,
    num_CR = 289,
    num_CS = 292,
    num_CT = 295,
    num_CU = 298,
    num_CV = 301,
    num_CW = 304,
    num_CX = 307,
    num_CY = 310,
    num_CZ = 313
}

public class PlayerPrefsManager : MonoBehaviour
{


    public void zJObjectSaveNew()
    {
        // JObject를 Serialize하여 json string 생성 
        string savestring = JsonConvert.SerializeObject(ListModel.Instance.nonSaveJsonMoney);
        File.WriteAllText(Application.persistentDataPath + "/_Mdsvp_.txt", savestring); // 생성된 string을  _data_ 파일에 쓴다 
    }



    public SupportManager sm;
    /// <summary>
    /// ㄴㄴㄴ
    /// </summary>
    //public TextAsset pie;
    /// <summary>
    /// 210117Update 늪지 업데이트 파일
    /// </summary>
    public TextAsset tuna;
    [Space]
    /// <summary>
    /// 오리지날 파일
    /// </summary>
    public TextAsset mayo;
    [Header("- 무기 애니메이션 뭉태기")]
    public GameObject[] WeaponAnims;
    [Header("- 매니저들")]
    public TutoManager tm;
    public BuffManager bm;
    public ModelHandler mh;

    /// <summary>
    /// 오디오 올 뮤트
    /// </summary>
    public static ObscuredBool isAllmute;                           /// 오디오 매니저 입닥쳐

    public static ObscuredBool isNickNameComp;                  /// 닉네임 정해졌니?
    public static ObscuredBool isLoadingComp;                  /// 플레이팹 세팅 다됐니?
    public static ObscuredBool isTutorialClear;                  /// 첫 튜토리얼 완료했니?
    public static ObscuredBool isGetOfflineReword;                  /// 오프라인 보상 수령 했니?

    public static ObscuredBool isSuperBox;                  /// 슈퍼 박스 돈 떨어지니?

    public static ObscuredBool isRuneInit;                  /// 룬 카테고리 초기화 했니?
    public static ObscuredBool isRuneFussionTab;                  /// 룬 카테고리 조합창이니?
    public static ObscuredBool isPetDiaTab;                              /// 펫 카테고리 다이아 탭이니?

    public static ObscuredBool isRefreshHeart;                  /// 유물 뽑기하고 컨텐츠 뷰 갱신되면 글로우 효과 나타나게 할때

    public static ObscuredInt storeIndex;                  /// /////////////////// 상점 3군데 분산 시킬거임

    public static ObscuredInt charaUpgrageMutiple;                  /// 캐릭터 카테고리 멀티플 +1 +10 +100
    public static ObscuredInt heartUpgrageMutiple;                  /// 유물 카테고리 멀티플 +1 +10 +100
    /// <summary>
    /// 스트링 하나로 묶어서 서버에 올리기
    /// </summary>
    //private string[] tunamayo;

    /// /////////////////// =---------------- 암호화
    /// 
    public static ObscuredInt DailyCount_Cheak;                            // 출석체크 며칠 했니?
    public static ObscuredBool isDailyCheak;                            // 오늘 자 출석체크 했니?
    public static ObscuredInt currentMyStage;                           // 최근 클리어한 스테이지 몇?
    public static ObscuredInt swampMonKillCount;                           // 현재 스왐피 킬 카운터
    public static ObscuredBool isSwampTimeOver;                           // 현재 스왐피 킬 카운터
    public static ObscuredBool isTutoAllClear;                           // 모든 튜토리얼 클리어
    public static ObscuredInt ZogarkMissionCnt;                           // 아마존 결정 샵 맥스치
    public static ObscuredInt AmaAdsTimer;                           // 아마존 결정 상점 10회 제한
    public static ObscuredInt FreeDiaCnt;                           // 무료 다이아 10회 제한
    public static ObscuredInt FreeWeaponCnt;                           // 무료 무기 뽑기 10회 제한

    /// 입장 / 소탕 제한
    public static ObscuredInt SwampyEnterCnt;                           // 늪지 입장 5회 제한
    public static ObscuredInt SwampySkipCnt;                           // 늪지 소탕 5회 제한


    ///  ---------------------------안 암호화.

    public static ObscuredBool isJObjectLoad;                   // json 로딩 완료
    public static ObscuredBool isEnterTheMine;                            // 혹시 광산에 입장했니?
    public static ObscuredBool isEnterTheSwamp;                            // 혹시 늪지에 입장했니?
    public static ObscuredBool isGoldposOnAir;                            // 늪지 있을때는 돈 안 떨구기 나가면 돈 떨구기
    public static ObscuredBool isBossBtnAlive;                           // 화면 전환 후 배틀 캔버스에 보스 도전버튼 표기 되어야함
    public static ObscuredBool isIdleModeOn;                   // 지금 방치모드 켜져있니?
    public static ObscuredBool isCheckOffline;                   // 오프라인 팝업 띄웠니?

    /// <summary>
    /// 이 데이터 파일이 진짜 구글 로그인 계정 데이터냐?
    /// </summary>
    public static ObscuredString CursedId;


    /// <summary>
    /// 공지사항 스트링 저장용
    /// </summary>
    public String CH_NOTICE;

    /// <summary>
    /// 튜토리얼에서 사용하는 어디까지 클리어했니 인덱스
    /// </summary>
    public static ObscuredInt currentTutoIndex;

    /// <summary>
    /// 섹터 5 없는 경우
    /// </summary>
    public static bool isMissingFive;

    /// <summary>
    /// 무기 장착하면 애니메이션 바꿔줌
    /// </summary>
    /// <param name="_index"></param>
    public void SetPlayerWeaponAnim(int _index)
    {
        for (int i = 0; i < WeaponAnims.Length; i++)
        {
            WeaponAnims[i].SetActive(false);
        }
        WeaponAnims[_index].SetActive(true);
    }


    public static PlayerPrefsManager instance;
    private void Awake()
    {
        /// 개발자 모드일때만 로컬 저장 버튼 보이게
        if (StartManager.instance.isDeerveloperMode)
            SafySaveBtn.SetActive(true);

        //tunamayo = new string[21];
        instance = this;
        EX_Start();
    }

    void OnApplicationQuit()
    {
        /* 앱이 종료 될 때 처리 */
        /// 서버에 저장하고 리셋하면 저장 하지 마
        if (!isResetAferSave)
        {
            TEST_SaveJson();
        }
    }

    bool isPaused;
    private void OnApplicationPause(bool pause)
    {
        if (!isJObjectLoad) return;
        /// 일시 정지 상태 진입
        if (pause)
        {
            isPaused = true;
            /// 후욱후욱
            isGetOfflineReword.RandomizeCryptoKey();
            isTutorialClear.RandomizeCryptoKey();
            isSuperBox.RandomizeCryptoKey();
            isRuneInit.RandomizeCryptoKey();
            isRuneFussionTab.RandomizeCryptoKey();
            isPetDiaTab.RandomizeCryptoKey();
            isRefreshHeart.RandomizeCryptoKey();
            storeIndex.RandomizeCryptoKey();
            charaUpgrageMutiple.RandomizeCryptoKey();
            heartUpgrageMutiple.RandomizeCryptoKey();
            DailyCount_Cheak.RandomizeCryptoKey();
            isDailyCheak.RandomizeCryptoKey();
            currentMyStage.RandomizeCryptoKey();
            swampMonKillCount.RandomizeCryptoKey();
            isSwampTimeOver.RandomizeCryptoKey();
            isTutoAllClear.RandomizeCryptoKey();
            ZogarkMissionCnt.RandomizeCryptoKey();
            AmaAdsTimer.RandomizeCryptoKey();
            FreeDiaCnt.RandomizeCryptoKey();
            FreeWeaponCnt.RandomizeCryptoKey();
            currentTutoIndex.RandomizeCryptoKey();
            SwampyEnterCnt.RandomizeCryptoKey();
            SwampySkipCnt.RandomizeCryptoKey();
        }
        else
        {
            if (isPaused)
            {
                isPaused = false;
            }
        }
    }

    IEnumerator OnApplicationFocus(bool focus)
    {
        if (isJObjectLoad)
        {
            if (focus)
            {
                yield return null;
                /// 로컬 저장
                TEST_SaveJson();
            }
        }
    }


    /// <summary>
    /// DataBoxCopy 에 변경가능한 수치만 저장
    /// tunamato[21]  는 건들지 않는다.
    /// </summary>
    public void TEST_SaveJson()
    {
        /// SetPrefs 대신 새로운 저장 방식
        DataBoxCopy.instance.SaveBox();
        DataBoxCopy.instance.SaveSeverBox();
        DataBoxCopy.instance.SaveDoctorWho();
        /// 디버그 모드일때만 로컬 버튼 감추기
        if (StartManager.instance.isDeerveloperMode)
            SafySaveBtn.SetActive(false);
        /// 너무 많이 불러오지 않게 텀 두기.
        if (isSafySave)
        {
            return;
        }
        else
        {
            isSafySave = true;
        }
        /// Json 파일 저장
        JObjectSave(ListModel.Instance.petList, 0);
        JObjectSave(ListModel.Instance.charatorList, 1);
        JObjectSave(ListModel.Instance.weaponList, 2);
        JObjectSave(ListModel.Instance.equipRuneList, 3);
        JObjectSave(ListModel.Instance.runeList, 4);
        JObjectSave(ListModel.Instance.invisibleruneList, 5);
        JObjectSave(ListModel.Instance.heartList, 6);
        JObjectSave(ListModel.Instance.invisibleheartList, 7);
        JObjectSave(ListModel.Instance.supList, 8);
        ///
        JObjectSave(ListModel.Instance.mvpDataList, 14);
        //
        JObjectSave(ListModel.Instance.missionDAYlist, 15);
        JObjectSave(ListModel.Instance.missionALLlist, 16);
        JObjectSave(ListModel.Instance.missionTUTOlist, 17);
        //
        JObjectSave(ListModel.Instance.mineCraft, 18);
        JObjectSave(ListModel.Instance.axeDataList, 19);
        //
        JObjectSave(ListModel.Instance.swampCaveData, 20);

        /// 테스트 세이브 버튼 위로 올려주기
        Invoke(nameof(SSSS), 1.5f);

        /// 아직 오프라인 보상 체크 하기 전이면 리턴
        if (!isCheckOffline)
            return;
        /// 오프라인 보상 체크 끝났다면 타이머 일괄적으로 저장
        string tmp = UnbiasedTime.Instance.Now().ToString("yyyyMMddHHmmss");
        ObscuredPrefs.SetString("DateTime", tmp);
        //ObscuredPrefs.SetString("AmazonShop", tmp);
        //ObscuredPrefs.SetString("Check_Daily", tmp);
        //ObscuredPrefs.SetString("FreeWeapon", tmp);
        //ObscuredPrefs.SetString("FreeDia", tmp);
        ObscuredPrefs.Save();
    }

    public GameObject SafySaveBtn;
    void SSSS()
    {
        isSafySave = false;
        /// 개발자 모드일 때만 다시 불러줘 
        if (StartManager.instance.isDeerveloperMode)
            SafySaveBtn.SetActive(true);
    }




    /// <summary>
    /// TEST 용 황금상자 확률 올리기
    /// </summary>
    /// 
    public static bool isGoldenDouble;

    public void TEST_isGoldenDouble()
    {
        isGoldenDouble = true;
    }


    public void TEST_Distance(float _dd)
    {
        PlayerInventory.RecentDistance = _dd;
    }










    /// <summary>
    /// TEST 용 황금상자 확률 올리기
    /// </summary>

    /// <summary>
    /// 30 / 30 개 표시하는 계산
    /// </summary>
    public static string MyHeart
    {
        get { return "유물 뽑기 ( " + ListModel.Instance.heartList.Count.ToString("D2") + " / 30 )"; }
    }

    [Header("-대미지 표기 캔버스")]
    public Transform topCanvas;

    [Header("-오른쪽 버튼 막기")]
    public Button[] RightEnterBtn;
    /// <summary>
    /// 오른쪽 버튼 true / false
    /// </summary>
    /// <param name="_Triger"></param>
    public void BlockRightThings(bool _Triger)
    {
        for (int i = 0; i < RightEnterBtn.Length; i++)
        {
            RightEnterBtn[i].interactable = _Triger;
            RightEnterBtn[i].transform.GetChild(0).gameObject.SetActive(!_Triger);
        }
    }





    // 키로 사용하기 위한 암호 정의
    private static string karudaisuki;
    // 인증키 정의
    private static string mattzip;

    private void EX_Start()
    {
        karudaisuki = transform.GetChild(0).name;
        mattzip = karudaisuki.Substring(0, 128 / 8);
    }

    //#region 인벤토리 초기화.

    //void PinkiePie()
    //{
    //    JObject savedata = new JObject
    //    {
    //        ["key-name"] = 1d,
    //        ["anyname"] = 1d,
    //        ["is-save"] = 1d,
    //    };

    //    // 파일로 저장 
    //    string savestring = JsonConvert.SerializeObject(savedata); // JObject를 Serialize하여 json string 생성 
    //    File.WriteAllText(Application.persistentDataPath + "/pinkiepieisbestpony.json", AESEncrypt128(savestring)); // 생성된 string을 파일에 쓴다 
    //}
    //void RainbowDash()
    //{
    //    string loadstring = File.ReadAllText(Application.persistentDataPath + "/pinkiepieisbestpony.json"); // string을 읽음 
    //    JObject loaddata = JObject.Parse(AESDecrypt128(loadstring)); // JObject 파싱 
    //}

    //#endregion

    #region 화폐 개혁

    string sResult;
    string sNumber;
    string sDigit;
    double dKeepNumber;
    double dRMV_Decimal;
    int iDoubleZari;
    /// <summary>
    /// 데이터 저장시 사용 X / PC에게 보여줄때만 사용
    /// </summary>
    /// <param name="dNumber">입력값은 따블</param>
    /// <returns>100.00A 형태로 리턴해요.</returns>
    public string DoubleToStringNumber(double dNumber)
    {
        sResult = string.Empty;
        sNumber = string.Empty;
        sDigit = string.Empty;

        string[] sNumberList = (dNumber.ToString()).Split('+');

        dKeepNumber = 0;

        // Split 안되었다 = 1.23E+3 형식이 아니다
        if (sNumberList.Length < 2)
        {
            dRMV_Decimal = Math.Truncate(dNumber);

            //4자리 수 미만
            if (dRMV_Decimal.ToString().Length < 4)
            {
                dKeepNumber = dRMV_Decimal;
                sNumber = string.Format("{0}", dKeepNumber);
            }
            else //4자리 수 이상 100010
            {
                dKeepNumber = double.Parse(dRMV_Decimal.ToString().Substring(0, 4));
                sNumber = string.Format("{0}", dKeepNumber);
            }

            //Enum 에 정의되어있으면
            if (Enum.IsDefined(typeof(EnumNumber), dRMV_Decimal.ToString().Length))
            {
                // 숫자 길이 만큼 얻어와서 이넘
                sDigit = ((EnumNumber)dRMV_Decimal.ToString().Length).ToString().Replace("num_", "");
                sNumber = string.Format("{0:f2}", dKeepNumber * 0.001);

            }
            else // Enum 에 정의 되어있지 않으면 5 6 8 9 11 12 3으로 나눠서 나머지 2 나머지 0
            {
                // 자릿수가 3으로 나눠서 2남으면 2자리수
                if (dRMV_Decimal.ToString().Length % 3 == 2 && dRMV_Decimal.ToString().Length > 3)
                {
                    sDigit = ((EnumNumber)dRMV_Decimal.ToString().Length - 1).ToString().Replace("num_", "");
                    sNumber = string.Format("{0:f2}", dKeepNumber * 0.01);
                }
                else if (dRMV_Decimal.ToString().Length % 3 == 0 && dRMV_Decimal.ToString().Length > 3)
                {
                    sDigit = ((EnumNumber)dRMV_Decimal.ToString().Length - 2).ToString().Replace("num_", "");
                    sNumber = string.Format("{0:f2}", dKeepNumber * 0.1);
                }

            }
        }
        else // 1.23E+3 형식일때 16자리 부터.
        {
            iDoubleZari = int.Parse(sNumberList[1]) + 1;
            dKeepNumber = double.Parse(sNumberList[0].ToString().Replace("E", ""));

            if (iDoubleZari % 3 == 2 && iDoubleZari > 3) //16 = 1,  17 = 2,  18 = 0
            {
                iDoubleZari -= 1;
                sNumber = string.Format("{0:f2}", dKeepNumber * 10);
                sDigit = ((EnumNumber)iDoubleZari).ToString().Replace("num_", "");
            }
            else if (iDoubleZari % 3 == 0 && iDoubleZari > 3)
            {
                iDoubleZari -= 2;
                sNumber = string.Format("{0:f2}", dKeepNumber * 100);
                sDigit = ((EnumNumber)iDoubleZari).ToString().Replace("num_", "");
            }
            else
            {
                sDigit = ((EnumNumber)iDoubleZari).ToString().Replace("num_", "");
                sNumber = string.Format("{0:f2}", dKeepNumber);

            }

            if (iDoubleZari > 69)
            {
                sResult = "N/A";
            }
        }

        if (double.Parse(sNumber) >= 100d && sDigit == "CX")
        {
            sNumber = "100.00";
        }

        sResult = string.Format("{0}{1}", sNumber, sDigit);

        //Debug.LogWarning("sNumber " + sNumber);
        //Debug.LogWarning("sDigit " + sDigit);

        return sResult;
    }

    #endregion




    /// <summary>
    /// 확장 가능한 NonJson 리스트
    /// </summary>
    /// <returns></returns>
    public string NonJsonDataOutput()
    {
        /// 세이브랑 변환 같이
        ListModel.Instance.nonSaveJsonMoney[0].RecentDistance = PlayerInventory.RecentDistance.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].Money_Gold = PlayerInventory.Money_Gold.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].Money_Elixir = PlayerInventory.Money_Elixir.ToString();

        ListModel.Instance.nonSaveJsonMoney[0].Money_AmazonCoin = PlayerInventory.Money_AmazonCoin.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].AmazonStoneCount = PlayerInventory.AmazonStoneCount.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].CurrentAmaLV = PlayerInventory.CurrentAmaLV.ToString();

        ListModel.Instance.nonSaveJsonMoney[0].box_Coupon = PlayerInventory.box_Coupon.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].box_E = PlayerInventory.box_E.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].box_D = PlayerInventory.box_D.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].box_C = PlayerInventory.box_C.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].box_B = PlayerInventory.box_B.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].box_A = PlayerInventory.box_A.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].box_S = PlayerInventory.box_S.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].box_L = PlayerInventory.box_L.ToString();

        ListModel.Instance.nonSaveJsonMoney[0].ticket_reinforce_box = PlayerInventory.ticket_reinforce_box.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].ticket_leaf_box = PlayerInventory.ticket_leaf_box.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].ticket_pvp_enter = PlayerInventory.ticket_pvp_enter.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].ticket_cave_enter = PlayerInventory.ticket_cave_enter.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].ticket_cave_clear = PlayerInventory.ticket_cave_clear.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].S_reinforce_box = PlayerInventory.S_reinforce_box.ToString();
        ///
        /// ------------------------------------------- 아마존 포션 저장
        ListModel.Instance.nonSaveJsonMoney[0].S_leaf_box = PlayerInventory.S_leaf_box.ToString();
        ///
        ///
        ListModel.Instance.nonSaveJsonMoney[0].mining = PlayerInventory.mining.ToString();
        ListModel.Instance.nonSaveJsonMoney[0].amber = PlayerInventory.amber.ToString();
        ///  인트 저장
        ListModel.Instance.nonSaveJsonMoney[0].isTutoAllClear = isTutoAllClear ? 525 : 0;




        ///[1].RecentDistance = DailyCount_Cheak (출석체크 일자 저장)
        ListModel.Instance.nonSaveJsonMoney[1].RecentDistance = DailyCount_Cheak.ToString();
        ///[1].Money_Gold 
        ListModel.Instance.nonSaveJsonMoney[1].Money_Gold = isDailyCheak == true ? "TRUE" : "FALSE";
        ///[1].Money_Elixir
        ListModel.Instance.nonSaveJsonMoney[1].Money_Elixir = ZogarkMissionCnt.ToString();
        ///[1].Money_AmazonCoin
        ListModel.Instance.nonSaveJsonMoney[1].Money_AmazonCoin = AmaAdsTimer.ToString();
        ///[1].AmazonStoneCount
        ListModel.Instance.nonSaveJsonMoney[1].AmazonStoneCount = FreeDiaCnt.ToString();
        ///[1].FreeWeaponCnt
        ListModel.Instance.nonSaveJsonMoney[1].CurrentAmaLV = FreeWeaponCnt.ToString();

        /// 0117 추가 데이터

        ///[1].SwampyEnterCnt
        ListModel.Instance.nonSaveJsonMoney[1].box_Coupon = SwampyEnterCnt.ToString();
        ///[1].SwampySkipCnt
        ListModel.Instance.nonSaveJsonMoney[1].box_E = SwampySkipCnt.ToString();
        /// [1] 다이아
        ListModel.Instance.nonSaveJsonMoney[1].box_D = PlayerInventory.Money_Dia.ToString();
        /// [1] 나뭇잎
        ListModel.Instance.nonSaveJsonMoney[1].box_C = PlayerInventory.Money_Leaf.ToString();
        /// [1] 강화석
        ListModel.Instance.nonSaveJsonMoney[1].box_B = PlayerInventory.Money_EnchantStone.ToString();

        ///... [1] [2] 쭉쭉 저장 가능하게
        //ListModel.Instance.nonSaveJsonMoney[1].box_A = PlayerInventory.box_A.ToString();
        //ListModel.Instance.nonSaveJsonMoney[1].box_S = PlayerInventory.box_S.ToString();
        //ListModel.Instance.nonSaveJsonMoney[1].box_L = PlayerInventory.box_L.ToString();
        //ListModel.Instance.nonSaveJsonMoney[1].ticket_reinforce_box = PlayerInventory.ticket_reinforce_box.ToString();
        //ListModel.Instance.nonSaveJsonMoney[1].ticket_leaf_box = PlayerInventory.ticket_leaf_box.ToString();
        //ListModel.Instance.nonSaveJsonMoney[1].ticket_pvp_enter = PlayerInventory.ticket_pvp_enter.ToString();
        //ListModel.Instance.nonSaveJsonMoney[1].ticket_cave_enter = PlayerInventory.ticket_cave_enter.ToString();
        //ListModel.Instance.nonSaveJsonMoney[1].ticket_cave_clear = PlayerInventory.ticket_cave_clear.ToString();
        //ListModel.Instance.nonSaveJsonMoney[1].S_reinforce_box = PlayerInventory.S_reinforce_box.ToString();
        //ListModel.Instance.nonSaveJsonMoney[1].S_leaf_box = PlayerInventory.S_leaf_box.ToString();
        //ListModel.Instance.nonSaveJsonMoney[1].mining = PlayerInventory.mining.ToString();
        //ListModel.Instance.nonSaveJsonMoney[1].amber = PlayerInventory.amber.ToString();
        /////  인트 저장
        //ListModel.Instance.nonSaveJsonMoney[1].isTutoAllClear = PlayerPrefsManager.isTutoAllClear ? 525 : 0;


        string savestring = JsonConvert.SerializeObject(ListModel.Instance.nonSaveJsonMoney);
        return AESEncrypt128(savestring);
    }

    public void NonJsonDataLoad(string Mormo)
    {
        /// TODO : 1.0.0 데이터를 가지고 있는 경우 예외처리
        if (Mormo == "8") return;

        /// 배열 복구
        ListModel.Instance.nonSaveJsonMoney = JsonConvert.DeserializeObject<List<NonJson>>(AESDecrypt128(Mormo));
    }

    public void NonDataBoxList(string mayo)
    {
        //string[] tunamayo = new string[5];

        /// TODO : 1.0.5 데이터를 가지고 있는 경우 예외처리
        if (mayo == "9") 
        {
            //tunamayo[0] = File.ReadAllText(Application.persistentDataPath + "/GameData/_data_child_3");
            //tunamayo[1] = File.ReadAllText(Application.persistentDataPath + "/GameData/_data_child_4");
            //tunamayo[2] = File.ReadAllText(Application.persistentDataPath + "/GameData/_data_child_5");
            //tunamayo[3] = File.ReadAllText(Application.persistentDataPath + "/GameData/_data_child_6");
            //tunamayo[4] = File.ReadAllText(Application.persistentDataPath + "/GameData/_data_child_7");
        }
        else
        {
            string[] tunamayo = JsonConvert.DeserializeObject<string[]>(AESDecrypt128(mayo));

            Debug.LogError(" saddsa : " + mayo);
            Debug.LogError(" saddsa[0] : " + tunamayo[0]);
            ///// 배열 복구
            //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_3", tunamayo[0]);
            //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_4", tunamayo[1]);
            //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_5", tunamayo[2]);
            //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_6", tunamayo[3]);
            //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_7", tunamayo[4]);
            ////
            //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_14", tunamayo[5]);
            //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_19", tunamayo[6]);
            /// 배열 복구
            ListModel.Instance.equipRuneList = JsonConvert.DeserializeObject<List<RuneInventory>>(AESDecrypt128(tunamayo[0]));
            ListModel.Instance.runeList = JsonConvert.DeserializeObject<List<RuneInventory>>(AESDecrypt128(tunamayo[1]));
            ListModel.Instance.invisibleruneList = JsonConvert.DeserializeObject<List<RuneContent>>(AESDecrypt128(tunamayo[2]));
            ListModel.Instance.heartList = JsonConvert.DeserializeObject<List<HeartContent>>(AESDecrypt128(tunamayo[3]));
            ListModel.Instance.invisibleheartList = JsonConvert.DeserializeObject<List<HeartContent>>(AESDecrypt128(tunamayo[4]));
            //
            ListModel.Instance.mvpDataList = JsonConvert.DeserializeObject<List<MVP>>(AESDecrypt128(tunamayo[5]));
            ListModel.Instance.axeDataList = JsonConvert.DeserializeObject<List<AxeStat>>(AESDecrypt128(tunamayo[6]));
        }
    }


    /// <summary>
    /// int로 가져오는 건 스트링[]에 write한다.
    /// </summary>
    /// <param name="sd"></param>
    /// <param name="dir"></param>
    public void JObjectSave(object sd, int dir)
    {
        // JObject를 Serialize하여 json string 생성 
        string savestring = JsonConvert.SerializeObject(sd);
        /// 각 차일드 별로 분리해서 파일 저장
        File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_" + dir, AESEncrypt128(savestring));
    }



    bool isSafySave;
    bool isSafySeverSave;

    /// <summary>
    /// 무조건 서버 저장 true 로만 호출한다.
    /// 리스트를 Json으로 저장해서 업로드
    /// </summary>
    public void JObjectSave(bool _isSeverSave)
    {
        if (!_isSeverSave)
            return;

        /// 너무 많이 불러오지 않게 텀 두기.
        if (isSafySeverSave)
        {
            SystemPopUp.instance.StopLoopLoading();
            return;
        }
        else
        {
            isSafySeverSave = true;
        }

        SeverDataBox data;
        /// 뉴데이터 존재 안하면 생성하고 리턴
        if (!File.Exists(Application.persistentDataPath + "/GameData/_data_child_22"))
        {
            /// 저장 데이터 없음!
            DataBoxCopy.instance.SaveSeverBox();
            return;
        }
        /// 저장해논 뉴 데이터가 있으면
        else
        {
            /// 뉴 데이터 - 이전에 서버 저장 _data_child_22 가져오면
            data = DataBoxCopy.instance.LoadSeverBox();
        }

        string[] tunamayo = new string[7];
        /// 실전 압축 서버 업로드 데이터
        //tunamayo[0] = JsonConvert.SerializeObject(ListModel.Instance.petList);
        //tunamayo[1] = JsonConvert.SerializeObject(ListModel.Instance.charatorList);
        //tunamayo[2] = JsonConvert.SerializeObject(ListModel.Instance.weaponList);
        tunamayo[0] = AESEncrypt128(JsonConvert.SerializeObject(ListModel.Instance.equipRuneList));
        tunamayo[1] = AESEncrypt128(JsonConvert.SerializeObject(ListModel.Instance.runeList));
        tunamayo[2] = AESEncrypt128(JsonConvert.SerializeObject(ListModel.Instance.invisibleruneList));
        tunamayo[3] = AESEncrypt128(JsonConvert.SerializeObject(ListModel.Instance.heartList));
        tunamayo[4] = AESEncrypt128(JsonConvert.SerializeObject(ListModel.Instance.invisibleheartList));
        //
        tunamayo[5] = AESEncrypt128(JsonConvert.SerializeObject(ListModel.Instance.mvpDataList));
        tunamayo[6] = AESEncrypt128(JsonConvert.SerializeObject(ListModel.Instance.axeDataList));
        ///// 
        //tunamayo[8] = JsonConvert.SerializeObject(ListModel.Instance.supList);
        //tunamayo[10] = JsonConvert.SerializeObject(ListModel.Instance.missionDAYlist);
        //tunamayo[11] = JsonConvert.SerializeObject(ListModel.Instance.missionALLlist);
        //tunamayo[12] = JsonConvert.SerializeObject(ListModel.Instance.missionTUTOlist);
        //tunamayo[13] = JsonConvert.SerializeObject(ListModel.Instance.mineCraft);
        //tunamayo[15] = JsonConvert.SerializeObject(ListModel.Instance.swampCaveData);
        ///// 
        /// JObject를 Serialize하여 json string 생성 
        string savestring = AESEncrypt128(JsonConvert.SerializeObject(tunamayo));

        TimeLoadBox _107jsonString = new TimeLoadBox();
        // 각기 데이터 데입
        _107jsonString.currentHPs = MineManager.currentHPs;
        _107jsonString.currentTimes = sm.currentTimes;
        //Debug.LogError("savestring : " + savestring);
        /// 플레이팹에 상태 저장
        GameObject.FindWithTag("PFM").GetComponent<PlayFabManage>()
            .SetUserData(JsonConvert.SerializeObject(data), savestring, JsonConvert.SerializeObject(_107jsonString));
        /// 서버저장 최소 5.0초
        Invoke(nameof(SSeSvSer), 5.0f);
    }

    void SSeSvSer()
    {
        isSafySeverSave = false;
    }


    int iTryResult;
    long lTryResult;

    /// <summary>
    /// 1kb 짜리 깨진 파일 읽음
    /// </summary>
    /// <returns></returns>
    public string MoahanInit()
    {
        return File.ReadAllText(Application.persistentDataPath + "/_data_"); // string을 읽음 
    }

    /// <summary>
    /// 105 -> 106 업데이트 하는 사람만 이거 1번 실행해.
    /// </summary>
    public void SuperDataPatch()
    {
        Debug.LogError("105 -> 106 업데이트 하는 사람만 이거 1번 실행해.");

        int iTryResult;
        long lTryResult;
        double dTryResult;

        if (int.TryParse(ObscuredPrefs.GetString("CurrentAmaLV"), out iTryResult)) PlayerInventory.CurrentAmaLV = iTryResult;
        else PlayerInventory.CurrentAmaLV = 0;
        if (long.TryParse(ObscuredPrefs.GetString("AmazonStoneCount"), out lTryResult)) PlayerInventory.AmazonStoneCount = lTryResult;
        else PlayerInventory.AmazonStoneCount = 0;
        if (double.TryParse(ObscuredPrefs.GetString("RecentDistance"), out dTryResult)) PlayerInventory.RecentDistance = dTryResult;
        else PlayerInventory.RecentDistance = 0;
        if (double.TryParse(ObscuredPrefs.GetString("Money_Gold"), out dTryResult)) PlayerInventory.Money_Gold = dTryResult;
        else PlayerInventory.Money_Gold = 0;
        if (double.TryParse(ObscuredPrefs.GetString("Money_Leaf"), out dTryResult)) PlayerInventory.Money_Leaf = dTryResult;
        else PlayerInventory.Money_Leaf = 0;
        if (long.TryParse(ObscuredPrefs.GetString("Money_Elixir"), out lTryResult)) PlayerInventory.Money_Elixir = lTryResult;
        else PlayerInventory.Money_Elixir = 0;
        if (long.TryParse(ObscuredPrefs.GetString("Money_Dia"), out lTryResult)) PlayerInventory.Money_Dia = lTryResult;
        else PlayerInventory.Money_Dia = 0;
        if (double.TryParse(ObscuredPrefs.GetString("Money_EnchantStone"), out dTryResult)) PlayerInventory.Money_EnchantStone = dTryResult;
        else PlayerInventory.Money_EnchantStone = 0;
        if (long.TryParse(ObscuredPrefs.GetString("Money_AmazonCoin"), out lTryResult)) PlayerInventory.Money_AmazonCoin = lTryResult;
        else PlayerInventory.Money_AmazonCoin = 0;
        if (int.TryParse(ObscuredPrefs.GetString("box_Coupon"), out iTryResult)) PlayerInventory.box_Coupon = iTryResult;
        else PlayerInventory.box_Coupon = 0;
        if (int.TryParse(ObscuredPrefs.GetString("box_E"), out iTryResult)) PlayerInventory.box_E = iTryResult;
        else PlayerInventory.box_E = 0;
        if (int.TryParse(ObscuredPrefs.GetString("box_D"), out iTryResult)) PlayerInventory.box_D = iTryResult;
        else PlayerInventory.box_D = 0;
        if (int.TryParse(ObscuredPrefs.GetString("box_C"), out iTryResult)) PlayerInventory.box_C = iTryResult;
        else PlayerInventory.box_C = 0;
        if (int.TryParse(ObscuredPrefs.GetString("box_B"), out iTryResult)) PlayerInventory.box_B = iTryResult;
        else PlayerInventory.box_B = 0;
        if (int.TryParse(ObscuredPrefs.GetString("box_A"), out iTryResult)) PlayerInventory.box_A = iTryResult;
        else PlayerInventory.box_A = 0;
        if (int.TryParse(ObscuredPrefs.GetString("box_S"), out iTryResult)) PlayerInventory.box_S = iTryResult;
        else PlayerInventory.box_S = 0;
        if (int.TryParse(ObscuredPrefs.GetString("box_L"), out iTryResult)) PlayerInventory.box_L = iTryResult;
        else PlayerInventory.box_L = 0;
        if (int.TryParse(ObscuredPrefs.GetString("ticket_reinforce_box"), out iTryResult)) PlayerInventory.ticket_reinforce_box = iTryResult;
        else PlayerInventory.ticket_reinforce_box = 0;
        if (int.TryParse(ObscuredPrefs.GetString("ticket_leaf_box"), out iTryResult)) PlayerInventory.ticket_leaf_box = iTryResult;
        else PlayerInventory.ticket_leaf_box = 0;
        if (int.TryParse(ObscuredPrefs.GetString("ticket_pvp_enter"), out iTryResult)) PlayerInventory.ticket_pvp_enter = iTryResult;
        else PlayerInventory.ticket_pvp_enter = 0;
        if (int.TryParse(ObscuredPrefs.GetString("ticket_cave_enter"), out iTryResult)) PlayerInventory.ticket_cave_enter = iTryResult;
        else PlayerInventory.ticket_cave_enter = 0;
        if (int.TryParse(ObscuredPrefs.GetString("ticket_cave_clear"), out iTryResult)) PlayerInventory.ticket_cave_clear = iTryResult;
        else PlayerInventory.ticket_cave_clear = 0;
        if (int.TryParse(ObscuredPrefs.GetString("S_reinforce_box"), out iTryResult)) PlayerInventory.S_reinforce_box = iTryResult;
        else PlayerInventory.S_reinforce_box = 0;
        if (int.TryParse(ObscuredPrefs.GetString("S_leaf_box"), out iTryResult)) PlayerInventory.S_leaf_box = iTryResult;
        else PlayerInventory.S_leaf_box = 0;
        if (int.TryParse(ObscuredPrefs.GetString("mining"), out iTryResult)) PlayerInventory.mining = iTryResult;
        else PlayerInventory.mining = 0;
        if (int.TryParse(ObscuredPrefs.GetString("amber"), out iTryResult)) PlayerInventory.amber = iTryResult;
        else PlayerInventory.amber = 0;
        /// 튜토리얼 클리어 했니?
        isTutoAllClear = ObscuredPrefs.GetInt("isTutoAllClear", 0) != 0 ? true : false;
        /// 출석체크 며칠째니?
        DailyCount_Cheak = ObscuredPrefs.GetInt("DailyCount_Cheak", 0);
        /// 210115 업데이트 추가
        isDailyCheak = ObscuredPrefs.GetInt("isDailyCheak", 0) != 0 ? true : false;
        ZogarkMissionCnt = ObscuredPrefs.GetInt("ZogarkMissionCnt", 0);
        AmaAdsTimer = ObscuredPrefs.GetInt("AmaAdsTimer", 0);
        FreeDiaCnt = ObscuredPrefs.GetInt("FreeDiaCnt", 0);
        FreeWeaponCnt = ObscuredPrefs.GetInt("FreeWeaponCnt", 0);
        /// 210117 업데이트 추가
        SwampyEnterCnt = ObscuredPrefs.GetInt("SwampyEnterCnt", 5);
        SwampySkipCnt = ObscuredPrefs.GetInt("SwampySkipCnt", 5);

        string[] tunamayo = new string[21];
        string loadstring = "";
        //
        if (File.Exists(Application.persistentDataPath + "/_data_"))
        {
            loadstring = File.ReadAllText(Application.persistentDataPath + "/_data_"); // string을 읽음 
        }
        else
        {
            /// 잘못된 접근은 꺼라.
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                    Application.Quit(); // 어플리케이션 종료
            #endif
            return;
        }

        /// 1.0.6 전 데이터 는 현상유지
        if (loadstring != "n1u2l3l" + CursedId)
        {
            /// 데이터 쪼개기 전이라면 현상유지
            tunamayo = JsonConvert.DeserializeObject<string[]>(AESDecrypt128(loadstring));
        }

        ListModel.Instance.petList = JsonConvert.DeserializeObject<List<PetContent>>(AESDecrypt128(tunamayo[0]));
        ListModel.Instance.charatorList = JsonConvert.DeserializeObject<List<CharatorContent>>(AESDecrypt128(tunamayo[1]));
        ListModel.Instance.weaponList = JsonConvert.DeserializeObject<List<WeaponContent>>(AESDecrypt128(tunamayo[2]));
        //
        ListModel.Instance.equipRuneList = JsonConvert.DeserializeObject<List<RuneInventory>>(AESDecrypt128(tunamayo[3]));
        ListModel.Instance.runeList = JsonConvert.DeserializeObject<List<RuneInventory>>(AESDecrypt128(tunamayo[4]));
        ListModel.Instance.invisibleruneList = JsonConvert.DeserializeObject<List<RuneContent>>(AESDecrypt128(tunamayo[5]));
        ListModel.Instance.heartList = JsonConvert.DeserializeObject<List<HeartContent>>(AESDecrypt128(tunamayo[6]));
        ListModel.Instance.invisibleheartList = JsonConvert.DeserializeObject<List<HeartContent>>(AESDecrypt128(tunamayo[7]));
        //
        ListModel.Instance.supList = JsonConvert.DeserializeObject<List<SupContent>>(AESDecrypt128(tunamayo[8]));

        /// --------------------------------------------------------
        ListModel.Instance.shopList = JsonConvert.DeserializeObject<List<ShopPrice>>(AESDecrypt128(tunamayo[9]));
        ListModel.Instance.shopListSPEC = JsonConvert.DeserializeObject<List<ShopPrice>>(AESDecrypt128(tunamayo[10]));
        ListModel.Instance.shopListNOR = JsonConvert.DeserializeObject<List<ShopPrice>>(AESDecrypt128(tunamayo[11]));
        ListModel.Instance.shopListPACK = JsonConvert.DeserializeObject<List<ShopPrice>>(AESDecrypt128(tunamayo[12]));
        ListModel.Instance.shopListAMA = JsonConvert.DeserializeObject<List<ShopPrice>>(AESDecrypt128(tunamayo[13]));
        /// --------------------------------------------------------

        // 유료 구매 부분
        ListModel.Instance.mvpDataList = JsonConvert.DeserializeObject<List<MVP>>(AESDecrypt128(tunamayo[14]));
        //일퀘 / 업적 / 튜토리얼
        ListModel.Instance.missionDAYlist = JsonConvert.DeserializeObject<List<MissonSchool>>(AESDecrypt128(tunamayo[15]));
        ListModel.Instance.missionALLlist = JsonConvert.DeserializeObject<List<MissonSchool>>(AESDecrypt128(tunamayo[16]));
        ListModel.Instance.missionTUTOlist = JsonConvert.DeserializeObject<List<MissonSchool>>(AESDecrypt128(tunamayo[17]));
        // 채굴 관련
        ListModel.Instance.mineCraft = JsonConvert.DeserializeObject<List<MineCraft>>(AESDecrypt128(tunamayo[18]));
        ListModel.Instance.axeDataList = JsonConvert.DeserializeObject<List<AxeStat>>(AESDecrypt128(tunamayo[19]));
        ListModel.Instance.swampCaveData = JsonConvert.DeserializeObject<List<SwampCave>>(AESDecrypt128(tunamayo[20]));

        /// 유물 뽑은거 트리거 로드
        for (int i = 0; i < ListModel.Instance.heartList.Count; i++)
        {
            PlayerInventory.heartIndexs[int.Parse(ListModel.Instance.heartList[i].imgIndex) - 1] = i + 1;
        }
        /// 장착 룬 스탯 새로고침
        ListModel.Instance.SetEquipedRuneEffect();
        /// 광고 제거 로드
        PlayerInventory.isSuperUser = ListModel.Instance.mvpDataList[0].SuperUser;
        /// TODO : 영구적인 버프 효과 아이콘 활성화
        PlayerInventory.isbuff_power_up = ListModel.Instance.mvpDataList[0].buff_power_up != 0 ? true : false;
        if (PlayerInventory.isbuff_power_up) bm.MoneyLoveBuff(0);
        PlayerInventory.isbuff_attack_speed_up = ListModel.Instance.mvpDataList[0].buff_attack_speed_up != 0 ? true : false;
        if (PlayerInventory.isbuff_attack_speed_up) bm.MoneyLoveBuff(1);
        PlayerInventory.isbuff_move_speed_up = ListModel.Instance.mvpDataList[0].buff_move_speed_up != 0 ? true : false;
        if (PlayerInventory.isbuff_move_speed_up) bm.MoneyLoveBuff(2);
        PlayerInventory.isbuff_gold_earned_up = ListModel.Instance.mvpDataList[0].buff_gold_earned_up != 0 ? true : false;
        if (PlayerInventory.isbuff_gold_earned_up) bm.MoneyLoveBuff(3);

        /// 장착 무기 스탯 새로고침
        for (int i = 0; i < ListModel.Instance.weaponList.Count; i++)
        {
            if (ListModel.Instance.weaponList[i].isEnable == "TRUE")
            {
                /// 무기 애니메이션 바꿔줌
                SetPlayerWeaponAnim(i);
                /// 스탯 바꿔줌
                var sc = ListModel.Instance.weaponList[i];
                ListModel.Instance.CurrentEquiped = sc.startPower + ((double.Parse(sc.weaponLevel) + PlayerInventory.Weapon_Lv_Plus) * sc.increedPower);
                break;
            }
        }


        /// -------------------------- 불러온 JSON 들 수치 패치 해줌

        /// 210126 수정 charatorList[5] 스탯 치명타 대미지 5% -> 2.5%로 조정
        ListModel.Instance.Chara_NerfThis(5, 2.5f);
        /// 210126 수정 invisibleheartList[28] 오프라인 보상 5% -> 1%
        ListModel.Instance.Heart_invisNeaf(28, 1f);
        /// 210117 업데이트 추가
        SwampyEnterCnt = ObscuredPrefs.GetInt("SwampyEnterCnt", 5);
        SwampySkipCnt = ObscuredPrefs.GetInt("SwampySkipCnt", 5);
        /// 초창기 초기화 후 호출할때만 데이터 쪼개서 생성해주기.
        /// 재 실행 부터는 이거 안해도 된다.
        JObjectSave(ListModel.Instance.petList, 0);
        JObjectSave(ListModel.Instance.charatorList, 1);
        JObjectSave(ListModel.Instance.weaponList, 2);
        JObjectSave(ListModel.Instance.equipRuneList, 3);
        JObjectSave(ListModel.Instance.runeList, 4);
        JObjectSave(ListModel.Instance.invisibleruneList, 5);
        JObjectSave(ListModel.Instance.heartList, 6);
        JObjectSave(ListModel.Instance.invisibleheartList, 7);
        JObjectSave(ListModel.Instance.supList, 8);
        /// ---------- 변동없는 상점 리스트
        JObjectSave(ListModel.Instance.shopList, 9);
        JObjectSave(ListModel.Instance.shopListSPEC, 10);
        JObjectSave(ListModel.Instance.shopListNOR, 11);
        JObjectSave(ListModel.Instance.shopListPACK, 12);
        JObjectSave(ListModel.Instance.shopListAMA, 13);
        ///
        JObjectSave(ListModel.Instance.mvpDataList, 14);
        //
        JObjectSave(ListModel.Instance.missionDAYlist, 15);
        JObjectSave(ListModel.Instance.missionALLlist, 16);
        JObjectSave(ListModel.Instance.missionTUTOlist, 17);
        //
        JObjectSave(ListModel.Instance.mineCraft, 18);
        JObjectSave(ListModel.Instance.axeDataList, 19);
        //
        JObjectSave(ListModel.Instance.swampCaveData, 20);

        File.WriteAllText(Application.persistentDataPath + "/_data_", "n1u2l3l" + CursedId); // 이제 안쓰는 부모

        /// 로딩바 올려주는 걸 허락한다.
        isJObjectLoad = true;
        /// SetPrefs 대신 새로운 저장 방식 -> 로컬에 저장
        DataBoxCopy.instance.SaveBox();
        DataBoxCopy.instance.SaveSeverBox();
    }

    /// <summary>
    ///  true  는 첫 실행시 불러오는 값
    ///  false 는 재 실행시 로컬 불러오는 값 
    /// 파일명으로 접근해서 해당 리스트 로드
    /// </summary>
    /// <param name="_isInit">나중에 스위치로 전환해야할 때 입력 받으</param>
    public void JObjectLoad(bool _isInit)
    {
        string[] tunamayo = new string[21];
        string loadstring = "";
        /// 늪지 밸패 210117 ture/false 양쪽다 쓴다. (수정X)
        string loadstringSwapy = tuna.text;
        /// 첫시작 파일은 리소시즈
        if (_isInit)
        {
            Debug.LogError("mayo 는 게임 처음 실행하는 깨끗한 데이터 List[21]");
            /// 배열 복구
            tunamayo = JsonConvert.DeserializeObject<string[]>(AESDecrypt128(mayo.text));
        }
        /// 첫 시작이 아니라 재실행은 여기서 불러와
        else
        {
            if(File.Exists(Application.persistentDataPath + "/_data_"))
                loadstring = File.ReadAllText(Application.persistentDataPath + "/_data_"); // string을 읽음 
            else
            {
                /// 잘못된 접근은 꺼라.
                Application.Quit();
                return;
            }
                

            /// 1.0.6 전 데이터 는 현상유지
            if (loadstring != "n1u2l3l"+ CursedId)
            {
                /// 데이터 쪼개기 전이라면 현상유지
                tunamayo = JsonConvert.DeserializeObject<string[]>(AESDecrypt128(loadstring));
            }
            else
            {
                /// 각 차일드 별로 분리한 파일호출
                for (int i = 0; i < tunamayo.Length; i++)
                {
                    if (File.Exists(Application.persistentDataPath + "/GameData/_data_child_" + i))
                        tunamayo[i] = File.ReadAllText(Application.persistentDataPath + "/GameData/_data_child_" + i);
                    else
                    {
                        Debug.LogError("뭐가 빠져 ? : " + i);
                    }

                }
            }
        }

        ListModel.Instance.petList = JsonConvert.DeserializeObject<List<PetContent>>(AESDecrypt128(tunamayo[0]));
        ListModel.Instance.charatorList = JsonConvert.DeserializeObject<List<CharatorContent>>(AESDecrypt128(tunamayo[1]));
        ListModel.Instance.weaponList = JsonConvert.DeserializeObject<List<WeaponContent>>(AESDecrypt128(tunamayo[2]));
        //
        ListModel.Instance.equipRuneList = JsonConvert.DeserializeObject<List<RuneInventory>>(AESDecrypt128(tunamayo[3]));
        ListModel.Instance.runeList = JsonConvert.DeserializeObject<List<RuneInventory>>(AESDecrypt128(tunamayo[4]));
        ListModel.Instance.invisibleruneList = JsonConvert.DeserializeObject<List<RuneContent>>(AESDecrypt128(tunamayo[5]));
        ListModel.Instance.heartList = JsonConvert.DeserializeObject<List<HeartContent>>(AESDecrypt128(tunamayo[6]));
        ListModel.Instance.invisibleheartList = JsonConvert.DeserializeObject<List<HeartContent>>(AESDecrypt128(tunamayo[7]));
        //
        ListModel.Instance.supList = JsonConvert.DeserializeObject<List<SupContent>>(AESDecrypt128(tunamayo[8]));

        /// --------------------------------------------------------
        ListModel.Instance.shopList = JsonConvert.DeserializeObject<List<ShopPrice>>(AESDecrypt128(tunamayo[9]));
        ListModel.Instance.shopListSPEC = JsonConvert.DeserializeObject<List<ShopPrice>>(AESDecrypt128(tunamayo[10]));
        ListModel.Instance.shopListNOR = JsonConvert.DeserializeObject<List<ShopPrice>>(AESDecrypt128(tunamayo[11]));
        ListModel.Instance.shopListPACK = JsonConvert.DeserializeObject<List<ShopPrice>>(AESDecrypt128(tunamayo[12]));
        ListModel.Instance.shopListAMA = JsonConvert.DeserializeObject<List<ShopPrice>>(AESDecrypt128(tunamayo[13]));
        /// --------------------------------------------------------

        // 유료 구매 부분
        ListModel.Instance.mvpDataList = JsonConvert.DeserializeObject<List<MVP>>(AESDecrypt128(tunamayo[14]));
        //일퀘 / 업적 / 튜토리얼
        ListModel.Instance.missionDAYlist = JsonConvert.DeserializeObject<List<MissonSchool>>(AESDecrypt128(tunamayo[15]));
        ListModel.Instance.missionALLlist = JsonConvert.DeserializeObject<List<MissonSchool>>(AESDecrypt128(tunamayo[16]));
        ListModel.Instance.missionTUTOlist = JsonConvert.DeserializeObject<List<MissonSchool>>(AESDecrypt128(tunamayo[17]));
        // 채굴 관련
        ListModel.Instance.mineCraft = JsonConvert.DeserializeObject<List<MineCraft>>(AESDecrypt128(tunamayo[18]));
        ListModel.Instance.axeDataList = JsonConvert.DeserializeObject<List<AxeStat>>(AESDecrypt128(tunamayo[19]));
        /// 늪지 관련
        if (_isInit)
        {
            /// 첫 실행시 구버전 늪지 버리고 신버전 늪지 데이터 넣어줌.
            ListModel.Instance.swampCaveData = JsonConvert.DeserializeObject<List<SwampCave>>(AESDecrypt128(loadstringSwapy));
        }
        else
        {
            /// 두번 째 실행이라면 새로 변경된 늪지 데이터 가지고 있다.
            ListModel.Instance.swampCaveData = JsonConvert.DeserializeObject<List<SwampCave>>(AESDecrypt128(tunamayo[20]));
        }
        /// 유물 뽑은거 트리거 로드
        for (int i = 0; i < ListModel.Instance.heartList.Count; i++)
        {
            PlayerInventory.heartIndexs[int.Parse(ListModel.Instance.heartList[i].imgIndex) - 1] = i + 1;
        }
        /// 장착 룬 스탯 새로고침
        ListModel.Instance.SetEquipedRuneEffect();
        /// 광고 제거 로드 525
        PlayerInventory.isSuperUser = ListModel.Instance.mvpDataList[0].SuperUser;

        /// TODO : 영구적인 버프 효과 아이콘 활성화
        PlayerInventory.isbuff_power_up = ListModel.Instance.mvpDataList[0].buff_power_up != 0 ? true : false;
        if (PlayerInventory.isbuff_power_up) bm.MoneyLoveBuff(0);
        PlayerInventory.isbuff_attack_speed_up = ListModel.Instance.mvpDataList[0].buff_attack_speed_up != 0 ? true : false;
        if (PlayerInventory.isbuff_attack_speed_up) bm.MoneyLoveBuff(1);
        PlayerInventory.isbuff_move_speed_up = ListModel.Instance.mvpDataList[0].buff_move_speed_up != 0 ? true : false;
        if (PlayerInventory.isbuff_move_speed_up) bm.MoneyLoveBuff(2);
        PlayerInventory.isbuff_gold_earned_up = ListModel.Instance.mvpDataList[0].buff_gold_earned_up != 0 ? true : false;
        if (PlayerInventory.isbuff_gold_earned_up) bm.MoneyLoveBuff(3);

        /// 장착 무기 스탯 새로고침
        for (int i = 0; i < ListModel.Instance.weaponList.Count; i++)
        {
            if (ListModel.Instance.weaponList[i].isEnable == "TRUE")
            {
                /// 무기 애니메이션 바꿔줌
                SetPlayerWeaponAnim(i);
                /// 스탯 바꿔줌
                var sc = ListModel.Instance.weaponList[i];
                ListModel.Instance.CurrentEquiped = sc.startPower + ((double.Parse(sc.weaponLevel) + PlayerInventory.Weapon_Lv_Plus) * sc.increedPower);
                break;
            }
        }


        /// -------------------------- 불러온 JSON 들 수치 패치 해줌
        /// -------------------------- 불러온 JSON 들 수치 패치 해줌
        /// -------------------------- 불러온 JSON 들 수치 패치 해줌

        /// 210126 수정 charatorList[5] 스탯 치명타 대미지 5% -> 2.5%로 조정
        ListModel.Instance.Chara_NerfThis(5, 2.5f);
        /// 210126 수정 invisibleheartList[28] 오프라인 보상 5% -> 1%
        ListModel.Instance.Heart_invisNeaf(28, 1f);



        ///update210204
        ///--------------------------------------1.0.7 에서 저려미 패키지 추가 ----------------------------------------------
        ///update210204
        
        /// 1.0.7 에서 저려미 패키지 추가
        mh.CreateCheepPack();

        ///update210204
        ///--------------------------------------1.0.7 에서 저려미 패키지 추가 ----------------------------------------------
        ///update210204
        ///




        ///update210209
        ///--------------------------------------1.1.0 펫 초기값 수치 변경 ----------------------------------------------
        ///update210209

        /// 0 1 2 3 4
        ListModel.Instance.Pet_NeafThis(0, 300);
        ListModel.Instance.Pet_NeafThis(1, 500);
        ListModel.Instance.Pet_NeafThis(2, 700);
        ListModel.Instance.Pet_NeafThis(3, 600);
        ListModel.Instance.Pet_NeafThis(4, 400);

        /// 수집단계 30 -> 40으로 확장
        if (ListModel.Instance.supList.Count < 35)
            mh.Suppoter_Parser();



        ///update210209
        ///--------------------------------------1.1.0 펫 수치 변경 ----------------------------------------------
        ///update210209


        /// 닥터후 뉴 시즌
        TimeLoadBox doctor = DataBoxCopy.instance.LoadDoctorWho();
        //
        MineManager.currentHPs = doctor.currentHPs;

        /// 30 -> 40 늘리기 전 이라면 10개 늘려줌.
        if (doctor.currentTimes.Length < 35)
        {
            for (int i = 0; i < 30; i++)
            {
                sm.currentTimes[i] = doctor.currentTimes[i];
            }
        }
        /// 이미 40개로 늘어난 상태라면 걍 해.
        else
        {
            sm.currentTimes = doctor.currentTimes;
        }



        /// 초창기 초기화 후 호출할때만 데이터 세이브
        if (_isInit)
        {
            /// 초반 초기화 완료 됐을때 키 초기화
            ObscuredPrefs.SetInt("tunamayo", 75);
            ObscuredPrefs.Save();
            /// 210117 업데이트 추가
            SwampyEnterCnt = ObscuredPrefs.GetInt("SwampyEnterCnt", 5);
            SwampySkipCnt = ObscuredPrefs.GetInt("SwampySkipCnt", 5);
            /// 초창기 초기화 후 호출할때만 데이터 쪼개서 생성해주기.
            /// 재 실행 부터는 이거 안해도 된다.
            JObjectSave(ListModel.Instance.petList, 0);
            JObjectSave(ListModel.Instance.charatorList, 1);
            JObjectSave(ListModel.Instance.weaponList, 2);
            JObjectSave(ListModel.Instance.equipRuneList, 3);
            JObjectSave(ListModel.Instance.runeList, 4);
            JObjectSave(ListModel.Instance.invisibleruneList, 5);
            JObjectSave(ListModel.Instance.heartList, 6);
            JObjectSave(ListModel.Instance.invisibleheartList, 7);
            JObjectSave(ListModel.Instance.supList, 8);
            /// ---------- 변동없는 상점 리스트
            JObjectSave(ListModel.Instance.shopList, 9);
            JObjectSave(ListModel.Instance.shopListSPEC, 10);
            JObjectSave(ListModel.Instance.shopListNOR, 11);
            JObjectSave(ListModel.Instance.shopListPACK, 12);
            JObjectSave(ListModel.Instance.shopListAMA, 13);
            ///
            JObjectSave(ListModel.Instance.mvpDataList, 14);
            //
            JObjectSave(ListModel.Instance.missionDAYlist, 15);
            JObjectSave(ListModel.Instance.missionALLlist, 16);
            JObjectSave(ListModel.Instance.missionTUTOlist, 17);
            //
            JObjectSave(ListModel.Instance.mineCraft, 18);
            JObjectSave(ListModel.Instance.axeDataList, 19);
            //
            JObjectSave(ListModel.Instance.swampCaveData, 20);
            
            File.WriteAllText(Application.persistentDataPath + "/_data_", "n1u2l3l" + CursedId); // 이제 안쓰는 부모

            /// 로딩바 올려주는 걸 허락한다.
            isJObjectLoad = true;
        }
        /// 서버에서 덮어 씌우기 한 다음 다시 게임에 적용시켜
        else
        {
            /// --------------------------------------------------------------------------------------------------------------------

            //if (int.TryParse(ObscuredPrefs.GetString("CurrentAmaLV"), out iTryResult)) PlayerInventory.CurrentAmaLV = iTryResult;
            //else PlayerInventory.CurrentAmaLV = 0;
            //if (long.TryParse(ObscuredPrefs.GetString("AmazonStoneCount"), out lTryResult)) PlayerInventory.AmazonStoneCount = lTryResult;
            //else PlayerInventory.AmazonStoneCount = 0;
            isResetAferSave = false;

            /// --------------------------------------------------------------------------------------------------------------------
            /// 
            ///update210117
            ///--------------------------------------update210117 ----------------------------------------------
            ///update210117
            
            if (!ObscuredPrefs.HasKey("update210117"))
            {
                /// 늪지 기록 초기화.
                ListModel.Instance.swampCaveData.Clear();
                ListModel.Instance.swampCaveData = JsonConvert.DeserializeObject<List<SwampCave>>(AESDecrypt128(loadstringSwapy));
               
                /// 초반 초기화 완료 됐을때 키 초기화
                ObscuredPrefs.SetInt("update210117", 956);
                ObscuredPrefs.Save();
            }

            /// 로딩바 올려주는 걸 허락한다.
            isJObjectLoad = true;
        }


    }

    /// <summary>
    /// 서버 불러오기 한 다음 로컬에 JSON 저장하기
    /// </summary>
    public void NewNewSector5()
    {
        /// Json 파일 저장
        JObjectSave(ListModel.Instance.petList, 0);
        JObjectSave(ListModel.Instance.charatorList, 1);
        JObjectSave(ListModel.Instance.weaponList, 2);
        /// 룬 / 유물 빠짐
        JObjectSave(ListModel.Instance.supList, 8);
        /// 상점 리스트 빠짐
        JObjectSave(ListModel.Instance.mvpDataList, 14);
        JObjectSave(ListModel.Instance.missionDAYlist, 15);
        JObjectSave(ListModel.Instance.missionALLlist, 16);
        JObjectSave(ListModel.Instance.missionTUTOlist, 17);
        JObjectSave(ListModel.Instance.mineCraft, 18);
        JObjectSave(ListModel.Instance.axeDataList, 19);
        JObjectSave(ListModel.Instance.swampCaveData, 20);
    }

    /// <summary>
    /// 1.0.6 이후에 json 파일 저정 서버에서 불러올때 바로 꽂아줌
    /// </summary>
    /// <param name="loadstring"></param>
    public void NewSector5SaveJson(string loadstring, string _107jsonString)
    {
        int tmpInt = -1;
        /// 배열 복구
        SeverDataBox data = JsonConvert.DeserializeObject<SeverDataBox>(loadstring);
        ListModel.Instance.Sector5LoadBox(data, _107jsonString);

        int.TryParse(_107jsonString, out tmpInt);
        /// 숫자가 들어있다면 리턴
        if (tmpInt != -1)
            return;

        /// 1.0.7 업데이트 이후 채굴시간 & 수집시간도 덮어쓰기
        TimeLoadBox doctor = JsonConvert.DeserializeObject<TimeLoadBox>(_107jsonString);
        MineManager.currentHPs = doctor.currentHPs;
        sm.currentTimes = doctor.currentTimes;

        ///// 16개 짜리 실전 압축 데이터
        //string[] tunamayo = JsonConvert.DeserializeObject<string[]>(AESDecrypt128(loadstring));
        ///// 배열 복구
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_0", tunamayo[0]);
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_1", tunamayo[1]);
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_2", tunamayo[2]);
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_3", tunamayo[3]);
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_4", tunamayo[4]);
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_5", tunamayo[5]);
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_6", tunamayo[6]);
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_7", tunamayo[7]);
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_8", tunamayo[8]);
        /////  상점은 무시한다.
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_14", tunamayo[9]);
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_15", tunamayo[10]);
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_16", tunamayo[11]);
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_17", tunamayo[12]);
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_18", tunamayo[13]);
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_19", tunamayo[14]);
        //File.WriteAllText(Application.persistentDataPath + "/GameData/_data_child_20", tunamayo[15]);

        //ListModel.Instance.petList = JsonConvert.DeserializeObject<List<PetContent>>(tunamayo[0])
        //ListModel.Instance.charatorList = JsonConvert.DeserializeObject<List<CharatorContent>>(tunamayo[1]);
        //ListModel.Instance.weaponList = JsonConvert.DeserializeObject<List<WeaponContent>>(tunamayo[2]);
        //ListModel.Instance.equipRuneList = JsonConvert.DeserializeObject<List<RuneInventory>>(tunamayo[3]);
        //ListModel.Instance.runeList = JsonConvert.DeserializeObject<List<RuneInventory>>(tunamayo[4]);
        //ListModel.Instance.invisibleruneList = JsonConvert.DeserializeObject<List<RuneContent>>(tunamayo[5]);
        //ListModel.Instance.heartList = JsonConvert.DeserializeObject<List<HeartContent>>(tunamayo[6]);
        //ListModel.Instance.invisibleheartList = JsonConvert.DeserializeObject<List<HeartContent>>(tunamayo[7]);
        //ListModel.Instance.supList = JsonConvert.DeserializeObject<List<SupContent>>(tunamayo[8]);
        //// 유료 구매 부분
        //ListModel.Instance.mvpDataList = JsonConvert.DeserializeObject<List<MVP>>(tunamayo[9]);
        ////일퀘 / 업적 / 튜토리얼
        //ListModel.Instance.missionDAYlist = JsonConvert.DeserializeObject<List<MissonSchool>>(tunamayo[10]);
        //ListModel.Instance.missionALLlist = JsonConvert.DeserializeObject<List<MissonSchool>>(tunamayo[11]);
        //ListModel.Instance.missionTUTOlist = JsonConvert.DeserializeObject<List<MissonSchool>>(tunamayo[12]);
        //// 채굴 관련
        //ListModel.Instance.mineCraft = JsonConvert.DeserializeObject<List<MineCraft>>(tunamayo[13]);
        //ListModel.Instance.axeDataList = JsonConvert.DeserializeObject<List<AxeStat>>(tunamayo[14]);
        //ListModel.Instance.swampCaveData = JsonConvert.DeserializeObject<List<SwampCave>>(tunamayo[15]);
    }

    public void SeverLoadMaser(string _bn)
    {
        _BulidNum = _bn;
        Invoke(nameof(InvoSeverLoadMaser), 1.6f);
    }
    string _BulidNum;
    /// <summary>
    /// 얘는 특별히 서버에서 불러오기 할때만 해준다.
    /// </summary>
    void InvoSeverLoadMaser()
    {
        string loadstring = "";
        if (_BulidNum == "1.0.1" || _BulidNum == "1.0.2" || _BulidNum == "1.0.3" || _BulidNum == "1.0.4" || _BulidNum == "1.0.5")
        {
            /// 1.0.6 이전 버전 데이터 처리
            loadstring = File.ReadAllText(Application.persistentDataPath + "/_data_"); // string을 읽음 
            Debug.LogError(" 해당 데이터는 " + _BulidNum + "버전인데스 " + loadstring);
            string[] tunamayo = JsonConvert.DeserializeObject<string[]>(AESDecrypt128(loadstring));
            /// 배열 복구
            ListModel.Instance.petList = JsonConvert.DeserializeObject<List<PetContent>>(AESDecrypt128(tunamayo[0]));
            ListModel.Instance.charatorList = JsonConvert.DeserializeObject<List<CharatorContent>>(AESDecrypt128(tunamayo[1]));
            ListModel.Instance.weaponList = JsonConvert.DeserializeObject<List<WeaponContent>>(AESDecrypt128(tunamayo[2]));
            ListModel.Instance.equipRuneList = JsonConvert.DeserializeObject<List<RuneInventory>>(AESDecrypt128(tunamayo[3]));
            ListModel.Instance.runeList = JsonConvert.DeserializeObject<List<RuneInventory>>(AESDecrypt128(tunamayo[4]));
            ListModel.Instance.invisibleruneList = JsonConvert.DeserializeObject<List<RuneContent>>(AESDecrypt128(tunamayo[5]));
            ListModel.Instance.heartList = JsonConvert.DeserializeObject<List<HeartContent>>(AESDecrypt128(tunamayo[6]));
            ListModel.Instance.invisibleheartList = JsonConvert.DeserializeObject<List<HeartContent>>(AESDecrypt128(tunamayo[7]));
            ListModel.Instance.supList = JsonConvert.DeserializeObject<List<SupContent>>(AESDecrypt128(tunamayo[8]));
            ListModel.Instance.shopList = JsonConvert.DeserializeObject<List<ShopPrice>>(AESDecrypt128(tunamayo[9]));
            ListModel.Instance.shopListSPEC = JsonConvert.DeserializeObject<List<ShopPrice>>(AESDecrypt128(tunamayo[10]));
            ListModel.Instance.shopListNOR = JsonConvert.DeserializeObject<List<ShopPrice>>(AESDecrypt128(tunamayo[11]));
            ListModel.Instance.shopListPACK = JsonConvert.DeserializeObject<List<ShopPrice>>(AESDecrypt128(tunamayo[12]));
            ListModel.Instance.shopListAMA = JsonConvert.DeserializeObject<List<ShopPrice>>(AESDecrypt128(tunamayo[13]));
            ListModel.Instance.mvpDataList = JsonConvert.DeserializeObject<List<MVP>>(AESDecrypt128(tunamayo[14]));
            ListModel.Instance.missionDAYlist = JsonConvert.DeserializeObject<List<MissonSchool>>(AESDecrypt128(tunamayo[15]));
            ListModel.Instance.missionALLlist = JsonConvert.DeserializeObject<List<MissonSchool>>(AESDecrypt128(tunamayo[16]));
            ListModel.Instance.missionTUTOlist = JsonConvert.DeserializeObject<List<MissonSchool>>(AESDecrypt128(tunamayo[17]));
            ListModel.Instance.mineCraft = JsonConvert.DeserializeObject<List<MineCraft>>(AESDecrypt128(tunamayo[18]));
            ListModel.Instance.axeDataList = JsonConvert.DeserializeObject<List<AxeStat>>(AESDecrypt128(tunamayo[19]));
            ///늪지 -> 서버 데이터가 1.0.4 버전 이하 라면 늪지 조정 버전
            if (_BulidNum == "1.0.3" || _BulidNum == "1.0.2" || _BulidNum == "1.0.1")
                ListModel.Instance.swampCaveData = JsonConvert.DeserializeObject<List<SwampCave>>(AESDecrypt128(tuna.text));
            else
                ListModel.Instance.swampCaveData = JsonConvert.DeserializeObject<List<SwampCave>>(AESDecrypt128(tunamayo[20]));
            // 이제 안쓰는 부모
            File.WriteAllText(Application.persistentDataPath + "/_data_", "n1u2l3l" + CursedId);

        }
        else
        {
            /// 1.0.6 이후 최신버전에서 서버에서 불러오기 하면?
            /// 이미
            ///     public void NewSector5SaveJson(string loadstring) 
            ///  에서 처리 다함
             // 이제 안쓰는 부모
            File.WriteAllText(Application.persistentDataPath + "/_data_", "n1u2l3l" + CursedId);
        }

        /// 거리는  result.Data["SECTOR_7"].Value 랑 비교해서 더 큰 것으로 교체
        if (PlayerInventory.RecentDistance < double.Parse(ListModel.Instance.nonSaveJsonMoney[0].RecentDistance))
            PlayerInventory.RecentDistance = double.Parse(ListModel.Instance.nonSaveJsonMoney[0].RecentDistance);
        PlayerInventory.Money_Gold = double.Parse(ListModel.Instance.nonSaveJsonMoney[0].Money_Gold);
        PlayerInventory.Money_Elixir = long.Parse(ListModel.Instance.nonSaveJsonMoney[0].Money_Elixir);
        PlayerInventory.Money_AmazonCoin = long.Parse(ListModel.Instance.nonSaveJsonMoney[0].Money_AmazonCoin);
        PlayerInventory.AmazonStoneCount = long.Parse(ListModel.Instance.nonSaveJsonMoney[0].AmazonStoneCount);
        PlayerInventory.CurrentAmaLV = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].CurrentAmaLV);
        PlayerInventory.box_Coupon = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].box_Coupon);
        PlayerInventory.box_E = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].box_E);
        PlayerInventory.box_D = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].box_D);
        PlayerInventory.box_C = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].box_C);
        PlayerInventory.box_B = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].box_B);
        PlayerInventory.box_A = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].box_A);
        PlayerInventory.box_S = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].box_S);
        PlayerInventory.box_L = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].box_L);
        PlayerInventory.ticket_reinforce_box = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].ticket_reinforce_box);
        PlayerInventory.ticket_leaf_box = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].ticket_leaf_box);
        PlayerInventory.ticket_pvp_enter = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].ticket_pvp_enter);
        PlayerInventory.ticket_cave_enter = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].ticket_cave_enter);
        PlayerInventory.ticket_cave_clear = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].ticket_cave_clear);
        PlayerInventory.S_reinforce_box = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].S_reinforce_box);
        PlayerInventory.S_leaf_box = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].S_leaf_box);
        PlayerInventory.mining = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].mining);
        PlayerInventory.amber = int.Parse(ListModel.Instance.nonSaveJsonMoney[0].amber);

        isTutoAllClear = ListModel.Instance.nonSaveJsonMoney[0].isTutoAllClear != 0 ? true : false;

        /// 210115 업데이트 추가
        if (ListModel.Instance.nonSaveJsonMoney.Count > 1)
        {
            DailyCount_Cheak = int.Parse(ListModel.Instance.nonSaveJsonMoney[1].RecentDistance);
            isDailyCheak = ListModel.Instance.nonSaveJsonMoney[1].Money_Gold == "TRUE" ? true : false;
            ZogarkMissionCnt = int.Parse(ListModel.Instance.nonSaveJsonMoney[1].Money_Elixir);
            AmaAdsTimer = int.Parse(ListModel.Instance.nonSaveJsonMoney[1].Money_AmazonCoin);
            FreeDiaCnt = int.Parse(ListModel.Instance.nonSaveJsonMoney[1].AmazonStoneCount);
            FreeWeaponCnt = int.Parse(ListModel.Instance.nonSaveJsonMoney[1].CurrentAmaLV);
            /// 210117 업데이트 추가 예외 처리
            if (ListModel.Instance.nonSaveJsonMoney[1].box_Coupon == null)
                SwampyEnterCnt = 5;
            else
                SwampyEnterCnt = int.Parse(ListModel.Instance.nonSaveJsonMoney[1].box_Coupon);
            if (ListModel.Instance.nonSaveJsonMoney[1].box_E == null)
                SwampySkipCnt = 5;
            else
                SwampySkipCnt = int.Parse(ListModel.Instance.nonSaveJsonMoney[1].box_E);

            /// 1.0.7 업데이트 추가
            /// 1.0.7 업데이트 추가
            /// 1.0.7 업데이트 추가
            PlayerInventory.Money_Dia = long.Parse(ListModel.Instance.nonSaveJsonMoney[1].box_D);
            PlayerInventory.Money_Leaf = double.Parse(ListModel.Instance.nonSaveJsonMoney[1].box_C);
            PlayerInventory.Money_EnchantStone = double.Parse(ListModel.Instance.nonSaveJsonMoney[1].box_B);

        }

        /// 리스트를 Prefs 로 저장 -> 뉴-타입으로 저장
        /// "/_data_child_21"
        /// SetPrefs 대신 새로운 저장 방식 -> 로컬에 저장
        DataBoxCopy.instance.SaveBox();
        DataBoxCopy.instance.SaveDoctorWho();
        /// 오프라인 보상 체크 끝났다면 타이머 일괄적으로 저장
        string tmp = UnbiasedTime.Instance.Now().ToString("yyyyMMddHHmmss");
        ObscuredPrefs.SetString("DateTime", tmp);
        //ObscuredPrefs.SetString("AmazonShop", tmp);
        //ObscuredPrefs.SetString("Check_Daily", tmp);
        //ObscuredPrefs.SetString("FreeWeapon", tmp);
        //ObscuredPrefs.SetString("FreeDia", tmp);
        ObscuredPrefs.Save();
        ///
        SystemPopUp.instance.StopLoopLoading();
    }



    ///// <summary>
    ///// 플레이 팹 저장용 json 로드후 리턴
    ///// </summary>
    ///// <param name="dir"></param>
    ///// <returns></returns>
    //public string LoadStringJsonn(string dir)
    //{
    //    string LOAD_DIR = "/" + dir + ".json";
    //    // 불러오기는 저장의 역순 
    //    string loadstring = File.ReadAllText(Application.persistentDataPath + LOAD_DIR); // string을 읽음 

    //    return loadstring;
    //}

    static string AESEncrypt128(string plain)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plain);

        RijndaelManaged myRijndael = new RijndaelManaged();
        myRijndael.Mode = CipherMode.CBC;
        myRijndael.Padding = PaddingMode.PKCS7;
        myRijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream();

        ICryptoTransform encryptor = myRijndael.CreateEncryptor(Encoding.UTF8.GetBytes(mattzip), Encoding.UTF8.GetBytes(mattzip));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
        cryptoStream.FlushFinalBlock();

        byte[] encryptBytes = memoryStream.ToArray();

        string encryptString = Convert.ToBase64String(encryptBytes);

        cryptoStream.Close();
        memoryStream.Close();

        return encryptString;
    }

    static string AESDecrypt128(string encrypt)
    {
        byte[] encryptBytes = Convert.FromBase64String(encrypt);

        RijndaelManaged myRijndael = new RijndaelManaged();
        myRijndael.Mode = CipherMode.CBC;
        myRijndael.Padding = PaddingMode.PKCS7;
        myRijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream(encryptBytes);

        ICryptoTransform decryptor = myRijndael.CreateDecryptor(Encoding.UTF8.GetBytes(mattzip), Encoding.UTF8.GetBytes(mattzip));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        byte[] plainBytes = new byte[encryptBytes.Length];

        int plainCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

        string plainString = Encoding.UTF8.GetString(plainBytes, 0, plainCount);

        cryptoStream.Close();
        memoryStream.Close();

        return plainString;
    }

    /// <summary>
    /// 데이터 리셋을 위해 게임을 종료합니다.
    /// </summary>
    public void DataReset()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }



        //#region Json.Net 예제

        ///// <summary>
        ///// Json 저장 메소드 예제
        ///// </summary>
        //void EX_JObjectSave()
        //{
        //    // key-value 사용 
        //    //  JObject 인스턴스 생성
        //    JObject savedata = new JObject
        //    {
        //        ["key-name"] = "INPUT.text", // key-value 삽입 
        //        ["anyname"] = 1d, // int, float, string 
        //        ["is-save"] = true // bool 등 다양한 자료형 사용 가능 
        //    };

        //    // json에서 배열 사용하기 
        //    JArray arraydata = new JArray(); // JArray 인스턴스 생성 
        //    for (int i = 0; i < 5; i++)
        //    {
        //        // 랜덤한 값을 추가한다. 
        //        // C++에서 사용하는 vector의 push_back과 같다고 보면 된다. 
        //        arraydata.Add(UnityEngine.Random.Range(0.0f, 10.0f));
        //    }

        //    savedata["arraydata"] = arraydata; // 위에서 만든 JArray를 대입. 

        //    // 다른 방법으로 JArray 사용하기 
        //    savedata["newarr"] = new JArray(); // 새로운 key에 value로 JArray 할당. 

        //    for (int i = 0; i < 5; i++)
        //    {
        //        ((JArray)savedata["newarr"]).Add(UnityEngine.Random.Range(0, 50)); // JArray 변수를 만들어서 축약 가능 
        //    }

        //    // json 형식을 value로 사용하기 
        //    savedata["parent"] = new JObject(); // key를 지정하고 value에 new JObject()를 대입. 
        //    savedata["parent"]["child1"] = 123;
        //    savedata["parent"]["child2"] = 456;

        //    //출처: https://blog.komastar.kr/232 [World of Komastar]

        //    // 구조체 class를 json으로 변환하기 
        //    SaveData s = new SaveData(); // 인스턴스화 시키고 적당히 데이터를 입력. 
        //    s.id = 0;
        //    s.namelist1 = "komastar";
        //    s.namelist2 = "santaman";
        //    savedata["class-savedata"] = JToken.FromObject(s); // 파싱.


        //    // 파일로 저장 
        //    string savestring = JsonConvert.SerializeObject(savedata, Formatting.Indented); // JObject를 Serialize하여 json string 생성 
        //    File.WriteAllText(Application.persistentDataPath + "/pinkiepieisbestpony.json", AESEncrypt128(savestring)); // 생성된 string을 파일에 쓴다 
        //}


        ///// <summary>
        ///// 암호화된 json 불러오기
        ///// </summary>
        //void EX_JObjectLoad()
        //{
        //    // 불러오기는 저장의 역순 
        //    string loadstring = File.ReadAllText(Application.persistentDataPath + "/pinkiepieisbestpony.json"); // string을 읽음 
        //    JObject loaddata = JObject.Parse(AESDecrypt128(loadstring)); // JObject 파싱 

        //    Debug.Log(loaddata["key-name"]);

        //    // key 값으로 데이터 접근하여 적절히 사용 
        //    Debug.Log("key-value 개수 : " + loaddata.Count);
        //    Debug.Log("----------------------------");
        //    Debug.Log(loaddata["class-savedata"]);
        //    Debug.Log("----------------------------");
        //    JArray loadarray = (JArray)loaddata["arraydata"];

        //    for (int i = 0; i < loadarray.Count; i++)
        //    {
        //        Debug.Log(loadarray[i]);
        //    }

        //    Debug.Log("----------------------------");

        //    foreach (JToken item in loaddata["newarr"])
        //    {
        //        Debug.Log(item);
        //    }

        //    Debug.Log("----------------------------");
        //    Debug.Log(loaddata["newarr"]);
        //}

        //public struct SaveData
        //{
        //    // 변수 이름이 key값으로 사용된다. 
        //    public int id;
        //    public string namelist1;
        //    public string namelist2;
        //}

        //#endregion


        /// <summary>
        ///  랜덤 시드를 통해서 가중치 랜덤 뽑아낸다
        /// </summary>
        /// <param name="inputDatas"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public float GetRandom(float[] inputDatas, int seed)
    {
        System.Random random = new System.Random(seed);

        float total = 0;
        for (int i = 0; i < inputDatas.Length; i++)
        {
            total += inputDatas[i];
        }
        float pivot = (float)random.NextDouble() * total;
        for (int i = 0; i < inputDatas.Length; i++)
        {
            if (pivot < inputDatas[i])
            {
                return inputDatas[i];
            }
            else
            {
                pivot -= inputDatas[i];
            }
        }
        return inputDatas[inputDatas.Length - 1];
    }

    /// <summary>
    /// 랜덤 예제
    /// </summary>
    private void RandomTest()
    {
        /// 가중치 설정 합계가 100f 가 되도록.
        float[] probs = new float[] { 10f, 20f, 30f, 40f };
        /// 시행 횟수
        int tryNum = 10;

        System.Random seedRnd = new System.Random();
        int startIndex = seedRnd.Next();

        Dictionary<float, int> datas = new Dictionary<float, int>();
        for (int i = startIndex; i < startIndex + tryNum; i++)
        {
            float returnValue = GetRandom(probs, i);
            if (datas.ContainsKey(returnValue) == false)
            {
                datas.Add(returnValue, 1);
            }
            else
            {
                datas[returnValue]++;
            }
        }

        Debug.LogWarning(string.Format("시행 횟수 : {0}", tryNum));

        List<float> keys = datas.Keys.ToList();
        keys.Sort();

        for (int i = 0; i < keys.Count; i++)
        {
            Debug.LogWarning(string.Format("{0}이 {1}번 나옴 비율 : {2}%", keys[i], datas[keys[i]], (datas[keys[i]] / (float)tryNum) * 100f));
        }
    }




    [Header("-서버 데이터 팝업")]
    public GameObject SaveMyData;
    public GameObject LoadMyData;

    /// <summary>
    /// 세이브 후에 로컬 리셋
    /// </summary>
    public bool isResetAferSave;

    /// <summary>
    /// 이거 이전에 TEST_Save 해주고
    /// 진짜냐? 에 서버에 저장할래에 붙여줌.
    /// </summary>
    public void SaveMyDataReal()
    {
        SystemPopUp.instance.LoopSavingImg();
        /// 서버에서 JSON 몽땅 저장.
        JObjectSave(true);
        isResetAferSave = true;
    }


    /// <summary>
    /// 가끔씩 나는 서버에 백업한다.
    /// </summary>
    /// <returns></returns>
    public string ZZoGGoMiDataSave()
    {
        string result = "";

        result += (PlayerInventory.RecentDistance -1d).ToString("F0") + "*";
        result += PlayerInventory.Money_Dia.ToString() + "*";
        result += PlayerInventory.Money_Gold.ToString() + "*";
        result += PlayerInventory.Money_Leaf.ToString() + "*";
        result += PlayerInventory.Money_EnchantStone.ToString() + "*";
        result += PlayerInventory.Money_AmazonCoin.ToString();

        return result;
    }

    /// <summary>
    /// 데이터 불러오기 하기전에 서버 쪼꼬미 데이터 불러오기
    /// </summary>
    /// <param name="_Data"></param>
    public void ZZoGGoMiDataLoad(string _Data)
    {
        if (_Data == null || _Data == "")
        {
            isNODATA = true;
            return;
        }

        string[] sDataList = (_Data).Split('*');

        PlayerPrefs.SetString("Z_RD", sDataList[0]);
        PlayerPrefs.SetString("Z_Dia", sDataList[1]);
        PlayerPrefs.SetString("Z_Gold", sDataList[2]);
        PlayerPrefs.SetString("Z_Leaf", sDataList[3]);
        PlayerPrefs.SetString("Z_ES", sDataList[4]);
        PlayerPrefs.SetString("Z_Stone", sDataList[5]);
        PlayerPrefs.Save();

        isNODATA = false;
    }

    [Header("-쪼꼬미 데이터 팝업")]
    public GameObject DataCheckPanel;
    public Text DescField;

    bool isNODATA;

    /// <summary>
    /// 데이터 불ㄹ
    /// </summary>
    public void InitZZoGGo()
    {
        if (isNODATA)
        {
            DescField.text =
                "거리 : 0km"+  Environment.NewLine +
                "다이아 : 0"  + Environment.NewLine +
                "골드 : 0" + Environment.NewLine +
                "나뭇잎 : 0"  + Environment.NewLine +
                "강화석 : 0" + Environment.NewLine +
                "아마존 결정 : 0";
        }
        else
        {
            DescField.text =
                "거리 : " + (double.Parse(PlayerPrefs.GetString("Z_RD")) * 0.1d).ToString("N1") + "km" + Environment.NewLine +
                "다이아 : " + DoubleToStringNumber(double.Parse(PlayerPrefs.GetString("Z_Dia"))) + Environment.NewLine +
                "골드 : " + DoubleToStringNumber(double.Parse(PlayerPrefs.GetString("Z_Gold"))) + Environment.NewLine +
                "나뭇잎 : " + DoubleToStringNumber(double.Parse(PlayerPrefs.GetString("Z_Leaf"))) + Environment.NewLine +
                "강화석 : " + DoubleToStringNumber(double.Parse(PlayerPrefs.GetString("Z_ES"))) + Environment.NewLine +
                "아마존 결정 : " + DoubleToStringNumber(double.Parse(PlayerPrefs.GetString("Z_Stone")));
        }

        DataCheckPanel.SetActive(true);
    }







}
