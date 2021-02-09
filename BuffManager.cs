using EasyMobile;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    public GameObject FreeBuffRewordPop;
    [Header("- 버프 이펙트")]
    public GameObject[] BuffEffect;
    public PetManager pm;
    /// <summary>
    /// 버프 쿨타임 돌아가는 fillamount
    /// </summary>
    [Header("- 회색 이미지 위에 씌울 칼라 이미지들")]
    public Image[] filedImgs;
    public Text[] buffTimeText;
    /// <summary>
    /// 상점에서 사는 버프 시간은 30분 고정이다. -> 1시간으로 상향
    /// </summary>
    private float SHOP_BUFF_TIME = 3600f;
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
        /// 상점 버프
        SHOP_BUFF_TIME = 3600f;
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
                /// 쿨타임 텍스트 표기
                buffTimeText[_id].text = (cooltime - time).ToString("F0");
                /// 탈출 조건
                if (time >= cooltime)
                {
                    buffTimeText[_id].text = "";
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






    #region <Rewarded Ads> 무료 버프 보기 광고


    public void Ads_FreeBuffClicked()
    {
        PlayerPrefsManager.instance.TEST_SaveJson();
        SystemPopUp.instance.LoopLoadingImg();
        Invoke(nameof(InvoStopLoop), 5.0f);

        if (Advertising.IsRewardedAdReady(RewardedAdNetwork.AdMob, AdPlacement.Default))
        {
            Advertising.RewardedAdCompleted += AdsCompleated;
            Advertising.RewardedAdSkipped += AdsSkipped;
            Advertising.ShowRewardedAd(RewardedAdNetwork.AdMob, AdPlacement.Default);
        }
        else
        {
            /// 광고 없으면 안돼
            //Invoke(nameof(AdsInvo), 0.5f);
            SystemPopUp.instance.StopLoopLoading();
            /// 프리 버프 팝업 꺼주기
            PopUpManager.instance.HidePopUP(32);
            /// 15초 타이머
            PopUpManager.instance.fwm.AdsHolding20s();
        }

    }

    void InvoStopLoop()
    {
        SystemPopUp.instance.StopLoopLoading();
    }

    // Event handler called when a rewarded ad has completed
    void AdsCompleated(RewardedAdNetwork network, AdPlacement location)
    {
        Invoke(nameof(AdsInvo), 0.5f);
        Advertising.RewardedAdCompleted -= AdsCompleated;
        Advertising.RewardedAdSkipped -= AdsSkipped;
    }

    // Event handler called when a rewarded ad has been skipped
    void AdsSkipped(RewardedAdNetwork network, AdPlacement location)
    {
        Advertising.RewardedAdCompleted -= AdsCompleated;
        Advertising.RewardedAdSkipped -= AdsSkipped;
        SystemPopUp.instance.StopLoopLoading();
    }
    /// <summary>
    /// 보상 지급 메소드
    /// </summary>
    void AdsInvo()
    {
        SystemPopUp.instance.StopLoopLoading();
        ///  광고 1회 시청 완료 카운트
        ListModel.Instance.ALLlist_Update(0, 1);
        /// 광고 시청 일일 업적
        ListModel.Instance.DAYlist_Update(7);
        /// 광고제거 구매 안했으면  버프 4종 돌아가
        if (PlayerInventory.isSuperUser == 0)
        {
            SHOP_BUFF_TIME = 1200f;
            /// 영구 버프일 경우는 빼주고
            if (!PlayerInventory.isbuff_power_up) buffTimer[0] = StartCoroutine(BuffTimerStart(0));
            if (!PlayerInventory.isbuff_attack_speed_up) buffTimer[1] = StartCoroutine(BuffTimerStart(1));
            if (!PlayerInventory.isbuff_move_speed_up) buffTimer[2] = StartCoroutine(BuffTimerStart(2));
            if (!PlayerInventory.isbuff_gold_earned_up) buffTimer[3] = StartCoroutine(BuffTimerStart(3));
        }



        /// 보상 팝업
        FreeBuffRewordPop.SetActive(true);
    }


    #endregion














}
