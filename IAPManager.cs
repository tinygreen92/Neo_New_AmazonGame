using EasyMobile;
using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour
{
    [Header("- 패키지 팝업 세팅")]
    public Sprite[] PakageCons;
    public GameObject PackGiftPop;
    public GameObject[] PackCategory;
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
    public Transform[] purcBtns;
    public int[] purchaseIndex;



    public void ClickedPakageShop()
    {
        SwichPackageCatgory(0);
        /// 팝업
        PopUpManager.instance.ShowPopUP(30);
    }

    public void SwichPackageCatgory(int _index)
    {
        for (int i = 0; i < PackCategory.Length; i++)
        {
            PackCategory[i].SetActive(false);
        }
        PackCategory[_index].SetActive(true);
        switch (_index)
        {
            case 0:
                btnImgs[0].sprite = EnableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = DisableBtn;
                break;

            case 1:
                btnImgs[0].sprite = DisableBtn;
                btnImgs[1].sprite = EnableBtn;
                btnImgs[2].sprite = DisableBtn;
                break;

            case 2:
                btnImgs[0].sprite = DisableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = EnableBtn;
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
        DescObject.text = LeanLocalization.GetTranslationText("Shop_Desc");
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
    public void Purchase_Spec(int _indx)
    {
        ShutUpMalpoi();
        /// 청약철회 오브젝트 살려줌
        DescObject.text = LeanLocalization.GetTranslationText("Shop_Desc");

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
    }
    public void Purchase_Nor(int _indx)
    {
        ShutUpMalpoi();
        /// 청약철회 오브젝트 가려줌
        DescObject.text = ListModel.Instance.shopListNOR[_indx].korTailDesc;
        IconImg.sprite = sim.NoriCons[_indx];
        IconDesc.text = "";
        /// 다이아 가격
        purcBtns[2].GetComponentInChildren<Text>().text = ListModel.Instance.shopListNOR[_indx].korPrice;
        purchaseIndex[2] = _indx;
        purcBtns[2].gameObject.SetActive(true);
        PopUpManager.instance.ShowPopUP(13);
    }

    public void Test_Purchase_Pakage(int _indx)
    {
        purchaseIndex[3] = _indx;
    }

    public void Purchase_Pakage(int _indx)
    {
        ShutUpMalpoi();
        /// 청약철회 오브젝트 살려줌
        DescObject.text = LeanLocalization.GetTranslationText("Shop_Desc");

        IconImg.sprite = PakageCons[_indx];
        IconDesc.text = "";

        if (LeanLocalization.CurrentLanguage == "Korean")
        {
            System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("ko-KR", false).NumberFormat;
            purcBtns[3].GetComponentInChildren<Text>().text = System.Convert.ToInt64(ListModel.Instance.shopListPACK[_indx].korPrice).ToString("C", numberFormat);
        }
        else
        {
            System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
            purcBtns[3].GetComponentInChildren<Text>().text = System.Convert.ToInt64(ListModel.Instance.shopListPACK[_indx].engPrice).ToString("C", numberFormat);
        }
        purchaseIndex[3] = _indx;
        purcBtns[3].gameObject.SetActive(true);
        PopUpManager.instance.ShowPopUP(13);
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
            ///
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
                    if (PlayerInventory.Money_Dia < 100) return;
                    PlayerInventory.Money_Dia -= 100;

                    dtimeBae = 100d * (3d * 1.15d * PlayerInventory.RecentDistance) * PlayerInventory.Player_Gold_Earned;
                    /// 계산한 값 더해줌.
                    SetPopContents(sim.NoriCons[4], -1, 0, 2);
                    PlayerInventory.Money_Gold += dtimeBae;
                    ///  골드 업적 카운트 올리기
                    ListModel.Instance.ALLlist_Update(3, dtimeBae);
                    break;

                case 5:
                    if (PlayerInventory.Money_Dia < 100) return;
                    PlayerInventory.Money_Dia -= 100;
                    SetPopContents(sim.NoriCons[5], 1, 0, 2);
                    PlayerInventory.SetTicketCount("cave_enter", 1);
                    break;

                case 6:
                    if (PlayerInventory.Money_Dia < 100) return;
                    PlayerInventory.Money_Dia -= 100;
                    SetPopContents(sim.NoriCons[6], 1, 0, 2);
                    PlayerInventory.SetTicketCount("cave_clear", 1);
                    break;

                case 7:
                    if (PlayerInventory.Money_Dia < 100) return;
                    PlayerInventory.Money_Dia -= 100;
                    SetPopContents(sim.NoriCons[7], 1, 0, 2);
                    PlayerInventory.SetTicketCount("mining", 1);
                    break;

                case 8:
                    if (PlayerInventory.Money_Dia < 100) return;
                    PlayerInventory.Money_Dia -= 100;
                    SetPopContents(sim.NoriCons[8], 1, 0, 2);
                    PlayerInventory.SetTicketCount("amber", 1);
                    break;

                case 9:
                    if (PlayerInventory.Money_Dia < 100) return;
                    PlayerInventory.Money_Dia -= 100;
                    SetPopContents(sim.NoriCons[9], 1, 0, 2);
                    PlayerInventory.SetBoxsCount("weapon_coupon", 1);
                    break;

                case 10:
                    if (PlayerInventory.Money_Dia < 100) return;
                    PlayerInventory.Money_Dia -= 100;
                    SetPopContents(sim.NoriCons[10], 1, 0, 2);
                    PlayerInventory.SetTicketCount("pvp", 1);
                    break;

                case 11:
                    if (PlayerInventory.Money_Dia < 100) return;
                    PlayerInventory.Money_Dia -= 100;
                    SetPopContents(sim.NoriCons[11], 1, 0, 2);
                    PlayerInventory.SetTicketCount("reinforce_box", 1);
                    break;

                case 12:
                    if (PlayerInventory.Money_Dia < 100) return;
                    PlayerInventory.Money_Dia -= 100;
                    SetPopContents(sim.NoriCons[12], 1, 0, 2);
                    PlayerInventory.SetTicketCount("leaf_box", 1);
                    break;

                case 13:
                    if (PlayerInventory.Money_Dia < 100) return;
                    PlayerInventory.Money_Dia -= 100;
                    SetPopContents(sim.NoriCons[13], 1, 0, 2);
                    PlayerInventory.Money_Elixir++;
                    break;

                default:
                    break;
            }
        }
        /// PACK) 일반 패키지 또는 한정 패키지
        else if (purcBtns[3].gameObject.activeSelf || purcBtns[4].gameObject.activeSelf)
        {
            PackGiftPop.SetActive(true);
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

                case 5:
                    Purchase_Product_pack_06();
                    break;

                case 6:
                    Purchase_Product_pack_07();
                    break;

                case 7:
                    Purchase_Product_pack_08();
                    break;

                case 8:
                    Purchase_Product_pack_09();
                    break;

                case 9:
                    Purchase_Product_pack_10();
                    break;

                default:
                    break;
            }
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
            /// 정상 구매 되었으면 몽땅 세이브
            PlayerPrefsManager.instance.TEST_SaveJson();
        }
        /// TODO : 패키지 상점이에용
        else
        {

        }

        /// 두번째 팝업에서 결정 증가 갯수 (sim.SpeciCons[2], 1, 3);
        tmpStone = _stoneAmong;
        /// 팝업 표시해줌
        sim.popUp.SetActive(true);
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
                GameObject.FindWithTag("PFM").GetComponent<PlayFabManage>().SetUserData();
                PlayerPrefs.Save();

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
                //SetPopContents(sim.PackCons[0], 1, 0, 3);
                /// TODO : 외부 매니저를 끌어오지 그냥?

                break;

            case EM_IAPConstants.Product_pack_02:
                //SetPopContents(sim.PackCons[1], 1, 0, 3);
                break;
            case EM_IAPConstants.Product_pack_03:
                //SetPopContents(sim.PackCons[1], 1, 0, 3);
                break;
            case EM_IAPConstants.Product_pack_04:
                //SetPopContents(sim.PackCons[1], 1, 0, 3);
                break;
            case EM_IAPConstants.Product_pack_05:
                //SetPopContents(sim.PackCons[1], 1, 0, 3);
                break;
            case EM_IAPConstants.Product_pack_06:
                //SetPopContents(sim.PackCons[1], 1, 0, 3);
                break;
            case EM_IAPConstants.Product_pack_07:
                //SetPopContents(sim.PackCons[1], 1, 0, 3);
                break;
            case EM_IAPConstants.Product_pack_08:
                //SetPopContents(sim.PackCons[1], 1, 0, 3);
                break;
            case EM_IAPConstants.Product_pack_09:
                //SetPopContents(sim.PackCons[1], 1, 0, 3);
                break;
            case EM_IAPConstants.Product_pack_10:
                //SetPopContents(sim.PackCons[1], 1, 0, 3);
                break;
            default:
                break;
        }

        InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;
        /// 정상 구매 되었으면 몽땅 세이브
        PlayerPrefsManager.instance.TEST_SaveJson();
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




    #endregion

}
