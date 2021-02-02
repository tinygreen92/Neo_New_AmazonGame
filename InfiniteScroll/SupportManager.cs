using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// 버튼 동작 관리
/// </summary>
public class SupportManager : MonoBehaviour
{
    public TopViewPannel tvp; // 탑뷰 레벨업 버튼 컨트롤러.
    public Transform InfiContents; // Item 부모
    /// <summary>
    /// 수집 남은 시간 
    /// </summary>
    public float[] currentTimes;        // 수집 타이머

    public delegate void ChainFunc();       // 아웃라인 델리게이트
    public ChainFunc chain;                 // 체인 메서드

    [HideInInspector]
    public int upgrageMutiple = 1;           /// 1배 10배 100배 

    double earnGold;            // 수집 골드 저장용

    [HideInInspector]
    public bool isFristUnlock;

    //private void Update()
    //{
    //    if (isFristUnlock) return;

    //    /// 골드 10있는지 계속 감시해서 잠금 해제해줌
    //    if(PlayerInventory.Money_Gold >= 10 && int.Parse(InfiContents.GetChild(1).name) == 0)
    //    {
    //        Debug.LogError("해제");
    //        UnlockNextSoozip(0);
    //        isFristUnlock = true;
    //    }
    //}
    Coroutine[] C_Routine;

    private void Awake()
    {
        C_Routine = new Coroutine[30];

    }

    /// <summary>
    /// <수집> 외부에서 영구적인 코루틴 타이머 호출
    /// </summary>
    /// <param name="_id"></param>
    public void DieHardCoTimer(int _id)
    {
        if (C_Routine[_id] == null)
        {
            C_Routine[_id] = StartCoroutine(TimerStart(_id));
        }
    }

    /// <summary>
    /// 유물 값 적용한 index 별 맥스 타임 불러오는 값
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public float MaxTime(int _id)
    {
        float result = ListModel.Instance.supList[_id].maxTime - (float)PlayerInventory.Soozip_Time;
        if (result <= 1.0f) result = 1.0f;
        return result;
    }

    bool isMotherItemInit;
    /// <summary>
    /// 수집된 저장된 
    /// </summary>
    public void InitTimeLoad()
    {
        if (isMotherItemInit)
            return;
        for (int i = 0; i < currentTimes.Length; i++)
        {
            /// 레벨 1 이상일때만
            if (int.Parse(ListModel.Instance.supList[i].supporterLevel) > 0)
            {
                DieHardCoTimer(i);
            }
        }
        /// 첫실행시 한번만
        isMotherItemInit = true;
    }

    /// <summary>
    /// 실제로 각 탭에서 골드 획득하는 코루틴
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    IEnumerator TimerStart(int _id)
    {
        yield return null;

        while (true)
        {
            yield return new WaitForFixedUpdate();

            currentTimes[_id] += Time.deltaTime;

            if (currentTimes[_id] >= MaxTime(_id))
            {
                /// 골드 획득
                GetSoozipGold(_id);
                yield return new WaitForSeconds(0.1f);
                currentTimes[_id] = 0;
            }
        }

    }


    /// <summary>
    /// 수집 타이머 끝나면 골드 수집
    /// 1. 골드 창 <하단> 에 + 수집 골드 그래픽 표기
    /// 2. 표기 사라지면 실제 플레이어 골드에 더해줌 
    /// 3. 그래픽 표기 끝나면 골드 창에 더해주고 Refresh
    /// </summary>
    public void GetSoozipGold(int _id)
    {
        /// 1. 골드 창 [하단] 에 + 수집 골드 그래픽 표기


        /// 2. 표기 사라지면 실제 플레이어 골드에 더해줌 
        earnGold = ListModel.Instance.supList[_id].currentEarnGold * 0.5d;
        earnGold *= (double.Parse(ListModel.Instance.supList[_id].supporterLevel) + 1d);
        Debug.Log(name + _id + "번 인덱스 유물 전  골드 "+ earnGold);
        //Debug.Log(name + _id + "번 인덱스 평균치   골드 "+ Math.Truncate(earnGold));
        //Debug.Log(name + _id + "번 인덱스 적용 골드 "+ earnGold * PlayerInventory.Soozip_Gold_Earned + " 수집!");

        PlayerInventory.Money_Gold += Math.Truncate(earnGold) * PlayerInventory.Soozip_Gold_Earned;
        ///  골드 업적 카운트 올리기
        ListModel.Instance.ALLlist_Update(3, Math.Truncate(earnGold) * PlayerInventory.Soozip_Gold_Earned);
        /// 3. 그래픽 표기 끝나면 골드 창 Refresh
        MoneyManager.instance.DisplayGold();

        /// 4. 골드 증가한만큼 버튼 리프레쉬
        RefleshAllitem();

    }

    public void RefleshAllitem()
    {
        /// 지금 활성화된 자식만큼 배수 적용해줌. -> 배수 적용된 뒤에 새로고침.
        for (int i = 1; i < InfiContents.childCount; i++)
        {
            InfiContents.GetChild(i).GetComponent<SupportItem>()
                .RefreshMutiple();
        }
    }


    /// <summary>
    /// 개별 수집 박스의 <SupportItem> 에서 조건 맞으면 호출
    /// 다음 회색 박스 잠금 해제 해준다.
    /// </summary>
    /// <param name="_id">param 넘길때 현재 레벨 + 1 해줄 것</param>
    public void UnlockNextSoozip(int _id)
    {
        if (name != "SupportManager") return;
        // 마지막 해금이면 리턴
        if (_id == 30) return;

        /// 원본 _id 는 +1 한 값 -> 처음 실행시 회색블록 삭제
        InfiContents.Find((_id).ToString()).GetComponent<SupportItem>().Clicked_LvUP();
    }

}
