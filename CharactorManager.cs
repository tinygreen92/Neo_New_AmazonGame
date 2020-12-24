using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorManager : MonoBehaviour
{
    public Transform InfiContents; // Item 부모

    public delegate void ChainFunc();       // 아웃라인 델리게이트
    public ChainFunc chain;                 // 체인 메서드

    [HideInInspector]
    public int upgrageMutiple = 1;           /// 1배 10배 100배 

    public void RefleshAllitem()
    {
        /// 지금 활성화된 자식만큼 배수 적용해줌. -> 배수 적용된 뒤에 새로고침.
        for (int i = 1; i < InfiContents.childCount; i++)
        {
            InfiContents.GetChild(i).GetComponent<CharactorItem>()
                .RefreshMutiple();
        }
    }
}
