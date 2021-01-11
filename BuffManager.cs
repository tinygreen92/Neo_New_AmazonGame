using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    public GameObject[] BuffEffect;
    public PetManager pm;
    /// <summary>
    /// 버프 쿨타임 돌아가는 fillamount
    /// </summary>
    [Header("- 회색 이미지 위에 씌울 칼라 이미지들")]
    public Image[] filedImgs;
    /// <summary>
    /// 상점에서 사는 버프 시간은 30분 고정이다.
    /// </summary>
    private const float SHOP_BUFF_TIME = 1800.0f;
    //private const float SHOP_BUFF_TIME = 30.0f;
    /// <summary>
    /// 버프 타이머 코루틴 8가지
    /// </summary>
    Coroutine[] buffTimer = new Coroutine[8];



    /// <summary>
    /// <버프> 외부에서 영구적인 코루틴 타이머 호출
    /// </summary>
    /// <param name="_id"></param>
    public void DieHardCoTimer(int _id)
    {
        /// 버프
        if (buffTimer[_id] != null && _id > 3) return;

        buffTimer[_id] = StartCoroutine(BuffTimerStart(_id));
    }



    /// <summary>
    /// 현금으로 버프 사면 버프 이미지 1f로 고정
    /// </summary>
    public void MoneyLoveBuff(int _id)
    {
        if (buffTimer[_id] != null)
        {
            StopCoroutine(buffTimer[_id]);
        }
        filedImgs[_id].fillAmount = 1f;
    }

    WaitForSeconds wfs;

    /// <summary>
    /// !!!!!!!!! 최초 한번만 실행 되어야 함
    /// 상점 코루틴은 [공격력 =0 공속 = 1  이속 = 2 골드 = 3 > 4종류 취급
    /// 펫 코루틴은 [공벞 = 4 공속벞 = 5  이속벞 = 6 골드 벞 7 > 5종류로 취급
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    IEnumerator BuffTimerStart(int _id)
    {
        float time = 0;
        float maxTime;

        /// 버프 켜줌
        BuffOnOff(_id, true);

        yield return null;

        while (true)
        {
            yield return new WaitForFixedUpdate();

            time += Time.deltaTime;
            /// 펫 버프는 레벨에따라 실시간 갱신
            if (_id > 3)
            {
                maxTime = ListModel.Instance.petList[_id - 3].usingTimeDam * PlayerInventory.Pet_lv(_id - 3);
            }
            /// 상점 버프는 초 고정
            else
            {
                maxTime = SHOP_BUFF_TIME;
            }
            /// 이미지 fill
            filedImgs[_id].fillAmount = (maxTime - time) / maxTime;

            /// 종료 조건
            if (time >= maxTime)
            {
                /// 버프 꺼줌
                BuffOnOff(_id, false);
                break;
            }
        }

        /// 펫 버프 일 경우에는 쿨 타임 대기 후, 무한 반복 / 상점 버프는 꺼줌
        if (_id > 3)
        {
            float cooltime;
            time = 0;

            while (true)
            {
                yield return new WaitForFixedUpdate();

                time += Time.deltaTime;
                cooltime = (ListModel.Instance.petList[_id - 3].coolTime - (PlayerInventory.Pet_lv(_id - 3) * 3));
                /// 탈출 조건
                if (time >= cooltime)
                {
                    break;
                }
            }
            /// 펫 버프일 경우 다시 코루틴 시작
            StartCoroutine(BuffTimerStart(_id));
        }

    }


    /// <summary>
    /// 해당하는 버프 값 켜줌 (0은 예외)
    /// </summary>
    /// <param name="_index"></param>
    private void BuffOnOff(int _index, bool _sw)
    {
        /// 그레이 이미지 표시 = 칼라 이미지 0 %
        if (_sw == false)
        {
            filedImgs[_index].fillAmount = 0;
        }
        else
        {
            filedImgs[_index].fillAmount = 1f;
        }

        switch (_index)
        {
            case 0:
                PlayerInventory.dia_power_up = _sw;
                if(_sw) PlayEffectPetBuff(0);
                break;
            case 1:
                PlayerInventory.dia_attack_speed_up = _sw;
                if (_sw) PlayEffectPetBuff(1);
                break;
            case 2:
                PlayerInventory.dia_move_speed_up = _sw;
                if (_sw) PlayEffectPetBuff(2);
                break;
            case 3:
                PlayerInventory.dia_gold_earned_up = _sw;
                if (_sw) PlayEffectPetBuff(3);
                break;
            case 4:
                PlayerInventory.ispet_equiped_power = _sw;
                if (_sw) pm.PlayEffectPetBuff(1);
                break;
            case 5:
                PlayerInventory.ispet_equiped_attack_speed = _sw;
                if (_sw) pm.PlayEffectPetBuff(2);
                break;
            case 6:
                PlayerInventory.ispet_equiped_move_speed = _sw;
                if (_sw) pm.PlayEffectPetBuff(3);
                break;
            case 7:
                PlayerInventory.ispet_equiped_gold_earned = _sw;
                if (_sw) pm.PlayEffectPetBuff(4);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 버프 이펙트 배틀 필드에 호출
    /// </summary>
    /// <param name="indx"></param>
    public void PlayEffectPetBuff(int indx)
    {
        BuffEffect[indx].SetActive(true);
        StartCoroutine(InvoEffect(indx));
    }

    IEnumerator InvoEffect(int indx)
    {
        yield return new WaitForSeconds(5);

        BuffEffect[indx].SetActive(false);
    }

}
