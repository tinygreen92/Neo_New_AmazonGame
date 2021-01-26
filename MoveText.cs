using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveText : MonoBehaviour
{
    /// <summary>
    /// 텍스트 스피드
    /// </summary>
    public float speed = 10f;
    private Vector3 startPos;


    private void Start()
    {
        startPos = transform.localPosition;
        Debug.LogError(" startPos :: " + startPos);
        LoopLoop();
    }


    void LoopLoop()
    {
        GetComponent<Text>().text = PlayerPrefsManager.instance.CH_NOTICE;
        transform.DOLocalMoveX(-1100f, speed).SetEase(Ease.Linear).OnComplete(Refeat);
    }

    void Refeat()
    {
        transform.localPosition = startPos;
        LoopLoop();
    }
}
