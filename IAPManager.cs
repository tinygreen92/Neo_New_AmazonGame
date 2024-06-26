﻿using EasyMobile;
using Lean.Localization;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] // 이걸 적어줘야 인스펙터창에 나온다
public class PackageData
{
    public Sprite titleIcon;   // pack_01...
    [TextArea]
    public string inner;  // 내용물 5개에서 7개 각각 스트링
    [TextArea]
    public string outter;  // 보너스 글자
}

public class IAPManager : MonoBehaviour
{
    [Header("- 갯수 조정 팝업 매니저")]
    public CountBuyManager cbm;
    [Space]
    public GameObject TextPaakge;
    [Header("- 패키지 내용물 숫자 ")]
    public int[] front5Amount;
    public int[] back7Amount;
    [Header("- 패키지 데이터 스트링 뿌려줌")]
    [SerializeField]
    public PackageData[] pd;
    [SerializeField]
    public PackageData[] pdEng;                         // TODO : 글로벌 대응
    [Space]
    [Header("- 리얼리 팝업 세팅")]
    public GameObject motherlPopup;
    public GameObject ComplPopup;
    public Image ComplGifteIcon;
    [Header("- 패키지 팝업 세팅")]
    public GameObject PackGiftPop;
    public Image packGiftIcon;
    public Text innerPackText;
    public Text outterPackText;
    public Text btnPackText;
    [Space]
    public GameObject btnWonGo;
    public GameObject txRufundGo;
    //무료
    public GameObject btnFreeGo;
    // 다이아
    public GameObject btnDiaGo;
    public Text btnDiaText;

    [Space]
    public GameObject[] PackCategory;
    [Space]
    public Image[] btnImgs;
    public Sprite EnableBtn;
    public Sprite DisableBtn;
    [Header("- 하위 매니저 땡겨오기")]
    public BannerAdPanelController bac;
    public BuffManager bm;
    public NanooManager nm;
    public ShopItemManager sim;
    //
    public Image IconImg;
    public Text IconDesc;
    public Text DescObject;
    public Text GiftGetText;
    //
    [Space]
    public Transform[] purcBtns;
    [Space]
    public int[] purchaseIndex;

    [HideInInspector]
    public int reqDia;                      // 패키지 떨거지에서 받아오는 다이아 갯수



    /// <summary>
    /// 리얼 구매 팝업에서 나가기 누르면 레드닷 복구 해주기
    /// </summary>
    public void ExitBtnRealCalls()
    {
        PopUpManager.instance.HidePopUP(30);

        /// 무료 구매 몽땅하면 레드닷 꺼줌.
        var tmpppmt = ListModel.Instance.mvpDataList[0];
        /// 일간
        if (tmpppmt.daily_10 == 0)
        {
            RedDotManager.instance.RedDot[9].SetActive(true);
        }
        /// 주간
        if (tmpppmt.weekend_14 == 0)
        {
            RedDotManager.instance.RedDot[11].SetActive(true);
        }
        /// 월간
        if (tmpppmt.mouth_18 == 0)
        {
            RedDotManager.instance.RedDot[13].SetActive(true);
        }

    }










    public void ClickedPakageShop()
    {
        SwichPackageCatgory(0);
        /// 팝업
        PopUpManager.instance.ShowPopUP(30);
    }

    /// <summary>
    /// 패키지 구매 팝업 다 꺼주기 -> 구매화면 슝에 붙여
    /// </summary>
    public void ShutUpSnepe()
    {
        btnFreeGo.SetActive(false);
        btnDiaGo.SetActive(false);
        btnWonGo.SetActive(false);
        txRufundGo.SetActive(false);
    }

    /// <summary>
    /// 패키지 상점 상단 탭 관리
    /// </summary>
    /// <param name="_index"></param>
    public void SwichPackageCatgory(int _index)
    {
        for (int i = 0; i < PackCategory.Length; i++)
        {
            PackCategory[i].SetActive(false);
        }
        PackCategory[_index].SetActive(true);
        PackCategory[_index].GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        /// 어느 패널 열거니
        switch (_index)
        {
            case 0:
                btnImgs[0].sprite = EnableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = DisableBtn;
                btnImgs[3].sprite = DisableBtn;
                btnImgs[4].sprite = DisableBtn;
                break;

            case 1:
                btnImgs[0].sprite = DisableBtn;
                btnImgs[1].sprite = EnableBtn;
                btnImgs[2].sprite = DisableBtn;
                btnImgs[3].sprite = DisableBtn;
                btnImgs[4].sprite = DisableBtn;
                break;

            case 2:
                btnImgs[0].sprite = DisableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = EnableBtn;
                btnImgs[3].sprite = DisableBtn;
                btnImgs[4].sprite = DisableBtn;
                RedDotManager.instance.RedDot[9].SetActive(false);
                break;

            case 3:
                btnImgs[0].sprite = DisableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = DisableBtn;
                btnImgs[3].sprite = EnableBtn;
                btnImgs[4].sprite = DisableBtn;
                RedDotManager.instance.RedDot[11].SetActive(false);
                break; 


            case 4:
                btnImgs[0].sprite = DisableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = DisableBtn;
                btnImgs[3].sprite = DisableBtn;
                btnImgs[4].sprite = EnableBtn;
                RedDotManager.instance.RedDot[13].SetActive(false);
                break;

            default:
                break;
        }
    }







    /// <summary>
    /// 외부에서 자신의 인덱스로 호출하는 구매요청
    /// </summary>
    /// <param name="_indx"></param>
    public void Purchase_Dia(int _indx)
    {
        ShutUpMalpoi();
        /// 청약철회 오브젝트 살려줌
        DescObject.text = LeanLocalization.GetTranslationText("Shop_Ab_Shop_Desc2");
        IconImg.sprite = sim.DiaiCons[_indx];
        switch (_indx)
        {
            case 0:
                IconDesc.text = "x100";
                break;

            case 1:
                IconDesc.text = "x600";
                break;

            case 2:
                IconDesc.text = "x1,300";
                break;

            case 3:
                IconDesc.text = "x4,200";
                break;

            case 4:
                IconDesc.text = "x7,500";
                break;

            case 5:
                IconDesc.text = "x16,000";
                break;

            default:
                break;
        }

        if (LeanLocalization.CurrentLanguage == "Korean")
        {
            System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("ko-KR", false).NumberFormat;
            purcBtns[0].GetComponentInChildren<Text>().text = System.Convert.ToInt64(ListModel.Instance.shopList[_indx].korPrice).ToString("C", numberFormat);
        }
        else
        {
            System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
            purcBtns[0].GetComponentInChildren<Text>().text = System.Convert.ToInt64(ListModel.Instance.shopList[_indx].engPrice).ToString("C", numberFormat);
        }
        purchaseIndex[0] = _indx;
        purcBtns[0].gameObject.SetActive(true);
        PopUpManager.instance.ShowPopUP(13);
    }

    /// <summary>
    /// 특별 상점 (현금 버프 있는곳) 클릭하면 패키지 상점 소개 메시지 나오게
    /// </summary>
    /// <param name="_indx"></param>
    public void Purchase_Spec(int _indx)
    {
        ShutUpMalpoi();
        /// 청약철회 오브젝트 살려줌
        DescObject.text = LeanLocalization.GetTranslationText("Shop_Ab_Shop_Desc2");

        IconImg.sprite = sim.SpeciCons[_indx];
        IconDesc.text = "";

        if (LeanLocalization.CurrentLanguage == "Korean")
        {
            System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("ko-KR", false).NumberFormat;
            purcBtns[1].GetComponentInChildren<Text>().text = System.Convert.ToInt64(ListModel.Instance.shopListSPEC[_indx].korPrice).ToString("C", numberFormat);
        }
        else
        {
            System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
            purcBtns[1].GetComponentInChildren<Text>().text = System.Convert.ToInt64(ListModel.Instance.shopListSPEC[_indx].engPrice).ToString("C", numberFormat);
        }
        purchaseIndex[1] = _indx;
        purcBtns[1].gameObject.SetActive(true);
        PopUpManager.instance.ShowPopUP(13);
        /// TODO : 아이콘과 메세지 출력
        TextPaakge.SetActive(true);
    }

    /// <summary>
    /// 수량 카운트 팝업에서 사용할 것
    /// </summary>
    private int co_Amount;
    /// <summary>
    /// 일반상점 -> 다이아로 구매가능한 상점 
    /// 수량 팝업 호출하고 이거 호출
    /// </summary>
    /// <param name="_indx"></param>
    public void Purchase_Nor(int _indx, int _Amount)
    {
        ShutUpMalpoi();
        /// 청약철회 오브젝트 가려줌
        DescObject.text = ListModel.Instance.shopListNOR[_indx].korTailDesc;
        IconImg.sprite = sim.NoriCons[_indx];
        IconDesc.text = "";
        /// 다이아 가격
        purcBtns[2].GetComponentInChildren<Text>().text = PlayerPrefsManager.instance.DoubleToStringNumber(100 * _Amount);
        //purcBtns[2].GetComponentInChildren<Text>().text = ListModel.Instance.shopListNOR[_indx].korPrice;
        /// 상품 인덱스 & 상품 갯수
        purchaseIndex[2] = _indx;
        co_Amount = _Amount;
        //
        purcBtns[2].gameObject.SetActive(true);
        PopUpManager.instance.ShowPopUP(13);
    }



    /// <summary>
    /// 인덱스와 해당 상품 가격까지 같이 받아옴 진짜 구입
    /// </summary>
    /// <param name="_indx"></param>
    /// <param name="_wonhwa"></param>
    public void Purchase_Pakage(int _indioxyz, int[] _amount)
    {
        int _indx = _indioxyz;
        /// 어떤 버튼 보여줄까 숨겨.
        ShutUpSnepe();

        /// ------------저려미 패키지 추가 210203-------------
        /// ------------저려미 패키지 추가 210203-------------
        /// ------------저려미 패키지 추가 210203-------------
        if (_amount == null && _indx >= 10)
        {
            /// 새로 추가된 패키지는 인덱스 24 / 25 / 26 으로 만들어 줌
            _indx += 14;
        }

        if (_indx < 5)
        {
            front5Amount = _amount;
        }
        else
        {
            back7Amount = _amount;
        }
        /// 긴 텍스트  받아와
        var itemInfo = pd[_indx];
        /// 리얼 구매에서 쓸것
        purchaseIndex[3] = _indx;
        /// 내용물 채우기
        packGiftIcon.sprite = itemInfo.titleIcon;
        ComplGifteIcon.sprite = itemInfo.titleIcon;
        innerPackText.text = itemInfo.inner;
        outterPackText.text = itemInfo.outter;
        /// 청약 철회 메세지
        txRufundGo.SetActive(true);
        /// 현금 구매 버튼
        btnWonGo.SetActive(true);

        /// 버튼 가격 채우기
        if (LeanLocalization.CurrentLanguage == "Korean")
        {
            System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("ko-KR", false).NumberFormat;
            if(_indx < 24)
                btnPackText.text = System.Convert.ToInt64(ListModel.Instance.shopListPACK[_indx].korPrice).ToString("C", numberFormat);
            else
                btnPackText.text = System.Convert.ToInt64(ListModel.Instance.shopCheepPack[_indx-24].korPrice).ToString("C", numberFormat);
        }
        else
        {
            System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
            if(_indx < 24)  
                btnPackText.text = System.Convert.ToInt64(ListModel.Instance.shopListPACK[_indx].engPrice).ToString("C", numberFormat);
            else
                btnPackText.text = System.Convert.ToInt64(ListModel.Instance.shopCheepPack[_indx - 24].engPrice).ToString("C", numberFormat);
        }

        /// 진짜 구매? 팝업 버튼 띄우기
        PackGiftPop.SetActive(true);
    }

    public void Purchase_Pakage(int _indx)
    {
        /// 어떤 버튼 보여줄까 숨겨.
        ShutUpSnepe();
        /// 긴 텍스트  받아와
        var itemInfo = pd[_indx];
        /// 내용물 채우기
        packGiftIcon.sprite = itemInfo.titleIcon;
        ComplGifteIcon.sprite = itemInfo.titleIcon;
        innerPackText.text = itemInfo.inner;
        outterPackText.text = itemInfo.outter;
        /// 리얼 구매 팝업 데이터 넘기기용
        purchaseIndex[3] = _indx;

        /// 무료인지? 
        if (_indx == 10 || _indx == 14 || _indx == 18)
        {
            /// 진짜 구매? 팝업 버튼 띄우기 전 작업 버튼 1
            /// 청약 철회 메세지
            txRufundGo.SetActive(false);
            /// 무료 구매 버튼
            btnFreeGo.SetActive(true);
        }
        /// 다이아 구매인지? 
        else if (_indx == 11 || _indx == 12 || _indx == 15 || _indx == 16 || _indx == 19 || _indx == 20)
        {
            /// 진짜 구매? 팝업 버튼 띄우기 전 작업 다이아 버튼
            /// 청약 철회 메세지
            txRufundGo.SetActive(false);
            /// 다이아 구매 버튼
            btnDiaText.text = reqDia.ToString();
            btnDiaGo.SetActive(true);
        }
        else
        {
            /// 현질인지? 버튼 가격 채우기
            if (LeanLocalization.CurrentLanguage == "Korean")
            {
                System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("ko-KR", false).NumberFormat;
                btnPackText.text = System.Convert.ToInt64(ListModel.Instance.shopListPACK[_indx].korPrice).ToString("C", numberFormat);
            }
            else
            {
                System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
                btnPackText.text = System.Convert.ToInt64(ListModel.Instance.shopListPACK[_indx].engPrice).ToString("C", numberFormat);
            }
            /// 청약 철회 메세지
            txRufundGo.SetActive(true);
            /// 현질 구매 버튼
            btnWonGo.SetActive(true);
        }

        /// 진짜 구매? 팝업 버튼 띄우기
        PackGiftPop.SetActive(true);
    }

    /// <summary>
    /// 진짜 패키지 구매 버튼에 붙여줘라
    /// </summary>
    public void Clicked_Real_Pakege()
    {
        switch (purchaseIndex[3])
        {
            case 0:
                Purchase_Product_pack_01();
                break;

            case 1:
                Purchase_Product_pack_02();
                break;

            case 2:
                Purchase_Product_pack_03();
                break;

            case 3:
                Purchase_Product_pack_04();
                break;

            case 4:
                Purchase_Product_pack_05();
                break;


                /// -------------------------------------------------


            case 5:
                Purchase_Product_pack_06();
                //SwichPackageCatgory(1);
                break;

            case 6:
                Purchase_Product_pack_07();
                //SwichPackageCatgory(1);
                break;

            case 7:
                Purchase_Product_pack_08();
                //SwichPackageCatgory(1);
                break;

            case 8:
                Purchase_Product_pack_09();
                //SwichPackageCatgory(1);
                break;

            case 9:
                Purchase_Product_pack_10();
                //SwichPackageCatgory(1);
                break;

            /// -----------------------------------------------------------------------------------

            case 10:
                ListModel.Instance.mvpDataList[0].daily_10 = 39;
                PakageFreeItem(10);
                SwichPackageCatgory(2);
                ComplPopup.SetActive(true);
                break;
            case 11:
                PakageDiaItem(100);
                SwichPackageCatgory(2);
                ComplPopup.SetActive(true);
                break;
            case 12:
                PakageDiaItem(300);
                SwichPackageCatgory(2);
                ComplPopup.SetActive(true);
                break;
            case 13:
                Purchase_Product_day_01();
                break;

            /// -----------------------------------------------------------------------------------

            case 14:
                ListModel.Instance.mvpDataList[0].weekend_14 = 41;
                if (ListModel.Instance.mvpDataList[0].weekend_Day == 0)
                {
                    ListModel.Instance.mvpDataList[0].weekend_Day = (int)UnbiasedTime.Instance.Now().DayOfWeek;
                }
                PakageFreeItem(14);
                SwichPackageCatgory(3);
                ComplPopup.SetActive(true);
                break;
            case 15:
                PakageDiaItem(500);
                SwichPackageCatgory(3);
                ComplPopup.SetActive(true);
                break;
            case 16:
                PakageDiaItem(700);
                SwichPackageCatgory(3);
                ComplPopup.SetActive(true);
                break;
            case 17:
                Purchase_Product_week_01();
                break;

            /// -----------------------------------------------------------------------------------

            case 18:
                ListModel.Instance.mvpDataList[0].mouth_18 = 81;
                if (ListModel.Instance.mvpDataList[0].mouth_Day == 0)
                {
                    ListModel.Instance.mvpDataList[0].mouth_Day = UnbiasedTime.Instance.Now().Day;
                }
                PakageFreeItem(18);
                SwichPackageCatgory(4);
                ComplPopup.SetActive(true);
                break;
            case 19:
                PakageDiaItem(5000);
                SwichPackageCatgory(4);
                ComplPopup.SetActive(true);
                break;
            case 20:
                PakageDiaItem(5000);
                SwichPackageCatgory(4);
                ComplPopup.SetActive(true);
                break;
            case 21:
                Purchase_Product_month_01();
                break;
            case 22:
                Purchase_Product_month_02();
                break;
            case 23:
                Purchase_Product_month_03();
                break;


            ///update210204
            ///--------------------------------------update210204 ----------------------------------------------
            ///update210204
            /// 1.0.7 에서 저려미 패키지 추가
            case 24:
                Purchase_Product_pack_11();
                break;

            case 25:
                Purchase_Product_pack_12();
                break;

            case 26:
                Purchase_Product_pack_13();
                break;


        }

        CodeStage.AntiCheat.Storage.ObscuredPrefs.Save();
    }


    /// <summary>
    /// 무료 보급
    /// </summary>
    /// <param name="_Dia"></param>
    void PakageFreeItem(int _index)
    {
        /// 템 지급
        StartCoroutine(InvoFree(_index));
        /// 무료 구매 몽땅하면 레드닷 꺼줌.
        var tmpppmt = ListModel.Instance.mvpDataList[0];
        /// 일간
        if (tmpppmt.daily_10 != 0)
        {
            RedDotManager.instance.RedDot[9].SetActive(false);
            RedDotManager.instance.RedDot[10].SetActive(false);
        }
        /// 주간
        if (tmpppmt.weekend_14 != 0)
        {
            RedDotManager.instance.RedDot[11].SetActive(false);
            RedDotManager.instance.RedDot[12].SetActive(false);
        }
        /// 월간
        if (tmpppmt.mouth_18 != 0)
        {
            RedDotManager.instance.RedDot[13].SetActive(false);
            RedDotManager.instance.RedDot[14].SetActive(false);
        }
        if (tmpppmt.daily_10 != 0 && tmpppmt.weekend_14 != 0 && tmpppmt.mouth_18 != 0)
        {
            /// 패키지 레드닷  꺼줌
            RedDotManager.instance.RedDot[8].SetActive(false);
        }
    }

    IEnumerator InvoFree(int _index)
    {
        yield return null;

        switch (_index)
        {
            case 10:
                nm.PostboxItemSend("leaf_box", 1, "");
                yield return null;
                nm.PostboxItemSend("reinforce_box", 1, "");
                yield return null;
                break;
            case 14:
                nm.PostboxItemSend("diamond", 100, "");
                yield return null;
                nm.PostboxItemSend("elixr", 1, "");
                yield return null;
                nm.PostboxItemSend("amber", 1, "");
                yield return null;
                break;
            case 18:
                nm.PostboxItemSend("diamond", 300, "");
                yield return null;
                nm.PostboxItemSend("leaf_box", 5, "");
                yield return null;
                nm.PostboxItemSend("elixr", 5, "");
                yield return null;
                break;
        }
        CodeStage.AntiCheat.Storage.ObscuredPrefs.Save();
    }

    /// <summary>
    /// 다이아 가지고 있으면 패키지 구매 완료
    /// </summary>
    /// <param name="_Dia"></param>
    void PakageDiaItem(int _Dia)
    {
        if (PlayerInventory.Money_Dia < _Dia)
        {
            /// 다이어 없으면 팝업?
            return;
        }
        PlayerInventory.Money_Dia -= _Dia;
        /// 템 지급
        /// (_indx == 11 || _indx == 12 || _indx == 15 || _indx == 16 || _indx == 19 || _indx == 20)
        switch (purchaseIndex[3])
        {
            case 11:
                ListModel.Instance.mvpDataList[0].daily_11 = 102;
                nm.PostboxItemSend("reinforce_box", 3, "");
                break;
            case 12:
                ListModel.Instance.mvpDataList[0].daily_12 = 28;
                nm.PostboxItemSend("reinforce_box", 10, "");
                break;
            case 15:
                ListModel.Instance.mvpDataList[0].weekend_15 = 53;
                if (ListModel.Instance.mvpDataList[0].weekend_Day == 0)
                {
                    ListModel.Instance.mvpDataList[0].weekend_Day = (int)UnbiasedTime.Instance.Now().DayOfWeek;
                }
                nm.PostboxItemSend("weapon_coupon", 10, "");
                break;
            case 16:
                ListModel.Instance.mvpDataList[0].weekend_16 = 62;
                if (ListModel.Instance.mvpDataList[0].weekend_Day == 0)
                {
                    ListModel.Instance.mvpDataList[0].weekend_Day = (int)UnbiasedTime.Instance.Now().DayOfWeek;
                }
                nm.PostboxItemSend("weapon_coupon", 15, "");
                break;
            case 19:
                ListModel.Instance.mvpDataList[0].mouth_19 = 91;
                if (ListModel.Instance.mvpDataList[0].mouth_Day == 0)
                {
                    ListModel.Instance.mvpDataList[0].mouth_Day = UnbiasedTime.Instance.Now().Day;
                }
                StartCoroutine(DiaDiaPack(0));
                break;
            case 20:
                ListModel.Instance.mvpDataList[0].mouth_20 = 93;
                if (ListModel.Instance.mvpDataList[0].mouth_Day == 0)
                {
                    ListModel.Instance.mvpDataList[0].mouth_Day = UnbiasedTime.Instance.Now().Day;
                }
                StartCoroutine(DiaDiaPack(1));
                break;
        }
        CodeStage.AntiCheat.Storage.ObscuredPrefs.Save();
    }

    IEnumerator DiaDiaPack(int _Index)
    {
        yield return null;
        /// 0 이면 앞 1이면 뒤
        if (_Index == 0)
        {
            nm.PostboxItemSend("reinforce_box", 50, "");
            yield return null;
            nm.PostboxItemSend("mining", 25, "");
            yield return null;
            nm.PostboxItemSend("amber", 25, "");
            yield return null;
        }
        else
        {
            nm.PostboxItemSend("elixr", 50, "");
            yield return null;
            nm.PostboxItemSend("leaf_box", 25, "");
            yield return null;
            nm.PostboxItemSend("reinforce_box", 25, "");
            yield return null;
        }
        CodeStage.AntiCheat.Storage.ObscuredPrefs.Save();
    }









    /// <summary>
    /// 모든 버튼 setActive(false)
    /// </summary>
    void ShutUpMalpoi()
    {
        for (int i = 0; i < purcBtns.Length; i++)
        {
            purcBtns[i].gameObject.SetActive(false);
        }
        /// TODO : 패키지 홍보 메세지 꺼줌
        TextPaakge.SetActive(false);
    }

    double dtimeBae;
    /// <summary>
    /// 진짜 구매할래 확인 팝업에서 진짜 구매 -> 결제창 표기
    /// </summary>
    public void ClickedPurcahseBtn()
    {
        if (purcBtns[0].gameObject.activeSelf)
        {
            switch (purchaseIndex[0])
            {
                case 0:
                    Purchase_Product_dia100();
                    break;

                case 1:
                    Purchase_Product_dia600();
                    break;

                case 2:
                    Purchase_Product_dia1300();
                    break;

                case 3:
                    Purchase_Product_dia4200();
                    break;

                case 4:
                    Purchase_Product_dia7500();
                    break;

                case 5:
                    Purchase_Product_dia16000();
                    break;

                default:
                    break;
            }
        }
        else if (purcBtns[1].gameObject.activeSelf)
        {
            switch (purchaseIndex[1])
            {
                case 0:
                    Purchase_Product_noad();
                    break;

                case 1:
                    Purchase_Product_buff_attack();
                    break;

                case 2:
                    Purchase_Product_buff_attackspeed();
                    break;

                case 3:
                    Purchase_Product_buff_movespeed();
                    break;

                case 4:
                    Purchase_Product_buff_gold();
                    break;

                default:
                    break;
            }
        }
        /// 게임 내 다이아로 구매하는 클릭
        else if (purcBtns[2].gameObject.activeSelf)                                                                 
        {
            /// 인벤토리 레드닷 켜주기
            RedDotManager.instance.RedDot[3].SetActive(true);
            /// 0개 구입시 리턴
            if (co_Amount < 1) 
                return;
            switch (purchaseIndex[2])
            {
                case 0:
                    if (PlayerInventory.Money_Dia < 100) return;
                    PlayerInventory.Money_Dia -= 100;
                    SetPopContents(sim.NoriCons[0], 11, 0, 2);
                    bm.DieHardCoTimer(0);
                    break;

                case 1:
                    if (PlayerInventory.Money_Dia < 100) return;
                    PlayerInventory.Money_Dia -= 100;
                    SetPopContents(sim.NoriCons[1], 11, 0, 2);
                    bm.DieHardCoTimer(1);
                    break;

                case 2:
                    if (PlayerInventory.Money_Dia < 100) return;
                    PlayerInventory.Money_Dia -= 100;
                    SetPopContents(sim.NoriCons[2], 11, 0, 2);
                    bm.DieHardCoTimer(2);
                    break;

                case 3:
                    if (PlayerInventory.Money_Dia < 100) return;
                    PlayerInventory.Money_Dia -= 100;
                    SetPopContents(sim.NoriCons[3], 11, 0, 2);
                    bm.DieHardCoTimer(3);
                    break;

                case 4:
                    if (PlayerInventory.Money_Dia < co_Amount * 100) return;
                    PlayerInventory.Money_Dia -= co_Amount * 100;

                    dtimeBae = co_Amount * 100d * (3d * 1.15d * PlayerInventory.RecentDistance) * PlayerInventory.Player_Gold_Earned;
                    /// 계산한 값 더해줌.
                    SetPopContents(sim.NoriCons[4], -1, 0, 2);
                    PlayerInventory.Money_Gold += dtimeBae;
                    ///  골드 업적 카운트 올리기
                    ListModel.Instance.ALLlist_Update(3, dtimeBae);
                    break;

                case 5:
                    if (PlayerInventory.Money_Dia < co_Amount * 100) return;
                    PlayerInventory.Money_Dia -= co_Amount * 100;
                    SetPopContents(sim.NoriCons[5], co_Amount, 0, 2);
                    PlayerInventory.SetTicketCount("cave_enter", co_Amount);
                    break;

                case 6:
                    if (PlayerInventory.Money_Dia < co_Amount * 100) return;
                    PlayerInventory.Money_Dia -= co_Amount * 100;
                    SetPopContents(sim.NoriCons[6], co_Amount, 0, 2);
                    PlayerInventory.SetTicketCount("cave_clear", co_Amount);
                    break;

                case 7:
                    if (PlayerInventory.Money_Dia < co_Amount * 100) return;
                    PlayerInventory.Money_Dia -= co_Amount * 100;
                    SetPopContents(sim.NoriCons[7], co_Amount, 0, 2);
                    PlayerInventory.SetTicketCount("mining", co_Amount);
                    break;

                case 8:
                    if (PlayerInventory.Money_Dia < co_Amount * 100) return;
                    PlayerInventory.Money_Dia -= co_Amount * 100;
                    SetPopContents(sim.NoriCons[8], co_Amount, 0, 2);
                    PlayerInventory.SetTicketCount("amber", co_Amount);
                    break;

                case 9:
                    if (PlayerInventory.Money_Dia < co_Amount * 100) return;
                    PlayerInventory.Money_Dia -= co_Amount * 100;
                    SetPopContents(sim.NoriCons[9], co_Amount, 0, 2);
                    PlayerInventory.SetBoxsCount("weapon_coupon", co_Amount);
                    break;

                case 10:
                    if (PlayerInventory.Money_Dia < co_Amount * 100) return;
                    PlayerInventory.Money_Dia -= co_Amount * 100;
                    SetPopContents(sim.NoriCons[10], 1, 0, 2);
                    PlayerInventory.SetTicketCount("pvp", 1);
                    break;

                case 11:
                    if (PlayerInventory.Money_Dia < co_Amount * 100) return;
                    PlayerInventory.Money_Dia -= co_Amount * 100;
                    SetPopContents(sim.NoriCons[11], co_Amount, 0, 2);
                    PlayerInventory.SetTicketCount("reinforce_box", co_Amount);
                    break;

                case 12:
                    if (PlayerInventory.Money_Dia < co_Amount * 100) return;
                    PlayerInventory.Money_Dia -= co_Amount * 100;
                    SetPopContents(sim.NoriCons[12], co_Amount, 0, 2);
                    PlayerInventory.SetTicketCount("leaf_box", co_Amount);
                    break;

                case 13:
                    if (PlayerInventory.Money_Dia < co_Amount * 100) return;
                    PlayerInventory.Money_Dia -= co_Amount * 100;
                    SetPopContents(sim.NoriCons[13], co_Amount, 0, 2);
                    PlayerInventory.Money_Elixir+= co_Amount;
                    break;

                default:
                    break;
            }
        }
        /// PACK) 일반 패키지 또는 한정 패키지
        else if (purcBtns[3].gameObject.activeSelf || purcBtns[4].gameObject.activeSelf)
        {

        }
        /// PACK) 연속 패키지
        else if (purcBtns[5].gameObject.activeSelf)
        {

        }
    }

    private int tmpStone = 0;
    /// <summary>
    /// 1.실제로 재화 증가시켜주고 
    /// 2.선물함으로 지급 팝업 세팅해서 SetActive 까지
    /// </summary>
    /// <param name="_spr"></param>
    /// <param name="_amount"></param>
    void SetPopContents(Sprite _spr, int _amount, int _stoneAmong, int _tag)
    {
        /// 팝업 그림 통일
        sim.popUpIcon.sprite = _spr;
        /// 다이아 상점에서 현질함
        if (_tag == 0)
        {
            GiftGetText.text = LeanLocalization.GetTranslationText("Config_Gift_Desc");
            sim.popUpIconDesc.text = "x" + _amount.ToString("N0");
            nm.PostboxItemSend("diamond", _amount, "");
            CodeStage.AntiCheat.Storage.ObscuredPrefs.Save();
        }
        /// 특별 상점에서 현질함
        else if (_tag == 1)
        {
            /// 얘는 광고제거
            if (_amount == 21)
            {
                GiftGetText.text = LeanLocalization.GetTranslationText("Shop_Desc_Detail");
            }
            /// 나머진 버프 적용
            else
            {
                GiftGetText.text = LeanLocalization.GetTranslationText("Shop_Buff_Detail");
            }
            /// 버프 아이콘 갯수 표기 안함
            sim.popUpIconDesc.text = "";
        }
        /// 일반 상점에서 다이아로 뭐 사먹음
        else if(_tag == 2)
        {
            /// 얘는 골드 구매했을때
            if (_amount == -1)
            {
                sim.popUpIconDesc.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(dtimeBae);
                GiftGetText.text = LeanLocalization.GetTranslationText("YOUR_REWORD_IN_POCKET");
            }
            /// 버프 아이콘일경우 갯수 표기 안함
            else if (_amount == 11)
            {
                sim.popUpIconDesc.text = "";
                GiftGetText.text = LeanLocalization.GetTranslationText("Shop_Buff_Detail");
            }
            /// 입장권 등등
            else
            {
                sim.popUpIconDesc.text = "x" + _amount.ToString("N0");
                GiftGetText.text = LeanLocalization.GetTranslationText("YOUR_REWORD_IN_POCKET");
            }
        }
        /// TODO : 패키지 상점이에용
        else
        {

        }

        /// 두번째 팝업에서 결정 증가 갯수 (sim.SpeciCons[2], 1, 3);
        tmpStone = _stoneAmong;
        /// 팝업 표시해줌
        sim.popUp.SetActive(true);
        /// 정상 구매 되었으면 몽땅 세이브
        PlayerPrefsManager.instance.TEST_SaveJson();
    }
    /// <summary>
    /// 최초 버튼 클릭시 -> 확인 버튼에 달아줌
    /// </summary>
    /// <param name="_stoneAmong"></param>
    public void SecondPopUP()
    {
        if (tmpStone != 0)
        {
            sim.popUpIcon.sprite = sim.DiaiCons[6];
            sim.popUpIconDesc.text = "x" + tmpStone.ToString("N0");
            nm.PostboxItemSend("stone", tmpStone, "");
            tmpStone = 0;
            GiftGetText.text = LeanLocalization.GetTranslationText("Config_Gift_Desc");
            CodeStage.AntiCheat.Storage.ObscuredPrefs.Save();
        }
        else
        {
            sim.popUp.SetActive(false);
            PopUpManager.instance.HidePopUP(13);
        }
    }

    void PurchaseFailedHandler(IAPProduct product)
    {
        //Debug.Log("The purchase of product " + product.Name + " has failed.");
        // 구매실패시 핸들러 떼주고.
        InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;
    }
    void PurchaseCompletedHandler(IAPProduct product)
    {
        switch (product.Name)
        {
            /// <1번쨰탭> 
            case EM_IAPConstants.Product_dia100: SetPopContents(sim.DiaiCons[0], 100, 1,0); break;
            case EM_IAPConstants.Product_dia600: SetPopContents(sim.DiaiCons[1], 600, 5, 0); break;
            case EM_IAPConstants.Product_dia1300: SetPopContents(sim.DiaiCons[2], 1300, 10, 0); break;
            case EM_IAPConstants.Product_dia4200: SetPopContents(sim.DiaiCons[3], 4200, 40, 0); break;
            case EM_IAPConstants.Product_dia7500: SetPopContents(sim.DiaiCons[4], 7500, 70, 0); break;
            case EM_IAPConstants.Product_dia16000: SetPopContents(sim.DiaiCons[5], 16000, 150, 0); break;

            /// <2번째탭> 특별 상점
            case EM_IAPConstants.Product_noad: 
                /// 버튼 효과 바꿔주기
                SetPopContents(sim.SpeciCons[0], 21, 5, 1);
                /// 광고스킵 가능해짐.
                PlayerInventory.isSuperUser = 525;
                ListModel.Instance.mvpDataList[0].SuperUser = 525;
                bac.Banner525Hide();
                /// 플레이팹에 상태 저장
                PlayerPrefsManager.instance.JObjectSave(true);
                sim.ChangeSection();
                break;
            case EM_IAPConstants.Product_buff_attack:
                SetPopContents(sim.SpeciCons[1], 11, 3, 1);
                PlayerInventory.SetBuffStack(1);
                bm.MoneyLoveBuff(0);
                sim.ChangeSection();
                break;
            case EM_IAPConstants.Product_buff_attackspeed:
                SetPopContents(sim.SpeciCons[2], 11, 3, 1);
                PlayerInventory.SetBuffStack(2);
                bm.MoneyLoveBuff(1);
                sim.ChangeSection();
                break;
            case EM_IAPConstants.Product_buff_movespeed:
                SetPopContents(sim.SpeciCons[3], 11, 3, 1);
                PlayerInventory.SetBuffStack(3);
                bm.MoneyLoveBuff(2);
                sim.ChangeSection();
                break;
            case EM_IAPConstants.Product_buff_gold:
                SetPopContents(sim.SpeciCons[4], 11, 3, 1);
                PlayerInventory.SetBuffStack(4);
                bm.MoneyLoveBuff(3);
                sim.ChangeSection();
                break;



            /// 패키지 상점
            case EM_IAPConstants.Product_pack_01:
                StartCoroutine(Front5());
                ComplPopup.SetActive(true);
                break;

            case EM_IAPConstants.Product_pack_02:
                //SetPopContents(sim.PackCons[1], 1, 0, 3);
                StartCoroutine(Front5());
                ComplPopup.SetActive(true);
                break;
            case EM_IAPConstants.Product_pack_03:
                //SetPopContents(sim.PackCons[1], 1, 0, 3);
                StartCoroutine(Front5());
                ComplPopup.SetActive(true);
                break;
            case EM_IAPConstants.Product_pack_04:
                //SetPopContents(sim.PackCons[1], 1, 0, 3);
                StartCoroutine(Front5());
                ComplPopup.SetActive(true);
                break;
            case EM_IAPConstants.Product_pack_05:
                //SetPopContents(sim.PackCons[1], 1, 0, 3);
                StartCoroutine(Notouch(0));
                ComplPopup.SetActive(true);
                break;



                /// ------------------ 한정 패키지 1회 구매 가능



            case EM_IAPConstants.Product_pack_06:
                StartCoroutine(Notouch(1));
                ListModel.Instance.mvpDataList[0].pack_06 = 6;
                ComplPopup.SetActive(true);
                SwichPackageCatgory(1);
                break;
            case EM_IAPConstants.Product_pack_07:
                nm.PostboxItemSend("stone", 15, "");
                PlayerInventory.isSuperUser = 525;
                ListModel.Instance.mvpDataList[0].SuperUser = 525;
                ListModel.Instance.mvpDataList[0].pack_07 = 7;
                bac.Banner525Hide();
                PlayerInventory.SetBuffStack(1);
                bm.MoneyLoveBuff(0);
                PlayerInventory.SetBuffStack(2);
                bm.MoneyLoveBuff(1);
                PlayerInventory.SetBuffStack(3);
                bm.MoneyLoveBuff(2);
                PlayerInventory.SetBuffStack(4);
                bm.MoneyLoveBuff(3);
                /// 플레이팹에 상태 저장
                PlayerPrefsManager.instance.JObjectSave(true);
                ComplPopup.SetActive(true);
                SwichPackageCatgory(1);
                break;
            case EM_IAPConstants.Product_pack_08:
                ListModel.Instance.mvpDataList[0].pack_08 = 8;
                StartCoroutine(Back7());
                ComplPopup.SetActive(true);
                SwichPackageCatgory(1);
                break;
            case EM_IAPConstants.Product_pack_09:
                ListModel.Instance.mvpDataList[0].pack_09 = 9;
                StartCoroutine(Back7());
                ComplPopup.SetActive(true);
                SwichPackageCatgory(1);
                break;
            case EM_IAPConstants.Product_pack_10:
                ListModel.Instance.mvpDataList[0].pack_10 = 1;
                StartCoroutine(Back7());
                ComplPopup.SetActive(true);
                SwichPackageCatgory(1);
                break;


            case EM_IAPConstants.Product_day_01:
                ListModel.Instance.mvpDataList[0].daily_13 = 31;
                StartCoroutine(DailyPack());
                SwichPackageCatgory(2);
                ComplPopup.SetActive(true);
                break;
            case EM_IAPConstants.Product_week_01:
                ListModel.Instance.mvpDataList[0].weekend_17 = 71;
                if (ListModel.Instance.mvpDataList[0].weekend_Day == 0)
                {
                    ListModel.Instance.mvpDataList[0].weekend_Day = (int)UnbiasedTime.Instance.Now().DayOfWeek;
                }
                StartCoroutine(WeekPack());
                SwichPackageCatgory(3);
                ComplPopup.SetActive(true);
                break;
            case EM_IAPConstants.Product_month_01:
                ListModel.Instance.mvpDataList[0].mouth_21 = 21;
                if (ListModel.Instance.mvpDataList[0].mouth_Day == 0)
                {
                    ListModel.Instance.mvpDataList[0].mouth_Day = UnbiasedTime.Instance.Now().Day;
                }
                StartCoroutine(MonthPack(1));
                SwichPackageCatgory(4);
                ComplPopup.SetActive(true);
                break;
            case EM_IAPConstants.Product_month_02:
                ListModel.Instance.mvpDataList[0].mouth_22 = 12;
                if (ListModel.Instance.mvpDataList[0].mouth_Day == 0)
                {
                    ListModel.Instance.mvpDataList[0].mouth_Day = UnbiasedTime.Instance.Now().Day;
                }
                StartCoroutine(MonthPack(2));
                SwichPackageCatgory(4);
                ComplPopup.SetActive(true);
                break;
            case EM_IAPConstants.Product_month_03:
                ListModel.Instance.mvpDataList[0].mouth_23 = 32;
                if (ListModel.Instance.mvpDataList[0].mouth_Day == 0)
                {
                    ListModel.Instance.mvpDataList[0].mouth_Day = UnbiasedTime.Instance.Now().Day;
                }
                StartCoroutine(MonthPack(3));
                SwichPackageCatgory(4);
                ComplPopup.SetActive(true);
                break;



            ///update210204
            ///--------------------------------------update210204 ----------------------------------------------
            ///update210204
            /// 1.0.7 에서 저려미 패키지 추가
            case EM_IAPConstants.Product_pack_11:
                StartCoroutine(Notouch(2));
                ComplPopup.SetActive(true);
                break;

            case EM_IAPConstants.Product_pack_12:
                StartCoroutine(Notouch(3));
                ComplPopup.SetActive(true);
                break;

            case EM_IAPConstants.Product_pack_13:
                StartCoroutine(Notouch(4));
                ComplPopup.SetActive(true);
                break;









            default:
                break;
        }

        InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;
        /// 정상 구매 되었으면 몽땅 세이브
        PlayerPrefsManager.instance.TEST_SaveJson();
    }


    IEnumerator Notouch(int _index)
    {
        switch (_index)
        {
            case 0 :
                yield return null;
                nm.PostboxItemSend("weapon_coupon", 100, "");
                yield return null;
                nm.PostboxItemSend("reinforce", 10000, "");
                yield return null;
                nm.PostboxItemSend("mining", 50, "");
                yield return null;
                nm.PostboxItemSend("amber", 100, "");
                yield return null;
                nm.PostboxItemSend("stone", 150, "");
                yield return null;
                break;

            case 1:
                yield return null;
                nm.PostboxItemSend("diamond", 1000, "");
                yield return null;
                nm.PostboxItemSend("elixr", 5, "");
                yield return null;
                nm.PostboxItemSend("leaf", 5000, "");
                yield return null;
                nm.PostboxItemSend("reinforce", 2500, "");
                yield return null;
                nm.PostboxItemSend("stone", 5, "");
                yield return null;
                break;


                ///update210204
                ///--------------------------------------update210204 ----------------------------------------------
                ///update210204


            case 2:
                yield return null;
                nm.PostboxItemSend("diamond", 300, "");
                yield return null;
                nm.PostboxItemSend("weapon_coupon", 5, "");
                yield return null;
                nm.PostboxItemSend("reinforce_box", 5, "");
                yield return null;
                nm.PostboxItemSend("stone", 1, "");
                yield return null;
                break;

            case 3:
                yield return null;
                nm.PostboxItemSend("diamond", 500, "");
                yield return null;
                nm.PostboxItemSend("weapon_coupon", 10, "");
                yield return null;
                nm.PostboxItemSend("reinforce_box", 10, "");
                yield return null;
                nm.PostboxItemSend("stone", 3, "");
                yield return null;
                break;

            case 4:
                yield return null;
                nm.PostboxItemSend("weapon_coupon", 20, "");
                yield return null;
                nm.PostboxItemSend("reinforce_box", 20, "");
                yield return null;
                nm.PostboxItemSend("cave", 2, "");
                yield return null;
                nm.PostboxItemSend("mining", 2, "");
                yield return null;
                nm.PostboxItemSend("stone", 5, "");
                yield return null;
                break;

        }

        CodeStage.AntiCheat.Storage.ObscuredPrefs.Save();
    }

    IEnumerator Front5()
    {
        yield return null;
        nm.PostboxItemSend("diamond", front5Amount[0], "");
        yield return null;
        nm.PostboxItemSend("elixr", front5Amount[1], "");
        yield return null;
        nm.PostboxItemSend("cave", front5Amount[2], "");
        yield return null;
        nm.PostboxItemSend("mining", front5Amount[3], "");
        yield return null;
        nm.PostboxItemSend("stone", front5Amount[4], "");
        yield return null;
        CodeStage.AntiCheat.Storage.ObscuredPrefs.Save();
    }

    IEnumerator Back7()
    {
        yield return null;
        nm.PostboxItemSend("diamond", back7Amount[0], "");
        yield return null;
        nm.PostboxItemSend("elixr", back7Amount[1], "");
        yield return null;
        nm.PostboxItemSend("leaf", back7Amount[2], "");
        yield return null;
        nm.PostboxItemSend("reinforce", back7Amount[3], "");
        yield return null;
        nm.PostboxItemSend("cave_clear", back7Amount[4], "");
        yield return null;
        nm.PostboxItemSend("mining", back7Amount[5], "");
        yield return null;
        nm.PostboxItemSend("stone", back7Amount[6], "");
        yield return null;
        CodeStage.AntiCheat.Storage.ObscuredPrefs.Save();
    }

    IEnumerator DailyPack()
    {
        yield return null;
        nm.PostboxItemSend("diamond", 100, "");
        yield return null;
        nm.PostboxItemSend("leaf_box", 1, "");
        yield return null;
        nm.PostboxItemSend("reinforce_box", 1, "");
        yield return null;
        CodeStage.AntiCheat.Storage.ObscuredPrefs.Save();
    }
    IEnumerator WeekPack()
    {
        yield return null;
        nm.PostboxItemSend("reinforce_box", 20, "");
        yield return null;
        nm.PostboxItemSend("amber", 5, "");
        yield return null;
        nm.PostboxItemSend("stone", 5, "");
        yield return null;
        CodeStage.AntiCheat.Storage.ObscuredPrefs.Save();
    }
    IEnumerator MonthPack(int _Index)
    {
        yield return null;
        /// 월간패키지 1, 2, 3
        switch (_Index)
        {
            case 1:
                nm.PostboxItemSend("weapon_coupon", 10, "");
                yield return null;
                nm.PostboxItemSend("reinforce_box", 10, "");
                yield return null;
                nm.PostboxItemSend("amber", 5, "");
                yield return null;
                nm.PostboxItemSend("stone", 10, "");
                yield return null;
                break;
            case 2:
                nm.PostboxItemSend("S_box", 1, "");
                yield return null;
                nm.PostboxItemSend("reinforce_box", 30, "");
                yield return null;
                nm.PostboxItemSend("mining", 10, "");
                yield return null;
                nm.PostboxItemSend("stone", 15, "");
                yield return null;
                break;
            case 3:
                nm.PostboxItemSend("S_box", 5, "");
                yield return null;
                nm.PostboxItemSend("reinforce_box", 100, "");
                yield return null;
                nm.PostboxItemSend("amber", 50, "");
                yield return null;
                nm.PostboxItemSend("stone", 70, "");
                yield return null;
                break;
        }
        CodeStage.AntiCheat.Storage.ObscuredPrefs.Save();
    }



    #region <현금결제_상품_목록>

    void Purchase_Product_dia100()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_dia100);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_dia600()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_dia600);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_dia1300()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_dia1300);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_dia4200()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_dia4200);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_dia7500()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_dia7500);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_dia16000()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_dia16000);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_noad()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_noad);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_buff_attack()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_buff_attack);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_buff_attackspeed()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_buff_attackspeed);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_buff_gold()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_buff_gold);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_buff_movespeed()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_buff_movespeed);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }



    /// ----------------------------------------------------------- 패키지 상점 목록



    void Purchase_Product_pack_01()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_pack_01);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_pack_02()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_pack_02);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_pack_03()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_pack_03);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_pack_04()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_pack_04);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_pack_05()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_pack_05);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_pack_06()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_pack_06);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_pack_07()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_pack_07);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_pack_08()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_pack_08);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_pack_09()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_pack_09);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_pack_10()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_pack_10);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_day_01()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_day_01);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_week_01()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_week_01);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_month_01()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_month_01);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_month_02()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_month_02);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }
    void Purchase_Product_month_03()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_month_03);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }


    void Purchase_Product_pack_11()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_pack_11);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    void Purchase_Product_pack_12()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_pack_12);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    void Purchase_Product_pack_13()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_pack_13);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    #endregion

}

