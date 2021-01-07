using CodeStage.AntiCheat.Storage;
using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverManager : MonoBehaviour
{
    [Header("- 28.캐릭터 정보 팝업")]
    public Image[] ME_Img;
    public Sprite[] ME_Sprite;
    public GameObject[] MiddleEarth;

    [Header("- 내부캐릭터 정보창")]
    public HeartManager hm;
    public RuneManager rm;
    public Image[] charaHeartImg;
    public Image[] charaRuneImg;
    public Text[] charaInfoText;

    [Header("- 재화 정보창")]
    public Text[] moneyInfoText;
    public Text[] moneyDescText;

    [Header("- 기타 정보창")]
    public Text[] etcInfoText;
    public Text[] etcDescText;

    [Header("- 디버그 모드 활성화.")]
    public GameObject[] TEST_Btn;
    [Header("- 피버 이제 안쓰는거")]
    public Transform FeverParent;
    public GameObject testFeverSpr;
    public static int state_Monster_KillCount;                          // 죽인 몬스터 수
    public static bool isFeverTime;


    private void Awake()
    {
        state_Monster_KillCount = ObscuredPrefs.GetInt("state_Monster_KillCount");
        //FeverParent.GetChild(0).GetComponent<Image>().fillAmount = (state_Monster_KillCount / PlayerInventory.Kill_Cost);
    }


    #region <캐릭터 정보 창>

    /// <summary>
    /// 플레이어 종합 능력치 보여줌 (노란색)
    /// </summary>
    public void SetCharaInfoText()
    {
        charaInfoText[0].text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Player_DPS);
        charaInfoText[1].text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Player_HP);
        charaInfoText[2].text = PlayerInventory.Player_STAT_Attack_Speed.ToString("F1");
        charaInfoText[3].text = PlayerInventory.Player_STAT_Move_Speed.ToString("F2");
        charaInfoText[4].text = PlayerInventory.Player_Critical_Multiplier.ToString("F2") + " %";
        charaInfoText[5].text = PlayerInventory.Player_STAT_Critical_DPS.ToString("P2");
    }

    public void SetCharaRuneInfo()
    {
        /// 일단 투명화.
        int length = charaRuneImg.Length;
        for (int i = 0; i < length; i++)
        {
            charaRuneImg[i].gameObject.SetActive(false);
        }
        /// 리스트에 뭐라도 있다.
        for (int i = 0; i < ListModel.Instance.equipRuneList.Count; i++)
        {
            /// 아이콘 이미지 set
            charaRuneImg[i].sprite = rm.saveSprList[i];
            charaRuneImg[i].gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 유물 인포 0 ~30번 초기 이미지 세팅
    /// 비활성화 이미지까지 같이 세팅 -> 유물 DB 로딩 완료시 호출.
    /// </summary>
    public void InitCharaHeartInfo()
    {
        int length = hm.HeartSprs.Length;
        for (int i = 1; i < length; i++)
        {
            charaHeartImg[i-1].sprite = hm.HeartSprs[i];
            charaHeartImg[i-1].transform.GetChild(0).GetComponent<Image>().sprite = hm.HeartSprs[i];
        }
    }


    /// <summary>
    /// 현재 획득한 유물만 밝게 표시.
    /// </summary>
    public void SetCharaHeartInfo()
    {
        int length = ListModel.Instance.heartList.Count;
        for (int i = 0; i < length; i++)
        {
            charaHeartImg[int.Parse(ListModel.Instance.heartList[i].imgIndex)-1].transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    #endregion


    #region <재화 / 기타  정보>

    /// <summary>
    /// 최초실행시 대가리 텍스트 달아주는 용도
    /// </summary>
    public void InitMoneyInfoHead()
    {
        /// 골드
        moneyInfoText[0].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc30");
        moneyInfoText[1].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc17");
        /// 나뭇잎
        moneyInfoText[2].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc12");
        moneyInfoText[3].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc13");
        /// 강화석
        moneyInfoText[4].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc14");
        moneyInfoText[5].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc15");
        /// 수집
        moneyInfoText[6].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc10");
        moneyInfoText[7].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc09");
        moneyInfoText[8].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc11");
        /// 아마존 결정
        moneyInfoText[9].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc27");
        moneyInfoText[10].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc26");
    }

    /// <summary>
    /// 최초실행시 대가리 텍스트 달아주는 용도
    /// </summary>
    public void InitEtcInfoHead()
    {
        /// 몬스터
        etcInfoText[0].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc05");
        etcInfoText[1].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc06");
        /// 골드 박스
        etcInfoText[2].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc07");
        etcInfoText[3].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc08");
        /// 무기
        etcInfoText[4].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc25");
        etcInfoText[5].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc18");
        /// 오프라인
        etcInfoText[6].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc28");
        etcInfoText[7].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc29");
        /// 숨겨진 늪지
        etcInfoText[8].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc16");
        /// 피버타임
        etcInfoText[9].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc20");
        etcInfoText[10].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc21");
        etcInfoText[11].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc22");
        etcInfoText[12].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc23");
        etcInfoText[13].text = LeanLocalization.GetTranslationText("CharactorInfo_Heart_Desc24");
    }

    
    public void SetMoneyInfoTail()
    {
        /// 골드
        moneyDescText[0].text = (PlayerInventory.Player_Gold_Earned - 1.0d).ToString("P2");
        moneyDescText[1].text = PlayerInventory.STAT_Gold_Cost.ToString("P2");
        /// 나뭇잎
        moneyDescText[2].text = (PlayerInventory.Player_Leaf_Earned - 1.0d).ToString("P2");
        moneyDescText[3].text = PlayerInventory.STAT_Leaf_Cost.ToString("P2");
        /// 강화석
        moneyDescText[4].text = (PlayerInventory.EnchantStone_Earned - 1.0d).ToString("P2");
        moneyDescText[5].text = PlayerInventory.STAT_EnchantStone_Cost.ToString("P2");
        /// 수집
        moneyDescText[6].text = (PlayerInventory.Soozip_Gold_Earned - 1.0d).ToString("P2");
        moneyDescText[7].text = PlayerInventory.STAT_Soozip_Powerup_Gold.ToString("P2");
        moneyDescText[8].text = PlayerInventory.Soozip_Time.ToString("F1") +" 초";
        /// 아마존 결정
        moneyDescText[9].text = PlayerInventory.heart_equiped_amazonpoint_earned.ToString("F2") + " %";
        moneyDescText[10].text = PlayerInventory.STAT_AmazonPoint_Cost.ToString("P2");
    }

    public void SetEtcInfoTail()
    {
        /// 몬스터
        etcDescText[0].text = PlayerInventory.heart_equiped_monster_normal_HP.ToString("P2");
        etcDescText[1].text = PlayerInventory.heart_equiped_monster_boss_HP.ToString("P2");
        /// 골드박스
        etcDescText[2].text = PlayerInventory.heart_equiped_superbox_gold_earned.ToString("P2");
        etcDescText[3].text = PlayerInventory.heart_equiped_superbox_encounter.ToString("P2");
        /// 무기
        etcDescText[4].text = PlayerInventory.Weapon_Lv_Plus.ToString("+ 0");
        etcDescText[5].text = PlayerInventory.Weapon_owned_power.ToString("P2");
        /// 오프라인
        etcDescText[6].text = (PlayerInventory.Offline_Earned - 1.0d).ToString("P2");
        etcDescText[7].text = (PlayerInventory.Offline_Time - 10800.0d).ToString("F1") + " 초";
        /// 숨겨진 늪지
        etcDescText[8].text = PlayerInventory.heart_equiped_cave_time.ToString("F1") + " 초";
        /// 피버타임
        etcDescText[9].text = PlayerInventory.heart_equiped_fever_time.ToString("F1") + " 초";
        etcDescText[10].text = PlayerInventory.heart_equiped_fever_kill_cost.ToString("N0") + " 마리";
        etcDescText[11].text = (PlayerInventory.Fever_Power - 1.0d).ToString("P2");
        etcDescText[12].text = (PlayerInventory.Fever_Attack_Speed - 1.0d).ToString("P2");
        etcDescText[13].text = (PlayerInventory.Fever_Move_Speed - 1.0d).ToString("P2");
    }






    #endregion


    bool isFirstGameRun;
    /// <summary>
    /// 버튼 눌려서 팝업 호출
    /// </summary>
    public void ClickedCharactorInfo()
    {
        ///게임 껐다 켰을때만 초기 이미지 세팅해준당.
        if (!isFirstGameRun)
        {
            InitCharaHeartInfo();
            InitMoneyInfoHead();
            InitEtcInfoHead();
            isFirstGameRun = true;
        }
        // 탑버튼 0 번 눌러줌
        Clicked_Info_TopBtn(0);
    }

    /// <summary>
    /// 캐릭터 인포 탑 버튼 3가지
    /// </summary>
    public void Clicked_Info_TopBtn(int _Index)
    {
        MiddleEarth[0].SetActive(false);
        MiddleEarth[1].SetActive(false);
        MiddleEarth[2].SetActive(false);
        /// 선택 섹션 활성화
        MiddleEarth[_Index].SetActive(true);
        switch (_Index)
        {
            /// 캐릭터 정보
            case 0:
                SetCharaInfoText();
                SetCharaRuneInfo();
                SetCharaHeartInfo();
                //
                ME_Img[0].sprite = ME_Sprite[1];
                ME_Img[1].sprite = ME_Sprite[0];
                ME_Img[2].sprite = ME_Sprite[0];
                break;
            /// 재화 정보
            case 1:
                SetMoneyInfoTail();
                //
                ME_Img[0].sprite = ME_Sprite[0];
                ME_Img[1].sprite = ME_Sprite[1];
                ME_Img[2].sprite = ME_Sprite[0];
                break;
            /// 기타 정보
            case 2:
                SetEtcInfoTail();
                //
                ME_Img[0].sprite = ME_Sprite[0];
                ME_Img[1].sprite = ME_Sprite[0];
                ME_Img[2].sprite = ME_Sprite[1];
                break;
        }
        PopUpManager.instance.ShowPopUP(28);
    }


    /// <summary>
    /// 설정에 숨겨져있는 디버그 모드 활성화.
    /// </summary>
    public void TEST_Mode_Activate()
    {
        if (!StartManager.instance.isDeerveloperMode) return;
        TEST_Btn[0].SetActive(true);
        TEST_Btn[1].SetActive(true);
    }

    public int State_Monster_KillCount
    {
        get
        { 
            if (state_Monster_KillCount > 0) return state_Monster_KillCount; 
            else return 0; 
        }
        set
        {
            /// 피버 지속중엔 카운트 안 쌓임
            if (isFeverTime) return;
            ///
            state_Monster_KillCount = value;

            if (state_Monster_KillCount >= PlayerInventory.Kill_Cost)
            {
                StartCoroutine(ItsFeverTime());
            }
            else
            {
                Debug.LogWarning("누적 state_Monster_KillCount" + state_Monster_KillCount);
                //FeverParent.GetChild(0).GetComponent<Image>().fillAmount = (state_Monster_KillCount / PlayerInventory.Kill_Cost);
                ObscuredPrefs.SetInt("state_Monster_KillCount", state_Monster_KillCount);
                ///  킬 카운터 1 올리기
                ListModel.Instance.DAYlist_Update(5);
                ///  업적 카운트 올리기
                ListModel.Instance.ALLlist_Update(6,1);
            }
        }
    }

    IEnumerator ItsFeverTime()
    {
        yield return null;
        
        testFeverSpr.SetActive(true);
        /// 킬카운터 초기화 하고 피버 타임 활성화.
        state_Monster_KillCount = 0;
        //FeverParent.GetChild(0).GetComponent<Image>().fillAmount = 1;
        ObscuredPrefs.SetInt("state_Monster_KillCount", state_Monster_KillCount);

        isFeverTime = true;
        //
        float currntTime = 0;

        while (true)
        {
            yield return new WaitForFixedUpdate();
            currntTime += Time.deltaTime;
            //FeverParent.GetChild(0).GetComponent<Image>().fillAmount = (currntTime / maxTime);

            if (currntTime >= PlayerInventory.Fever_Time)
            {
                testFeverSpr.SetActive(false);

                //FeverParent.GetChild(0).GetComponent<Image>().fillAmount = 0;
                /// 피버 끝
                isFeverTime = false;
                break;
            }
        }
    }


    public void TestFeverCharge()
    {
        State_Monster_KillCount += 100;
    }




}
