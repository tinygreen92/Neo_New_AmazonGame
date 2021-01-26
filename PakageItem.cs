using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PakageItem : MonoBehaviour
{
    public Sprite NoDiaGray;
    public IAPManager IAPM;
    [Space]
    public Sprite[] iconSprs;
    [Header(" - 아이콘 스프라이트")]
    public Image iconImg;
    public Text iconText;
    [Space]
    public Text titleText;
    public Text bounsText;
    public Text descText;
    [Space]
    public Button Btnbtn;
    public Image[] goBtns;
    [Space]
    public Text diaAmount;
    public Text wonAmount;
    [Space]



    /// TODO : 일간 무료 ( 1 / 1) -> 일간 무료 ( 0 / 1 ) 로 바뀌고 구매 완료 버튼 goBtns[3].setActive(true);
    int _index;
    const string ONEONE = " ( 1 / 1 )";
    const string ZEROONE = " ( 0 / 1 )";

    private void OnEnable()
    {
        iconText.gameObject.SetActive(false);
        bounsText.gameObject.SetActive(false);
        ItemInit();
    }

    bool isNoDia;
    /// <summary>
    /// 다이아 부족하면 회색
    /// </summary>
    /// <param name="_Among"></param>
    void SetPurchaseDia(int _Among)
    {
        /// 다이아 부족할때
        if (PlayerInventory.Money_Dia < _Among)
        {
            isNoDia = true;
            Btnbtn.targetGraphic = goBtns[3];
            goBtns[1].sprite = NoDiaGray;
        }
        /// 구매 가능할때
        else
        {
            isNoDia = false;
            Btnbtn.targetGraphic = goBtns[1];
            goBtns[1].sprite = goBtns[2].sprite;
        }
        /// 다이아 클릭 버튼 활성화
        goBtns[1].gameObject.SetActive(true);
    }

    /// <summary>
    /// 패키지 내용물 세팅 -> 일간 / 주간 / 월간
    /// </summary>
    public void ItemInit()
    {
        _index = int.Parse(name);
        ///아이콘 세팅
        iconImg.sprite = iconSprs[_index - 10];
        /// TODO : 구매제한 ( 0 / 1 ) 이랑 체크 
        titleText.text = ListModel.Instance.shopListPACK[_index].korDesc + ONEONE;
        descText.text = ListModel.Instance.shopListPACK[_index].korTailDesc;
        if (LeanLocalization.CurrentLanguage == "Korean")
        {
            System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("ko-KR", false).NumberFormat;
            wonAmount.text = System.Convert.ToInt64(ListModel.Instance.shopListPACK[_index].korPrice).ToString("C", numberFormat);
        }
        else
        {
            System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
            wonAmount.text = System.Convert.ToInt64(ListModel.Instance.shopListPACK[_index].engPrice).ToString("C", numberFormat);
        }

        /// 패키지 이름으로 거르기 10 부터 시작
        switch (_index)
        {
            case 10:
                if (ListModel.Instance.mvpDataList[0].daily_10 != 0)  // 0이 아니면 구매했다는 거
                {
                    titleText.text = ListModel.Instance.shopListPACK[_index].korDesc + ZEROONE;
                    goBtns[3].gameObject.SetActive(true);
                }
                else
                {
                    goBtns[3].gameObject.SetActive(false);
                    goBtns[0].gameObject.SetActive(true);
                }
                Btnbtn.targetGraphic = goBtns[0];
                break;
            case 11:
                if (ListModel.Instance.mvpDataList[0].daily_11 != 0)  // 0이 아니면 구매했다는 거
                {
                    titleText.text = ListModel.Instance.shopListPACK[_index].korDesc + ZEROONE;
                    goBtns[3].gameObject.SetActive(true);
                }
                else
                {
                    goBtns[3].gameObject.SetActive(false);
                    SetPurchaseDia(100);
                }
                iconText.gameObject.SetActive(true);
                iconText.text = "x3";
                /// 다이아 소모 텍스트 세팅
                diaAmount.text = "100";
                break;
            case 12:
                if (ListModel.Instance.mvpDataList[0].daily_12 != 0)  // 0이 아니면 구매했다는 거
                {
                    titleText.text = ListModel.Instance.shopListPACK[_index].korDesc + ZEROONE;
                    goBtns[3].gameObject.SetActive(true);
                }
                else
                {
                    goBtns[3].gameObject.SetActive(false);
                    SetPurchaseDia(300);
                }
                iconText.gameObject.SetActive(true);
                iconText.text = "x10";
                /// 다이아 소모 텍스트 세팅
                diaAmount.text = "300";
                break;
            case 13:
                if (ListModel.Instance.mvpDataList[0].daily_13 != 0)  // 0이 아니면 구매했다는 거
                {
                    titleText.text = ListModel.Instance.shopListPACK[_index].korDesc + ZEROONE;
                    goBtns[3].gameObject.SetActive(true);
                }
                else
                {
                    goBtns[3].gameObject.SetActive(false);
                    goBtns[2].gameObject.SetActive(true);
                    Btnbtn.targetGraphic = goBtns[2];
                }
                break;

        /// -----------------------------------------------------------------------------------

            case 14:
                if (ListModel.Instance.mvpDataList[0].weekend_14 != 0)  // 0이 아니면 구매했다는 거
                {
                    titleText.text = ListModel.Instance.shopListPACK[_index].korDesc + ZEROONE;
                    goBtns[3].gameObject.SetActive(true);
                }
                else
                {
                    goBtns[3].gameObject.SetActive(false);
                    goBtns[0].gameObject.SetActive(true);
                }
                Btnbtn.targetGraphic = goBtns[0];
                break;
            case 15:
                if (ListModel.Instance.mvpDataList[0].weekend_15 != 0)  // 0이 아니면 구매했다는 거
                {
                    titleText.text = ListModel.Instance.shopListPACK[_index].korDesc + ZEROONE;
                    goBtns[3].gameObject.SetActive(true);
                }
                else
                {
                    goBtns[3].gameObject.SetActive(false);
                    SetPurchaseDia(500);
                }
                iconText.gameObject.SetActive(true);
                iconText.text = "x10";
                /// 다이아 소모 텍스트 세팅
                diaAmount.text = "500";
                break;
            case 16:
                if (ListModel.Instance.mvpDataList[0].weekend_16 != 0)  // 0이 아니면 구매했다는 거
                {
                    titleText.text = ListModel.Instance.shopListPACK[_index].korDesc + ZEROONE;
                    goBtns[3].gameObject.SetActive(true);
                }
                else
                {
                    goBtns[3].gameObject.SetActive(false);
                    SetPurchaseDia(700);
                }
                iconText.gameObject.SetActive(true);
                iconText.text = "x15";
                /// 다이아 소모 텍스트 세팅
                diaAmount.text = "700";
                break;
            case 17:
                if (ListModel.Instance.mvpDataList[0].weekend_17 != 0)  // 0이 아니면 구매했다는 거
                {
                    titleText.text = ListModel.Instance.shopListPACK[_index].korDesc + ZEROONE;
                    goBtns[3].gameObject.SetActive(true);
                }
                else
                {
                    goBtns[3].gameObject.SetActive(false);
                    goBtns[2].gameObject.SetActive(true);
                    Btnbtn.targetGraphic = goBtns[2];
                }
                bounsText.gameObject.SetActive(true);
                bounsText.text = "+ BOUNS  결정 5개";
                break;

            /// -----------------------------------------------------------------------------------

            case 18:
                if (ListModel.Instance.mvpDataList[0].mouth_18 != 0)  // 0이 아니면 구매했다는 거
                {
                    titleText.text = ListModel.Instance.shopListPACK[_index].korDesc + ZEROONE;
                    goBtns[3].gameObject.SetActive(true);
                }
                else
                {
                    goBtns[3].gameObject.SetActive(false);
                    goBtns[0].gameObject.SetActive(true);
                }
                Btnbtn.targetGraphic = goBtns[0];
                break;
            case 19:
                if (ListModel.Instance.mvpDataList[0].mouth_19 != 0)  // 0이 아니면 구매했다는 거
                {
                    titleText.text = ListModel.Instance.shopListPACK[_index].korDesc + ZEROONE;
                    goBtns[3].gameObject.SetActive(true);
                }
                else
                {
                    goBtns[3].gameObject.SetActive(false);
                    SetPurchaseDia(5000);
                }                /// 다이아 소모 텍스트 세팅
                diaAmount.text = "5000";
                break;
            case 20:
                if (ListModel.Instance.mvpDataList[0].mouth_20 != 0)  // 0이 아니면 구매했다는 거
                {
                    titleText.text = ListModel.Instance.shopListPACK[_index].korDesc + ZEROONE;
                    goBtns[3].gameObject.SetActive(true);
                }
                else
                {
                    goBtns[3].gameObject.SetActive(false);
                    SetPurchaseDia(5000);
                }                /// 다이아 소모 텍스트 세팅
                diaAmount.text = "5000";
                break;
            case 21:
                if (ListModel.Instance.mvpDataList[0].mouth_21 != 0)  // 0이 아니면 구매했다는 거
                {
                    titleText.text = ListModel.Instance.shopListPACK[_index].korDesc + ZEROONE;
                    goBtns[3].gameObject.SetActive(true);
                }
                else
                {
                    goBtns[3].gameObject.SetActive(false);
                    goBtns[2].gameObject.SetActive(true);
                    Btnbtn.targetGraphic = goBtns[2];
                }
                bounsText.gameObject.SetActive(true);
                bounsText.text = "+ BOUNS  결정 10개";
                break;
            case 22:
                if (ListModel.Instance.mvpDataList[0].mouth_22 != 0)  // 0이 아니면 구매했다는 거
                {
                    titleText.text = ListModel.Instance.shopListPACK[_index].korDesc + ZEROONE;
                    goBtns[3].gameObject.SetActive(true);
                }
                else
                {
                    goBtns[3].gameObject.SetActive(false);
                    goBtns[2].gameObject.SetActive(true);
                    Btnbtn.targetGraphic = goBtns[2];
                }
                bounsText.gameObject.SetActive(true);
                bounsText.text = "+ BOUNS  결정 15개";
                break;
            case 23:
                if (ListModel.Instance.mvpDataList[0].mouth_23 != 0)  // 0이 아니면 구매했다는 거
                {
                    titleText.text = ListModel.Instance.shopListPACK[_index].korDesc + ZEROONE;
                    goBtns[3].gameObject.SetActive(true);
                }
                else
                {
                    goBtns[3].gameObject.SetActive(false);
                    goBtns[2].gameObject.SetActive(true);
                    Btnbtn.targetGraphic = goBtns[2];
                }
                bounsText.gameObject.SetActive(true);
                bounsText.text = "+ BOUNS  결정 70개";
                break;
        }


    }

    /// <summary>
    /// 해당 인덱스 무슨 아이템인고?
    /// </summary>
    /// <param name="_ind"></param>
    public void PurchaseThisItem()
    {
        /// 구매완료 버튼이면 리턴
        if (goBtns[3].gameObject.activeSelf)
        {
            return;
        }

        /// 다이아 버튼이고 다이아 부족하면 리턴
        if (goBtns[1].gameObject.activeSelf && isNoDia)
        {
            return;
        }

        switch (_index)
        {
            case 10:
                break;
            case 11:
                IAPM.reqDia = 100;
                iconText.gameObject.SetActive(true);
                iconText.text = "x3";
                break;
            case 12:
                IAPM.reqDia = 300;
                iconText.gameObject.SetActive(true);
                iconText.text = "x10";
                break;
            case 13:
                break;

            /// -----------------------------------------------------------------------------------

            case 14:
                break;
            case 15:
                IAPM.reqDia = 500;
                iconText.gameObject.SetActive(true);
                iconText.text = "x10";
                break;
            case 16:
                IAPM.reqDia = 700;
                iconText.gameObject.SetActive(true);
                iconText.text = "x15";
                break;
            case 17:
                break;

            /// -----------------------------------------------------------------------------------

            case 18:
                break;
            case 19:
                IAPM.reqDia = 5000;
                break;
            case 20:
                IAPM.reqDia = 5000;
                break;
            case 21:
                break;
            case 22:
                break;
            case 23:
                break;
        }

        /// 구매 화면 슝
        IAPM.Purchase_Pakage(_index);
    }



}
