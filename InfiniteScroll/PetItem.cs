using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PetItem : MonoBehaviour
{
    [Header("- 강화시 버튼 반짝")]
    public Ease ease;
    public Image[] passOrFail;
    public Button targetBtn;
    [Header("- 매니저")]
    public PetManager sm;
    public BuffManager bm;
    [Header("- 회색 커버 오브젝트")]
    public GameObject GrayImage;
    [Header("- 아이콘")]
    public Image spriteBox;
    public Image moneyBox;
    [Header("- 정보 표기 부분")]
    public Text NameBox;
    public Text LevelBox;
    public Text DescBox1;
    public Text DescBox2;
    public Text DescBox3;
    public Text DescBox4;
    public Text TitleText1;
    public GameObject TitleBox4;

    [Header("- 버튼 부분")]
    public Text UpgradeBox;
    public Text EarnGoldBox;
    [Header("- 버튼 색 바꾸기")]
    public Image DisableImage;
    public Image ShopImage;
    public Sprite[] BtnSprite;
    public GameObject ShopButton;
    public GameObject MaxButton;

    [Header("- 글로우 이펙트")]
    public GameObject[] glowEffect;

    const string T_LV = "Lv. ";
    const string MAX_LV = " / 100";
    const string T_PLUS = "+";
    const string T_DESC = " 증가량";
    const string T_DESC2 = " 버프";

    int _index;
    int thisLevel;




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
        // 코루틴 도는 중에는 타이머 텍스트 표시 갱신 무시
        ///if (sm.currentTimes[_index] == -1) TimerBox.text = SiBunCho((int)maxTime);
                // 글로우 숨김 델리게이트
        sm.chain += HideGrowEffect;
        HideGrowEffect();
        /// 기본 내용물 채워주기
        spriteBox.sprite = sm.icons[_index];
        thisLevel = int.Parse(ListModel.Instance.petList[_index].petLevel);
        /// 현재레벨  / 맥스레벨
        LevelBox.text = T_LV + thisLevel + MAX_LV;

        /// 0 번 인덱스는 골드로 강화하는 캐릭터 레벨
        switch (_index)
        {
            case 0:
                NameBox.text = ListModel.Instance.petList[_index].Desc;
                /// 공격력 증가량
                DescBox1.text = (ListModel.Instance.petList[_index].percentDam * thisLevel).ToString("F0") + "%";
                /// 쿨 타임
                DescBox2.text = thisLevel != 0 ? (ListModel.Instance.petList[_index].coolTime - ((thisLevel-1) * 2)).ToString("N0") + "초" : "228초";
                /// 타이틀
                TitleText1.text = "대미지";
                /// 온 오프
                TitleBox4.transform.GetChild(0).gameObject.SetActive(false);
                TitleBox4.transform.GetChild(1).gameObject.SetActive(false);
                break;

            default:
                NameBox.text = ListModel.Instance.petList[_index].Desc + T_DESC2;
                /// 공격력 증가량
                DescBox1.text = (ListModel.Instance.petList[_index].percentDam * thisLevel).ToString("F0") + "%";
                /// 쿨 타임
                DescBox2.text = thisLevel != 0 ? (ListModel.Instance.petList[_index].coolTime - ((thisLevel-1) * 3)).ToString("N0") + "초" : "307초";
                /// 버프 지속시간
                DescBox4.text = (ListModel.Instance.petList[_index].usingTimeDam * thisLevel).ToString("N0") + "초";
                /// 타이틀
                TitleText1.text = ListModel.Instance.petList[_index].Desc + T_DESC;
                break;
        }




        /// <다음_업그레이드비용> 배수 곱하기 해서 텍스트 뿌려주기.
        /// 나뭇잎으로 강화 할때
        if (sm.diaORleaf == 0)                  
        {
            /// 강화 확률
            DescBox3.text = thisLevel != 0 ? (100.7f - (thisLevel * 0.7f)).ToString("F1") + "%" : "100.0%";
            //
            if (thisLevel == 1)
            {
                UpgradeBox.text = (PlayerInventory.Leaf_Cost * ListModel.Instance.petList[_index].needUpgrade).ToString("N0");
            }
            else
            {
                double tmpResult = PlayerInventory.Leaf_Cost * ListModel.Instance.petList[_index].needUpgrade * 1.1d;
                tmpResult = System.Math.Pow(tmpResult, (thisLevel - 1));

                UpgradeBox.text = PlayerPrefsManager.instance.DoubleToStringNumber(System.Math.Truncate(tmpResult));
            }
        }
        /// <다이아>로 강화 할때
        else
        {                
            /// 강화 확률
            DescBox3.text = thisLevel != 0 ? (100.3f - (thisLevel * 0.3f)).ToString("F1") + "%" : "100.0%";
            UpgradeBox.text = "100";
        }



        /// <획득골드량> 배수 곱하기 해서 텍스트 뿌려주기.
        EarnGoldBox.text = T_PLUS + (ListModel.Instance.petList[_index].percentDam).ToString("F0") + "%";


        // 상태가 활성화 아니라면 회색 이미지 true
        if (ListModel.Instance.petList[_index].isEnable != "TRUE")
        {
            //GrayImage.SetActive(true);
            ShopButton.SetActive(true);
        }
        else
        {
            ShopButton.SetActive(false);
            GrayImage.SetActive(false);

            targetBtn.targetGraphic = DisableImage;

            // 영구 타이머 미작동시 한번 실행
            if (sm.currentTimes[_index] == -1)
            {
                if (ListModel.Instance.petList[_index].isEnable == "TRUE")
                {
                    if (_index == 0)
                    {
                        sm.AutoPetAttack();
                    }
                    else
                    {
                        bm.DieHardCoTimer(_index + 3);
                        sm.PetAnimStart(_index);
                    }
                }
            }
            //// 단일 타이머 미작동시 한번 실행
            //if (c_time == null && gameObject.activeSelf && int.Parse(name) == _index) c_time = StartCoroutine(TimerStart());

        }

        /// 만렙 찍으면 맥스버튼 활성화.
        MaxButton.SetActive(thisLevel >= 100);

    }


    /// <summary>
    /// 펫 카테고리에서 탭 뷰 클릭하면 
    /// 다이아 <-> 나뭇잎 이미지 전환하는거
    /// </summary>
    public void DiaLeafPoke()
    {
        if (sm.diaORleaf == 0)                  /// 나뭇잎
        {
            moneyBox.sprite = sm.moIcons[0];
            sm.displayOJ[0].SetActive(true);
            sm.displayOJ[1].SetActive(false);
        }
        else                                                    /// 다이아
        {
            moneyBox.sprite = sm.moIcons[1];
            sm.displayOJ[0].SetActive(false);
            sm.displayOJ[1].SetActive(true);
        }

        BoxInfoUpdate(_index);
    }





    /// <summary>
    /// 레벨업 버튼 클릭
    /// </summary>
    public void Clicked_LvUP()
    {
        if (MaxButton.activeSelf) return;

        // 글로우 모두 숨김
        sm.chain();
        // 이 오브젝트 글로우 표기
        glowEffect[0].SetActive(true);
        glowEffect[1].SetActive(true);

        /// 구매전 다이아로 구매하게 끔
        if (ListModel.Instance.petList[_index].isEnable != "TRUE")
        {
            if (PlayerInventory.Money_Dia < 1500) return;
            PlayerInventory.Money_Dia -= 1500;
            /// 돈 없으면 강화 안되게 막는 역할 || 맥스버튼이면
            if (ShopImage.sprite == BtnSprite[0] || MaxButton.activeSelf) return;
            //
            ShopButton.GetComponentInParent<Button>().targetGraphic = DisableImage;
            ShopButton.SetActive(false);
            //
            ListModel.Instance.Pet_Unlock(_index);
            ListModel.Instance.Pet_LevelUp(_index, 1);
            ///  업적  완료 카운트
            ListModel.Instance.ALLlist_Update(17, 1);
            /// 코루틴 동작
            if (_index == 0)
            {
                sm.AutoPetAttack();
            }
            else
            {
                bm.DieHardCoTimer(_index + 3);
                sm.PetAnimStart(_index);
            }
        }
        /// 다이아로 구매후엔 나뭇잎 강화.
        else
        {
            if (sm.diaORleaf == 0)                  /// 나뭇잎
            {
                if (!IsPurchaseable()) return;

                /// 돈 없으면 강화 안되게 막는 역할 || 맥스버튼이면
                if (DisableImage.sprite == BtnSprite[0] || MaxButton.activeSelf) return;

                /// 각각 업그레이드 비용이 다름
                if (thisLevel == 1)
                {
                    PlayerInventory.Money_Leaf -= PlayerInventory.Leaf_Cost * ListModel.Instance.petList[_index].needUpgrade;
                }
                else if (_index == 1)
                {
                    PlayerInventory.Money_Leaf -= PlayerInventory.Leaf_Cost * ListModel.Instance.petList[_index].needUpgrade * thisLevel * 1.15f;
                }
                else if (_index == 4)
                {
                    PlayerInventory.Money_Leaf -= PlayerInventory.Leaf_Cost * ListModel.Instance.petList[_index].needUpgrade * thisLevel * 1.3f;
                }
                else
                {
                    PlayerInventory.Money_Leaf -= PlayerInventory.Leaf_Cost * ListModel.Instance.petList[_index].needUpgrade * thisLevel * 1.2f;
                }

                /// 강화 성공/실패 계산
                float temp = Time.time * 525f;
                Random.InitState((int)temp);
                float random = Random.Range(0, 100f);
                /// 강화 성공은 여기로
                if (random <= 100.7f - (thisLevel * 0.7f))
                {
                    EnchantPassOrFail(true);
                    Debug.LogWarning("펫 강화 성공! ");
                    thisLevel++;
                    ListModel.Instance.Pet_LevelUp(_index, thisLevel);
                }
                /// 강화 실패는 여기로
                else
                {
                    EnchantPassOrFail(false);
                    Debug.LogWarning("강화 실패 ㅠㅠ  : ");
                }

                /// 만렙 찍으면 맥스버튼 활성화.
                if (thisLevel >= 100) MaxButton.SetActive(true);
            }
            /// 다이아
            else
            {
                if (PlayerInventory.Money_Dia < 100) return;

                /// 돈 없으면 강화 안되게 막는 역할 || 맥스버튼이면
                if (DisableImage.sprite == BtnSprite[0] || MaxButton.activeSelf) return;

                PlayerInventory.Money_Dia -= 100;

                /// 강화 성공/실패 계산
                float temp = Time.time * 525f;
                Random.InitState((int)temp);
                float random = Random.Range(0, 100f);
                /// 강화 성공은 여기로
                if (random <= 100.3f - (thisLevel * 0.3f))
                {
                    EnchantPassOrFail(true);
                    Debug.LogWarning("펫 강화 성공! ");
                    thisLevel++;
                    ListModel.Instance.Pet_LevelUp(_index, thisLevel);
                }
                /// 강화 실패는 여기로
                else
                {
                    EnchantPassOrFail(false);
                    Debug.LogWarning("강화 실패 ㅠㅠ  : ");
                }

                /// 만렙 찍으면 맥스버튼 활성화.
                if (thisLevel >= 100) MaxButton.SetActive(true);
            }

            ///  업적  완료 카운트
            ListModel.Instance.ALLlist_Update(17, 1);
        }

        // 값 갱신 -> 레벨업은 이쪽에서 처리
        BoxInfoUpdate(_index);

        // 글로우 모두 숨김
        sm.chain();
        // 이 오브젝트 글로우 표기
        glowEffect[0].SetActive(true);
        glowEffect[1].SetActive(true);
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
            passOrFail[1].DOFade(0,0);
            passOrFail[1].gameObject.SetActive(true);
            passOrFail[1].DOFade(0.7f, 0.3f).SetEase(Ease.OutElastic);
            passOrFail[2].gameObject.SetActive(true);
            passOrFail[2].DOFade(0,0);
            passOrFail[2].DOFade(1, 0.3f).SetEase(ease).OnComplete(ShutUPEnchant);
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
        if (ListModel.Instance.petList[_index].isEnable == "TRUE")
        {
            /// TODO : 버튼 활성화 / 비활성화 전환.
            ChangeBtnColor();
        }
        else
        {
            ChangePuchace();
            //if (DisableImage.sprite != BtnSprite[0]) DisableImage.sprite = BtnSprite[0]; //  OFF 이미지
        }
    }

    /// <summary>
    /// 살 수 있는 나뭇잎 모였는지 체크
    /// </summary>
    bool IsPurchaseable()
    {
        if (sm.diaORleaf == 0)                  /// 나뭇잎
        {
            /// 각각 업그레이드 비용이 다름
            if (thisLevel == 1)
            {
                if (PlayerInventory.Money_Leaf >= PlayerInventory.Leaf_Cost * ListModel.Instance.petList[_index].needUpgrade) return true;
                else return false;
            }
            else if (_index == 1)
            {
                if (PlayerInventory.Money_Leaf >= PlayerInventory.Leaf_Cost * ListModel.Instance.petList[_index].needUpgrade * thisLevel * 1.15f) return true;
                else return false;
            }
            else if (_index == 4)
            {
                if (PlayerInventory.Money_Leaf >= PlayerInventory.Leaf_Cost * ListModel.Instance.petList[_index].needUpgrade * thisLevel * 1.3f) return true;
                else return false;
            }
            else
            {
                if (PlayerInventory.Money_Leaf >= PlayerInventory.Leaf_Cost * ListModel.Instance.petList[_index].needUpgrade * thisLevel * 1.2f) return true;
                else return false;
            }

        }
        else                            /// 다이아
        {
            if (PlayerInventory.Money_Dia >= 100) return true;
            else return false;
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


    /// <summary>
    /// 버튼 색 노랑 <-> 회색 체인지
    /// </summary>
    void ChangePuchace()
    {
        if (PlayerInventory.Money_Dia >= 1500) ShopImage.sprite = BtnSprite[2]; //  On 이미지\
        else if (ShopImage.sprite != BtnSprite[0]) ShopImage.sprite = BtnSprite[0]; //  OFF 이미지
    }
}
