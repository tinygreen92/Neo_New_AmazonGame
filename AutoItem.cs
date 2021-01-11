using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoItem : MonoBehaviour
{
    public Text miningCoin;
    public MineManager sm;
    [Header("- 회색 커버 오브젝트")]
    public GameObject GrayImage;
    [Header("- 정보 표기 부분")]
    public Text NameBox;
    public Slider slider;
    [Header("- 버튼 부분")]
    public Text UpgradeBox;
    [Header("- 버튼 색 바꾸기")]
    public Image[] TargetImage;
    public Sprite[] BtnSprite;
    [Header("- 글로우 이펙트")]
    public GameObject[] glowEffect;

    /// <summary>
    /// 해당 버튼 이미지만 활성화.
    /// </summary>
    /// <param name="_indx"></param>
    void OnTargetImg(int _indx)
    {
        for (int i = 0; i < TargetImage.Length; i++)
        {
            TargetImage[i].gameObject.SetActive(false);
        }
        TargetImage[_indx].gameObject.SetActive(true);
    }

    private void Update()
    {
        if (ListModel.Instance.mineCraft[_index].isEnable == "TRUE")
        {
            /// TODO : 버튼 활성화 / 비활성화 전환.
            ChangeBtnColor();
        }
        else
        {
            if (TargetImage[0].sprite != BtnSprite[2]) TargetImage[0].sprite = BtnSprite[2]; //  OFF 이미지
        }
    }
    /// <summary>
    /// 버튼 색 노랑 <-> 회색 체인지
    /// ListModel.Instance.mineCraft[_index].stage
    /// </summary>
    void ChangeBtnColor()
    {
        if (PlayerInventory.mining >= thisStageCoin)
        {
            TargetImage[0].sprite = BtnSprite[1]; //  On 이미지
        }
        else if (TargetImage[0].sprite != BtnSprite[2])
        {
            TargetImage[0].sprite = BtnSprite[2]; //  OFF 이미지
        }
    }

    int _index;
    Coroutine c_time;
    Coroutine d_time;
    private bool isInit;
    int thisStageCoin;

    public void BoxInfoUpdate(int cnt)
    {
        /// 인덱스 설정 -> 이 스크립트 전체
        _index = cnt;
        /// 소모 코인
        thisStageCoin = _index + 1;
        miningCoin.text = PlayerPrefsManager.instance.DoubleToStringNumber(thisStageCoin);
        // 코루틴 타이머 관련 갱신
        if (c_time != null) StopCoroutine(c_time);
        if (d_time != null) StopCoroutine(d_time);
        c_time = null;
        d_time = null;
        // 글로우 숨김 델리게이트
        sm.chain += HideGrowEffect;
        HideGrowEffect();

        NameBox.text = "광산 " + ListModel.Instance.mineCraft[_index].stage + "층";
        /// 즉시 완료 금액
        UpgradeBox.text = PlayerPrefsManager.instance.DoubleToStringNumber(int.Parse(ListModel.Instance.mineCraft[_index].unlockDia));

        switch (ListModel.Instance.mineCraft[_index].isEnable)
        {
            case "TRUE": 
                GrayImage.SetActive(false);
                OnTargetImg(0);
                slider.value = 0;
                break;
            case "ING":
                GrayImage.SetActive(false);
                OnTargetImg(1);
                if (!isInit)
                {
                    /// 앱을 껐다키면 1번만 외부 코루틴 시작
                    sm.DieHardCoTimer(_index);
                }
                if (c_time == null) c_time = StartCoroutine(TimerStart());
                break;
            case "BUY":
                GrayImage.SetActive(false);
                OnTargetImg(2);
                slider.value = 1f;
                break;
            case "COMP":
                GrayImage.SetActive(false);
                OnTargetImg(3);
                slider.value = 1f;
                break;
            case "DIA":
                GrayImage.SetActive(false);
                OnTargetImg(1);
                //if (!isInit)
                //{
                //    sm.InfinityCoTimer(_index);
                //}
                //if (d_time == null) d_time = StartCoroutine(LoopTimerStart());
                break;
            /// FALSE 라면 회색 이미지 활성화
            default:  // "FALSE"
                if (c_time != null) StopCoroutine(c_time);
                if (d_time != null) StopCoroutine(d_time);
                c_time = null;
                d_time = null;
                GrayImage.SetActive(true);
                OnTargetImg(0);
                slider.value = 0;
                if (_index == 0)
                {
                    ListModel.Instance.Mine_Unlock(0, "TRUE");
                    GrayImage.SetActive(false);
                }
                break;
        }

        // 첫 업데이트
        if (!isInit) isInit = true;
    }

    /// <summary>
    /// 이미지 슬라이더 움직이는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator TimerStart()
    {
        yield return null;
        float MAX_HP = ListModel.Instance.mineCraft[_index].mine_hp;
        while (true)
        {
            /// Manager에서 체력 깎아줌
            slider.value = sm.currentHPs[_index] / MAX_HP;
            yield return new WaitForFixedUpdate();
            if (sm.currentHPs[_index] >= MAX_HP) break;
        }
        slider.value = 1f;
    }

    IEnumerator LoopTimerStart()
    {
        yield return null;
        float MAX_HP = ListModel.Instance.mineCraft[_index].mine_hp;

        while (true)
        {
            /// Manager에서 체력 깎아줌
            slider.value = sm.currentHPs[_index] / MAX_HP;
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// 마인 크래프트에서 클릭 발생
    /// </summary>
    public void Clicked_Mine()
    {
        /// 채굴 시작
        if (TargetImage[0].gameObject.activeSelf)
        {
            /// 채굴권이 존재하면 팝업 켜준다.
            if (PlayerInventory.mining >= thisStageCoin)
            {
                sm.recentCoinSoMo = _index + 1;
                sm.PopWarnningUp(true, _index);
            }
            
        }
        /// 채굴 중
        else if (TargetImage[1].gameObject.activeSelf)
        {
            /// TODO : 채굴 즉시 완료
            if (PlayerInventory.Money_Dia < int.Parse(ListModel.Instance.mineCraft[_index].unlockDia))
            {
                PopUpManager.instance.ShowGrobalPopUP(1);
                return;
            }
            /// 다이아가 충분하면 팝업 켜준다.
            sm.PopWarnningUp(false, _index);
        }
        /// 자동 채굴 구입? (다이아)
        else if (TargetImage[2].gameObject.activeSelf)
        {
            //if (PlayerInventory.Money_Dia < 100) return;
            //PlayerInventory.Money_Dia -= 100;
            ///// 외부 코루틴 시작
            //sm.InfinityCoTimer(_index);
        }
        /// 채굴 완료
        else if (TargetImage[3].gameObject.activeSelf)
        {
            /// 저장소에 광물 보관
            sm.GetOverLoadReword(_index);
            /// 저장소에 광물 보관된다 팝업
            PopUpManager.instance.ShowGrobalPopUP(6);
        }

        // 글로우 모두 숨김
        sm.chain();
        // 이 오브젝트 글로우 표기
        glowEffect[0].SetActive(true);
        glowEffect[1].SetActive(true);
    }


    /// <summary>
    /// 아우터 글로우 숨기기
    /// </summary>
    void HideGrowEffect()
    {
        for (int i = 0; i < glowEffect.Length; i++)
        {
            glowEffect[i].SetActive(false);
        }
    }


    private void OnEnable()
    {
        // 최초 실행시 실행안하고 한번 오브젝트 껐다키면 실행
        if (isInit) BoxInfoUpdate(_index);
    }

    private void OnDisable()
    {
        if (c_time != null) StopCoroutine(c_time);
        if (d_time != null) StopCoroutine(d_time);
        c_time = null;
        d_time = null;
    }

}
