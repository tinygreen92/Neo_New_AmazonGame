using System.Collections;
using System.Collections.Generic;
using System.Deployment.Internal;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public struct RankData{
    public string _nickname; 
    public string _score;
}
public class RankManager : MonoBehaviour
{
    public Text exRenkText;
    public NanooManager nanoo;
    [Header("-트로피 모음집")]
    public Sprite[] headIcons;
    [Header("- 1-4위 표기해줄 아이템 위치")]
    public Transform[] userRankBox;
    public Transform myRankBox;
    public Text PageText;
    [Header("- 이전 다음 버튼")]
    public Sprite[] BtnSpr;
    public Image[] beforeNextBtn;
    [Header("- 내 랭킹")]
    public Image personalImg;
    public Text[] personalText;

    public List<RankData> rankList;
    public string[] result = new string[3];


    /// <summary>
    /// 0. 랭킹 버튼 누르면 뱅뱅이 아이콘 돌리고 서버 접속요청
    /// 1. 4페이지 만 로딩되면 첫 화면 띄워줌
    /// 2. 다음 버튼 누르면 화면 전환.
    /// </summary>
    public void ClickedOnEnable()
    {
        /// 기록이 - 라면 예외처리
        if(PlayerInventory.RecentDistance > 1.0d)
        {
            nanoo.RecordRankDistance(Mathf.RoundToInt((float)PlayerInventory.RecentDistance)-1);
            /// 랭킹 기록 시 플레이팹에 상태 저장
            PlayerPrefsManager.instance.JObjectSave(true);
        }
        ///// 로딩 뺑글이
        //SystemPopUp.instance.LoopLoadingImg();
        /// 지연 호출
        Invoke(nameof(InvoRank), 1.0f);

    }

    void InvoRank()
    {
        for (int i = 0; i < userRankBox.Length; i++)
        {
            userRankBox[i].GetChild(0).gameObject.SetActive(false);
            userRankBox[i].GetChild(1).gameObject.SetActive(false);
            userRankBox[i].GetChild(2).gameObject.SetActive(false);
        }

        if (rankList == null) rankList = new List<RankData>();
        rankList.Clear();
        /// 나누 랭킹 호출
        nanoo.ShowRankDistance();
        _page = -1;
    }

    /// <summary>
    /// 통신이 끝난 후에 나누에서 호출해준다. 
    /// </summary>
    public void InitRankPage()
    {
        /// 다음, 이전 누르는 메소드 -> 처음에는 둘다 아니므로 초기화
        ClickedRankingPage(-1);
        /// 뺑뺑이 끝
        SystemPopUp.instance.StopLoopLoading();
        gameObject.SetActive(true);
    }


    private int _page = 0;
    /// <summary>
    /// 버튼 <다음> <이전> 에 붙여줘서 예외처리 해준다.
    /// </summary>
    public void ClickedRankingPage(int _dir)
    {
        if (_dir == -1) 
        {
            _page++;
            beforeNextBtn[0].sprite = BtnSpr[0];
            beforeNextBtn[1].sprite = BtnSpr[1];
            PageText.text =  "1 / 13";
            InitContentRank();
            return;
        }
        else if (_dir == 0)
        {
            if (_page > 0) _page--;
        }
        else if (_dir == 1)
        {
            if (_page < 12) _page++;
        }
        

        if(_page == 0)
        {
            beforeNextBtn[0].sprite = BtnSpr[0];
            beforeNextBtn[1].sprite = BtnSpr[1];
            PageText.text = "1 / 13";
        }
        else if (_page == 12)
        {
            beforeNextBtn[0].sprite = BtnSpr[1];
            beforeNextBtn[1].sprite = BtnSpr[0];
            PageText.text = "13 / 13";
        }
        else
        {
            beforeNextBtn[0].sprite = BtnSpr[1];
            beforeNextBtn[1].sprite = BtnSpr[1];
            PageText.text = (_page + 1) + " / 13";
        }

        InitContentRank();
    }
    /// <summary>
    /// 하단 인덱스에 맞춰서 불러오는 값 조절
    /// </summary>
    private void InitContentRank()
    {
        for (int i = 0; i < userRankBox.Length; i++)
        {
            userRankBox[i].GetChild(0).gameObject.SetActive(false);
            userRankBox[i].GetChild(1).gameObject.SetActive(false);
            userRankBox[i].GetChild(2).gameObject.SetActive(false);
        }
        int _index = 0;
        int count = _page * 4;
        for (int i = count; i < count + 4; i++)
        {
            if (rankList.Count <= i) break;        /// 데이터 업승면 포문 탈출

            userRankBox[_index].GetChild(0).GetChild(0).GetComponent<Image>().sprite = GetHeadIcon(i);
            userRankBox[_index].GetChild(0).GetChild(1).GetComponent<Text>().text = (i + 1).ToString();
            userRankBox[_index].GetChild(1).GetChild(0).GetComponent<Text>().text =  rankList[i]._nickname;
            userRankBox[_index].GetChild(2).GetChild(0).GetComponent<Text>().text =  (float.Parse(rankList[i]._score) * 0.1f).ToString("f1") + "km";

            userRankBox[_index].GetChild(0).gameObject.SetActive(true);
            userRankBox[_index].GetChild(1).gameObject.SetActive(true);
            userRankBox[_index].GetChild(2).gameObject.SetActive(true);

            _index++;
        }
    }
    private Sprite GetHeadIcon(int _index)
    {
        int result;
        if (_index > 3)
        {
            result = 3;
        }
        else
        {
            result = _index;
        }
        return headIcons[result];
    } 
    public void ShowPersonal()
    {
        personalImg.sprite = GetHeadIcon(int.Parse(result[0]) - 1);
        personalText[0].text = result[0];
        personalText[1].text = result[1];
        personalText[2].text = (float.Parse(result[2]) * 0.1f).ToString("f1") + "km";
        /// 외부 랭킹 표시
        exRenkText.text = "내 랭킹 : " + int.Parse(result[0]).ToString("N0") + "위";
    }



}
