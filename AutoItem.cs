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
    private bool isInit;
    int thisStageCoin;

    public void BoxInfoUpdate(int cnt)
    {
        /// 인덱스 설정 -> 이 스크립트 전체
        _index = cnt;
        /// 소모 코인
        thisStageCoin = _index + 1;
        miningCoin.text = PlayerPrefsManager.instance.DoubleToStringNumber(thisStageCoin);

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
                /// 코루틴 작동중이면 꺼주라
                if (c_time != null)
                {
                    StopCoroutine(c_time);
                    c_time = null;
                }
                break;
            case "ING":
                GrayImage.SetActive(false);
                OnTargetImg(1);
                /// 앱을 껐다키면 1번만 외부 코루틴 시작
                sm.DieHardCoTimer(_index);

                /// 이미지 슬라이더 움직이는 코루틴
                if (c_time == null && sm.SuperMomObject.activeSelf && int.Parse(name) == _index)
                {
                    /// 슬라이더 코루틴
                    c_time = StartCoroutine(TimerStart());
                }
                break;
            case "BUY":
                GrayImage.SetActive(false);
                OnTargetImg(2);
                slider.value = 1f;
                /// 코루틴 작동중이면 꺼주라
                if (c_time != null)
                {
                    StopCoroutine(c_time);
                    c_time = null;
                }
                break;
            case "COMP":
                GrayImage.SetActive(false);
                OnTargetImg(3);
                slider.value = 1f;
                /// 코루틴 작동중이면 꺼주라
                if (c_time != null)
                {
                    StopCoroutine(c_time);
                    c_time = null;
                }
                break;
            case "DIA":
                GrayImage.SetActive(false);
                OnTargetImg(1);
                break;
            /// 0 이라면? -> 해금 안됐을 때.
            default:  // "FALSE"
                if (c_time != null)
                {
                    StopCoroutine(c_time);
                    c_time = null;
                }
                GrayImage.SetActive(true);
                OnTargetImg(0);
                slider.value = 0;
                /// 근데 인덱스 0 은 항상 열려있어야 해
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
        /// +가 되어서 이 수치가되면 채굴 끝.
        float MAX_HP = ListModel.Instance.mineCraft[_index].mine_hp;
        slider.value = MineManager.currentHPs[_index] / MAX_HP;
        yield return null;

        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (ListModel.Instance.mineCraft[_index].isEnable == "ING")
            {
                /// Manager에서 진행도 올려줌
                slider.value = MineManager.currentHPs[_index] / MAX_HP;
                /// 채굴 끝
                if (MineManager.currentHPs[_index] >= MAX_HP)
                {
                    /// 완료되면 1
                    slider.value = 1f;
                    c_time = null;
                    break;
                }
            }
            /// COMP 상태라면 게이지 1 인 상태
            else if (ListModel.Instance.mineCraft[_index].isEnable == "COMP")
            {
                slider.value = 1f;
                c_time = null;
                break;
            }
            /// 아무 상태도 아니면 게이지 0 인 상태
            else
            {
                slider.value = 0;
                c_time = null;
                break;
            }
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
        /// 채굴 중 즉시완료
        else if (TargetImage[1].gameObject.activeSelf)
        {
            /// TODO : 채굴 즉시 완료 다이아 안 충분하면 경고 팝업
            if (PlayerInventory.Money_Dia < int.Parse(ListModel.Instance.mineCraft[_index].unlockDia))
            {
                PopUpManager.instance.ShowGrobalPopUP(1);
                return;
            }
            /// 다이아가 충분하면 진짜 할래 팝업 켜준다. -> Clicked_DiaComplete()
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
        StopAllCoroutines();
        c_time = null;
    }

}
