using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RuneManager : MonoBehaviour
{
    public TopViewPannel TVP;
    [Header("- 아이콘")]
    public Sprite Etc_con;
    public Sprite[] B_con;
    public Sprite[] A_cons;
    public Sprite[] S_cons;
    public Sprite[] L_cons;
    public Sprite[] R_cons;
    [Header("- 다시 추가되는 룬 팝업")]
    public GameObject RuneDiaLeak;
    public GameObject[] InnerPopUP;      /// 0 = 첫 가차/ 1 = 퓨전 2=  ReFussionPop / 3 = Accept? Cancel? pop
    public GameObject RuneFullCover;
    public GameObject[] TipTouchObj;        /// 0 = TitleText / 1 = Tip Text / 2 = Btn_FullExit / 3 Btn_layout
    [Header("- 퓨전 창")]
    public Image[] reGetRuneImg;
    public Text[] reRankText;
    public Text[] reDescTexts1;
    public Text[] reDescTexts2;
    public Transform reBotImg;
    [Header("- 획득 창 / 조합 창")]
    public Image GetRuneImg;
    public Text RankText;
    public Text[] DescTexts;
    public Transform BotImg;
    [Header("- 중간 보기 / 조합 텍스트")]
    public Text middleBtnText;
    [Header("- 매니저들")]
    public NestedScrollManager NSM;
    [Header("- MiddleMoneyPanel 착용된 룬창 할머니")]
    public Transform IconPannel;
    [Header("- 4.룬 매니저 리스트")]
    /// <summary>
    /// 착용한 룬  이미지 저장 됨
    /// </summary>
    public List<Sprite> saveSprList = new List<Sprite>();
    /// <summary>
    /// 퓨전 할 이미지
    /// </summary>
    public List<Sprite> fussionSprList = new List<Sprite>();
    /// <summary>
    /// 퓨전할 요소
    /// </summary>
    public List<RuneInventory> fussionRuneList = new List<RuneInventory>();
    public List<RuneInventory> tmpRuneList = new List<RuneInventory>();
    public List<int> fusionIndex = new List<int>();
    [Header("- 확률 보정")]
    public int R_Count;
    public int L_Count;
    public int S_Count;


    [Header("- 4.RunePage")]
    public Transform[] IconImg;
    public Transform[] BackLayouts;
    public Transform[] innerPannels;

    private string GetRune_Desc_Head;
    private string GetRune_Desc_Tail;
    private string GetRune_Desc_UP;

    


    private void OnEnable()
    {
        if (!PlayerPrefsManager.isRuneFussionTab) middleBtnText.text = LeanLocalization.GetTranslationText("Category_Rune_DetailPage");
        else middleBtnText.text = LeanLocalization.GetTranslationText("Category_Rune_Fussion");
    }

    /// <summary>
    /// 룬 아이콘 눌러서 탈착
    /// 1. 인덱스는 0번 자리 부터 4번자리까지 (5개 공간)
    /// 2. 룬 이미지는 saveSprList에서 판단
    /// </summary>
    public void ClickedMidRuneIcon(int _index)
    {
        if (PlayerPrefsManager.isRuneFussionTab) /// ----------------------------- 조합 ---------------------------
        {
            int oriIndex = ListModel.Instance.runeList.IndexOf(fussionRuneList[_index]);
            /// <조합> isEquip 옵션 바꿔주기 "안착용중" 
            ListModel.Instance.Rune_ResetFromFussion(oriIndex, false);
            SetGatchaCount(ListModel.Instance.runeList[oriIndex].rank, false);
            /// <조합> 목록에 내부 리스트 제거
            fussionRuneList.RemoveAt(_index);
            fusionIndex.Remove(_index);
            /// <조합> 목록에 이미지 제거
            fussionSprList.RemoveAt(_index);
            /// <조합> 뷰에 남은 이미지 실제로 표기해주기
            ShowFusionIcon();
        }
        else                            /// ----------------------------- 보관함 --------------------------------
        {
            int oriIndex = ListModel.Instance.runeList.IndexOf(ListModel.Instance.equipRuneList[_index]);

            Debug.LogWarning("인덱스 : " + oriIndex);
            /// isEquip 옵션 바꿔주기 "안착용중" 
            ListModel.Instance.Rune_Equip(oriIndex, false);
            /// 장착 목록에 내부 리스트 제거
            ListModel.Instance.equipRuneList.RemoveAt(_index);
            /// 룬 제거시 스탯 새로 고침
            ListModel.Instance.SetEquipedRuneEffect();
            /// 장착 목록에 이미지 제거
            saveSprList.RemoveAt(_index);
            /// 미들 뷰에 남은 이미지 실제로 표기해주기
            ShowIconFive();
        }
        /// 글자 / 박스 색 새로고침
        TVP.SetRuneItemMid();
    }

    /// <summary>
    /// 보관함 항목일때 5개 아이콘에 이미지 표시해주기
    /// </summary>
    public void ShowIconFive()
    {
        /// 일단 이미지 싹지우기
        for (int i = 0; i < IconPannel.childCount; i++)
        {
            IconPannel.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
        /// 미들 뷰에 이미지 실제로 표기해주기
        for (int i = 0; i < saveSprList.Count; i++)
        {
            IconPannel.GetChild(i).GetChild(1).GetComponent<Image>().sprite = saveSprList[i];
            IconPannel.GetChild(i).GetChild(1).gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// <조합> 항목일때 5개 아이콘에 이미지 표시해주기
    /// </summary>
    public void ShowFusionIcon()
    {
        /// 조합창 비워주기
        for (int i = 0; i < IconPannel.childCount; i++)
        {
            IconPannel.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
        /// 미들 뷰에 이미지 실제로 표기해주기
        for (int i = 0; i < fussionSprList.Count; i++)
        {
            IconPannel.GetChild(i).GetChild(1).GetComponent<Image>().sprite = fussionSprList[i];
            IconPannel.GetChild(i).GetChild(1).gameObject.SetActive(true);
        }
    }






    bool isRareFussion;
    /// <summary>
    /// 확률 보정 카운트 초기화
    /// </summary>
    public void ResetGatchaCount()
    {
        /// 특수 조합일 경우에는 조합창 완전 벗어날때까지 퓨전 카운터 들고 있는다.
        if (isRareFussion) return;
        R_Count = 0;
        L_Count = 0;
        S_Count = 0;
    }
    /// <summary>
    /// 확률 보정
    /// </summary>
    /// <param name="_rank">LSR 세개 카운트</param>
    /// <param name="_isAdded">더할까? 뺄까?</param>
    public void SetGatchaCount(string _rank, bool _isAdded)
    {
        /// 카운트 더해주기
        if (_isAdded)
        {
            switch (_rank)
            {
                case "R": R_Count++; break;
                case "L": L_Count++; break;
                case "S": S_Count++; break;
                default:
                    break;
            }
        }
        else ///카운트 빼주기
        {
            switch (_rank)
            {
                case "R": R_Count--; break;
                case "L": L_Count--; break;
                case "S": S_Count--; break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 카운트를 바탕으로 적절한 확률 보정 알파벳 리턴
    /// </summary>
    /// <returns></returns>
    private string GetGatchaBojung()
    {
        string result = "";

        if (S_Count == 4)               /// LS4
        {
            if (L_Count == 1) result = "LS4";
        }
        else if (L_Count == 5)               /// L5
        {
            result = "L5";
        }
        else if (L_Count == 4)               /// RL4
        {
            if (R_Count == 1) result = "RL4";
        }
        else if (R_Count == 5)               /// R5
        {
            result = "R5";
        }

        /// 특수 조합 공식 발동
        if (result != "")
        {
             isRareFussion = true;
        }
        return result;
    }

    /// <summary>
    /// 보기 / 조합 버튼 누를때
    /// </summary>
    public void ClickedMidButton()
    {
        if (!PlayerPrefsManager.isRuneFussionTab) InitRunePage();
        else InitRuneFussion();
    }

    /// <summary>
    /// 룬 능력치 전부 보기 버튼
    /// </summary>
    void InitRunePage()
    {
        /// 싹 비워주기
        for (int i = 0; i < BackLayouts.Length; i++)
        {
            BackLayouts[i].gameObject.SetActive(false);
            /// 차일드 숨기기
            for (int j = 0; j < innerPannels[i].childCount; j++)
            {
                innerPannels[i].GetChild(j).gameObject.SetActive(false);
            }
        }

        /// 리스트 비어 있으면 텅빈거 팝업
        if (ListModel.Instance.equipRuneList.Count == 0)
        {
            PopUpManager.instance.ShowPopUP(4);
            return;
        }
        /// 리스트에 뭐라도 있다.
        for (int i = 0; i < ListModel.Instance.equipRuneList.Count; i++)
        {
            /// 아이콘 이미지 set
            IconImg[i].GetComponent<Image>().sprite = saveSprList[i];
            
            /// 개당 텍스트박스랑 이미지 세팅
            switch (ListModel.Instance.equipRuneList[i].rank)
            {
                case "B":
                    innerPannels[i].GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>().text =
                        ListModel.Instance.equipRuneList[i].desc_1;
                    innerPannels[i].GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text =
                        ListModel.Instance.equipRuneList[i].main_1.ToString("N1") + "%";

                    innerPannels[i].GetChild(3).gameObject.SetActive(true);
                    break;

                case "A":
                    innerPannels[i].GetChild(3).GetChild(0).GetChild(0)
                        .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].desc_1;
                    innerPannels[i].GetChild(3).GetChild(1).GetChild(0)
                        .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].main_1.ToString("N1") + "%";

                    innerPannels[i].GetChild(3).gameObject.SetActive(true);
                    break;

                case "S":
                    innerPannels[i].GetChild(0).GetChild(0).GetChild(0)
                        .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].desc_1;
                    innerPannels[i].GetChild(0).GetChild(1).GetChild(0)
                        .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].main_1.ToString("N1") + "%";
                    innerPannels[i].GetChild(2).GetChild(0).GetChild(0)
                         .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].desc_2;
                    innerPannels[i].GetChild(2).GetChild(1).GetChild(0)
                        .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].main_2.ToString("N1") + "%";

                    innerPannels[i].GetChild(0).gameObject.SetActive(true);
                    innerPannels[i].GetChild(2).gameObject.SetActive(true);
                    break;

                case "L":

                    innerPannels[i].GetChild(0).GetChild(0).GetChild(0)
                        .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].desc_1;
                    innerPannels[i].GetChild(0).GetChild(1).GetChild(0)
                        .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].main_1.ToString("N1") + "%";
                    innerPannels[i].GetChild(1).GetChild(0).GetChild(0)
                         .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].desc_2;
                    innerPannels[i].GetChild(1).GetChild(1).GetChild(0)
                        .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].main_2.ToString("N1") + "%";
                    innerPannels[i].GetChild(2).GetChild(0).GetChild(0)
                          .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].desc_3;
                    innerPannels[i].GetChild(2).GetChild(1).GetChild(0)
                        .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].sub_1.ToString("N1") + "%";

                    innerPannels[i].GetChild(0).gameObject.SetActive(true);
                    innerPannels[i].GetChild(1).gameObject.SetActive(true);
                    innerPannels[i].GetChild(2).gameObject.SetActive(true);
                    break;

                case "R":
                    innerPannels[i].GetChild(1).GetChild(0).GetChild(0)
                        .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].desc_1;
                    innerPannels[i].GetChild(1).GetChild(1).GetChild(0)
                        .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].main_1.ToString("N1") + "%";
                    innerPannels[i].GetChild(3).GetChild(0).GetChild(0)
                         .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].desc_2;
                    innerPannels[i].GetChild(3).GetChild(1).GetChild(0)
                        .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].main_2.ToString("N1") + "%";
                    innerPannels[i].GetChild(4).GetChild(0).GetChild(0)
                          .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].desc_3;
                    innerPannels[i].GetChild(4).GetChild(1).GetChild(0)
                        .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].sub_1.ToString("N1") + "%";
                    innerPannels[i].GetChild(5).GetChild(0).GetChild(0)
                         .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].desc_4;
                    innerPannels[i].GetChild(5).GetChild(1).GetChild(0)
                        .GetComponent<Text>().text = ListModel.Instance.equipRuneList[i].sub_2.ToString("N1") + "%";

                    innerPannels[i].GetChild(1).gameObject.SetActive(true);
                    innerPannels[i].GetChild(3).gameObject.SetActive(true);
                    innerPannels[i].GetChild(4).gameObject.SetActive(true);
                    innerPannels[i].GetChild(5).gameObject.SetActive(true);
                    break;

                default:
                    break;
            }

            /// 해당 항목 활성화
            BackLayouts[i].gameObject.SetActive(true);
        }
        ///팝업 호출
        PopUpManager.instance.ShowPopUP(4);
    }

    /// <summary>
    /// 조합 버튼 클릭시 조합 수행하는 메서드
    /// </summary>
    void InitRuneFussion()
    {
        isRefussion = false;
        _isLeft = false;
        tmpRuneList.Clear();
        /// 버튼 누를때 상단 공간에 5개가 다 모였나 체크
        if (fussionRuneList.Count != 5)
        {
            return;
        }
        /// 진짜 조합 하실?
        PopUpManager.instance.ShowGrobalPopUP(5);
    }
    /// <summary>
    /// 진짜 조합하실 확인 누르면 조합
    /// </summary>
    public void InvoBtnRuneFussion()
    {
        /// 룬페이지 커버 씌우고
        RuneFullCover.SetActive(true);
        /// 확률 보정 초기화. (이미 소모했으므로))
        fusionIndex.Clear();
        /// 위로 올리기 + 오브젝트 싹 지우기
        NSM.RefreshForRune();
        ///   확률 보정하고 뽑아1개 생성 -> list에 추가
        GenerateRadomRune();
        ///  업적  완료 카운트
        ListModel.Instance.ALLlist_Update(16, 1);
        ///  5개 다 모였다면5개 제거해주고
        ListModel.Instance.Rune_Fusion(fussionRuneList);
        /// 오브젝트 재생성
        NSM.GnerForRune();
        /// 보관함으로 되돌아가지마
        TVP.SwichPetRune(0);
        TVP.SwichPetRune(1);
        /// 팝업 표시
        ShowGatchaRunePop(false);
    }

    /// <summary>
    /// 아이템 가챠 하나 추가해주기 -> 버튼에서 호출
    /// </summary>
    public void GatchaRune()
    {
        isRefussion = false;
        /// 룬페이지 커버 씌우고
        RuneFullCover.SetActive(true);
        /// 룬 페이지 띄우고
        TVP.SwichPetRune(0);
        /// 확률에 따라 룬생성 / 룬조합
        GenerateRadomRune();
        /// 팝업 표시
        ShowGatchaRunePop(true);
        /// 인피니티 뷰에 추가 + 제일 위로 올라가
        NSM.RefreshForRune();
        /// 오브젝트 재생성
        NSM.GnerForRune();
        /// 보관함으로 되돌아가
        TVP.SwichPetRune(0);
        ///  업적  완료 카운트
        ListModel.Instance.ALLlist_Update(15, 1);
    }

    /// <summary>
    /// 첫 가챠시에는 false / 다이아 써서 재조합시 true
    /// </summary>
    bool isRefussion;
    string[] tmpDesc2 = new string[4];

    /// <summary>
    /// 가챠 룬 팝업창 보여주기
    /// </summary>
    public void ShowGatchaRunePop(bool _isFirst)
    {
        if (!_isFirst && isRefussion)
        {
            InnerPopUP[0].SetActive(false);
            InnerPopUP[1].SetActive(true);
        }
        else
        {
            InnerPopUP[0].SetActive(true);
            InnerPopUP[1].SetActive(false);
        }

        TipTouchObj[0].SetActive(_isFirst);
        TipTouchObj[1].SetActive(!_isFirst);
        TipTouchObj[2].SetActive(_isFirst);
        TipTouchObj[3].SetActive(!_isFirst);

        /// 상용구
        GetRune_Desc_Head = LeanLocalization.GetTranslationText("GetRune_Desc_Head");
        GetRune_Desc_Tail = LeanLocalization.GetTranslationText("GetRune_Desc_Tail");
        GetRune_Desc_UP = LeanLocalization.GetTranslationText("GetRune_Desc_UP");

        for (int i = 0; i < DescTexts.Length; i++)
        {
            DescTexts[i].gameObject.SetActive(false);
            reDescTexts1[i].gameObject.SetActive(false);
            if(!isRefussion) reDescTexts2[i].gameObject.SetActive(false);
        }
        /// 마지막 요소 가져옴
        var lastListObj = ListModel.Instance.runeList.Last();

        /// 이미지와 설명 세팅
        switch (lastListObj.rank)
        {
            case "B":
                GetRuneImg.sprite = int.Parse(lastListObj.imgIndex) < 10 ? B_con[int.Parse(lastListObj.imgIndex)-5] : Etc_con;
                if (!isRefussion) reGetRuneImg[1].sprite = GetRuneImg.sprite;
                else reGetRuneImg[0].sprite = GetRuneImg.sprite;

                RankText.text = lastListObj.rank + GetRune_Desc_Head + " "  + lastListObj.desc_1 + " " + GetRune_Desc_Tail;
                if (!isRefussion) reRankText[1].text = RankText.text;
                else reRankText[0].text = RankText.text;

                DescTexts[0].text = lastListObj.desc_1 + " " +  lastListObj.main_1.ToString("N1") + "% "+ GetRune_Desc_UP;
                
                if (isRefussion)
                {
                    reDescTexts1[0].text = DescTexts[0].text;
                    reDescTexts2[0].text = tmpDesc2[0];
                }
                else
                {
                    tmpDesc2[0] = DescTexts[0].text;
                    reDescTexts2[0].text = DescTexts[0].text;
                }


                /// 해당 라인 표시
                DescTexts[0].gameObject.SetActive(true);
                reDescTexts1[0].gameObject.SetActive(true);
                reDescTexts2[0].gameObject.SetActive(true);

                /// 봇 이미지 이동
                BotImg.SetSiblingIndex(2);
                /// 퓨전 봇 이미지 이동
                for (int i = 0; i < tmpDesc2.Length; i++)
                {
                    if(tmpDesc2[i] == "")
                    {
                        if (i + 1 > 2) reBotImg.SetSiblingIndex(i + 1);
                        else reBotImg.SetSiblingIndex(2);
                        break;
                    }
                }
                break;

            case "A":
                GetRuneImg.sprite = A_cons[int.Parse(lastListObj.imgIndex)];
                if (!isRefussion) reGetRuneImg[1].sprite = GetRuneImg.sprite;
                else reGetRuneImg[0].sprite = GetRuneImg.sprite;

                RankText.text = lastListObj.rank + GetRune_Desc_Head + " " + lastListObj.desc_1 + " " + GetRune_Desc_Tail;
                if (!isRefussion) reRankText[1].text = RankText.text;
                else reRankText[0].text = RankText.text;

                DescTexts[0].text = lastListObj.desc_1 +" " + lastListObj.main_1.ToString("N1") + "% " + GetRune_Desc_UP;
                
                if (isRefussion) reDescTexts1[0].text = DescTexts[0].text;
                else tmpDesc2[0] = DescTexts[0].text;
                reDescTexts2[0].text = tmpDesc2[0];

                /// 해당 라인 표시
                DescTexts[0].gameObject.SetActive(true);
                reDescTexts1[0].gameObject.SetActive(true);
                reDescTexts2[0].gameObject.SetActive(true);
                /// 봇 이미지 이동
                BotImg.SetSiblingIndex(2);
                /// 퓨전 봇 이미지 이동
                for (int i = 0; i < tmpDesc2.Length; i++)
                {
                    if (tmpDesc2[i] == "")
                    {
                        if (i + 1 > 2) reBotImg.SetSiblingIndex(i + 1);
                        else reBotImg.SetSiblingIndex(2);
                        break;
                    }
                }
                break;

            case "S":
                GetRuneImg.sprite = S_cons[int.Parse(lastListObj.imgIndex)];
                if (!isRefussion) reGetRuneImg[1].sprite = GetRuneImg.sprite;
                else reGetRuneImg[0].sprite = GetRuneImg.sprite;

                RankText.text = lastListObj.rank + GetRune_Desc_Head + " " + lastListObj.desc_1 + " " + GetRune_Desc_Tail;
                if (!isRefussion) reRankText[1].text = RankText.text;
                else reRankText[0].text = RankText.text;

                DescTexts[0].text = lastListObj.desc_1 +" "+ lastListObj.main_1.ToString("N1") + "% " + GetRune_Desc_UP;
                DescTexts[1].text = lastListObj.desc_2 +" "+ lastListObj.main_2.ToString("N1") + "% " + GetRune_Desc_UP;

                if (isRefussion)
                {
                    reDescTexts1[0].text = DescTexts[0].text;
                    reDescTexts1[1].text = DescTexts[1].text;
                    reDescTexts2[0].text = tmpDesc2[0];
                    reDescTexts2[1].text = tmpDesc2[1];
                }
                else
                {
                    tmpDesc2[0] = DescTexts[0].text;
                    tmpDesc2[1] = DescTexts[1].text;
                    reDescTexts2[0].text = DescTexts[0].text;
                    reDescTexts2[1].text = DescTexts[1].text;
                }

                /// 해당 라인 표시
                DescTexts[0].gameObject.SetActive(true);
                DescTexts[1].gameObject.SetActive(true);
                reDescTexts1[0].gameObject.SetActive(true);
                reDescTexts1[1].gameObject.SetActive(true);
                reDescTexts2[0].gameObject.SetActive(true);
                reDescTexts2[1].gameObject.SetActive(true);
                /// 봇 이미지 이동
                BotImg.SetSiblingIndex(3);
                /// 퓨전 봇 이미지 이동
                for (int i = 0; i < tmpDesc2.Length; i++)
                {
                    if (tmpDesc2[i] == "")
                    {
                        if (i + 1 > 3) reBotImg.SetSiblingIndex(i + 1);
                        else reBotImg.SetSiblingIndex(3);
                        break;
                    }
                }
                break;

            case "L":
                GetRuneImg.sprite = L_cons[int.Parse(lastListObj.imgIndex)];
                if (!isRefussion) reGetRuneImg[1].sprite = GetRuneImg.sprite;
                else reGetRuneImg[0].sprite = GetRuneImg.sprite;

                RankText.text = lastListObj.rank + GetRune_Desc_Head + " "  + lastListObj.desc_1 + " " + GetRune_Desc_Tail;
                if (!isRefussion) reRankText[1].text = RankText.text;
                else reRankText[0].text = RankText.text;

                DescTexts[0].text = lastListObj.desc_1 + " " + lastListObj.main_1.ToString("N1") + "% " + GetRune_Desc_UP;
                DescTexts[1].text = lastListObj.desc_2 +" "+ lastListObj.main_2.ToString("N1") + "% " + GetRune_Desc_UP;
                DescTexts[2].text = lastListObj.desc_3 +" "+ lastListObj.sub_1.ToString("N1") + "% " + GetRune_Desc_UP;

                if (isRefussion)
                {
                    reDescTexts1[0].text = DescTexts[0].text;
                    reDescTexts1[1].text = DescTexts[1].text;
                    reDescTexts1[2].text = DescTexts[2].text;
                    reDescTexts2[0].text = tmpDesc2[0];
                    reDescTexts2[1].text = tmpDesc2[1];
                    reDescTexts2[2].text = tmpDesc2[2];
                }
                else
                {
                    tmpDesc2[0] = DescTexts[0].text;
                    tmpDesc2[1] = DescTexts[1].text;
                    tmpDesc2[2] = DescTexts[2].text;
                    reDescTexts2[0].text = DescTexts[0].text;
                    reDescTexts2[1].text = DescTexts[1].text;
                    reDescTexts2[2].text = DescTexts[2].text;
                }
                /// 해당 라인 표시
                DescTexts[0].gameObject.SetActive(true);
                DescTexts[1].gameObject.SetActive(true);
                DescTexts[2].gameObject.SetActive(true);
                reDescTexts1[0].gameObject.SetActive(true);
                reDescTexts1[1].gameObject.SetActive(true);
                reDescTexts1[2].gameObject.SetActive(true);
                reDescTexts2[0].gameObject.SetActive(true);
                reDescTexts2[1].gameObject.SetActive(true);
                reDescTexts2[2].gameObject.SetActive(true);
                /// 봇 이미지 이동
                BotImg.SetSiblingIndex(4);
                /// 퓨전 봇 이미지 이동
                for (int i = 0; i < tmpDesc2.Length; i++)
                {
                    if (tmpDesc2[i] == "")
                    {
                        if (i + 1 > 4) reBotImg.SetSiblingIndex(i + 1);
                        else reBotImg.SetSiblingIndex(4);
                        break;
                    }
                }
                break;

            case "R":
                GetRuneImg.sprite = R_cons[int.Parse(lastListObj.imgIndex)];
                if (!isRefussion) reGetRuneImg[1].sprite = GetRuneImg.sprite;
                else reGetRuneImg[0].sprite = GetRuneImg.sprite;

                RankText.text = lastListObj.rank + GetRune_Desc_Head + " "  + lastListObj.desc_1 + " " + GetRune_Desc_Tail;
                if (!isRefussion) reRankText[1].text = RankText.text;
                else reRankText[0].text = RankText.text;

                DescTexts[0].text = lastListObj.desc_1 +" "+ lastListObj.main_1.ToString("N1") + "% " + GetRune_Desc_UP;
                DescTexts[1].text = lastListObj.desc_2 +" "+ lastListObj.main_2.ToString("N1") + "% " + GetRune_Desc_UP;
                DescTexts[2].text = lastListObj.desc_3 +" "+ lastListObj.sub_1.ToString("N1") + "% " + GetRune_Desc_UP;
                DescTexts[3].text = lastListObj.desc_4 +" "+ lastListObj.sub_2.ToString("N1") + "% " + GetRune_Desc_UP;

                if (isRefussion)
                {
                    reDescTexts1[0].text = DescTexts[0].text;
                    reDescTexts1[1].text = DescTexts[1].text;
                    reDescTexts1[2].text = DescTexts[2].text;
                    reDescTexts1[3].text = DescTexts[3].text;
                    reDescTexts2[0].text = tmpDesc2[0];
                    reDescTexts2[1].text = tmpDesc2[1];
                    reDescTexts2[2].text = tmpDesc2[2];
                    reDescTexts2[3].text = tmpDesc2[3];
                }
                else
                {
                    tmpDesc2[0] = DescTexts[0].text;
                    tmpDesc2[1] = DescTexts[1].text;
                    tmpDesc2[2] = DescTexts[2].text;
                    tmpDesc2[3] = DescTexts[3].text;
                    reDescTexts2[0].text = DescTexts[0].text;
                    reDescTexts2[1].text = DescTexts[1].text;
                    reDescTexts2[2].text = DescTexts[2].text;
                    reDescTexts2[3].text = DescTexts[3].text;
                }

                /// 해당 라인 표시
                DescTexts[0].gameObject.SetActive(true);
                DescTexts[1].gameObject.SetActive(true);
                DescTexts[2].gameObject.SetActive(true);
                DescTexts[3].gameObject.SetActive(true);
                reDescTexts1[0].gameObject.SetActive(true);
                reDescTexts1[1].gameObject.SetActive(true);
                reDescTexts1[2].gameObject.SetActive(true);
                reDescTexts1[3].gameObject.SetActive(true);
                reDescTexts2[0].gameObject.SetActive(true);
                reDescTexts2[1].gameObject.SetActive(true);
                reDescTexts2[2].gameObject.SetActive(true);
                reDescTexts2[3].gameObject.SetActive(true);
                /// 봇 이미지 이동
                BotImg.SetSiblingIndex(5);
                /// 퓨전 봇 이미지 이동
                for (int i = 0; i < tmpDesc2.Length; i++)
                {
                    if (tmpDesc2[i] == "")
                    {
                        if (i + 1 > 5) reBotImg.SetSiblingIndex(i + 1);
                        else reBotImg.SetSiblingIndex(5);
                        break;
                    }
                }
                break;

            default:
                break;
        }
        ///팝업 호출
        PopUpManager.instance.ShowPopUP(10);
    }

    bool _isLeft;
    public GameObject[] CheckImgs;
    public void ClickedCheckBtn(bool _isRight)
    {
        /// 왼쪽 체크
        if (!_isRight)
        {
            CheckImgs[0].SetActive(true);
            CheckImgs[1].SetActive(false);
        }
        /// 오른쪽 체크
        else
        {
            CheckImgs[0].SetActive(false);
            CheckImgs[1].SetActive(true);
        }
    }


    /// <summary>
    /// ShowPopUP(10) 팝업에서 확인 누르면 해당 룬 선택할래?
    /// </summary>
    public void AceptBtn()
    {
        InnerPopUP[3].SetActive(true);
    }

    bool isRuneSelect;
    public void AceptFussionBtn()
    {
        InnerPopUP[2].SetActive(true);
    }

    /// <summary>
    /// 해당 룬으로 선택하시겠습니까? 
    /// </summary>
    public void Invo_AceptFuBtn()
    {
        if (isRuneSelect)
        {
            /// 체크 버튼에 따라 죽이는거야
            if (CheckImgs[1].activeSelf)
            {
                ListModel.Instance.runeList.Remove(tmpRuneList.Last());
            }
            else
            {
                ListModel.Instance.runeList.Remove(tmpRuneList.First());
            }
            isRuneSelect = false;
        }

        /// 위로 올리기 + 오브젝트 싹 지우기
        NSM.RefreshForRune();
        /// 오브젝트 재생성
        NSM.GnerForRune();
        TVP.SwichPetRune(0);
        TVP.SwichPetRune(1);
        /// 초기화
        isRefussion = false;
        Invo_AceptBtn();
    }

    /// <summary>
    /// ~해당 룬으로 선택하시겠습니까? [확인] 누름
    /// </summary>
    public void Invo_AceptBtn()
    {
        isRareFussion = false;
        tmpRuneList.Clear();
        RuneFullCover.SetActive(false);
        PopUpManager.instance.HidePopUP(10);
        /// 임시 설명 스트링 비워주기
        for (int i = 0; i < tmpDesc2.Length; i++)
        {
            tmpDesc2[i] = "";
        }
        /// 왼쪽 체크로 옮겨주기
        CheckImgs[0].SetActive(true);
        CheckImgs[1].SetActive(false);
    }

    /// <summary>
    /// 재조합 버튼 누른다?
    /// </summary>
    public void NextBtn()
    {
        RuneFullCover.SetActive(true);
        InnerPopUP[2].SetActive(true);
    }

    /// <summary>
    /// 다이아 100개 소모하고 리얼 재조합
    /// </summary>
    public void Invo_NextBtn()
    {
        if (PlayerInventory.Money_Dia < 100)
        {
            RuneDiaLeak.SetActive(true);
            return;
        }
        PlayerInventory.Money_Dia -= 100;
        InnerPopUP[2].SetActive(false);
        /// 재조합 기회 얻음
        isRuneSelect = true;
        /// 재조합 2회째부터 최근 요소 하나 삭제.
        if (isRefussion) ListModel.Instance.runeList.Remove(tmpRuneList.Last());
        /// 스위치 온!!
        isRefussion = true;
        /// 위로 올리기 + 오브젝트 싹 지우기
        NSM.RefreshForRune();
        ///   확률 보정하고 뽑아1개 생성 -> list에 추가
        GenerateRadomRune();
        ///  업적  완료 카운트
        ListModel.Instance.ALLlist_Update(16, 1);
        /// 오브젝트 재생성
        NSM.GnerForRune();
        TVP.SwichPetRune(0);
        TVP.SwichPetRune(1);

        /// 팝업 표시
        ShowGatchaRunePop(false);
    }

    /// <summary>
    /// 갑자기 꺼질때 룬 복사 방지
    /// </summary>
    private void OnApplicationQuit()
    {
        if(isRuneSelect) ListModel.Instance.runeList.Remove(tmpRuneList.First());
    }

    private void OnApplicationPause(bool pause)
    {
        if (!PlayerPrefsManager.isLoadingComp) return;
        /// 일시 정지 상태 진입
        if (pause)
        {
            if (isRuneSelect) ListModel.Instance.runeList.Remove(tmpRuneList.First());
        }
    }

    /// <summary>
    /// 룬 등급 정해주고 1개 생성
    /// </summary>
    void GenerateRadomRune()
    {
        float temp = Time.time * 525f;
        Random.InitState((int)temp);
        float random = Random.Range(0, 100f);

        string _god = GetGatchaBojung();
        /// 확률 보정
        switch (_god)
        {
            case "LS4":
                if (random < 5f) random = 95f;
                else random = 89;
                break;

            case "L5":
                if (random < 95f) random = 95f;
                else random = 100f;
                break;

            case "RL4":
                if (random < 10f) random = 100f;
                else random = 95f;
                break;

            case "R5":
                random = 100f;
                break;

            default:
                random = Random.Range(0, 100f);
                break;
        }

        if (random < 40f)                      /// 40%
        {
            int iRand = Random.Range(5, ListModel.Instance.invisibleruneList.Count);                /// 서브 옵션
            ListModel.Instance.Rune_Unlock("B", iRand);
        }
        else if (random < 70f)             /// 40% +30%
        {
            int iRand = Random.Range(0, 5);                                                                                                  /// 메인옵션
            ListModel.Instance.Rune_Unlock("A", iRand);
        }
        else if (random < 90f)             /// 40% +30% +20%
        {
            int iRand1 = Random.Range(0, 5);                                                                                                  /// 메인옵션
            int iRand2 = Random.Range(0, 5);                                                                                                  /// 메인옵션
            ListModel.Instance.Rune_Unlock("S", iRand1, iRand2);
        }
        else if (random < 99f)             /// 40% +30% +20% + 9%
        {
            int iRand1 = Random.Range(0, 5);                                                                                                  /// 메인옵션
            int iRand2 = Random.Range(0, 5);                                                                                                  /// 메인옵션
            int iRand3 = Random.Range(5, ListModel.Instance.invisibleruneList.Count);                /// 서브 옵션
            ListModel.Instance.Rune_Unlock("L", iRand1, iRand2, iRand3);
        }
        else                                               /// 40% +30% +20% + 9% +1%
        {
            int iRand1 = Random.Range(0, 5);                                                                                                  /// 메인옵션
            int iRand2 = Random.Range(0, 5);                                                                                                  /// 메인옵션
            int iRand3 = Random.Range(5, ListModel.Instance.invisibleruneList.Count);                /// 서브 옵션
            int iRand4 = Random.Range(5, ListModel.Instance.invisibleruneList.Count);                /// 서브 옵션
            ListModel.Instance.Rune_Unlock("R", iRand1, iRand2, iRand3, iRand4);
        }

        /// 임시 리스트에 저장
        tmpRuneList.Add(ListModel.Instance.runeList.Last());

        /// 플레이팹에 상태 저장
        PlayerPrefsManager.instance.JObjectSave(true);
    }

    ///// <summary>
    ///// 비공식 퓨전일때만 룬 이렇게 생성
    ///// </summary>
    //void FussionGenerate(string _index)
    //{
    //    switch (_index)
    //    {
    //        case "B":
    //            int iRand = Random.Range(5, ListModel.Instance.invisibleruneList.Count);                /// 서브 옵션
    //            ListModel.Instance.Rune_Unlock("B", iRand);
    //            break;
    //        case "A":
    //            int iRand0 = Random.Range(0, 5);                                                                                                  /// 메인옵션
    //            ListModel.Instance.Rune_Unlock("A", iRand0);
    //            break;
    //        case "S":
    //            int iRand1 = Random.Range(0, 5);                                                                                                  /// 메인옵션
    //            int iRand2 = Random.Range(0, 5);                                                                                                  /// 메인옵션
    //            ListModel.Instance.Rune_Unlock("S", iRand1, iRand2);
    //            break;
    //        case "L":
    //            int iRand11 = Random.Range(0, 5);                                                                                                  /// 메인옵션
    //            int iRand22 = Random.Range(0, 5);                                                                                                  /// 메인옵션
    //            int iRand33 = Random.Range(5, ListModel.Instance.invisibleruneList.Count);                /// 서브 옵션
    //            ListModel.Instance.Rune_Unlock("L", iRand11, iRand22, iRand33);
    //            break;
    //        case "R":
    //            int iiRand1 = Random.Range(0, 5);                                                                                                  /// 메인옵션
    //            int iiRand2 = Random.Range(0, 5);                                                                                                  /// 메인옵션
    //            int iiRand3 = Random.Range(5, ListModel.Instance.invisibleruneList.Count);                /// 서브 옵션
    //            int iiRand4 = Random.Range(5, ListModel.Instance.invisibleruneList.Count);                /// 서브 옵션
    //            ListModel.Instance.Rune_Unlock("R", iiRand1, iiRand2, iiRand3, iiRand4);
    //            break;
    //    }
    //    /// 임시 리스트에 저장
    //    tmpRuneList.Add(ListModel.Instance.runeList.Last());
    //}


    /// <summary>
    /// 게임 재시작 시 착용한 룬 아이템 이미지 고대로 보여주기
    /// </summary>
    public void InitShowIconFive()
    {
        /// 미들 뷰에 이미지 실제로 표기해주기
        for (int i = 0; i < ListModel.Instance.equipRuneList.Count; i++)
        {
            switch (ListModel.Instance.equipRuneList[i].rank)
            {
                case "B":
                    saveSprList.Add(int.Parse(ListModel.Instance.equipRuneList[i].imgIndex) < 10 ?
                        B_con[int.Parse(ListModel.Instance.equipRuneList[i].imgIndex)-5] : Etc_con);
                    
                    break;

                case "A":
                    saveSprList.Add(A_cons[int.Parse(ListModel.Instance.equipRuneList[i].imgIndex)]);
                    break;

                case "S":
                    saveSprList.Add(S_cons[int.Parse(ListModel.Instance.equipRuneList[i].imgIndex)]);
                    break;

                case "L":
                    saveSprList.Add(L_cons[int.Parse(ListModel.Instance.equipRuneList[i].imgIndex)]);
                    break;

                case "R":
                    saveSprList.Add(R_cons[int.Parse(ListModel.Instance.equipRuneList[i].imgIndex)]);
                    break;

                default:
                    break;
            }
            IconPannel.GetChild(i).GetChild(1).GetComponent<Image>().sprite = saveSprList[i];
            IconPannel.GetChild(i).GetChild(1).gameObject.SetActive(true);
        }
    }


}
