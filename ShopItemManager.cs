using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemManager : MonoBehaviour
{
    [Header("- 선물함 지급 완료 오브젝트")]
    public GameObject popUp;
    public Image popUpIcon;
    public Text popUpIconDesc;
    [Header("- 실제 결제 관련 ")]
    public IAPManager IAPM;
    public NestedScrollManager NSM;
    [Header("- 아이콘")]
    public Sprite[] DiaiCons;
    public Sprite[] SpeciCons;
    public Sprite[] NoriCons;
    public Sprite[] PackFrontCons;
    public Sprite[] PackBackCons;
    public Image spriteBox;
    [Header("- 정보 표기 부분")]
    public Text NameBox;
    public Text BotDescBox;
    public Text MidleDescBox;                                               // 다이아 구매일때만 활성화
    [Header("- 버튼 부분")]
    public Text PriceBox;
    public Text KRWBox;
    [Header("- 버튼 색 바꾸기")]
    public Image[] Btns;
    //[Header("- 글로우 이펙트")]
    //public GameObject[] glowEffect;
    int _index = 0;


    /// <summary>
    /// 0=DiaIcon / 1=KRW / 2=Max
    /// </summary>
    /// <param name="_index"></param>
    private void ChangeCorectBtn(int _index)
    {
        /// 각 카테고리 버튼으로 세팅<전> 초기화
        for (int i = 0; i < Btns.Length; i++)
        {
            Btns[i].gameObject.SetActive(false);
        }
        Btns[_index].gameObject.SetActive(true);
        if(_index != 2) Btns[_index].GetComponentInParent<Button>().targetGraphic = Btns[_index];
    }

    /// <summary>
    ///  상점 아이템 세팅할 때 호출할 것
    /// </summary>
    /// <param name="_count"></param>
    public void BoxInfoUpdate(int _count)
    {
        _index = _count;
        // 1 / 10 / 100 일때 리스트 바꿔줌
        var targetList = ListModel.Instance.shopList;
        if (PlayerPrefsManager.storeIndex == 10)
        {
            /// 카테고리 무엇?
            targetList = ListModel.Instance.shopListSPEC;
            spriteBox.sprite = SpeciCons[_count];

            /// 세줄 짜리 설명
            BoxActivating(true);
            if (LeanLocalization.CurrentLanguage == "Korean")
            {
                NameBox.text = targetList[_count].korDesc;
                if (_count == 0)
                {
                    BotDescBox.text = LeanLocalization.GetTranslationText("Shop_Pakage_0115_Title");
                }
                else
                {
                    BotDescBox.text = targetList[_count].korTailDesc;
                }
                MidleDescBox.text = _count ==0? "+ BOUNUS 결정 5개" : "+ BOUNUS 결정 3개";
                System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("ko-KR", false).NumberFormat;
                KRWBox.text = System.Convert.ToInt64(targetList[_count].korPrice).ToString("C", numberFormat);
            }
            else
            {
                NameBox.text = targetList[_count].engDesc;
                BotDescBox.text = targetList[_count].engTailDesc;
                MidleDescBox.text = _count == 0 ? "+ Amazon Cystail 5" : "+ Amazon Cystail 3";
                System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
                KRWBox.text = System.Convert.ToInt64(targetList[_count].engPrice).ToString("C", numberFormat);
            }
            /// 영구 버프구매시 버튼 막아둠
            switch (_index)
            {
                case 0: ChangeCorectBtn(PlayerInventory.isSuperUser != 0? 2: 1); break;
                case 1: ChangeCorectBtn(PlayerInventory.isbuff_power_up? 2: 1); break;
                case 2: ChangeCorectBtn(PlayerInventory.isbuff_attack_speed_up ? 2: 1); break;
                case 3: ChangeCorectBtn(PlayerInventory.isbuff_move_speed_up ? 2: 1); break;
                case 4: ChangeCorectBtn(PlayerInventory.isbuff_gold_earned_up ? 2: 1); break;
                default:  break;
            }

            return;
        }
        else if (PlayerPrefsManager.storeIndex == 100)
        {
            /// 카테고리 무엇?
            targetList = ListModel.Instance.shopListNOR;
            spriteBox.sprite = NoriCons[_count];
            ///
            switch (_index)
            {
                case 0: ChangeCorectBtn(PlayerInventory.isbuff_power_up ? 2 : 0); break;
                case 1: ChangeCorectBtn(PlayerInventory.isbuff_attack_speed_up ? 2 : 0); break;
                case 2: ChangeCorectBtn(PlayerInventory.isbuff_move_speed_up ? 2 : 0); break;
                case 3: ChangeCorectBtn(PlayerInventory.isbuff_gold_earned_up ? 2 : 0); break;
                default: ChangeCorectBtn(0); break;
            }
        }
        else
        {
            ChangeCorectBtn(1);
            /// 카테고리 무엇?
            spriteBox.sprite = DiaiCons[_count];
            /// 그런거 없고 다이아상점도 2줄짜기 설명
            BoxActivating(false);
            if (LeanLocalization.CurrentLanguage == "Korean")
            {
                System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("ko-KR", false).NumberFormat;
                KRWBox.text = System.Convert.ToInt64(targetList[_count].korPrice).ToString("C", numberFormat);
            }
            else
            {
                System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
                KRWBox.text = System.Convert.ToInt64(targetList[_count].engPrice).ToString("C", numberFormat);
            }
        }

        /// 두줄 짜리 설명 = 스페셜이랑 일반
        BoxActivating(false);
        if (LeanLocalization.CurrentLanguage == "Korean")
        {
            NameBox.text = targetList[_count].korDesc;

            switch (_count)
            {
                case 0: BotDescBox.text = LeanLocalization.GetTranslationText("ShopInfo_Spec_Desc01"); break;
                case 1: BotDescBox.text = LeanLocalization.GetTranslationText("ShopInfo_Spec_Desc02"); break;
                case 2: BotDescBox.text = LeanLocalization.GetTranslationText("ShopInfo_Spec_Desc03"); break;
                case 3: BotDescBox.text = LeanLocalization.GetTranslationText("ShopInfo_Spec_Desc04"); break;

                default: BotDescBox.text = targetList[_count].korTailDesc; break;
            }

            if (PlayerPrefsManager.storeIndex == 10)
            {
                System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("ko-KR", false).NumberFormat;
                KRWBox.text = System.Convert.ToInt64(targetList[_count].korPrice).ToString("C", numberFormat);
            }
            else
            {
                PriceBox.text = targetList[_count].korPrice;
            }
        }
        else
        {
            NameBox.text = targetList[_count].engDesc;
            BotDescBox.text = targetList[_count].engTailDesc;
            if (PlayerPrefsManager.storeIndex == 10)
            {
                System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
                KRWBox.text = System.Convert.ToInt64(targetList[_count].engPrice).ToString("C", numberFormat);
            }
            else
            {
                PriceBox.text = targetList[_count].engPrice;
            }
        }

    }

    /// <summary>
    /// 한줄짜리 두줄짜리 박스 활성화
    /// </summary>
    void BoxActivating(bool _isONE)
    {
        if (_isONE)
        {
            NameBox.transform.parent.parent.parent.gameObject.SetActive(true);
            BotDescBox.transform.parent.parent.parent.gameObject.SetActive(true);
            MidleDescBox.transform.parent.parent.parent.gameObject.SetActive(true);
        }
        else
        {
            NameBox.transform.parent.parent.parent.gameObject.SetActive(true);
            BotDescBox.transform.parent.parent.parent.gameObject.SetActive(true);
            MidleDescBox.transform.parent.parent.parent.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// 해당 인덱스 무슨 아이템인고?
    /// </summary>
    /// <param name="_ind"></param>
    public  void PurchaseThisItem()
    {
        /// 구매완료 버튼이면 리턴
        if (Btns[2].gameObject.activeSelf) 
            return;

        /// _index 사용해서
        if (PlayerPrefsManager.storeIndex == 10)                       /// 특별 상점 (현금)
            IAPM.Purchase_Spec(_index);
        else if (PlayerPrefsManager.storeIndex == 100)                    /// 일반 상점  (다이아)
        {
            /// 버프 4종은 그냥 구매할래 버튼 표기
            if (_index < 4)
                IAPM.Purchase_Nor(_index, 1);
            else
                IAPM.cbm.ShowPopUp(_index, 100, ShopType.NormalShop);
        }
        else                                 /// 다이아 상점 (현금)
            IAPM.Purchase_Dia(_index);

    }


    /// <summary>
    /// 상점 카테고리 바꿔주기 위해서 아이템 싹 밀어버리기
    /// </summary>
    public void ChangeSection()
    {
        /// 상점 리프레쉬
        NSM.RefreshForSHOP();
        /// 재생성
        NSM.GnerSHOP();
    }




}
