using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{
    [Header("- 배틀필드 펫 스프라이트")]
    public Animator[] petAnim;
    [Header("- 펫 쿨타임 관리")]
    public DamageController dc;
    public float[] currentTimes;        // 펫 타이머
    [Header("- 아이콘")]
    public Sprite[] icons;

    [Header("- 다이아 나뭇잎 아이콘")]
    public Sprite[] moIcons;
    public GameObject[] displayOJ;

    public delegate void ChainFunc();       // 아웃라인 델리게이트
    public ChainFunc chain;                 // 체인 메서드

    [HideInInspector]
    public int diaORleaf = 0;           /// 0은 다이아 1 은 리프.



    /// <summary>
    /// 0~4 해당 펫 움직임
    /// </summary>
    /// <param name="_index"></param>
    public void PetAnimStart(int _index)
    {
        petAnim[_index].gameObject.SetActive(true);
        petAnim[_index].Play(petAnim[_index].name + "_Idle", -1, 0f);
    }

    /// <summary>
    /// 펫 0번 해금 되면 작동
    /// </summary>
    public void AutoPetAttack()
    {
        /// 아이들 애니메이션
        PetAnimStart(0);
        /// 오토 공격 시작!
        StartCoroutine(AutoPet());
    }


    IEnumerator AutoPet()
    {
        float time = 0;
        float maxTime = ListModel.Instance.petList[0].usingTimeDam * int.Parse(ListModel.Instance.petList[0].petLevel);
        Debug.LogWarning("펫 maxTime : " + maxTime);
        yield return null;
        Debug.LogWarning(" 펫의 공격!! " + PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.character_DPS * 0.1d));
        
        /// TODO : 공격 애니메이션
        
        dc.Create(PlayerPrefsManager.instance.topCanvas, PlayerInventory.character_DPS * 0.1d, false);
        while (true)
        {
            yield return new WaitForFixedUpdate();

            time += Time.deltaTime;
            if (time >= maxTime)
            {
                time = 0;
                float cooltime = (ListModel.Instance.petList[0].coolTime - (int.Parse(ListModel.Instance.petList[0].petLevel) * 3));
                yield return new WaitForSeconds(cooltime);

                maxTime = ListModel.Instance.petList[0].usingTimeDam * int.Parse(ListModel.Instance.petList[0].petLevel);
            }
        }

    }

}
