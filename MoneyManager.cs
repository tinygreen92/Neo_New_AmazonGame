using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    public PlayFabManage PFM;

    public Text[] InvenTicketTextBoxs;
    public Text[] InvenBoxsTextBoxs;
    public Text[] GoldTextBoxs;
    public Text[] LeafTextBoxs;
    public Text[] DiaTextBoxs;
    public Text[] ElixirTextBoxs;
    public Text[] EnchantStoneTextBoxs;
    public Text[] AmazonStoneTextBoxs;
    public Text[] CostZogakTextBoxs;

    private void Start()
    {
        instance = this;
        RefreshAllMoney();
    }

    /// <summary>
    /// 유료재화는 플레이 팹 매니저에서
    /// </summary>
    public void RefreshAllMoney()
    {
        DisplayGold();
        DisplayLeaf();
        DisplayDia();
        DisplayElixir();
        DisplayEnchantStone();
        DisplayAmazonStone();
        DisplayCostZogak();
        ///무기 재호ㅘ
        UpdateBoxs();
        UpdateTicket();
    }




    public void UpdateBoxs()
    {
        /// 박스 쿠폰 들
        InvenBoxsTextBoxs[0].text = PlayerInventory.box_Coupon.ToString("N0");
        InvenBoxsTextBoxs[1].text = PlayerInventory.box_E.ToString("N0");
        InvenBoxsTextBoxs[2].text = PlayerInventory.box_D.ToString("N0");
        InvenBoxsTextBoxs[3].text = PlayerInventory.box_C.ToString("N0");
        InvenBoxsTextBoxs[4].text = PlayerInventory.box_B.ToString("N0");
        InvenBoxsTextBoxs[5].text = PlayerInventory.box_A.ToString("N0");
        InvenBoxsTextBoxs[6].text = PlayerInventory.box_S.ToString("N0");
        InvenBoxsTextBoxs[7].text = PlayerInventory.box_L.ToString("N0");
    }

    public void UpdateTicket()
    {
        /// 티켓 쿠폰들
        InvenTicketTextBoxs[0].text = PlayerInventory.ticket_reinforce_box.ToString("N0");
        InvenTicketTextBoxs[1].text = PlayerInventory.ticket_leaf_box.ToString("N0");
        InvenTicketTextBoxs[2].text = PlayerInventory.ticket_pvp_enter.ToString("N0");
        InvenTicketTextBoxs[3].text = PlayerInventory.ticket_cave_enter.ToString("N0");
        InvenTicketTextBoxs[4].text = PlayerInventory.ticket_cave_clear.ToString("N0");
        InvenTicketTextBoxs[5].text = PlayerInventory.S_reinforce_box.ToString("N0");

        /// 아마존 포션
        InvenTicketTextBoxs[6].text = PlayerInventory.S_leaf_box.ToString("N0");
        //
        InvenTicketTextBoxs[7].text = PlayerInventory.mining.ToString("N0");
        InvenTicketTextBoxs[8].text = PlayerInventory.amber.ToString("N0");
        //
        InvenTicketTextBoxs[9].text = PlayerInventory.Crazy_dia.ToString("N0");
        InvenTicketTextBoxs[10].text = PlayerInventory.Crazy_elixr.ToString("N0");
        //
        InvenTicketTextBoxs[11].text = PlayerInventory.mining.ToString("N0");
        InvenTicketTextBoxs[12].text = PlayerInventory.amber.ToString("N0");
    }


    string tmpStr;
    public void DisplayGold()
    {
        tmpStr = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Money_Gold);
        for (int i = 0; i < GoldTextBoxs.Length; i++)
        {
            GoldTextBoxs[i].text = tmpStr;
        }
    }

    public void DisplayLeaf()
    {
        tmpStr = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Money_Leaf);
        for (int i = 0; i < LeafTextBoxs.Length; i++)
        {
            LeafTextBoxs[i].text = tmpStr;
        }
    }

    public void DisplayDia()
    {
        tmpStr = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Money_Dia);
        for (int i = 0; i < DiaTextBoxs.Length; i++)
        {
            DiaTextBoxs[i].text = tmpStr;
        }
    }

    public void DisplayElixir()
    {
        tmpStr = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Money_Elixir);
        for (int i = 0; i < ElixirTextBoxs.Length; i++)
        {
            ElixirTextBoxs[i].text = tmpStr;
        }
    }

    public void DisplayEnchantStone()
    {
        tmpStr = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Money_EnchantStone);
        for (int i = 0; i < EnchantStoneTextBoxs.Length; i++)
        {
            EnchantStoneTextBoxs[i].text = tmpStr;
        }
    }

    public void DisplayAmazonStone()
    {
        tmpStr = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Money_AmazonCoin);
        for (int i = 0; i < AmazonStoneTextBoxs.Length; i++)
        {
            AmazonStoneTextBoxs[i].text = tmpStr;
        }
    }

    public void DisplayCostZogak()
    {
        tmpStr = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.AmazonStoneCount);
        for (int i = 0; i < CostZogakTextBoxs.Length; i++)
        {
            CostZogakTextBoxs[i].text = tmpStr;
        }
    }



    /// <summary>
    /// 올 머니에 붙어서 재화 다 올려줘
    /// </summary>
    public void AddGoldTest()
    {
        PlayerInventory.Money_Gold += 1.797679e+250;
        PlayerInventory.Money_Dia += 214748364;
        PlayerInventory.Money_Elixir += 214748364;
        PlayerInventory.Money_EnchantStone += 214748364;
        PlayerInventory.Money_Leaf += 214748364;
        PlayerInventory.Money_AmazonCoin += 214748364;
    }

    private TouchScreenKeyboard keyboard;
    private bool isAddMoneyTyping;
    private bool isSubMoneyTyping;
    //
    string inputGoldText = "100";
    /// 골드 증가 메서드 입력 받음.
    public void AddAllTest()
    {
        isAddMoneyTyping = true;
        keyboard = TouchScreenKeyboard.Open(inputGoldText, TouchScreenKeyboardType.NumberPad);
    }
    public void SubAllTest()
    {
        isSubMoneyTyping = true;
        keyboard = TouchScreenKeyboard.Open(inputGoldText, TouchScreenKeyboardType.NumberPad);
    }

    private void AddAllMoneyTest(string _amount)
    {
        if (_amount == "" || _amount == null) return;

        PlayerInventory.Money_Gold += double.Parse( _amount);
        ///  업적 카운트 올리기
        ListModel.Instance.ALLlist_Update(3, double.Parse(_amount));
    }

    private void SubAllMoneyTest(string _amount)
    {
        if (_amount == "" || _amount == null) return;

        PlayerInventory.Money_Gold -= double.Parse(_amount);
    }

    private void Update()
    {
        if (TouchScreenKeyboard.visible == false && keyboard != null)
        {
            /// 가상 키보드에서 확인 버튼을 눌렀을때
            if (keyboard.status == TouchScreenKeyboard.Status.Done)
            {
                inputGoldText = keyboard.text;
                if (isAddMoneyTyping)
                {
                    /// 실제 머니 증가 메소드
                    AddAllMoneyTest(inputGoldText);
                    isAddMoneyTyping = false;
                }
                else if (isSubMoneyTyping)
                {
                    SubAllMoneyTest(inputGoldText);
                    isSubMoneyTyping = false;
                }

                inputGoldText = "";
                keyboard = null;
            }
        }
    }


    /// TODO : 머니 디스플레이 하단에 글자 올라오는 애니메이션 +100 식으로


}
