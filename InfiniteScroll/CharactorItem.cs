using System;
using UnityEngine;
using UnityEngine.UI;

public class CharactorItem : MonoBehaviour
{
    public CharactorManager sm;
    [Header("- 아이콘")]
    public Sprite[] icons;
    public Image spriteBox;
    [Header("- 정보 표기 부분")]
    public Text NameBox;
    public Text LevelBox;
    public Text[] DescBox;
    public Text[] EffectBox;
    [Header("- 버튼 부분")]
    public Text UpgradeBox;
    public Text EarnGoldBox;
    [Header("- 버튼 색 바꾸기")]
    public Image DisableImage;
    public Sprite[] BtnSprite;
    public GameObject MaxButton;
    [Header("- 글로우 이펙트")]
    public GameObject[] glowEffect;
    [Header("- 0번 인덱스 전용")]
    public Transform midPannel;
    public Image buttonIcon;
    public Sprite[] buttonIconSprite;



    const string T_LV = "Lv. ";
    const string T_PLUS = "+";

    int _index = 0;

    int thisLevel;
    int mutiple = 1;

    double _MultiResult;
    double _MultiLv;

    /// <summary>
    /// 인덱스 0 은  Desc 영역이 2개
    /// </summary>
    /// <param name="_isOn"></param>
    void ChractorLvUp(bool _isOn)
    {
        midPannel.GetChild(0).gameObject.SetActive(_isOn);
        midPannel.GetChild(1).gameObject.SetActive(_isOn);
        midPannel.GetChild(2).gameObject.SetActive(!_isOn);
        /// 재화 아이콘 바꿔 끼우기
        if (_isOn) buttonIcon.sprite = buttonIconSprite[0];  /// 골드 아이콘
        else buttonIcon.sprite = buttonIconSprite[1];           /// 엘릭서 아이콘
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

    public void BoxInfoUpdate(int cnt)
    {
        /// 인덱스 설정->이 스크립트 전체
        _index = cnt;

        thisLevel = int.Parse(ListModel.Instance.charatorList[_index].charLevel);

        spriteBox.sprite = icons[_index];

        //for (int i = 0; i < PlayerPrefsManager.charaUpgrageMutiple; i++)
        //{
        //    mutiple = i + 1;

        //    // 돈 안 충분하다.
        //    if (!IsPurchaseable())
        //    {
        //        if (mutiple == 1) break;
        //        mutiple -= 1;       break;
        //    }
        //}
        mutiple = PlayerPrefsManager.charaUpgrageMutiple;

        /// 만렙 예외 처리 1000레벨 고정
        if (mutiple == 10 && thisLevel >= 9990) mutiple = (9999 - thisLevel);
        else if (mutiple == 100 && thisLevel >= 9900) mutiple = (9999 - thisLevel);

        /// 0 번 인덱스는 골드로 강화하는 캐릭터 레벨
        switch (_index)
        {
            case 0:
                ChractorLvUp(true);
                DescBox[0].text = "캐릭터 공격력";
                DescBox[1].text = "캐릭터 체력";
                EffectBox[0].text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.character_DPS);
                EffectBox[1].text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.character_HP);
                break;

            case 1:
                ChractorLvUp(false);
                DescBox[2].text = ListModel.Instance.charatorList[_index].description;
                EffectBox[2].text = PlayerInventory.stat_power.ToString("N0");

                break;

            case 2:
                ChractorLvUp(false);
                DescBox[2].text = ListModel.Instance.charatorList[_index].description;
                EffectBox[2].text = PlayerInventory.stat_attack_speed.ToString("F1");        /// 소수점 1자리, 공격속도 증가
                break;

            case 3:
                ChractorLvUp(false);
                DescBox[2].text = ListModel.Instance.charatorList[_index].description;
                EffectBox[2].text = PlayerInventory.stat_move_speed.ToString("F2");        /// 소수점 1자리, 이동속도 증가                
                break;

            case 4:
                ChractorLvUp(false);
                DescBox[2].text = ListModel.Instance.charatorList[_index].description;
                EffectBox[2].text = PlayerInventory.stat_cri_multi.ToString("F2") + "%";        /// 소줏점 2자리
                break;

            case 5:
                ChractorLvUp(false);
                DescBox[2].text = ListModel.Instance.charatorList[_index].description;
                EffectBox[2].text = PlayerInventory.stat_cri_dps.ToString("F1") + "%";        ///  
                break;

            //case 6:
            //    ChractorLvUp(false);
            //    DescBox[2].text = ListModel.Instance.charatorList[_index].desc;
            //    EffectBox[2].text = PlayerInventory.stat_gold_earned.ToString("F0") + "%";        ///    
            //    break;

            //case 7:
            //    ChractorLvUp(false);
            //    DescBox[2].text = ListModel.Instance.charatorList[_index].desc;
            //    EffectBox[2].text = PlayerInventory.stat_leaf_earned.ToString("F0") + "%";        ///            
            //    break;

            default:
                break;
        }

        // 글로우 숨김 델리게이트
        sm.chain += HideGrowEffect;
        HideGrowEffect();

        NameBox.text = ListModel.Instance.charatorList[_index].title;

        LevelBox.text = T_LV + thisLevel;

        /// < 다음_업그레이드비용 > 배수 곱하기 해서 텍스트 뿌려주기.
        /// 인덱스 0번 캐릭터 레벨 증가 예외처리 -> 레벨업 공식에 따름
        if (_index == 0)
        {
            UpgradeBox.text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Gold_Cost * GetMutipleUpgrade(mutiple, thisLevel));
            /// < 획득골드량 > 배수 곱하기 해서 텍스트 뿌려주기.
            EarnGoldBox.text = T_PLUS + PlayerPrefsManager.instance.DoubleToStringNumber(GetMutipleEarnGold(mutiple, 0)) + " (" + mutiple + ")";
        }
        else if(_index == 2 || _index == 3 ||_index == 4)
        {
            UpgradeBox.text = PlayerPrefsManager.instance.DoubleToStringNumber(GetMutipleUpgrade(mutiple, thisLevel));
            /// < 획득골드량 > 배수 곱하기 해서 텍스트 뿌려주기.
            EarnGoldBox.text = T_PLUS + GetMutipleEarnGold(mutiple, 0).ToString("F2") + "% (" + mutiple + ")";
        }
        else
        {
            UpgradeBox.text = PlayerPrefsManager.instance.DoubleToStringNumber(GetMutipleUpgrade(mutiple, thisLevel));
            /// < 획득골드량 > 배수 곱하기 해서 텍스트 뿌려주기.
            if (_index == 5) EarnGoldBox.text = T_PLUS + GetMutipleEarnGold(mutiple, 0).ToString("F1") + "% (" + mutiple + ")";
            else EarnGoldBox.text = T_PLUS + GetMutipleEarnGold(mutiple, 0).ToString("F0") + " (" + mutiple + ")";
        }



        /// 만렙 찍으면 맥스버튼 활성화.
        if(_index == 4)
        {
            MaxButton.SetActive(thisLevel >= 500);
        }
        else
        {
            MaxButton.SetActive(thisLevel >= 9999);
        }
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

        /// 돈없음 꺼져잇
        if (!IsPurchaseable()) return;
        /// 돈 없으면 강화 안되게 막는 역할 || 맥스버튼이면
        if (DisableImage.sprite == BtnSprite[0] || MaxButton.activeSelf) return;

        /// 돈 있으면 재화 소모
        if (_index == 0)
        {
            /// 멀티플 적용 받고 골드 소모
            PlayerInventory.Money_Gold -= PlayerInventory.Gold_Cost * GetMutipleUpgrade(mutiple, thisLevel);
            /// 엘릭서 지급
            PlayerInventory.Money_Elixir += mutiple;

            /// 캐릭터 레벨 업
            thisLevel = (int)_MultiLv;
            ListModel.Instance.Chara_LvUP(_index, thisLevel);

            ///  캐릭터 레벨업 업적  완료 카운트
            ListModel.Instance.ALLlist_Update(1, mutiple);
            /// 캐릭터 1회 레벨업 진행
            if (PlayerPrefsManager.currentTutoIndex == 1) ListModel.Instance.TUTO_Update(1);
            if (PlayerPrefsManager.currentTutoIndex == 9) ListModel.Instance.TUTO_Update(9);
            if (PlayerPrefsManager.currentTutoIndex == 15) ListModel.Instance.TUTO_Update(15);
            if (PlayerPrefsManager.currentTutoIndex == 23) ListModel.Instance.TUTO_Update(23);
            if (PlayerPrefsManager.currentTutoIndex == 30) ListModel.Instance.TUTO_Update(30);
            if (PlayerPrefsManager.currentTutoIndex == 37) ListModel.Instance.TUTO_Update(37);
            ///캐릭터 Lv. 5 달성
            if (PlayerPrefsManager.currentTutoIndex == 11) ListModel.Instance.TUTO_Update(11);
            /// 캐릭터 Lv. 10 달성
            if (PlayerPrefsManager.currentTutoIndex == 41) ListModel.Instance.TUTO_Update(41);
        }
        else
        {
            /// 멀티플 적용 받고 <엘릭서> 소모
            PlayerInventory.Money_Elixir -= (int)GetMutipleUpgrade(mutiple, thisLevel);

            /// 스텟 레벨 업
            thisLevel = (int)_MultiLv;
            ListModel.Instance.Chara_LvUP(_index, thisLevel);

            ///  스탯 공격력 증가 레벨업 1회 진행
            if (_index == 1 &&  PlayerPrefsManager.currentTutoIndex == 2) ListModel.Instance.TUTO_Update(2);
            if (_index == 1 &&  PlayerPrefsManager.currentTutoIndex == 31) ListModel.Instance.TUTO_Update(31);
            /// 스탯 공격속도 증가
            if (_index == 2 &&  PlayerPrefsManager.currentTutoIndex == 38) ListModel.Instance.TUTO_Update(38);
            /// 스탯 이동속도 증가 레벨업 1회 진행
            if (_index == 3 &&  PlayerPrefsManager.currentTutoIndex == 10) ListModel.Instance.TUTO_Update(10);
            /// 스탯 치명타 확률 증가 레벨업 1회 진행
            if (_index == 4 &&  PlayerPrefsManager.currentTutoIndex == 16) ListModel.Instance.TUTO_Update(16);
            /// 스탯 치명타 대미지 증가 레벨업 1회 진행
            if (_index == 5 && PlayerPrefsManager.currentTutoIndex == 24)
            {
                ListModel.Instance.TUTO_Update(24);
            }
        }



        // 값 갱신 -> 레벨업은 이쪽에서 처리
        BoxInfoUpdate(_index);
        /// 멀티플 값을 활성화된 아이템에 모두 적용
        sm.RefleshAllitem();

        // 글로우 모두 숨김
        sm.chain();

        // 이 오브젝트 글로우 표기
        glowEffect[0].SetActive(true);
        glowEffect[1].SetActive(true);


    }



    /// <summary>
    /// 업그레이드 배수 만큼 포문 돌려서 적정 주가 계산
    /// </summary>
    public void RefreshMutiple()
    {
        //for (int i = 0; i < sm.upgrageMutiple; i++)
        //{
        //    mutiple = i + 1;
        //    // 돈 안 충분하다.
        //    if (!IsPurchaseable())
        //    {
        //        if (mutiple == 1) break;
        //        mutiple -= 1;
        //        break;
        //    }
        //}
        mutiple = sm.upgrageMutiple;

        if (_index == 4)
        {
            /// 만렙 예외 처리 1000레벨 고정
            if (mutiple == 10 && thisLevel >= 491)
            {
                mutiple = (500 - thisLevel);
            }
            else if (mutiple == 100 && thisLevel >= 401)
            {
                mutiple = (500 - thisLevel);
            }
        }
        else
        {
            /// 만렙 예외 처리 1000레벨 고정
            if (mutiple == 10 && thisLevel >= 9990)
            {
                mutiple = (9999 - thisLevel);
            }
            else if (mutiple == 100 && thisLevel >= 9900)
            {
                mutiple = (9999 - thisLevel);
            }
        }

        BoxInfoUpdate(_index);

        ///// <다음_업그레이드비용> 배수 곱하기 해서 텍스트 뿌려주기.
        //UpgradeBox.text = PlayerPrefsManager.instance.DoubleToStringNumber(GetMutipleUpgrade(mutiple, thisLevel));
        ///// <획득골드량> 배수 곱하기 해서 텍스트 뿌려주기.
        //EarnGoldBox.text = T_PLUS + PlayerPrefsManager.instance.DoubleToStringNumber(GetMutipleEarnGold(mutiple, 0)) + " (" + mutiple + ")";
    }

    /// <summary>
    /// 살 수 있는 골드가 모였는지 체크
    /// </summary>
    bool IsPurchaseable()
    {
        if (_index == 0)            /// 골드
        {
            if (PlayerInventory.Money_Gold >= PlayerInventory.Gold_Cost * GetMutipleUpgrade(mutiple, thisLevel)) return true;
            else return false;
        }
        else                                /// 엘릭서는 한개씩 소모한다고
        {
            if (PlayerInventory.Money_Elixir >= GetMutipleUpgrade(mutiple, thisLevel)) return true;
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

    private void Update()
    {
        /// TODO : 버튼 활성화 / 비활성화 전환.
        ChangeBtnColor();
    }


    /// <summary>
    /// 업그레이드 골드 소모 
    /// </summary>
    /// <param name="muti"> 곱해줄 배수 </param>
    /// <param name="lv"> 현재 레벨 thisLevel </param>
    /// <returns></returns>
    double GetMutipleUpgrade(int muti, int lv)
    {
        if(_index == 4)
        {
            // 만렙 찍으면  MAX 
            if (thisLevel >= 500)
            {
                thisLevel = 500;
                return 0;
            }
        }
        else
        {
            // 만렙 찍으면  MAX 
            if (thisLevel >= 9999)
            {
                thisLevel = 9999;
                return 0;
            }
        }

        _MultiResult = ListModel.Instance.charatorList[_index].nextUpgradeCost;
        // 다음 1레벨 / 다음 10레벨 / 다음 100레벨
        _MultiLv = muti + lv;

        if (_index == 4)
        {
            /// 만렙 예외 처리 1000레벨 고정
            if (_MultiLv > 499) _MultiLv = 500;
        }
        else
        {
            /// 만렙 예외 처리 1000레벨 고정
            if (_MultiLv > 9998) _MultiLv = 9999;
        }

        /// 골드 소모일때
        if (_index == 0)
        {
            /// 레벨 0일때 예외처리
            if (_MultiLv == 1)
            {
                /// 얘 기본값이 10임. 곱해주는 값
                _MultiResult = ListModel.Instance.charatorList[_index].nextUpgradeCost;
            }
            else
            {
                double tmpMuti = 0;
                for (int i = thisLevel + 1; i < thisLevel + muti + 1; i++)
                {
                    tmpMuti += 10d * (1d + Math.Pow(1.07d, i));
                }
                ///_MultiResult = 기본값 곱하기 10
                _MultiResult = tmpMuti;
            }
        }
        /// 엘릭서 소모일때
        else
        {
            int tmpMuti = 0;
            for (int i = thisLevel+1; i < thisLevel + muti+1; i++)
            {
                tmpMuti += i;
            }

            if (_MultiLv > 1) _MultiResult = tmpMuti;
        }

        return Math.Truncate(_MultiResult);
    }

    /// <summary>
    /// 해당 스탯으로 올라가는 증가값
    /// </summary>
    /// <param name="muti"> 곱해줄 배수 </param>
    /// <param name="lv"> 현재 레벨 thisLevel </param>
    /// <returns></returns>
    double GetMutipleEarnGold(int muti, int lv)
    {
        if (_index == 4)
        {
            // 만렙 찍으면  MAX 
            if (thisLevel >= 500)
            {
                thisLevel = 500;
                return 0;
            }
        }
        else
        {
            // 만렙 찍으면  MAX 
            if (thisLevel >= 9999)
            {
                thisLevel = 9999;
                return 0;
            }
        }

        _MultiResult = ListModel.Instance.charatorList[_index].powerPer;
        // 다음 1레벨 / 다음 10레벨 / 다음 100레벨
        _MultiLv = muti + lv;

        /// 만렙 예외 처리 1000레벨 고정
        if (_index == 4)
        {
            if (_MultiLv > 499) _MultiLv = 500;
        }
        else
        {
            if (_MultiLv > 9998) _MultiLv = 9999;
        }

        _MultiResult *= _MultiLv;
        return _MultiResult;

    }


}
