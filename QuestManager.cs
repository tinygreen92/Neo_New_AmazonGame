using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestManager : MonoBehaviour
{
    public Image[] dayAllBtnImg;
    public Sprite[] allBtnSprs;
    [Space]
    public ScrollRect scBar1;
    public ScrollRect scBar2;
    [Space]
    public Transform C5;
    public Transform C17;
    [Space]
    public GameObject[] missonPanel;
    [HideInInspector]
    public bool isALLquest;
    [Header("-0은 회색 1은 파랑")]
    public Sprite[] BtnSprite;
    //
    public delegate void ChainFunc();       // 아웃라인 델리게이트
    public ChainFunc chain;                 // 체인 메서드

    public bool isAceptEnable;

    private void OnEnable()
    {
        /// 상단 버튼 호출
        SwichTapBtn(0);
    }

    private void OnDisable()
    {
        /// 모두받기 일단 회색
        isAceptEnable = false;
        dayAllBtnImg[0].sprite = allBtnSprs[0];
        dayAllBtnImg[1].sprite = allBtnSprs[0];
    }

    /// <summary>
    /// 상단 버튼 2개 각각에 달아주기 
    /// </summary>
    /// <param name="_index"></param>
    public void SwichTapBtn(int _index)
    {
        /// 스크롤 위로
        scBar1.verticalNormalizedPosition = 1;
        scBar2.verticalNormalizedPosition = 1;

        switch (_index)
        {
            case 0:
                isALLquest = false;

                /// 평생 받기 일단 회색
                dayAllBtnImg[1].sprite = allBtnSprs[0];
                missonPanel[0].SetActive(true);
                missonPanel[1].SetActive(false);
                break;

            case 1:
                isALLquest = true;
                /// 일일 받기 일단 회색
                dayAllBtnImg[0].sprite = allBtnSprs[0];
                missonPanel[0].SetActive(false);
                missonPanel[1].SetActive(true);
                break;

            default:
                break;
        }
    }
    public void QMc5Update()
    {
        isAceptEnable = false;
        dayAllBtnImg[0].sprite = allBtnSprs[0];
        /// 지금 활성화된 자식만큼 배수 적용해줌. -> 배수 적용된 뒤에 새로고침.
        for (int i = 0; i < C5.childCount; i++)
        {
            C5.GetChild(i).GetComponent<MissionItem>().UpdateMission();
        }
        if (!isAceptEnable) return;
        /// 일일 미션 0 모두 받기 파랑 1
        dayAllBtnImg[0].sprite = allBtnSprs[1];
    }
    public void QMc17Update()
    {
        isAceptEnable = false;
        dayAllBtnImg[1].sprite = allBtnSprs[0];
        /// 지금 활성화된 자식만큼 배수 적용해줌. -> 배수 적용된 뒤에 새로고침.
        for (int i = 0; i < C17.childCount; i++)
        {
            C17.GetChild(i).GetComponent<MissionItem>().UpdateMission();
        }
        if (!isAceptEnable) return;
        /// 평생 미션 1 모두 받기 파랑 1
        dayAllBtnImg[1].sprite = allBtnSprs[1];
    }

    /// <summary>
    /// Day 미션에 붙이는 모두 받기
    /// </summary>
    public void GetDayAllReword()
    {
        isAllaceptLoop = true;
        isAceptEnable = false;
        dayAllBtnImg[0].sprite = allBtnSprs[0];
        for (int i = 0; i < C5.childCount; i++)
        {
            if (ListModel.Instance.missionDAYlist[i].maxValue == ListModel.Instance.missionDAYlist[i].curentValue)
            {
                /// 일퀘 리스트 초기화 해주시고
                ListModel.Instance.DAYlist_Update(i, -1);
                /// 보상 지급
                PlayerInventory.Money_Dia += int.Parse(ListModel.Instance.missionDAYlist[i].reword);
                /// 그레이 패널 활성화
                transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                /// 일퀘 미션 모두 완료 한 칸 올려
                ListModel.Instance.DAYlist_Update(0);
                /// 새로 고침
                C5.GetChild(i).GetComponent<MissionItem>().UpdateMission();
            }
        }
        ///// 0번 인덱스 새로고침
        //C5.GetChild(0).GetComponent<MissionItem>().UpdateMission();
        for (int i = 0; i < C5.childCount; i++)
        {
            /// 새로고침
            C5.GetChild(i).GetComponent<MissionItem>().UpdateMission();
        }
        /// 레드닷 끄기
        RedDotManager.instance.RedDot[5].SetActive(false);
        /// 다시 상단 정렬
        isAllaceptLoop = false;
        if (!isAceptEnable) return;
        /// 일일 미션 0 모두 받기 파랑 1
        dayAllBtnImg[0].sprite = allBtnSprs[1];
    }

    /// <summary>
    /// 모두 받기 루프중에는 상단 정렬 중단~
    /// </summary>
    public bool isAllaceptLoop;

    /// <summary>
    /// ALL 미션에 붙이는 모두 받기
    /// </summary>
    public void GetALLAllReword()
    {
        isAllaceptLoop = true;
        isAceptEnable = false;
        dayAllBtnImg[1].sprite = allBtnSprs[0];
        /// 지금 활성화된 자식만큼 배수 적용해줌. -> 배수 적용된 뒤에 새로고침.
        for (int i = 0; i < C17.childCount; i++)
        {
            if (double.Parse(ListModel.Instance.missionALLlist[i].maxValue) <= double.Parse(ListModel.Instance.missionALLlist[i].curentValue))
            {
                /// Max 확장해주시고 (내부에서 curentValue 빼줌)
                ListModel.Instance.ALLlist_Max_Update(i);
                /// 모든 미션은 보상 수령후 횟수 초기화
                ListModel.Instance.ALLlist_Update(i, -1);
                /// 보상 지급
                PlayerInventory.Money_Dia += int.Parse(ListModel.Instance.missionALLlist[i].reword);
                /// 모두 받을 때까지 루프
                if (i > 0) i--;
            }
        }

        for (int i = 0; i < C17.childCount; i++)
        {
            /// 새로고침
            C17.GetChild(i).GetComponent<MissionItem>().UpdateMission();
        }
        /// 레드닷 끄기
        RedDotManager.instance.RedDot[5].SetActive(false);
        /// 다시 상단 정렬
        isAllaceptLoop = false;
        if (!isAceptEnable) return;
        /// 평생 미션 1 모두 받기 파랑 1
        dayAllBtnImg[1].sprite = allBtnSprs[1];
    }




}
