using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDotManager : MonoBehaviour
{
    public static RedDotManager instance;

    [Header("- 레드도트 이미지 GO")]
    public GameObject[] RedDot;


    private void Awake()
    {
        instance = this;
        /// 처음에 레드도트 다 꺼주기
        for (int i = 0; i < RedDot.Length; i++)
        {
            RedDot[i].SetActive(false);
        }
    }

}
