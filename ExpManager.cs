using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    public Transform expParent;
    [Header("- 외부캐릭터 정보창")]
    public Image infill;
    public Text lvText;
    public Text lvfillText;
    public Text infillText;


    private static string LEVEL_TEXT = "Amazon Lv. ";


    public static ExpManager instance;
    private void Awake()
    {
        instance = this;
    }


    /// <summary>
    /// 아마존 결정 포인트 경험치 움직일거
    /// </summary>
    /// <param name="tmp">아마존결정 누적 획득량</param>
    public void UpdateExpGage(float maxGage)
    {
        /// 게이지 갱신
        infill.fillAmount = (PlayerInventory.AmazonStoneCount *1f) / maxGage;
        infillText.text = "EXP : ( " + PlayerInventory.AmazonStoneCount + " / " + maxGage.ToString("N0") + " )";
        /// 레벨 텍스트 갱신
        lvText.text = "Lv. " + PlayerInventory.CurrentAmaLV;
        lvfillText.text = LEVEL_TEXT + PlayerInventory.CurrentAmaLV;
    }








    /// <summary>
    /// 테스트 버튼에 달라붙음
    /// </summary>
    public void TEST_CLICKED_AMA(int _amont)
    {
        //PlayerInventory.AmazonStoneCount += 20;
        ///// 결정조각  업적  카운트
        //ListModel.Instance.ALLlist_Update(2, 20);
        /// 아마존 포션 추가
        PlayerInventory.SetTicketCount("S_leaf_box", _amont);
    }
}

