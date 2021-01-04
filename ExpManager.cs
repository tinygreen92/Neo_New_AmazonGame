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
    public void UpdateExpGage()
    {
        /// 외부 표기 레벨
        float calv = PlayerInventory.CurrentAmaLV;

        infill.fillAmount = PlayerInventory.CurrentAmaValue / ((calv+1f) * 100f * (float)PlayerInventory.AmazonPoint_Cost);
        //expParent.GetChild(1).GetComponent<Text>().text = LEVEL_TEXT + calv.ToString("F0");
        lvText.text = "Lv. " + calv.ToString("N0");
        lvfillText.text = LEVEL_TEXT + calv.ToString("N0");
        /// 결정수 텍스트 증가
        infillText.text = "결정조각 : ( " + PlayerInventory.CurrentAmaValue.ToString("N0") + " / " + ((calv + 1f) * 100f * (float)PlayerInventory.AmazonPoint_Cost).ToString("N0") + " )";
        /// TODO :  Eng plz infillText.text = "결정조각 ( " + PlayerInventory.CurrentAmaValue.ToString("N0") + " / " + ((calv + 1f) * 100f * (float)PlayerInventory.AmazonPoint_Cost).ToString("N0") + " )";
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

