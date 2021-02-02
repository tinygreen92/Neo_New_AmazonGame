using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public void UpdateItem(int count)
    {
        if (count == -100)
        {
            gameObject.SetActive(false);
        }
        else
        {
            name = string.Format("{0}", count);

            ///  박스 생성 될 때 서포트 뷰 소속 아이템 박스라면?
            switch (transform.parent.name)
            {
                case "Char_INFINI_Content":
                    GetComponent<CharactorItem>().BoxInfoUpdate(count);
                    break;

                case "Wea_INFINI_Content":
                    GetComponent<WeaponItem>().BoxInfoUpdate(count);
                    break;

                case "Heat_INFINI_Content":
                    if (ListModel.Instance.heartList.Count < 1)
                    {
                        GetComponent<HeartItem>().BoxInfoUpdate(0);
                    }
                    else
                    {
                        GetComponent<HeartItem>().BoxInfoUpdate(count);
                    }
                    break;

                case "Sup_INFINI_Content":
                    GetComponent<SupportItem>().BoxInfoUpdate(count);
                    /// 한번 싹 돌려주기
                    GetComponent<SupportItem>().sm.InitTimeLoad();
                    break;

                case "Rune_INFINI_Content":
                    GetComponent<RuntItem>().BoxInfoUpdate(count);
                    break;

                case "Pet_INFINI_Content":
                    GetComponent<PetItem>().BoxInfoUpdate(count);
                    break;

                case "SHOP_INFINI_Content":
                    GetComponent<ShopItemManager>().BoxInfoUpdate(count);
                    break;

                case "Auto_INFINI_Content":
                    GetComponent<AutoItem>().BoxInfoUpdate(count);
                    /// 한번 싹 돌려주기
                    GetComponent<AutoItem>().sm.InitTimeLoad();
                    break;

                default:
                    break;
            }

            gameObject.SetActive(true);
        }
    }


}

