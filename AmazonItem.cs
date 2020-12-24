using UnityEngine;
using UnityEngine.UI;

public class AmazonItem : MonoBehaviour
{
    public AmazonShopManager asm;

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
        if (PlayerInventory.Money_AmazonCoin < _Cost) return;
        PlayerInventory.Money_AmazonCoin -= _Cost;
        /// 아마존 상점에서 룬 구매하기
        if (_index == 6 && PlayerPrefsManager.currentTutoIndex == 32) ListModel.Instance.TUTO_Update(32);
        /// 팝업 호출 + 우편함으로 보내기
        asm.SetGiftBoxDesc(_index, 1);
    }



}
