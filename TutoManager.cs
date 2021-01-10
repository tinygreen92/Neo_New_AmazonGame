using CodeStage.AntiCheat.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoManager : MonoBehaviour
{
    public SupplyBoxManager sbm;
    public IAReviewManager iar;
    public GameObject tutoView;
    public GameObject InnerYello;
    public Text tutoText;


    public Text TESTSTRING;
    public void TEST_Step_Skip()
    {
        ListModel.Instance.TUTO_Complite(PlayerPrefsManager.currentTutoIndex++);
        TESTSTRING.text = "튜토리얼 단계 : " + PlayerPrefsManager.currentTutoIndex;
        InitTutorial();
    }

    public void TEST_MissionSkip()
    {
        for (int i = 0; i < ListModel.Instance.missionTUTOlist.Count; i++)
        {
            ListModel.Instance.TUTO_Complite(i);
        }
        ///
        InitTutorial();
    }

    /// <summary>
    /// 모델 핸들러 초기화 되면 FakeLoading 에서 호출해줌
    /// </summary>
    public void InitTutorial()
    {
        for (int i = 0; i < ListModel.Instance.missionTUTOlist.Count; i++)
        {
            /// -1 이면 클리어한 내용
            if (ListModel.Instance.missionTUTOlist[i].curentValue != "-1")
            {
                /// 어디까지 했어 카운터 올려줌
                PlayerPrefsManager.currentTutoIndex = i;
                /// 진행사항 텍스트 표기
                SetTutoText(i);
                /// 완료 된상태라면?
                if (int.Parse(ListModel.Instance.missionTUTOlist[i].curentValue) >= int.Parse(ListModel.Instance.missionTUTOlist[i].maxValue))
                {
                    CompliteThisTuto();
                }
                return;
            }
        }

        /// 포문 다 돌면 튜토리얼 다 깬거임.
        PlayerPrefsManager.isTutoAllClear = true;
        tutoView.SetActive(false);
        /// 서버에 튜토 클리어 저장
        if (ObscuredPrefs.GetInt("isTutoAllClear", 0) == 0)
        {
            ObscuredPrefs.SetInt("isTutoAllClear", 525);
            /// 스트링[] 몽땅 저장
            PlayerPrefsManager.instance.TEST_SaveJson();
            // 리뷰 해줘
            iar.ReviewReqStart();
        }
    }

    /// <summary>
    /// 버튼에 표기할 텍스트 세팅
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public  void  SetTutoText(int _index)
    {
        /// 거리 증가 미션이면 true;
        bool isDistanceMission = false;
        /// TODO : 해당 시점에서 초과 레벨인 경우 즉시 해금
        switch (_index)
        {
            
            case 3 :            // 새 깃털 수집 Lv.5 달성
                ListModel.Instance.TUTO_BeforeComp(_index, int.Parse(ListModel.Instance.supList[0].supporterLevel));
                break;
            case 5:            // 새 깃털 수집 Lv.20 달성
                ListModel.Instance.TUTO_BeforeComp(_index, int.Parse(ListModel.Instance.supList[0].supporterLevel));
                break;
            case 11:            // 캐릭터 Lv. 5 달성
                ListModel.Instance.TUTO_BeforeComp(_index, int.Parse(ListModel.Instance.charatorList[0].charLevel));
                break;

            case 19:            // 하이에나 두개골 수집  달성
                ListModel.Instance.TUTO_BeforeComp(_index, int.Parse(ListModel.Instance.supList[1].supporterLevel));
                break;
            case 20:            // 유물 MAX 시 예외처리
                if (ListModel.Instance.heartList.Count >= 30)
                {
                    ListModel.Instance.TUTO_BeforeComp(_index, 1);
                }
                break;
            case 28:            // 새 부리 수집 Lv. 10 달성
                ListModel.Instance.TUTO_BeforeComp(_index, int.Parse(ListModel.Instance.supList[2].supporterLevel));
                break;
            case 41:            // 캐릭터 Lv. 10 달성
                ListModel.Instance.TUTO_BeforeComp(_index, int.Parse(ListModel.Instance.charatorList[0].charLevel));
                break;
            case 42:            // 곰 발톱 수집 Lv. 10 달성
                ListModel.Instance.TUTO_BeforeComp(_index, int.Parse(ListModel.Instance.supList[3].supporterLevel));
                break;
            case 47:            // 사마귀 다리 수집 Lv.10 달성
                ListModel.Instance.TUTO_BeforeComp(_index, int.Parse(ListModel.Instance.supList[4].supporterLevel));
                break;
            case 48:            // 아무 무기 Lv. 10 달성하기
                ListModel.Instance.TUTO_BeforeComp(_index, PlayerInventory.MAX_WEAPON_LV);
                break;


            case 18:            // 5km 돌파하기 -> 50
                ListModel.Instance.TUTO_BeforeComp(_index, Mathf.RoundToInt((float)PlayerInventory.RecentDistance));
                isDistanceMission = true;
                break;
            case 36:            // 10Km 돌파하기 -> 100
                ListModel.Instance.TUTO_BeforeComp(_index, Mathf.RoundToInt((float)PlayerInventory.RecentDistance));
                isDistanceMission = true;
                break;
            case 40:            // 10Km 돌파하기 -> 100
                ListModel.Instance.TUTO_BeforeComp(_index, Mathf.RoundToInt((float)PlayerInventory.RecentDistance));
                isDistanceMission = true;
                break;


            default:
                break;
        }
        var ttlist = ListModel.Instance.missionTUTOlist[_index];
        /// 텍스트 세팅
        if (isDistanceMission)
        {
            tutoText.text = ttlist.korDesc + " ( " + ttlist.curentValue + " / " + (int.Parse(ttlist.maxValue)-1).ToString() + " )";
        }
        else
        {
            tutoText.text = ttlist.korDesc + " ( " + ttlist.curentValue + " / " + ttlist.maxValue + " )";
        }
    }


    /// <summary>
    /// current = MAX 일때 퀘스트 완료 표시
    /// </summary>
    public void CompliteThisTuto()
    {
        InnerYello.SetActive(true);
    }


    /// <summary>
    /// HP 바 밑에 튜토리얼 버튼 클릭하면 작동
    /// </summary>
    public void ClickedTutoBtn()
    {
        /// 완료 버튼 노란색 활성화 아니면 리턴
        if (!InnerYello.activeSelf) return;
        InnerYello.SetActive(false);
        /// -1 로 만들어주기 -> 완료된 튜토리얼 마일 스톤
        ListModel.Instance.TUTO_Complite(PlayerPrefsManager.currentTutoIndex);
        /// 보상지급
        switch (PlayerPrefsManager.currentTutoIndex)
        {
            case 0: PlayerInventory.Money_Dia += int.Parse(ListModel.Instance.missionTUTOlist[0].rewordAmount);
                PopUpManager.instance.ShowGetPop(1, ListModel.Instance.missionTUTOlist[0].rewordAmount);
                break;
            case 1: PlayerInventory.Money_Leaf += int.Parse(ListModel.Instance.missionTUTOlist[1].rewordAmount);
                PopUpManager.instance.ShowGetPop(2, ListModel.Instance.missionTUTOlist[1].rewordAmount);
                break;
            case 2: PlayerInventory.Money_Elixir += int.Parse(ListModel.Instance.missionTUTOlist[2].rewordAmount);
                PopUpManager.instance.ShowGetPop(3, ListModel.Instance.missionTUTOlist[2].rewordAmount);
                break;
            case 3: PlayerInventory.Money_EnchantStone += int.Parse(ListModel.Instance.missionTUTOlist[3].rewordAmount); 
                PopUpManager.instance.ShowGetPop(4, ListModel.Instance.missionTUTOlist[3].rewordAmount);
                break;
            case 4: PlayerInventory.Money_AmazonCoin += long.Parse(ListModel.Instance.missionTUTOlist[4].rewordAmount);
                PopUpManager.instance.ShowGetPop(5, ListModel.Instance.missionTUTOlist[4].rewordAmount);
                break;
            case 5: PlayerInventory.SetBoxsCount("weapon_coupon", int.Parse(ListModel.Instance.missionTUTOlist[5].rewordAmount)); 
                PopUpManager.instance.ShowGetPop(6, ListModel.Instance.missionTUTOlist[5].rewordAmount);
                break;
            case 6: PlayerInventory.Money_Dia += int.Parse(ListModel.Instance.missionTUTOlist[6].rewordAmount); 
                PopUpManager.instance.ShowGetPop(1, ListModel.Instance.missionTUTOlist[6].rewordAmount);
                break;
            case 7: PlayerInventory.Money_Leaf += int.Parse(ListModel.Instance.missionTUTOlist[7].rewordAmount);
                PopUpManager.instance.ShowGetPop(2, ListModel.Instance.missionTUTOlist[7].rewordAmount);
                break;
            case 8: PlayerInventory.Money_EnchantStone += int.Parse(ListModel.Instance.missionTUTOlist[8].rewordAmount);
                PopUpManager.instance.ShowGetPop(4, ListModel.Instance.missionTUTOlist[8].rewordAmount);
                break;
            case 9: PlayerInventory.Money_Elixir += int.Parse(ListModel.Instance.missionTUTOlist[9].rewordAmount); 
                PopUpManager.instance.ShowGetPop(3, ListModel.Instance.missionTUTOlist[9].rewordAmount);
                break;
            case 10: PlayerInventory.Money_Leaf += int.Parse(ListModel.Instance.missionTUTOlist[10].rewordAmount);
                PopUpManager.instance.ShowGetPop(2, ListModel.Instance.missionTUTOlist[10].rewordAmount);
                break;
            case 11: PlayerInventory.Money_AmazonCoin += long.Parse(ListModel.Instance.missionTUTOlist[11].rewordAmount);
                PopUpManager.instance.ShowGetPop(5, ListModel.Instance.missionTUTOlist[11].rewordAmount);
                break;
            case 12: PlayerInventory.Money_Dia += int.Parse(ListModel.Instance.missionTUTOlist[12].rewordAmount);
                PopUpManager.instance.ShowGetPop(1, ListModel.Instance.missionTUTOlist[12].rewordAmount);
                break;
            case 13: PlayerInventory.SetTicketCount("cave_enter", int.Parse(ListModel.Instance.missionTUTOlist[13].rewordAmount)); 
                PopUpManager.instance.ShowGetPop(7, ListModel.Instance.missionTUTOlist[13].rewordAmount);
                break;
            case 14: PlayerInventory.Money_Leaf += int.Parse(ListModel.Instance.missionTUTOlist[14].rewordAmount);
                PopUpManager.instance.ShowGetPop(2, ListModel.Instance.missionTUTOlist[14].rewordAmount);
                break;
            case 15: PlayerInventory.SetBoxsCount("weapon_coupon", int.Parse(ListModel.Instance.missionTUTOlist[15].rewordAmount)); 
                PopUpManager.instance.ShowGetPop(6, ListModel.Instance.missionTUTOlist[15].rewordAmount);
                break;
            case 16: PlayerInventory.Money_EnchantStone += int.Parse(ListModel.Instance.missionTUTOlist[16].rewordAmount);
                PopUpManager.instance.ShowGetPop(4, ListModel.Instance.missionTUTOlist[16].rewordAmount);
                break;
            case 17: PlayerInventory.Money_Leaf += int.Parse(ListModel.Instance.missionTUTOlist[17].rewordAmount);
                PopUpManager.instance.ShowGetPop(2, ListModel.Instance.missionTUTOlist[17].rewordAmount);
                break;
            case 18: PlayerInventory.Money_AmazonCoin += int.Parse(ListModel.Instance.missionTUTOlist[18].rewordAmount);
                PopUpManager.instance.ShowGetPop(5, ListModel.Instance.missionTUTOlist[18].rewordAmount);
                break;
            case 19: PlayerInventory.Money_Dia += int.Parse(ListModel.Instance.missionTUTOlist[19].rewordAmount); 
                PopUpManager.instance.ShowGetPop(1, ListModel.Instance.missionTUTOlist[19].rewordAmount);
                break;
            case 20: PlayerInventory.Money_Leaf += int.Parse(ListModel.Instance.missionTUTOlist[20].rewordAmount);
                PopUpManager.instance.ShowGetPop(2, ListModel.Instance.missionTUTOlist[20].rewordAmount);
                break;
            case 21: PlayerInventory.Money_Elixir += int.Parse(ListModel.Instance.missionTUTOlist[21].rewordAmount); 
                PopUpManager.instance.ShowGetPop(3, ListModel.Instance.missionTUTOlist[21].rewordAmount);
                break;
            case 22: PlayerInventory.Money_EnchantStone += int.Parse(ListModel.Instance.missionTUTOlist[22].rewordAmount);
                PopUpManager.instance.ShowGetPop(4, ListModel.Instance.missionTUTOlist[22].rewordAmount);
                break;
            case 23: PlayerInventory.SetTicketCount("cave_enter", int.Parse(ListModel.Instance.missionTUTOlist[23].rewordAmount));
                PopUpManager.instance.ShowGetPop(7, ListModel.Instance.missionTUTOlist[23].rewordAmount);
                break;
            case 24: PlayerInventory.Money_Leaf += int.Parse(ListModel.Instance.missionTUTOlist[24].rewordAmount);
                PopUpManager.instance.ShowGetPop(2, ListModel.Instance.missionTUTOlist[24].rewordAmount);
                /// 다음 미션은 보급품 1회 획득이니까 임시로 5초뒤 내려줌
                sbm.MissionSupplySex();
                break;
            case 25: PlayerInventory.SetTicketCount("cave_clear", int.Parse(ListModel.Instance.missionTUTOlist[25].rewordAmount));
                PopUpManager.instance.ShowGetPop(8, ListModel.Instance.missionTUTOlist[25].rewordAmount);
                break;
            case 26: PlayerInventory.Money_AmazonCoin += int.Parse(ListModel.Instance.missionTUTOlist[26].rewordAmount);
                PopUpManager.instance.ShowGetPop(5, ListModel.Instance.missionTUTOlist[26].rewordAmount);
                /// 27번째 미션은 황금 상자 죽이기 - 임시로 골드상자 확률 올려줌.
                PlayerPrefsManager.isGoldenDouble = true;
                break;
            case 27: PlayerInventory.Money_Dia += int.Parse(ListModel.Instance.missionTUTOlist[27].rewordAmount); 
                PopUpManager.instance.ShowGetPop(1, ListModel.Instance.missionTUTOlist[27].rewordAmount);
                break;
            case 28: PlayerInventory.SetBoxsCount("weapon_coupon", int.Parse(ListModel.Instance.missionTUTOlist[28].rewordAmount)); 
                PopUpManager.instance.ShowGetPop(6, ListModel.Instance.missionTUTOlist[28].rewordAmount);
                break;
            case 29: PlayerInventory.Money_Leaf += int.Parse(ListModel.Instance.missionTUTOlist[29].rewordAmount);
                PopUpManager.instance.ShowGetPop(2, ListModel.Instance.missionTUTOlist[29].rewordAmount);
                break;
            case 30: PlayerInventory.Money_EnchantStone += int.Parse(ListModel.Instance.missionTUTOlist[30].rewordAmount);
                PopUpManager.instance.ShowGetPop(4, ListModel.Instance.missionTUTOlist[30].rewordAmount);
                break;
            case 31: PlayerInventory.Money_AmazonCoin += int.Parse(ListModel.Instance.missionTUTOlist[31].rewordAmount);
                PopUpManager.instance.ShowGetPop(5, ListModel.Instance.missionTUTOlist[31].rewordAmount);
                break;
            case 32: PlayerInventory.Money_Dia += int.Parse(ListModel.Instance.missionTUTOlist[32].rewordAmount); 
                PopUpManager.instance.ShowGetPop(1, ListModel.Instance.missionTUTOlist[32].rewordAmount);
                break;
            case 33: PlayerInventory.SetTicketCount("cave_enter", int.Parse(ListModel.Instance.missionTUTOlist[33].rewordAmount)); 
                PopUpManager.instance.ShowGetPop(7, ListModel.Instance.missionTUTOlist[33].rewordAmount);
                break;
            case 34: PlayerInventory.Money_Elixir += int.Parse(ListModel.Instance.missionTUTOlist[34].rewordAmount); 
                PopUpManager.instance.ShowGetPop(3, ListModel.Instance.missionTUTOlist[34].rewordAmount);
                break;
            case 35: PlayerInventory.SetTicketCount("cave_clear", int.Parse(ListModel.Instance.missionTUTOlist[35].rewordAmount));
                PopUpManager.instance.ShowGetPop(8, ListModel.Instance.missionTUTOlist[35].rewordAmount);
                break;
            case 36: PlayerInventory.SetTicketCount("cave_enter", int.Parse(ListModel.Instance.missionTUTOlist[36].rewordAmount)); 
                PopUpManager.instance.ShowGetPop(7, ListModel.Instance.missionTUTOlist[36].rewordAmount);
                break;
            case 37: PlayerInventory.SetBoxsCount("weapon_coupon", int.Parse(ListModel.Instance.missionTUTOlist[37].rewordAmount)); 
                PopUpManager.instance.ShowGetPop(6, ListModel.Instance.missionTUTOlist[37].rewordAmount);
                break;
            case 38: PlayerInventory.Money_Dia += int.Parse(ListModel.Instance.missionTUTOlist[38].rewordAmount); 
                PopUpManager.instance.ShowGetPop(1, ListModel.Instance.missionTUTOlist[38].rewordAmount);
                break;
            case 39: PlayerInventory.SetTicketCount("cave_enter", int.Parse(ListModel.Instance.missionTUTOlist[39].rewordAmount)); 
                PopUpManager.instance.ShowGetPop(7, ListModel.Instance.missionTUTOlist[39].rewordAmount);
                break;
            case 40: PlayerInventory.SetTicketCount("reinforce_box", int.Parse(ListModel.Instance.missionTUTOlist[40].rewordAmount));
                PopUpManager.instance.ShowGetPop(0, ListModel.Instance.missionTUTOlist[40].rewordAmount);
                break;
            case 41: PlayerInventory.SetTicketCount("cave_clear", int.Parse(ListModel.Instance.missionTUTOlist[41].rewordAmount)); 
                PopUpManager.instance.ShowGetPop(8, ListModel.Instance.missionTUTOlist[41].rewordAmount);
                break;
            case 42: PlayerInventory.Money_Elixir += int.Parse(ListModel.Instance.missionTUTOlist[42].rewordAmount); 
                PopUpManager.instance.ShowGetPop(3, ListModel.Instance.missionTUTOlist[42].rewordAmount);
                break;
            case 43: PlayerInventory.Money_Dia += int.Parse(ListModel.Instance.missionTUTOlist[43].rewordAmount);
                PopUpManager.instance.ShowGetPop(1, ListModel.Instance.missionTUTOlist[43].rewordAmount);
                break;
            case 44: PlayerInventory.Money_Dia += int.Parse(ListModel.Instance.missionTUTOlist[44].rewordAmount); 
                PopUpManager.instance.ShowGetPop(1, ListModel.Instance.missionTUTOlist[44].rewordAmount);
                break;
            case 45: PlayerInventory.SetTicketCount("reinforce_box", int.Parse(ListModel.Instance.missionTUTOlist[45].rewordAmount));
                PopUpManager.instance.ShowGetPop(0, ListModel.Instance.missionTUTOlist[45].rewordAmount);
                break;
            case 46: PlayerInventory.SetTicketCount("leaf_box", int.Parse(ListModel.Instance.missionTUTOlist[46].rewordAmount)); 
                PopUpManager.instance.ShowGetPop(9, ListModel.Instance.missionTUTOlist[46].rewordAmount);
                break;
            case 47: PlayerInventory.SetTicketCount("cave_clear", int.Parse(ListModel.Instance.missionTUTOlist[47].rewordAmount)); 
                PopUpManager.instance.ShowGetPop(8, ListModel.Instance.missionTUTOlist[47].rewordAmount);
                break;
            case 48: PlayerInventory.SetBoxsCount("weapon_coupon", int.Parse(ListModel.Instance.missionTUTOlist[48].rewordAmount)); 
                PopUpManager.instance.ShowGetPop(6, ListModel.Instance.missionTUTOlist[48].rewordAmount);
                break;
            case 49: PlayerInventory.Money_Dia += int.Parse(ListModel.Instance.missionTUTOlist[49].rewordAmount); 
                PopUpManager.instance.ShowGetPop(1, ListModel.Instance.missionTUTOlist[49].rewordAmount);
                break;

            default: 
                break;
        }
        /// 새로고침
        InitTutorial();
    }





}
