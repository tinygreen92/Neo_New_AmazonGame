using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
///  유물 (Heart) 아이템 박스에서만 붙어있을 스크립트.
/// </summary>
public class HeartItem : MonoBehaviour
{
    public TopViewPannel WeaponTVP;
    public HeartManager hm;
    [Header("- 회색 커버 오브젝트")]
    public GameObject GrayImage;
    [Header("- 아이콘")]
    public Sprite[] icons;
    public Image spriteBox;
    [Header("- 정보 표기 부분")]
    public Text ShopBox;
    public Text NameBox;
    public Text LevelBox;
    public Text[] DescBox;
    [Header("- 버튼 부분")]
    public Text UpgradeBox;
    public Text EarnGoldBox;
    [Header("- 버튼 색 바꾸기")]
    public Image GatchaImage;
    public Sprite[] BtnGatSprite;
    public Image DisableImage;
    public Sprite[] BtnSprite;
    public GameObject MaxButton;
    [Header("- 글로우 이펙트")]
    public GameObject[] glowEffect;
    [Header("- 0번 인덱스 전용")]
    public Transform midPannel;
    public Transform buttonParent;

    const string T_LV = "Lv. ";
    const string T_PLUS = "+";

    int _index = 0;

    int thisLevel;
    int mutiple = 1;

    double _MultiResult;
    double _MultiLv;
    /// <summary>
    /// 유물마다 맥스레벨 다르므로 설정해줘야 한다
    /// </summary>
    int _MaxLv;
    /// <summary>
    /// 증가하는 스탯 값 적어주기 -> 유물은 고정치
    /// </summary>
    /// <param name="muti"> 곱해줄 배수 </param>
    /// <param name="lv"> 현재 레벨 thisLevel </param>
    /// <returns></returns>
    double GetMutipleEarnGold(int muti, int lv)
    {
        _MaxLv = int.Parse(ListModel.Instance.heartList[_index - 1].maxLevel);

        // 만렙 찍으면  MAX 
        if (thisLevel >= _MaxLv)
        {
            thisLevel = _MaxLv;
            /// TODO : MAX 버튼 활성화.
            return 0;
        }


        /// 요놈 고정치
        _MultiResult = ListModel.Instance.heartList[_index - 1].powerToLvUP;
        // 다음 1레벨 / 다음 10레벨 / 다음 100레벨
        _MultiLv = muti + lv - 1;

        /// 만렙 예외 처리 맥스 레벨 고정
        if (_MultiLv > _MaxLv - 1) _MultiLv = _MaxLv;

        _MultiResult *= muti;

        return _MultiResult;
    }

    /// <summary>
    /// 업그레이드 골드 소모 
    /// </summary>
    /// <param name="muti"> 곱해줄 배수 </param>
    /// <param name="lv"> 현재 레벨 thisLevel </param>
    /// <returns></returns>
    double GetMutipleUpgrade(int muti, int lv)
    {
        _MaxLv = int.Parse(ListModel.Instance.heartList[_index - 1].maxLevel);

        // 만렙 찍으면  MAX 
        if (thisLevel >= _MaxLv)
        {
            thisLevel = _MaxLv;
            return 0;
        }
        /// 시작값.
        _MultiResult = ListModel.Instance.heartList[_index - 1].nextUpgradeNeed;
        // 다음 1레벨 / 다음 10레벨 / 다음 100레벨
        _MultiLv = muti + lv - 1;

        /// 만렙 예외 처리 맥스 레벨 고정
        if (_MultiLv > _MaxLv - 1) _MultiLv = _MaxLv;

        /// 나뭇잎 소모량
        if (_MultiLv != 1)
        {
            double tmpMuti = 0;
            for (int i = thisLevel + 1; i < thisLevel + muti + 1; i++)
            {
                tmpMuti += _MultiResult * (1 + ListModel.Instance.heartList[_index - 1].leafToLvUP * i);
            }
            //_MultiResult *= ListModel.Instance.heartList[_index - 1].leafToLvUP * (_MultiLv - 1);
            _MultiResult = tmpMuti;
        }

        return Math.Truncate(_MultiResult);
    }

    /// <summary>
    /// 박스 SetActive(true)일때 호출
    /// 변경 값 갱신 해줌
    /// 1. 아우터 글로우 해제
    /// 2. 해당 코루틴 일때만 재생
    /// 3. 배수 적용시 지닌 화폐만큼만 뻥튀기 해서 (int) 수치 붙이기
    /// </summary>
    public void BoxInfoUpdate(int cnt)
    {
        // 글로우 숨김 델리게이트
        hm.chain += HideGrowEffect;
        HideGrowEffect();

        if (ListModel.Instance.heartList.Count == cnt && PlayerPrefsManager.isRefreshHeart)
        {
            PlayerPrefsManager.isRefreshHeart = false;
            // 이 오브젝트 글로우 표기
            glowEffect[0].SetActive(true);
            glowEffect[1].SetActive(true);
        }

        /// 인덱스 설정 -> 이 스크립트 전체
        _index = cnt;

        /// 0 번 인덱스는 유물 뽑기로 넘어가는 버튼
        if (_index == 0)
        {
            spriteBox.sprite = icons[0];
            ShopBox.text = PlayerPrefsManager.MyHeart;
            /// 유물 뽑기 버튼 표시
            GetHeartGatcha(true);
            /// GrayImage.SetActive(false);
            buttonParent.GetComponent<Button>().targetGraphic = buttonParent.GetChild(1).GetComponent<Image>();

            if (ListModel.Instance.heartList.Count >= 30) MaxButton.SetActive(true);
            return;
        }
        spriteBox.sprite = icons[int.Parse(ListModel.Instance.heartList[_index - 1].imgIndex)];
        /// 유물 뽑기 버튼 가리기
        GetHeartGatcha(false);
        buttonParent.GetComponent<Button>().targetGraphic = buttonParent.GetChild(0).GetComponent<Image>();

        //for (int i = 0; i < PlayerPrefsManager.heartUpgrageMutiple; i++)
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
        mutiple = PlayerPrefsManager.heartUpgrageMutiple;

        /// 만렙 예외 처리 
        if (mutiple == 10 && thisLevel >= _MaxLv -9) mutiple = (_MaxLv - thisLevel);
        else if (mutiple == 100 && thisLevel >= _MaxLv -99) mutiple = (_MaxLv - thisLevel);

        NameBox.text = ListModel.Instance.heartList[_index - 1].heartName;
        DescBox[0].text = ListModel.Instance.heartList[_index - 1].descHead + " " + ListModel.Instance.heartList[_index - 1].descTail;

        thisLevel = int.Parse(ListModel.Instance.heartList[_index - 1].heartLevel);
        LevelBox.text = T_LV + thisLevel;


        /// <현재_증가_스탯량> 레벨 곱해서 표기
        if (int.Parse(ListModel.Instance.heartList[_index - 1].imgIndex) == 12
            || int.Parse(ListModel.Instance.heartList[_index - 1].imgIndex) == 17
            || int.Parse(ListModel.Instance.heartList[_index - 1].imgIndex) == 21)
        {
            DescBox[1].text = (ListModel.Instance.heartList[_index - 1].powerToLvUP * thisLevel).ToString("N1") + "초";
            EarnGoldBox.text = T_PLUS + (GetMutipleEarnGold(mutiple, thisLevel)).ToString("N1") + "초 (" + mutiple + ")";
        }
        else if (int.Parse(ListModel.Instance.heartList[_index - 1].imgIndex) == 22)
        {
            DescBox[1].text = (ListModel.Instance.heartList[_index - 1].powerToLvUP * thisLevel).ToString("N0");
            EarnGoldBox.text = T_PLUS + (GetMutipleEarnGold(mutiple, thisLevel)).ToString("N0") + " (" + mutiple + ")";
        }
        /// [ 27. 무기 레벨 증가 ] 일 때 적용시키기
        else if(int.Parse(ListModel.Instance.heartList[_index - 1].imgIndex) == 26)
        {
            DescBox[1].text = (ListModel.Instance.heartList[_index - 1].powerToLvUP * thisLevel).ToString("N0");
            EarnGoldBox.text = T_PLUS + (GetMutipleEarnGold(mutiple, thisLevel)).ToString("N0") + " (" + mutiple + ")";
            /// 여기에 진입했다는건 유물 1레벨 이상이라는 거니까 무기 텍스트 새로 고쳐줌
            WeaponTVP.WeaponAllRefresh();
        }
        else
        {
            DescBox[1].text = (ListModel.Instance.heartList[_index - 1].powerToLvUP * thisLevel).ToString("N2") + "%";
            EarnGoldBox.text = T_PLUS + (GetMutipleEarnGold(mutiple, thisLevel)).ToString("N2") + "% (" + mutiple + ")";
        }


        /// <다음_업그레이드비용> 배수 곱하기 해서 텍스트 뿌려주기.
        UpgradeBox.text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Leaf_Cost * GetMutipleUpgrade(mutiple, thisLevel));

        


        /// 만렙 찍으면 맥스버튼 활성화.
        MaxButton.SetActive(thisLevel >= _MaxLv);
    }


    /// <summary>
    /// 0 번 인덱스는 유물 뽑기로 넘어가는 버튼
    /// </summary>
    private void GetHeartGatcha(bool _isOn)
    {
        midPannel.GetChild(0).gameObject.SetActive(!_isOn);
        midPannel.GetChild(1).gameObject.SetActive(!_isOn);
        midPannel.GetChild(2).gameObject.SetActive(_isOn);
        //
        buttonParent.GetChild(0).gameObject.SetActive(!_isOn);
        buttonParent.GetChild(1).gameObject.SetActive(_isOn);
    }

    /// <summary>
    /// 레벨업 버튼 클릭
    /// </summary>
    public void Clicked_LvUP()
    {
        if (MaxButton.activeSelf) return;
        // 글로우 모두 숨김
        hm.chain();
        // 이 오브젝트 글로우 표기
        glowEffect[0].SetActive(true);
        glowEffect[1].SetActive(true);

        /// TODO :  0번 인덱스 유물 뽑기일때만 이 쪽
        if (buttonParent.GetChild(1).gameObject.activeSelf)
        {
            if(ListModel.Instance.heartList.Count < 30 && PlayerInventory.Money_Dia >= 300)
            {
                hm.GatChaHerat();
                return;
            }
            else
            {
                return;
            }
        }


        /// 돈없음 꺼져잇
        if (thisLevel == 0 && !IsPurchaseable()) return;

        /// 돈 없으면 강화 안되게 막는 역할 || 맥스버튼이면
        if (DisableImage.sprite == BtnSprite[0] || MaxButton.activeSelf) return;

        /// 멀티플 적용 받고 나뭇잎 소모
        PlayerInventory.Money_Leaf -= Mathf.CeilToInt((float)(PlayerInventory.Leaf_Cost * GetMutipleUpgrade(mutiple, thisLevel)));
        /// TODO : 나뭇잎 서버 통신

        /// 음영 없고 나뭇잎 강화. +1 +10 +100
        thisLevel = (int)_MultiLv + 1;
        ListModel.Instance.Heart_LvUP(_index - 1, thisLevel);
        /// 유물 강화 1회 진행
        if (PlayerPrefsManager.currentTutoIndex == 21) ListModel.Instance.TUTO_Update(21);
        if (PlayerPrefsManager.currentTutoIndex == 46) ListModel.Instance.TUTO_Update(46);

        /// 골드획득량 증가는 SupportManager 스크립트에 

        /// 만렙 찍으면 맥스버튼 활성화.
        if (thisLevel >= _MaxLv) MaxButton.SetActive(true);


        // 값 갱신 -> 레벨업은 이쪽에서 처리
        BoxInfoUpdate(_index);

        // 글로우 모두 숨김
        hm.chain();
        // 이 오브젝트 글로우 표기
        glowEffect[0].SetActive(true);
        glowEffect[1].SetActive(true);

        ///  유물 카운터 1 올리기
        ListModel.Instance.DAYlist_Update(4);
        /// 수치 새로 고침
        WeaponTVP.SetHeartRefresh();
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

    /// <summary>
    /// NSM 에서 불러와준다.
    /// </summary>
    public void DestroyChain()
    {
        hm.chain = null;
    }

    private void Update()
    {
        /// 유물 뽑기는 다르게
        if (_index == 0)
        {
            if (PlayerInventory.Money_Dia >= 300) GatchaImage.sprite = BtnGatSprite[1]; //  On 이미지\
            else GatchaImage.sprite = BtnGatSprite[0]; //  OFF 이미지
        }
        else
        {
            ChangeBtnColor();
        }
    }

    /// <summary>
    /// 살 수 있는 골드가 모였는지 체크
    /// </summary>
    bool IsPurchaseable()
    {
        if (PlayerInventory.Money_Leaf >= PlayerInventory.Leaf_Cost * GetMutipleUpgrade(mutiple, thisLevel)) return true;
        else return false;
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
