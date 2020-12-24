using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 7.InventoryCanvas 에 달려있는 플레이어 인벤토리(보여지는 것) 관리 스크립트
/// 인벤토리 관리할 구조체가 있어야 겠는디 ->
/// 1. 재화관련 표기는 훼이크 : 초기에 리스트에 4칸은 자리 잡아 준다.
/// 2. 실제 존재하는 항목은 상자.
/// </summary>
public class PopInventory : MonoBehaviour
{
    public GameObject popDesc;
    [Header("-미니 팝업 가챠")]
    public Sprite[] miniSprs;
    public GameObject miniPop;
    public Image miniIcon;
    public Text miniAmount;
    [Header("-활성화 그룹")]
    public GameObject[] moneyGOgroup;
    public GameObject[] boxGOgroup;
    public GameObject[] emptyGOgroup;
    [Header("-탑뷰")]
    public Image[] btnImgs;
    public Sprite EnableBtn;
    public Sprite DisableBtn;
    [Header("-현재 인벤토리 목록")]
    public ScrollRect scBar;
    [Header("- 인벤토리 아이템 설명")]
    public Sprite[] itemIcons;
    public Sprite[] gatChaIcons;
    public Image itemIcon;
    public Text itemDesc;
    public Text itemAmount;
    public GameObject[] switchItem;




    private static int MAX_CONT;
    private int localIndex;

    private void OnEnable()
    {
        if (MAX_CONT == 0) MAX_CONT = moneyGOgroup.Length;
        ClickedTopView(0);
        MoneyManager.instance.UpdateBoxs();
        MoneyManager.instance.UpdateTicket();
        RedDotManager.instance.RedDot[3].SetActive(false);
    }

    /// <summary>
    /// 외부에서 새로고침 호출 해줌
    /// </summary>
    public void ExtraRefresh()
    {
        AddEmptyItem(localIndex);
        MoneyManager.instance.UpdateBoxs();
        MoneyManager.instance.UpdateTicket();
    }

    /// <summary>
    /// 잃은 만큼 추가해주는 빈 박스
    /// </summary>
    /// <param name="_index"></param>
    public void AddEmptyItem(int _index)
    {
        for (int i = 0; i < emptyGOgroup.Length; i++)
        {
            emptyGOgroup[i].SetActive(false);
        }

        switch (_index)
        {
            case 0:
                for (int i = 0; i < moneyGOgroup.Length; i++)
                {
                    moneyGOgroup[i].SetActive(true);
                    switch (i)
                    {
                        case 0: 
                            if (PlayerInventory.Money_Gold < 1.0d)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 1:
                            if (PlayerInventory.Money_Dia < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 2: 
                            if(PlayerInventory.Money_Leaf < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 3:
                            if(PlayerInventory.Money_EnchantStone < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 4:
                            if (PlayerInventory.amber < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 5:
                            if(PlayerInventory.Money_Elixir < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 6:
                            if (PlayerInventory.Money_AmazonCoin < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 7: 
                            if(PlayerInventory.box_Coupon < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        //
                        case 8:
                            if (PlayerInventory.ticket_reinforce_box < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 9:
                            if (PlayerInventory.ticket_leaf_box < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 10:
                            if (PlayerInventory.ticket_pvp_enter < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 11:
                            if (PlayerInventory.ticket_cave_enter < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 12:
                            if (PlayerInventory.ticket_cave_clear < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 13:
                            if (PlayerInventory.mining < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 14:
                            if (PlayerInventory.S_reinforce_box < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 15:
                            if (PlayerInventory.S_leaf_box < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 16:
                            if (PlayerInventory.Crazy_dia < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 17:
                            if (PlayerInventory.Crazy_elixr < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        default: break;
                    }

                }
                for (int i = 0; i < boxGOgroup.Length; i++)
                {
                    boxGOgroup[i].SetActive(true);
                    switch (i)
                    {
                        case 0: if (PlayerInventory.box_E < 1)
                            {
                                boxGOgroup[i].SetActive(false);
                                emptyGOgroup[i + MAX_CONT].SetActive(true);
                            }
                            continue;
                        case 1: if (PlayerInventory.box_D < 1)
                            {
                                boxGOgroup[i].SetActive(false);
                                emptyGOgroup[i+ MAX_CONT].SetActive(true);
                            }
                            continue;
                        case 2: if (PlayerInventory.box_C < 1)
                            {
                                boxGOgroup[i].SetActive(false);
                                emptyGOgroup[i + MAX_CONT].SetActive(true);
                            }
                            continue;
                        case 3: if (PlayerInventory.box_B < 1)
                            {
                                boxGOgroup[i].SetActive(false);
                                emptyGOgroup[i + MAX_CONT].SetActive(true);
                            }
                            continue;
                        case 4: if (PlayerInventory.box_A < 1)
                            {
                                boxGOgroup[i].SetActive(false);
                                emptyGOgroup[i + MAX_CONT].SetActive(true);
                            }
                            continue;
                        case 5: if (PlayerInventory.box_S < 1)
                            {
                                boxGOgroup[i].SetActive(false);
                                emptyGOgroup[i + MAX_CONT].SetActive(true);
                            }
                            continue;
                        case 6: if (PlayerInventory.box_L < 1)
                            {
                                boxGOgroup[i].SetActive(false);
                                emptyGOgroup[i + MAX_CONT].SetActive(true);
                            }
                            continue;
                        default: break;
                    }
                }
                break;
            case 1:
                for (int i = 0; i < moneyGOgroup.Length; i++)
                {
                    moneyGOgroup[i].SetActive(true);
                    switch (i)
                    {
                        case 0:
                            if (PlayerInventory.Money_Gold < 1.0d)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 1:
                            if (PlayerInventory.Money_Dia < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 2:
                            if (PlayerInventory.Money_Leaf < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 3:
                            if (PlayerInventory.Money_EnchantStone < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 4:
                            if (PlayerInventory.amber < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 5:
                            if (PlayerInventory.Money_Elixir < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 6:
                            if (PlayerInventory.Money_AmazonCoin < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 7:
                            if (PlayerInventory.box_Coupon < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        //
                        case 8:
                            if (PlayerInventory.ticket_reinforce_box < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 9:
                            if (PlayerInventory.ticket_leaf_box < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 10:
                            if (PlayerInventory.ticket_pvp_enter < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 11:
                            if (PlayerInventory.ticket_cave_enter < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 12:
                            if (PlayerInventory.ticket_cave_clear < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 13:
                            if (PlayerInventory.mining < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 14:
                            if (PlayerInventory.S_reinforce_box < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 15:
                            if (PlayerInventory.S_leaf_box < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 16:
                            if (PlayerInventory.Crazy_dia < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        case 17:
                            if (PlayerInventory.Crazy_elixr < 1)
                            {
                                moneyGOgroup[i].SetActive(false);
                                emptyGOgroup[i].SetActive(true);
                            }
                            continue;
                        default: break;
                    }
                }
                for (int i = 0; i < boxGOgroup.Length; i++)
                {
                    boxGOgroup[i].SetActive(false);
                    emptyGOgroup[i + MAX_CONT].SetActive(true);
                }
                break;
            case 2:
                for (int i = 0; i < moneyGOgroup.Length; i++)
                {
                    moneyGOgroup[i].SetActive(false);
                    emptyGOgroup[i].SetActive(true);
                }
                for (int i = 0; i < boxGOgroup.Length; i++)
                {
                    boxGOgroup[i].SetActive(true);
                    switch (i)
                    {
                        case 0: if (PlayerInventory.box_E < 1)
                            {
                                boxGOgroup[i].SetActive(false);
                                emptyGOgroup[i + MAX_CONT].SetActive(true);
                            }
                            continue;
                        case 1: if (PlayerInventory.box_D < 1)
                            {
                                boxGOgroup[i].SetActive(false);
                                emptyGOgroup[i + MAX_CONT].SetActive(true);
                            }
                            continue;
                        case 2: if (PlayerInventory.box_C < 1)
                            {
                                boxGOgroup[i].SetActive(false);
                                emptyGOgroup[i + MAX_CONT].SetActive(true);
                            }
                            continue;
                        case 3: if (PlayerInventory.box_B < 1)
                            {
                                boxGOgroup[i].SetActive(false);
                                emptyGOgroup[i + MAX_CONT].SetActive(true);
                            }
                            continue;
                        case 4: if (PlayerInventory.box_A < 1)
                            {
                                boxGOgroup[i].SetActive(false);
                                emptyGOgroup[i + MAX_CONT].SetActive(true);
                            }
                            continue;
                        case 5: if (PlayerInventory.box_S < 1)
                            {
                                boxGOgroup[i].SetActive(false);
                                emptyGOgroup[i + MAX_CONT].SetActive(true);
                            }
                            continue;
                        case 6: if (PlayerInventory.box_L < 1)
                            {
                                boxGOgroup[i].SetActive(false);
                                emptyGOgroup[i + MAX_CONT].SetActive(true);
                            }
                            continue;
                        default: break;
                    }
                }

                break;

            default: break;
        }
    }

    /// <summary>
    /// 해당 버튼 누르면 활성화
    /// 1 / 10 / 100
    /// </summary>
    public void ClickedTopView(int _multiple)
    {
        localIndex = _multiple;

        scBar.verticalNormalizedPosition = 1f;
        AddEmptyItem(_multiple);

        switch (_multiple)
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

            default: break;
        }
    }

    int gatChaIndex;
    /// <summary>
    /// 팝 인벤토리에 붙여서 박스 소모
    /// </summary>
    /// <param name="_index"></param>
    public void UseGatchaBox(int _index)
    {
        /// 수량 파악
        switch (_index)
        {
            case 0:
                itemDesc.text = ListModel.Instance.shopListNOR[11].korTailDesc;
                itemAmount.text = PlayerInventory.ticket_reinforce_box.ToString("N0");
                itemIcon.sprite = gatChaIcons[0];
                break;
            case 1:
                itemDesc.text = ListModel.Instance.shopListNOR[12].korTailDesc;
                itemAmount.text = PlayerInventory.ticket_leaf_box.ToString("N0");
                itemIcon.sprite = gatChaIcons[1];
                break;
            case 5:
                itemDesc.text = ListModel.Instance.shopListAMA[3].korTailDesc;
                itemAmount.text = PlayerInventory.S_reinforce_box.ToString("N0");
                itemIcon.sprite = gatChaIcons[2];
                break;
            case 6:
                itemDesc.text = ListModel.Instance.shopListAMA[5].korTailDesc;
                itemAmount.text = PlayerInventory.S_leaf_box.ToString("N0");
                itemIcon.sprite = gatChaIcons[3];
                break;
            case 7:
                itemDesc.text = ListModel.Instance.shopListAMA[2].korTailDesc;
                itemAmount.text = PlayerInventory.Crazy_dia.ToString("N0");
                itemIcon.sprite = gatChaIcons[4];
                break;
            case 8:
                itemDesc.text = ListModel.Instance.shopListAMA[4].korTailDesc;
                itemAmount.text = PlayerInventory.Crazy_elixr.ToString("N0");
                itemIcon.sprite = gatChaIcons[5];
                break;
        }

        /// x1 / x10 버튼 출력
        switchItem[0].SetActive(true);
        switchItem[1].SetActive(false);
        /// 찐 버튼 누를 때.
        gatChaIndex = _index;
        /// 팝업 출력
        popDesc.SetActive(true);
    }

    /// <summary>
    /// 1개 열거야 10개 열거야?
    /// </summary>
    public void InvoUseBox(bool _is10Inverse)
    {
        float temp = Time.time * 525f;
        int random;
        Random.InitState((int)temp);
        bool isLastPPOP = false;
        // 현재 가진 상자 임시저장
        int tmpAllamont;
        //
        switch (gatChaIndex)
        {
            // ticket_reinforce_box
            case 0:
                if (PlayerInventory.ticket_reinforce_box < 1) return;
                /// 10회?
                if (_is10Inverse)
                {
                    tmpAllamont = PlayerInventory.ticket_reinforce_box;
                    PlayerInventory.ticket_reinforce_box = 0;
                    isLastPPOP = true;
                    /// 강화석 묶음 10회 뽑
                    random = UnityEngine.Random.Range(100 * tmpAllamont, (300* tmpAllamont) +1);
                    PlayerInventory.Money_EnchantStone += random;
                    ///  강화석 업적 카운트 올리기
                    ListModel.Instance.ALLlist_Update(5, random);
                    /// 미니팝업 세팅
                    miniIcon.sprite = miniSprs[0];
                    miniAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(random);
                    miniPop.SetActive(true);
                }
                else
                {
                    PlayerInventory.ticket_reinforce_box--;
                    /// 강화석 묶음 단뽑
                    random = UnityEngine.Random.Range(100, 301);
                    PlayerInventory.Money_EnchantStone += random;
                    ///  강화석 업적 카운트 올리기
                    ListModel.Instance.ALLlist_Update(5, random);
                    /// 미니팝업 세팅
                    miniIcon.sprite = miniSprs[0];
                    miniAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(random);
                    miniPop.SetActive(true);
                }
                break;
            // ticket_leaf_box
            case 1:
                if (PlayerInventory.ticket_leaf_box < 1) return;
                /// 10회?
                if (_is10Inverse)
                {
                    tmpAllamont = PlayerInventory.ticket_leaf_box;
                    PlayerInventory.ticket_leaf_box = 0;
                    isLastPPOP = true;
                    /// 나뭇잎 묶음 = 1000~10000개 랜덤 드랍
                    random = UnityEngine.Random.Range(100 * tmpAllamont, (500 * tmpAllamont) + 1);
                    PlayerInventory.Money_Leaf += random;
                    /// 나뭇잎 획득량 업적 올리기
                    ListModel.Instance.ALLlist_Update(4, random);
                    /// 미니팝업 세팅
                    miniIcon.sprite = miniSprs[1];
                    miniAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(random);
                    miniPop.SetActive(true);
                }
                else
                {
                    PlayerInventory.ticket_leaf_box--;
                    /// 나뭇잎 묶음 = 100~1000개 랜덤 드랍
                    random = UnityEngine.Random.Range(100, 501);
                    PlayerInventory.Money_Leaf += random;
                    /// 나뭇잎 획득량 업적 올리기
                    ListModel.Instance.ALLlist_Update(4, random);
                    /// 미니팝업 세팅
                    miniIcon.sprite = miniSprs[1];
                    miniAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(random);
                    miniPop.SetActive(true);
                }
                break;
            case 2: break; // ticket_pvp_enter
            case 3: break; // ticket_cave_enter
            case 4: break; // ticket_cave_clear
            // S_reinforce_box
            case 5:
                if (PlayerInventory.S_reinforce_box < 1) return;
                /// 10회?
                if (_is10Inverse)
                {
                    tmpAllamont = PlayerInventory.S_reinforce_box;
                    PlayerInventory.S_reinforce_box = 0;
                    isLastPPOP = true;
                    /// 대박 강화석 10회 뽑
                    random = UnityEngine.Random.Range(1000 * tmpAllamont, (3000 * tmpAllamont) +1);
                    PlayerInventory.Money_EnchantStone += random;
                    ///  강화석 업적 카운트 올리기
                    ListModel.Instance.ALLlist_Update(5, random);
                    /// 미니팝업 세팅
                    miniIcon.sprite = miniSprs[0];
                    miniAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(random);
                    miniPop.SetActive(true);
                }
                else
                {
                    PlayerInventory.S_reinforce_box--;
                    /// 대박 강화석  단뽑
                    random = UnityEngine.Random.Range(1000, 3001);
                    PlayerInventory.Money_EnchantStone += random;
                    ///  강화석 업적 카운트 올리기
                    ListModel.Instance.ALLlist_Update(5, random);
                    /// 미니팝업 세팅
                    miniIcon.sprite = miniSprs[0];
                    miniAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(random);
                    miniPop.SetActive(true);
                }

                break;
            // S_leaf_box
            case 6:
                if (PlayerInventory.S_leaf_box < 1) return;
                /// 10회?
                if (_is10Inverse)
                {
                    tmpAllamont = PlayerInventory.S_leaf_box;
                    PlayerInventory.S_leaf_box = 0;
                    isLastPPOP = true;
                    /// 대박 나뭇잎 10회 뽑
                    random = UnityEngine.Random.Range(1000* tmpAllamont, (5000 * tmpAllamont) +1);
                    PlayerInventory.Money_Leaf += random;
                    /// 나뭇잎 획득량 업적 올리기
                    ListModel.Instance.ALLlist_Update(4, random);
                    /// 미니팝업 세팅
                    miniIcon.sprite = miniSprs[1];
                    miniAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(random);
                    miniPop.SetActive(true);
                }
                else
                {
                    PlayerInventory.S_leaf_box--;
                    /// 대박 나뭇잎  1 회 뽑
                    random = UnityEngine.Random.Range(1000, 5001);
                    PlayerInventory.Money_Leaf += random;
                    /// 나뭇잎 획득량 업적 올리기
                    ListModel.Instance.ALLlist_Update(4, random);
                    /// 미니팝업 세팅
                    miniIcon.sprite = miniSprs[1];
                    miniAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(random);
                    miniPop.SetActive(true);
                }

                break;
            //
            case 7:
                if (PlayerInventory.Crazy_dia < 1) return;
                /// 10회?
                if (_is10Inverse)
                {
                    tmpAllamont = PlayerInventory.Crazy_dia;
                    PlayerInventory.Crazy_dia =0;
                    isLastPPOP = true;
                    /// 대박 다이아 10회 뽑
                    random = UnityEngine.Random.Range(500* tmpAllamont, (1001* tmpAllamont)+1);
                    PlayerInventory.Money_Dia += random;
                    /// 미니팝업 세팅
                    miniIcon.sprite = miniSprs[2];
                    miniAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(random);
                    miniPop.SetActive(true);
                }
                else
                {
                    PlayerInventory.Crazy_dia--;
                    /// 대박 다이아 단뽑
                    random = UnityEngine.Random.Range(500, 1001);
                    PlayerInventory.Money_Dia += random;
                    /// 미니팝업 세팅
                    miniIcon.sprite = miniSprs[2];
                    miniAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(random);
                    miniPop.SetActive(true);
                }

                break;
            case 8:
                if (PlayerInventory.Crazy_elixr < 1) return;
                /// 10회?
                if (_is10Inverse)
                {
                    tmpAllamont = PlayerInventory.Crazy_elixr;
                    PlayerInventory.Crazy_elixr =0;
                    isLastPPOP = true;
                    /// 엘릭서 10회 뽑
                    random = UnityEngine.Random.Range(50* tmpAllamont, (300 * tmpAllamont)+1);
                    PlayerInventory.Money_Elixir += random;
                    /// 미니팝업 세팅
                    miniIcon.sprite = miniSprs[3];
                    miniAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(random);
                    miniPop.SetActive(true);
                }
                else
                {
                    PlayerInventory.Crazy_elixr--;
                    /// 엘릭서 단뽑
                    random = UnityEngine.Random.Range(50, 301);
                    PlayerInventory.Money_Elixir += random;
                    /// 미니팝업 세팅
                    miniIcon.sprite = miniSprs[3];
                    miniAmount.text = "x" + PlayerPrefsManager.instance.DoubleToStringNumber(random);
                    miniPop.SetActive(true);
                }
                break;
        }
        /// 10회 뽑기에서 내부 팝업 표기 새로고침
        if (_is10Inverse)
        {
            UseGatchaBox(gatChaIndex);
            /// 10회 뽑일 때 갯수가 0 되면 팝업 꺼줌
            if(isLastPPOP) popDesc.SetActive(false);
        }
        /// 외부 인벤토리 표기 새로고침
        ExtraRefresh();
    }


    /// <summary>
    /// 설명을 출력할 아이템에 붙여 - 순서 기억
    /// </summary>
    /// <param name="_index"></param>
    public void ClickedMoneyItem(int _index)
    {
        /// 확인 버튼 출력
        switchItem[0].SetActive(false);
        switchItem[1].SetActive(true);
        /// 아이콘 셋팅
        itemIcon.sprite = itemIcons[_index];
        /// 설명 세팅
        switch (_index)
        {
            case 0:
                itemDesc.text = LeanLocalization.GetTranslationText("Inven_Money_Desc_01");
                itemAmount.text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Money_Gold);
                break;
            case 1:
                /// 나뭇잎 셋팅
                itemDesc.text = LeanLocalization.GetTranslationText("Inven_Money_Desc_02");
                itemAmount.text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Money_Leaf);
                break;
            case 2:
                /// 강화석 셋팅
                itemDesc.text = LeanLocalization.GetTranslationText("Inven_Money_Desc_03");
                itemAmount.text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Money_EnchantStone);
                break;
            case 3:
                itemDesc.text = LeanLocalization.GetTranslationText("Inven_Money_Desc_04");
                itemAmount.text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Money_Elixir);
                break;
            case 4:
                itemDesc.text = LeanLocalization.GetTranslationText("Inven_Money_Desc_05");
                itemAmount.text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Money_AmazonCoin);
                break;

            /// ----------------- 재화 설명 끝 ------------------

            case 5:
                itemDesc.text = ListModel.Instance.shopListNOR[10].korTailDesc;
                itemAmount.text = PlayerInventory.ticket_pvp_enter.ToString("N0");
                break;
            case 6:
                itemDesc.text = ListModel.Instance.shopListNOR[5].korTailDesc;
                itemAmount.text = PlayerInventory.ticket_cave_enter.ToString("N0");
                break;
            case 7:
                itemDesc.text = ListModel.Instance.shopListNOR[6].korTailDesc;
                itemAmount.text = PlayerInventory.ticket_cave_clear.ToString("N0");
                break;


            /// -------- 다이아 / 호박석 /  채굴권 추가 --------

            case 8:
                itemDesc.text = LeanLocalization.GetTranslationText("Inven_Money_Desc_06");
                itemAmount.text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Money_Dia);
                break;

            case 9:
                itemDesc.text = ListModel.Instance.shopListNOR[8].korTailDesc;
                itemAmount.text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.amber * 1.0d);
                break;

            case 10:
                itemDesc.text = ListModel.Instance.shopListNOR[7].korTailDesc;
                itemAmount.text = PlayerInventory.mining.ToString("N0");
                break;



        }
        /// 팝업 출력
        popDesc.SetActive(true);
    }
















    /// <summary>
    /// 테스트용 모든 아이템 하나 씩 추가
    /// </summary>
    public void TEST_All_Item()
    {
        PlayerInventory.SetTicketCount("reinforce_box", 10);
        PlayerInventory.SetTicketCount("leaf_box", 10);
        PlayerInventory.SetTicketCount("pvp", 10);
        PlayerInventory.SetTicketCount("cave_enter", 10);
        PlayerInventory.SetTicketCount("cave_clear", 10);
        PlayerInventory.SetTicketCount("S_leaf_box", 10);
        PlayerInventory.SetTicketCount("S_reinforce_box", 10);
        PlayerInventory.SetTicketCount("mining", 10);
        PlayerInventory.SetTicketCount("amber", 10);
        //
        PlayerInventory.SetTicketCount("Crazy_dia", 10);
        PlayerInventory.SetTicketCount("Crazy_elixr", 10);
        //
        PlayerInventory.SetBoxsCount("weapon_coupon", 10);
        PlayerInventory.SetBoxsCount("E", 10);
        PlayerInventory.SetBoxsCount("D", 10);
        PlayerInventory.SetBoxsCount("C", 10);
        PlayerInventory.SetBoxsCount("B", 10);
        PlayerInventory.SetBoxsCount("A", 10);
        PlayerInventory.SetBoxsCount("S", 10);
        PlayerInventory.SetBoxsCount("L", 10);
        //
        //PlayerInventory.Money_Dia++;
        PlayerInventory.Money_AmazonCoin+=10;
        //PlayerInventory.Money_Elixir++;
        PlayerInventory.Money_EnchantStone+=10;
        PlayerInventory.Money_Gold+=10;
        PlayerInventory.Money_Leaf+=10;


        MoneyManager.instance.RefreshAllMoney();
    }
    public void TEST_Coupon_10x()
    {
        PlayerInventory.SetBoxsCount("weapon_coupon", 10);

        MoneyManager.instance.RefreshAllMoney();
    }


}
