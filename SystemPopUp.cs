using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemPopUp : MonoBehaviour
{
    public static SystemPopUp instance;

    [Header("- 웨이 포인트 5개")]
    public Transform[] wayPoints;
    public Transform ufoTransf;
    [Header("- 서버에서 받아오는 텍스트")]
    public GameObject warningText;
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

    private Vector3[] wayPointVector;
    public  void RandomTweenDoPath()
    {
        ufoTransf.gameObject.SetActive(true);
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        wayPointVector = new Vector3[5];
        //
        wayPointVector.SetValue(wayPoints[0].position, 0);
        wayPointVector.SetValue(wayPoints[1].position, 1);
        wayPointVector.SetValue(wayPoints[2].position, 2);
        wayPointVector.SetValue(wayPoints[3].position, 3);
        wayPointVector.SetValue(wayPoints[4].position, 4);
        // wayPoints = new[] { wayPoint1.position, wayPoint2.position, wayPoint3.position };

        // DOPath(Vector3[] waypoints, float duration, PathType pathType = Linear, PathMode pathMode = Full3D, int resolution = 10, Color gizmoColor = null)
        // Tweens a Transform's position through the given path waypoints, using the chosen path algorithm.
        ufoTransf.DOPath(wayPointVector, 6.0f, PathType.CatmullRom)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
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

    public void LoopSavingImg()
    {
        warningText.SetActive(true);
        Pops[0].SetActive(true);
        loading.DORotate(new Vector3(0, 0, -360), 3, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
    }





}
