using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponItem : MonoBehaviour
{
    public Image[] passOrFail;
    public WeaponManager sm;
    [Header("- 회색 커버 오브젝트")]
    public GameObject GrayImage;
    public GameObject SuperImage;
    [Header("- 아이콘")]
    public Image[] spriteBox;
    [Header("- 정보 표기 부분")]
    public Text NameBox;
    public Text LevelBox;
    public Text Desc1Box;
    public Text Desc2Box;
    public Text Desc3Box;
    public Transform DescParent;
    [Header("- 버튼 부분")]
    public Text UpgradeBox;
    public Text EarnGoldBox;
    public Transform EQUIP_BTN_OJ;

    [Header("- 버튼 색 바꾸기")]
    public Image DisableImage;
    public Sprite[] BtnSprite;
    public Sprite[] BtnSprite2;

    public GameObject MaxButton;

    [Header("- 글로우 이펙트")]
    public GameObject[] glowEffect;

    const string T_LV = "Lv. ";
    const string T_PLUS = "+";

    int _index;
    double _MultiResult;

    int thisLevel;


    /// <summary>
    /// 장착 버튼 노란 회색 컨트롤 -1 0 1 2 3
    /// </summary>
    /// <param name="_mode">-1 0 1 2 3</param>
    /// <param name="_on">on/off</param>
    void SetEQUIP_BTN_OJ(int _mode, bool _on)
    {
        if (_mode == 3)
        {
            if (thisLevel <= 100)
            {
                EarnGoldBox.text = Lean.Localization.LeanLocalization.GetTranslationText("Category_Weapon_RankUpText");
                DisableImage.sprite = BtnSprite[2];
                EQUIP_BTN_OJ.parent.GetComponent<Button>().targetGraphic = DisableImage;
                EQUIP_BTN_OJ.gameObject.SetActive(false);
                return;
            }
            else
            {
                EQUIP_BTN_OJ.parent.GetComponent<Button>().targetGraphic = DisableImage;
                EQUIP_BTN_OJ.gameObject.SetActive(false);
                return;
            }
        }

        if (_mode < 0)
        {
            EQUIP_BTN_OJ.parent.GetComponent<Button>().targetGraphic = DisableImage;
            EQUIP_BTN_OJ.gameObject.SetActive(false);
            return;
        }

        if (_on) EQUIP_BTN_OJ.GetComponent<Image>().sprite = BtnSprite2[1];
        else EQUIP_BTN_OJ.GetComponent<Image>().sprite = BtnSprite2[0];

        EQUIP_BTN_OJ.parent.GetComponent<Button>().targetGraphic = EQUIP_BTN_OJ.GetComponent<Image>();

        /// 최상단 켜주고
        EQUIP_BTN_OJ.gameObject.SetActive(true);
        /// 자식들 다 꺼주고
        for (int i = 0; i < EQUIP_BTN_OJ.childCount; i++)
        {
            EQUIP_BTN_OJ.GetChild(i).gameObject.SetActive(false);
        }
        /// 해당 자식 텍스트만 활성화.
        EQUIP_BTN_OJ.GetChild(_mode).gameObject.SetActive(true);
    }


    /// <summary>
    /// 업그레이드 필요 재화 표기
    /// </summary>
    /// <returns></returns>
    double GetUpgradeNeed()
    {
        if (sm.ModeName == 1)          /// 강화
        {
            _MultiResult = thisLevel > 1 ? ListModel.Instance.weaponList[_index].nextUpgradeCost * (1d+0.15d * thisLevel) : ListModel.Instance.weaponList[_index].nextUpgradeCost;

            return Math.Truncate(_MultiResult);
        }
        else if (sm.ModeName == 3)          /// 초월
        {
            _MultiResult = thisLevel <= 100 ? ListModel.Instance.weaponList[_index].rankUpENstone : ListModel.Instance.weaponList[_index].rankUpENstone * (1d + 0.15d * (thisLevel-100));
            return Math.Truncate(_MultiResult);
        }
        else return 0;
    }

    string addWeponLvText = "";
    int addWeponInt;
    float floDesc1Box;
    /// <summary>
    /// 박스 SetActive(true)일때 호출
    /// 변경 값 갱신 해줌
    /// 1. 아우터 글로우 해제
    /// 2. 해당 코루틴 일때만 재생
    /// 3. 배수 적용시 지닌 화폐만큼만 뻥튀기 해서 (int) 수치 붙이기
    /// </summary>
    public void BoxInfoUpdate(int cnt)
    {
        /// 인덱스 설정 -> 이 스크립트 전체
        _index = cnt;
        // 글로우 숨김 델리게이트
        sm.chain += HideGrowEffect;
        HideGrowEffect();


        /// 갯수 0개 라면 비활성화.
        if (int.Parse(ListModel.Instance.weaponList[_index].weaAmount) == 0)
        {
            GrayImage.SetActive(true);
            spriteBox[1].gameObject.SetActive(false);
        }
        else
        {
            GrayImage.SetActive(false);
            spriteBox[1].gameObject.SetActive(true);
        }

        /// ====================================== 공통 =============================
         
        thisLevel = int.Parse(ListModel.Instance.weaponList[_index].weaponLevel);
        addWeponInt = Mathf.RoundToInt((float)PlayerInventory.Weapon_Lv_Plus);
        /// 무기 레벨 증가 유물 레벨이 0 이상이면 추가 텍스트 활성화
        if (addWeponInt > 0)
        {
            addWeponLvText = " ( +" + addWeponInt + " )";
        }
        else
        {
            /// 유물 없으면 빈칸
            addWeponLvText = "";
        }

        MaxButton.SetActive(false);
        spriteBox[0].sprite = sm.blackSprs[_index];
        spriteBox[1].sprite = sm.WeaponSprs[_index];
        //spriteBox.transform.parent.GetComponent<Image>().sprite = sm.WeaponSprs[_index];
        NameBox.text = ListModel.Instance.weaponList[_index].headRank + ListModel.Instance.weaponList[_index].tailRank + "등급 무기 ( " + ListModel.Instance.weaponList[_index].weaAmount + " / 10 )";
        LevelBox.text = T_LV + thisLevel + addWeponLvText;
        ///
        floDesc1Box = (ListModel.Instance.weaponList[_index].startPower + (ListModel.Instance.weaponList[_index].increedPower * (thisLevel + addWeponInt)));
        Desc1Box.text = thisLevel + addWeponInt <= 1 ? (ListModel.Instance.weaponList[_index].startPower).ToString("F2") + "%" : floDesc1Box.ToString("F2") + "%";
        Desc2Box.text = thisLevel + addWeponInt <= 1 ? (ListModel.Instance.weaponList[_index].startPower * 0.1f).ToString("F2") + "%" : (floDesc1Box * 0.1f).ToString("F2") + "%";
        Desc3Box.text = thisLevel < 100 ? (ListModel.Instance.weaponList[_index].startPassFail).ToString("F2") + "%" : (ListModel.Instance.weaponList[_index].startPassFail - (ListModel.Instance.weaponList[_index].passFailPer * (thisLevel - 100))).ToString("F2") + "%";

        /// <다음_업그레이드비용> 배수 곱하기 해서 텍스트 뿌려주기.
        UpgradeBox.text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.EnchantStone_Cost * GetUpgradeNeed());
        /// <상승량> 배수 곱하기 해서 텍스트 뿌려주기.
        EarnGoldBox.text = T_PLUS + ListModel.Instance.weaponList[_index].increedPower.ToString("F2") + "%";
        /// 2개 짜리 파렛트
        DescParent.GetChild(0).gameObject.SetActive(true);
        DescParent.GetChild(1).gameObject.SetActive(true);
        DescParent.GetChild(2).gameObject.SetActive(false);

        /// ====================================== 개별 =============================

        if (sm.ModeName == 0)            /// 장착
        {
            SuperImage.SetActive(false);
            /// 장착중이 아니면?
            if (ListModel.Instance.weaponList[_index].isEnable != "TRUE")
            {
                SetEQUIP_BTN_OJ(0, true);
            }
            else
            {
                SetEQUIP_BTN_OJ(1, false);
            }

        }
        else if (sm.ModeName == 1)          /// 강화
        {
            SuperImage.SetActive(false);
            SetEQUIP_BTN_OJ(-1, true);
            /// 만렙 찍으면 맥스버튼 활성화.
            MaxButton.SetActive(thisLevel >= 100);
        }
        else if (sm.ModeName == 2)           /// 합성
        {
            SuperImage.SetActive(false);
            if (int.Parse(ListModel.Instance.weaponList[_index].weaAmount) >= 10)
            {
                SetEQUIP_BTN_OJ(2, true);
            }
            else
            {
                SetEQUIP_BTN_OJ(2, false);
            }
            /// 마지막 무기는 예외처리
            if(_index == 30) SetEQUIP_BTN_OJ(2, false);
        }
        else                                                        /// 초월 강화
        {
            if (thisLevel < 100)
            {
                SuperImage.SetActive(true);
            }
            else
            {
                SuperImage.SetActive(false);
            }
            /// 2개 짜리 파렛트
            DescParent.GetChild(0).gameObject.SetActive(false);
            DescParent.GetChild(1).gameObject.SetActive(false);
            DescParent.GetChild(2).gameObject.SetActive(true);

            SetEQUIP_BTN_OJ(3, true);
            /// 만렙 찍으면 맥스버튼 활성화.
            MaxButton.SetActive(thisLevel >= 150);
        }

    }


    /// <summary>
    /// 버튼 뭉태기  클릭
    /// </summary>
    public void Clicked_LvUP()
    {
        if (MaxButton.activeSelf) return;

        // 글로우 모두 숨김
        sm.chain();
        // 이 오브젝트 글로우 표기
        glowEffect[0].SetActive(true);
        glowEffect[1].SetActive(true);


        if (sm.ModeName == 0)            /// 장착
        {
            /// 장착중이 아니면?
            if (ListModel.Instance.weaponList[_index].isEnable != "TRUE")
            {
                /// 선택된 인덱스 말고 전부 장착해제
                for (int i = 0; i < ListModel.Instance.weaponList.Count; i++)
                {
                    if (ListModel.Instance.weaponList[i].isEnable == "TRUE")
                    {
                        ListModel.Instance.Weapon_Equip(i, false);                  /// 무기 해제

                        for (int j = 1; j < sm.transform.childCount; j++)
                        {
                            sm.transform.GetChild(j).GetComponent<WeaponItem>()
                                .BoxInfoUpdate(int.Parse(sm.transform.GetChild(j).name));
                        }
                    }
                }
                /// 현재 무기 장착
                ListModel.Instance.Weapon_Equip(_index, true);
            }
        }
        else if (sm.ModeName == 1)          /// 강화
        {
            if (!IsPurchaseable()) return;
            /// 돈 없으면 강화 안되게 막는 역할 || 맥스버튼이면 리턴
            if (DisableImage.sprite == BtnSprite[0] || MaxButton.activeSelf) return;

            /// 소숫점 아래 버림
            PlayerInventory.Money_EnchantStone -= Mathf.CeilToInt((float)(PlayerInventory.EnchantStone_Cost * GetUpgradeNeed()));

            thisLevel++;
            
            ///  업적  완료 카운트
            ListModel.Instance.ALLlist_Update(13, 1);
            ListModel.Instance.Weapon_LvUP(_index, thisLevel);
            /// 무기 강화 1회 진행
            if (PlayerPrefsManager.currentTutoIndex == 7) ListModel.Instance.TUTO_Update(7);
            /// 내가 가진 최고레벨 무기인가 ?? -> 튜토리얼 퀘스트용
            if (int.Parse(ListModel.Instance.weaponList[_index].weaponLevel)> PlayerInventory.MAX_WEAPON_LV)
            {
                PlayerInventory.MAX_WEAPON_LV = int.Parse(ListModel.Instance.weaponList[_index].weaponLevel);
                Debug.LogWarning("아무 무기 최대값 : " + PlayerInventory.MAX_WEAPON_LV);
            }
            /// 아무 무기 Lv. 10 달성하기
            if (PlayerPrefsManager.currentTutoIndex == 48) ListModel.Instance.TUTO_Update(48);

        }
        else if (sm.ModeName == 2)           /// 합성
        {
            /// 인덱스 30 = 마지막 무기는 합성 버튼 눌러도 반응 없게 하라
            if (EQUIP_BTN_OJ.GetComponent<Image>().sprite != BtnSprite2[1] || _index == 30) return;
            /// 해당 무기 5개 빼주고
            ListModel.Instance.Weapon_Sub(_index);
            /// 다음 단계 +1
            ListModel.Instance.Weapon_Add(_index+1);
            /// 새로고침
            sm.RefreshWeapon();
        }
        else                                                        /// 초월 강화
        {
            if (!IsPurchaseable() || thisLevel < 100) return;
            /// 돈 없으면 강화 안되게 막는 역할 || 맥스버튼이면 리턴
            if (DisableImage.sprite == BtnSprite[0] || MaxButton.activeSelf) return;
            /// 강화석 소모 소숫점 아래 버림
            PlayerInventory.Money_EnchantStone -= Mathf.CeilToInt((float)(PlayerInventory.EnchantStone_Cost * GetUpgradeNeed()));
            /// 강화 성공/실패 계산
            float temp = Time.time * 525f;
            UnityEngine.Random.InitState((int)temp);
            float random = UnityEngine.Random.Range(0, 100f);
            ///  초월 업적  완료 카운트
            ListModel.Instance.ALLlist_Update(14, 1);
            /// 강화 성공은 여기로
            if (random <= (ListModel.Instance.weaponList[_index].startPassFail - (ListModel.Instance.weaponList[_index].passFailPer * (thisLevel - 100))))
            {
                thisLevel++;
                ListModel.Instance.Weapon_LvUP(_index, thisLevel);
                EnchantPassOrFail(true);
            }
            /// 강화 실패는 여기로
            else
            {
                EnchantPassOrFail(false);
            }

        }


        // 값 갱신 -> 레벨업은 이쪽에서 처리
        BoxInfoUpdate(_index);
    }


    /// <summary>
    /// 아우터 글로우 숨기기
    /// </summary>
    void HideGrowEffect()
    {
        for (int i = 0; i < glowEffect.Length; i++)
        {
            glowEffect[i].SetActive(false);
        }
    }

    void EnchantPassOrFail(bool _ispass)
    {
        if (_ispass)
        {
            passOrFail[1].DOFade(0, 0);
            passOrFail[1].gameObject.SetActive(true);
            passOrFail[1].DOFade(0.7f, 0.3f).SetEase(Ease.OutElastic);
            passOrFail[2].gameObject.SetActive(true);
            passOrFail[2].DOFade(0, 0);
            passOrFail[2].DOFade(1, 0.3f).SetEase(Ease.OutBack).OnComplete(ShutUPEnchant);
        }
        else
        {
            passOrFail[0].DOFade(0, 0);
            passOrFail[0].gameObject.SetActive(true);
            passOrFail[0].DOFade(0.7f, 0.3f).SetEase(Ease.OutElastic).OnComplete(ShutUPEnchant);
        }
    }
    private void ShutUPEnchant()
    {
        passOrFail[0].gameObject.SetActive(false);
        passOrFail[1].gameObject.SetActive(false);
        passOrFail[2].gameObject.SetActive(false);
    }
    private void Update()
    {
        if (sm.ModeName == 3 && thisLevel <= 100) return;

        /// TODO : 버튼 활성화 / 비활성화 전환.
        ChangeBtnColor();
    }

    /// <summary>
    /// 살 수 있는 강화석가 모였는지 체크
    /// </summary>
    bool IsPurchaseable()
    {
        if (PlayerInventory.Money_EnchantStone >= (PlayerInventory.EnchantStone_Cost * GetUpgradeNeed()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// 버튼 색 노랑 <-> 회색 체인지
    /// </summary>
    void ChangeBtnColor()
    {
        if (IsPurchaseable()) DisableImage.sprite = BtnSprite[1]; //  On 이미지\
        else if (DisableImage.sprite != BtnSprite[0]) DisableImage.sprite = BtnSprite[0]; //  OFF 이미지
    }
}
