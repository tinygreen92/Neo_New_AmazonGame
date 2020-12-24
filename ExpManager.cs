using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    public Transform expParent;



    private static string LEVEL_TEXT = "Lv. ";


    public static ExpManager instance;
    private void Awake()
    {
        instance = this;
    }


    /// <summary>
    /// 아마존 결정 포인트 경험치 움직일거
    /// </summary>
    /// <param name="tmp">아마존결정 누적 획득량</param>
    public void UpdateExpGage()
    {
        /// 외부 표기 레벨
        float calv = PlayerInventory.CurrentAmaLV;

        expParent.GetChild(0).GetComponent<Image>().fillAmount = PlayerInventory.CurrentAmaValue / ((calv+1f) * 100f * (float)PlayerInventory.AmazonPoint_Cost);
        expParent.GetChild(1).GetComponent<Text>().text = LEVEL_TEXT + calv.ToString("F0");
    }
    /// <summary>
    /// 테스트 버튼에 달라붙음
    /// </summary>
    public void TEST_CLICKED_AMA()
    {
        PlayerInventory.AmazonStoneCount += 20;
        /// 결정조각  업적  카운트
        ListModel.Instance.ALLlist_Update(2, 20);
    }
}

