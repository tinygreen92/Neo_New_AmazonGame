using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPackageItem : MonoBehaviour
{
    public GameObject BtnPuchased;
    public Text BtnText;
    [Header("- 매니저 ")]
    public IAPManager IAPM;
    public ShopItemManager sim;
    [Header("- 내용물 ")]
    public Text packTitle;
    public Text packDesc;
    public Text packMyDragonWater;
    public Image[] packImg;
    public Text[] packAmount;
    [Header("- 내용물 숫자 ")]
    public int[] front5Amount;
    public int[] back7Amount;


    private const string HEAD = "x";
    private int _index;

    private void OnEnable()
    {
        ItemInit();
    }

    /// <summary>
    /// 패키지 내용물 세팅 -> 일반인지 / 한정인지
    /// </summary>
    public void ItemInit()
    {
        _index = int.Parse(name);
        /// 무슨 말을 해야할까랑 / 내용물 채우기 / 버튼 가격
        //packDesc.text = ListModel.Instance.shopListPACK[_index].korTailDesc;
        packMyDragonWater.text = ListModel.Instance.shopListPACK[_index].korTailDesc;
        if (_index == 6)
        {
            packMyDragonWater.text = LeanLocalization.GetTranslationText("Shop_Pakage_0115_Spec");
        }

        if (LeanLocalization.CurrentLanguage == "Korean")
        {
            System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("ko-KR", false).NumberFormat;
            BtnText.text = System.Convert.ToInt64(ListModel.Instance.shopListPACK[_index].korPrice).ToString("C", numberFormat);
        }
        else
        {
            System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
            BtnText.text = System.Convert.ToInt64(ListModel.Instance.shopListPACK[_index].engPrice).ToString("C", numberFormat);
        }
        
        /// 얼터네이티브 거르기
        int AlternativeCnt = 0;

        /// 패키지 이름
        switch (_index)
        {
            case 0:
                packTitle.text = LeanLocalization.GetTranslationText("Shop_Ab_Pack01");
                front5Amount[0] = 1000;
                front5Amount[1] = 10;
                front5Amount[2] = 5;
                front5Amount[3] = 5;
                front5Amount[4] = 10;
                break;
            case 1:
                packTitle.text = LeanLocalization.GetTranslationText("Shop_Ab_Pack02");
                front5Amount[0] = 3300;
                front5Amount[1] = 30;
                front5Amount[2] = 10;
                front5Amount[3] = 10;
                front5Amount[4] = 40;
                break;
            case 2:
                packTitle.text = LeanLocalization.GetTranslationText("Shop_Ab_Pack03");
                front5Amount[0] = 5500;
                front5Amount[1] = 50;
                front5Amount[2] = 15;
                front5Amount[3] = 15;
                front5Amount[4] = 70;
                break;
            case 3:
                packTitle.text = LeanLocalization.GetTranslationText("Shop_Ab_Pack04");
                front5Amount[0] = 11000;
                front5Amount[1] = 100;
                front5Amount[2] = 30;
                front5Amount[3] = 30;
                front5Amount[4] = 150;
                break;
            case 4:
                AlternativeCnt = 1;
                packTitle.text = LeanLocalization.GetTranslationText("Shop_Ab_Pack05");
                break;

                /// ----------------------------- 구매는 한번만


            case 5:
                if (ListModel.Instance.mvpDataList[0].pack_06 != 0)  // 0이 아니면 구매했다는 거
                {
                    BtnPuchased.SetActive(true);
                }
                AlternativeCnt = 1;
                packTitle.text = LeanLocalization.GetTranslationText("Shop_Ab_Pack06");
                break;
            case 6:
                if (ListModel.Instance.mvpDataList[0].pack_07 != 0)  // 0이 아니면 구매했다는 거
                {
                    BtnPuchased.SetActive(true);
                }
                AlternativeCnt = 1;
                packTitle.text = LeanLocalization.GetTranslationText("Shop_Ab_Pack07");
                break;
            case 7:
                if (ListModel.Instance.mvpDataList[0].pack_08 != 0)  // 0이 아니면 구매했다는 거
                {
                    BtnPuchased.SetActive(true);
                }
                AlternativeCnt = 2;
                packTitle.text = LeanLocalization.GetTranslationText("Shop_Ab_Pack08");
                back7Amount[0] = 5500;
                back7Amount[1] = 30;
                back7Amount[2] = 30000;
                back7Amount[3] = 15000;
                back7Amount[4] = 10;
                back7Amount[5] = 10;
                back7Amount[6] = 70;
                break;
            case 8:
                if (ListModel.Instance.mvpDataList[0].pack_09 != 0)  // 0이 아니면 구매했다는 거
                {
                    BtnPuchased.SetActive(true);
                }
                AlternativeCnt = 2;
                packTitle.text = LeanLocalization.GetTranslationText("Shop_Ab_Pack09");
                back7Amount[0] = 7900;
                back7Amount[1] = 50;
                back7Amount[2] = 50000;
                back7Amount[3] = 25000;
                back7Amount[4] = 25;
                back7Amount[5] = 25;
                back7Amount[6] = 100;
                break;
            case 9:
                if (ListModel.Instance.mvpDataList[0].pack_10 != 0)  // 0이 아니면 구매했다는 거
                {
                    BtnPuchased.SetActive(true);
                }
                AlternativeCnt = 2;
                packTitle.text = LeanLocalization.GetTranslationText("Shop_Ab_Pack10");
                back7Amount[0] = 13000;
                back7Amount[1] = 100;
                back7Amount[2] = 100000;
                back7Amount[3] = 50000;
                back7Amount[4] = 50;
                back7Amount[5] = 50;
                back7Amount[6] = 150;
                break;

            default:
                break;
        }
        /// 5개짜리 자동 이미지 채우기
        if (AlternativeCnt < 1)
        {
            for (int i = 0; i < 5; i++)
            {
                packImg[i].sprite = sim.PackFrontCons[i];
                packAmount[i].text = HEAD + front5Amount[i].ToString("N0");
            }
        }
        /// 7개 짜리 자동 이미지 채우기
        else if(AlternativeCnt > 1)
        {
            for (int i = 0; i < 7; i++)
            {
                packImg[i].sprite = sim.PackBackCons[i];
                packAmount[i].text = HEAD + back7Amount[i].ToString("N0");
            }
        }

    }

    /// <summary>
    /// 해당 인덱스 무슨 아이템인고?
    /// </summary>
    /// <param name="_ind"></param>
    public void PurchaseThisItem()
    {
        /// 이미지 있어야 체크
        if (BtnPuchased != null)
        {
            /// 구매완료 버튼 활성화면 리턴
            if (BtnPuchased.activeSelf) return;
        }

        //
        if (_index < 5)
        {
            IAPM.Purchase_Pakage(_index, front5Amount);
        }
        else
        {
            IAPM.Purchase_Pakage(_index, back7Amount);
        }
    }
}
