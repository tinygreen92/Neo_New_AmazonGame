using Newtonsoft.Json;
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
    [Header("- 무기 애니메이션 뭉태기")]
    public GameObject[] WeaponAnims;
    [Header("- 매니저들")]
    public TutoManager tm;
    public BuffManager bm;
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
    private string[] tunamayo;

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


    ///  ---------------------------안 암호화.

    public static ObscuredBool isJObjectLoad;                   // json 로딩 완료
    public static ObscuredBool isEnterTheMine;                            // 혹시 광산에 입장했니?
    public static ObscuredBool isEnterTheSwamp;                            // 혹시 늪지에 입장했니?
    public static ObscuredBool isGoldposOnAir;                            // 늪지 있을때는 돈 안 떨구기 나가면 돈 떨구기
    public static ObscuredBool isBossBtnAlive;                           // 화면 전환 후 배틀 캔버스에 보스 도전버튼 표기 되어야함
    public static ObscuredBool isIdleModeOn;                   // 지금 방치모드 켜져있니?
    public static ObscuredBool isCheckOffline;                   // 오프라인 팝업 띄웠니?

    /// <summary>
    /// 튜토리얼에서 사용하는 어디까지 클리어했니 인덱스
    /// </summary>
    public static ObscuredInt currentTutoIndex;


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
        tunamayo = new string[21];
        instance = this;
        EX_Start();
    }

    void OnApplicationQuit()
    {
        /* 앱이 종료 될 때 처리 */
        TEST_SaveJson();
    }

    private void OnApplicationPause(bool pause)
    {
        if (!isLoadingComp) return;
        /// 일시 정지 상태 진입
        if (pause)
        {
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
        }
    }

    /// <summary>
    /// 갓 모드에서 사용하는 것 _ 버튼 눌러서 데이터 저장 ?//  임시로 재화 저장
    /// </summary>
    public void TEST_SaveJson()
    {
        JObjectSave(ListModel.Instance.petList, 0);
        JObjectSave(ListModel.Instance.charatorList, 1);
        JObjectSave(ListModel.Instance.weaponList, 2);
        JObjectSave(ListModel.Instance.equipRuneList, 3);
        JObjectSave(ListModel.Instance.runeList, 4);
        JObjectSave(ListModel.Instance.invisibleruneList, 5);
        JObjectSave(ListModel.Instance.heartList, 6);
        JObjectSave(ListModel.Instance.invisibleheartList, 7);
        JObjectSave(ListModel.Instance.supList, 8);
        JObjectSave(ListModel.Instance.shopList, 9);
        JObjectSave(ListModel.Instance.shopListSPEC, 10);
        JObjectSave(ListModel.Instance.shopListNOR, 11);
        JObjectSave(ListModel.Instance.shopListPACK, 12);
        JObjectSave(ListModel.Instance.shopListAMA, 13);
        //
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
        //      
        /// 스트링 배열 몽땅 저장
        JObjectSave();

        /// 임시 골드 등등 저장
        ObscuredPrefs.SetString("RecentDistance", PlayerInventory.RecentDistance.ToString());
        ObscuredPrefs.SetString("Money_Gold", PlayerInventory.Money_Gold.ToString());
        ObscuredPrefs.SetString("Money_Elixir", PlayerInventory.Money_Elixir.ToString());
        ObscuredPrefs.SetString("Money_Dia", PlayerInventory.Money_Dia.ToString());
        ObscuredPrefs.SetString("Money_Leaf", PlayerInventory.Money_Leaf.ToString());
        ObscuredPrefs.SetString("Money_EnchantStone", PlayerInventory.Money_EnchantStone.ToString());
        ObscuredPrefs.SetString("Money_AmazonCoin", PlayerInventory.Money_AmazonCoin.ToString());
        ObscuredPrefs.SetString("AmazonStoneCount", PlayerInventory.AmazonStoneCount.ToString());
        ObscuredPrefs.SetString("CurrentAmaLV", PlayerInventory.CurrentAmaLV.ToString());
        ObscuredPrefs.SetString("box_Coupon", PlayerInventory.box_Coupon.ToString());
        ObscuredPrefs.SetString("box_E", PlayerInventory.box_E.ToString());
        ObscuredPrefs.SetString("box_D", PlayerInventory.box_D.ToString());
        ObscuredPrefs.SetString("box_C", PlayerInventory.box_C.ToString());
        ObscuredPrefs.SetString("box_B", PlayerInventory.box_B.ToString());
        ObscuredPrefs.SetString("box_A", PlayerInventory.box_A.ToString());
        ObscuredPrefs.SetString("box_S", PlayerInventory.box_S.ToString());
        ObscuredPrefs.SetString("box_L", PlayerInventory.box_L.ToString());
        ObscuredPrefs.SetString("ticket_reinforce_box", PlayerInventory.ticket_reinforce_box.ToString());
        ObscuredPrefs.SetString("ticket_leaf_box", PlayerInventory.ticket_leaf_box.ToString());
        ObscuredPrefs.SetString("ticket_pvp_enter", PlayerInventory.ticket_pvp_enter.ToString());
        ObscuredPrefs.SetString("ticket_cave_enter", PlayerInventory.ticket_cave_enter.ToString());
        ObscuredPrefs.SetString("ticket_cave_clear", PlayerInventory.ticket_cave_clear.ToString());
        ObscuredPrefs.SetString("S_reinforce_box", PlayerInventory.S_reinforce_box.ToString());
        ObscuredPrefs.SetString("S_leaf_box", PlayerInventory.S_leaf_box.ToString());
        ObscuredPrefs.SetString("mining", PlayerInventory.mining.ToString());
        ObscuredPrefs.SetString("amber", PlayerInventory.amber.ToString());
        ObscuredPrefs.SetInt("ZogarkMissionCnt", ZogarkMissionCnt);
        ObscuredPrefs.SetInt("AmaAdsTimer", AmaAdsTimer);
        ObscuredPrefs.SetInt("FreeDiaCnt", FreeDiaCnt);
        ObscuredPrefs.SetInt("FreeWeaponCnt", FreeWeaponCnt);
        ObscuredPrefs.SetInt("isDailyCheak", isDailyCheak ? 525 : 0);
        ObscuredPrefs.SetInt("isTutoAllClear", isTutoAllClear ? 525 : 0);


        /// 아직 오프라인 체크 안함
        if (!isCheckOffline)
        {
            return;
        }

        /// 오프라인 보상 체크 끝났다면 타이머 일괄적으로 저장
        string tmp = UnbiasedTime.Instance.Now().ToString("yyyyMMddHHmmss");
        ObscuredPrefs.SetString("DateTime", tmp);
        //ObscuredPrefs.SetString("AmazonShop", tmp);
        //ObscuredPrefs.SetString("Check_Daily", tmp);
        //ObscuredPrefs.SetString("FreeWeapon", tmp);
        //ObscuredPrefs.SetString("FreeDia", tmp);

        ObscuredPrefs.Save();

        Debug.LogWarning("세이브 데이터 타임 " + tmp);
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


    public void TEST_Distance()
    {
        PlayerInventory.RecentDistance += 1000.0d;
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
    /// int로 가져오는 건 스트링[]에 write한다.
    /// </summary>
    /// <param name="sd"></param>
    /// <param name="dir"></param>
    public void JObjectSave(object sd, int dir)
    {
        // JObject를 Serialize하여 json string 생성 
        string savestring = JsonConvert.SerializeObject(sd);
        tunamayo[dir] = AESEncrypt128(savestring);
    }

    /// <summary>
    /// 리스트를 Json으로 저장
    /// </summary>
    public void JObjectSave()
    {
        // 파일로 저장 
        string savestring = JsonConvert.SerializeObject(tunamayo); // JObject를 Serialize하여 json string 생성 
        File.WriteAllText(Application.persistentDataPath + "/_data_", AESEncrypt128(savestring)); // 생성된 string을 파일에 쓴다 
    }

    /// <summary>
    /// 파일명으로 접근해서 해당 리스트 로드
    /// </summary>
    /// <param name="dir">나중에 스위치로 전환해야할 때 입력 받으</param>
    public void JObjectLoad()
    {
        // 불러오기는 저장의 역순 
        string loadstring = File.ReadAllText(Application.persistentDataPath + "/_data_"); // string을 읽음 
        /// 배열 복구
        tunamayo = JsonConvert.DeserializeObject<string[]>(AESDecrypt128(loadstring));
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
        // 유료 구매 부분
        ListModel.Instance.mvpDataList = JsonConvert.DeserializeObject<List<MVP>>(AESDecrypt128(tunamayo[14]));
        //일퀘 / 업적 / 튜토리얼
        ListModel.Instance.missionDAYlist = JsonConvert.DeserializeObject<List<MissonSchool>>(AESDecrypt128(tunamayo[15]));
        ListModel.Instance.missionALLlist = JsonConvert.DeserializeObject<List<MissonSchool>>(AESDecrypt128(tunamayo[16]));
        ListModel.Instance.missionTUTOlist = JsonConvert.DeserializeObject<List<MissonSchool>>(AESDecrypt128(tunamayo[17]));
        // 채굴 관련
        ListModel.Instance.mineCraft = JsonConvert.DeserializeObject<List<MineCraft>>(AESDecrypt128(tunamayo[18]));
        ListModel.Instance.axeDataList = JsonConvert.DeserializeObject<List<AxeStat>>(AESDecrypt128(tunamayo[19]));
        // 늪지 관련
        ListModel.Instance.swampCaveData = JsonConvert.DeserializeObject<List<SwampCave>>(AESDecrypt128(tunamayo[20]));



        /// 유물 뽑은거 트리거 로드
        for (int i = 0; i < ListModel.Instance.heartList.Count; i++)
        {
            PlayerInventory.heartIndexs[int.Parse(ListModel.Instance.heartList[i].imgIndex) - 1] = i + 1;
        }
        /// 장착 룬 스탯 새로고침
        ListModel.Instance.SetEquipedRuneEffect();
        /// 유료 재화 구매 여부 로드
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
        /// 1회 한정 패키지 구매 여부 필요 한지?
        



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

        /// 스트링[] 몽땅 저장
        TEST_SaveJson();
        /// 로딩바 올려주는 걸 허락한다.
       isJObjectLoad = true;
    }

    /// <summary>
    /// 플레이 팹 저장용 json 로드후 리턴
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public string LoadStringJsonn(string dir)
    {
        string LOAD_DIR = "/" + dir + ".json";
        // 불러오기는 저장의 역순 
        string loadstring = File.ReadAllText(Application.persistentDataPath + LOAD_DIR); // string을 읽음 

        return loadstring;
    }




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




    #region Json.Net 예제

    /// <summary>
    /// Json 저장 메소드 예제
    /// </summary>
    void EX_JObjectSave()
    {
        // key-value 사용 
        //  JObject 인스턴스 생성
        JObject savedata = new JObject
        {
            ["key-name"] = "INPUT.text", // key-value 삽입 
            ["anyname"] = 1d, // int, float, string 
            ["is-save"] = true // bool 등 다양한 자료형 사용 가능 
        };

        // json에서 배열 사용하기 
        JArray arraydata = new JArray(); // JArray 인스턴스 생성 
        for (int i = 0; i < 5; i++)
        {
            // 랜덤한 값을 추가한다. 
            // C++에서 사용하는 vector의 push_back과 같다고 보면 된다. 
            arraydata.Add(UnityEngine.Random.Range(0.0f, 10.0f));
        }

        savedata["arraydata"] = arraydata; // 위에서 만든 JArray를 대입. 

        // 다른 방법으로 JArray 사용하기 
        savedata["newarr"] = new JArray(); // 새로운 key에 value로 JArray 할당. 

        for (int i = 0; i < 5; i++)
        {
            ((JArray)savedata["newarr"]).Add(UnityEngine.Random.Range(0, 50)); // JArray 변수를 만들어서 축약 가능 
        }

        // json 형식을 value로 사용하기 
        savedata["parent"] = new JObject(); // key를 지정하고 value에 new JObject()를 대입. 
        savedata["parent"]["child1"] = 123;
        savedata["parent"]["child2"] = 456;

        //출처: https://blog.komastar.kr/232 [World of Komastar]

        // 구조체 class를 json으로 변환하기 
        SaveData s = new SaveData(); // 인스턴스화 시키고 적당히 데이터를 입력. 
        s.id = 0;
        s.namelist1 = "komastar";
        s.namelist2 = "santaman";
        savedata["class-savedata"] = JToken.FromObject(s); // 파싱.


        // 파일로 저장 
        string savestring = JsonConvert.SerializeObject(savedata, Formatting.Indented); // JObject를 Serialize하여 json string 생성 
        File.WriteAllText(Application.persistentDataPath + "/pinkiepieisbestpony.json", AESEncrypt128(savestring)); // 생성된 string을 파일에 쓴다 
    }


    /// <summary>
    /// 암호화된 json 불러오기
    /// </summary>
    void EX_JObjectLoad()
    {
        // 불러오기는 저장의 역순 
        string loadstring = File.ReadAllText(Application.persistentDataPath + "/pinkiepieisbestpony.json"); // string을 읽음 
        JObject loaddata = JObject.Parse(AESDecrypt128(loadstring)); // JObject 파싱 

        Debug.Log(loaddata["key-name"]);

        // key 값으로 데이터 접근하여 적절히 사용 
        Debug.Log("key-value 개수 : " + loaddata.Count);
        Debug.Log("----------------------------");
        Debug.Log(loaddata["class-savedata"]);
        Debug.Log("----------------------------");
        JArray loadarray = (JArray)loaddata["arraydata"];

        for (int i = 0; i < loadarray.Count; i++)
        {
            Debug.Log(loadarray[i]);
        }

        Debug.Log("----------------------------");

        foreach (JToken item in loaddata["newarr"])
        {
            Debug.Log(item);
        }

        Debug.Log("----------------------------");
        Debug.Log(loaddata["newarr"]);
    }

    public struct SaveData
    {
        // 변수 이름이 key값으로 사용된다. 
        public int id;
        public string namelist1;
        public string namelist2;
    }

    #endregion


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



}
