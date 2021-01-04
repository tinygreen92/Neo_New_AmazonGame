using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{
    [Header("- 버프 파티클 오브젝트")]
    public GameObject[] petBuffEffect;
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

    Coroutine Zeropet = null;
    /// <summary>
    /// 펫 0번 해금 되면 작동
    /// </summary>
    public void AutoPetAttack()
    {
        /// 아이들 애니메이션
        PetAnimStart(0);
        /// 오토 공격 시작!
        if (Zeropet == null)
        {
            Zeropet = StartCoroutine(AutoPet());    
        }
    }

    IEnumerator AutoPet()
    {
        float time = 0;
        float maxTime = ListModel.Instance.petList[0].usingTimeDam * int.Parse(ListModel.Instance.petList[0].petLevel);

        var petDamege = PlayerInventory.character_DPS * ListModel.Instance.petList[1].percentDam * PlayerInventory.Pet_lv(0) * 0.01d;

        Debug.LogWarning("펫 maxTime : " + maxTime);
        Debug.LogError(" 펫의 공격!! " + petDamege);
        yield return null;

        /// TODO : 공격 애니메이션
        PlayEffectPetBuff(0);
        dc.Create(PlayerPrefsManager.instance.topCanvas, petDamege, false);

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

    

    /// <summary>
    /// 펫 버프 이펙트 배틀 필드에 호출
    /// </summary>
    /// <param name="indx"></param>
    public void PlayEffectPetBuff(int indx)
    {
        petBuffEffect[indx].SetActive(true);
        StartCoroutine(InvoEffect(indx));
    }

    IEnumerator InvoEffect(int indx)
    {
        yield return new WaitForSeconds(5);

        petBuffEffect[indx].SetActive(false);
    }

}
