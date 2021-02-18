using UnityEngine;
using UnityEngine.UI;

public class AmazonItem : MonoBehaviour
{
    [Header("- 아이템 구매시 우편으로 들어가")]
    public AmazonShopManager asm;
    [Header("- 아마존 상점 설명들")]
    public Image iconImg;
    public Image BtnImg;
    public Text headText;
    public Text descText;
    public Text priceText;

    private int _index;
    private int _Cost;

    private void OnEnable()
    {
        _index = int.Parse(name);
        ThisItemUpdate();
    }

    private void Update()
    {
        if (PlayerInventory.Money_AmazonCoin < _Cost) BtnImg.sprite = asm.BtnsSprs[0];
        else if(BtnImg.sprite == asm.BtnsSprs[0]) BtnImg.sprite = asm.BtnsSprs[1];
    }



    public void ThisItemUpdate()
    {
        iconImg.sprite = asm.Icons[_index];
        headText.text = ListModel.Instance.shopListAMA[_index].korDesc;
        descText.text = ListModel.Instance.shopListAMA[_index].korTailDesc;
        /// 가격
        _Cost = int.Parse(ListModel.Instance.shopListAMA[_index].korPrice);
        priceText.text = _Cost.ToString("N0");
    }


    /// <summary>
    /// 외부에서 이거 클릭
    /// </summary>
    public void ClickedThisItem()
    {
        /// 돈 없으면 버튼 클릭 X
        if (PlayerInventory.Money_AmazonCoin < _Cost) 
            return;
        /// 팝업 호출 
        asm.cbm.ShowPopUp(_index, _Cost, ShopType.AmazonShop);
        /// 우편함으로 보내기
        //asm.SetGiftBoxDesc(_index, 1);
    }



}
