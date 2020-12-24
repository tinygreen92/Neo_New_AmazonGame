using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemPopUp : MonoBehaviour
{
    public static SystemPopUp instance;

    [Header("- 팝업 게임 오브젝트 배열")]
    [SerializeField]
    private GameObject[] Pops;
    [Header("- 회전초")]
    public Transform loading;

    private void Awake()
    {
        instance = this;
        Pops = new GameObject[transform.childCount];
        /// 팝업창 갯수 파악해서 배열에 저장
        for (int i = 0; i < Pops.Length; i++)
        {
            Pops[i] = transform.GetChild(i).gameObject;
        }
    }

    /// <summary>
    /// 경고 팝업 출력 (부정적) + 사운드
    /// </summary>
    /// <param name="_input"></param>
    public void ShowWarningPop(string _input)
    {

    }

    /// <summary>
    /// 알림 팝업 출력 (긍정적) + 사운드
    /// </summary>
    /// <param name="_input"></param>
    public void ShowNoticePop(string _input)
    {

    }


    /// <summary>
    /// 뺑글뺑글 돌아가는거
    /// </summary>
    public void LoopLoadingImg( )
    {
        Pops[0].SetActive(true);
        loading.DORotate(new Vector3(0,0,-360), 3, RotateMode.FastBeyond360)
            .SetLoops(-1,LoopType.Incremental)
            .SetEase(Ease.Linear);
    }
    public void StopLoopLoading()
    {
        loading.DOKill();
        Pops[0].SetActive(false);
    }





}
