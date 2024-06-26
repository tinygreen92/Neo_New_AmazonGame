﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotBtnBackImg : MonoBehaviour
{
    public GameObject[] TopImg;
    public GameObject[] BotImg;
    [Header("-바깥 채팅 박스 텍스트")]
    public Image ArrowIcon;
    public Sprite ArroIcons;

    private void Awake()
    {
        for (int i = 0; i < TopImg.Length; i++)
        {
            TopImg[i].SetActive(false);
            BotImg[i].SetActive(false);
        }
    }

    /// <summary>
    /// 클릭하면 해당 백그라운드 동일 색상으로 물들이기
    /// </summary>
    /// <param name="_index"></param>
    public void BBB_Changer(int _index)
    {   /// 채팅 화살표 바꿔주기
        ArrowIcon.sprite = ArroIcons;
        /// 동일 버튼 누를땐 동작 없음
        if (!TopImg[_index].activeSelf)
        {
            for (int i = 0; i < TopImg.Length; i++)
            {
                TopImg[i].SetActive(false);
                BotImg[i].SetActive(false);
            }
            /// 해당 켜주기
            TopImg[_index].SetActive(true);
            BotImg[_index].SetActive(true);
        }


    }
}
