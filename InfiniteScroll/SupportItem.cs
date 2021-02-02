using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 서포터 아이템 박스에서만 붙어있을 스크립트.
/// </summary>
public class SupportItem : MonoBehaviour
{
    public SupportManager sm;
    [Header("- 회색 커버 오브젝트")]
    public GameObject GrayImage;
    [Header("- 아이콘")]
    public Sprite[] icons; 
    public Image spriteBox;
    [Header("- 정보 표기 부분")]
    public Text NameBox;
    public Text LevelBox;
    public Text TimerBox;
    public Slider slider;
    [Header("- 버튼 부분")]
    public Text UpgradeBox;
    public Text EarnGoldBox;
    [Header("- 버튼 색 바꾸기")]
    public Image DisableImage;
    public Sprite[] BtnSprite;
    public GameObject MaxButton;

    [Header("- 글로우 이펙트")]
    public GameObject[] glowEffect;

    const string T_LV = "Lv. ";
    const string T_PLUS = "+";

    int _index;

    int si, bun, cho;
    string timeStr;

    Coroutine c_time;

    int thisLevel;
    int mutiple = 1;

    bool isInit;

    double _MultiResult;
    double _MultiLv;

    /// <summary>
    /// 골드 획득 초기값 + (초기값 / 2) * lv
    /// </summary>
    /// <param name="muti"> 곱해줄 배수 </param>
    /// <param name="lv"> 현재 레벨 thisLevel </param>
    /// <returns></returns>
    double GetMutipleEarnGold(int muti, int lv)
    {
        // 만렙 찍으면  MAX 
        if (thisLevel >= 1000)
        {
            thisLevel = 1000;
            /// TODO : MAX 버튼 활성화.
            return 0;
        }

        _MultiResult = ListModel.Instance.supList[_index].currentEarnGold * 0.5;
        // 다음 1레벨 / 다음 10레벨 / 다음 100레벨
        _MultiLv = muti + lv;

        /// 만렙 예외 처리 1000레벨 고정
        if (muti == 1 && _MultiLv > 999) _MultiLv = 1000;
        else if (muti == 10 && _MultiLv > 990) _MultiLv = 1000;
        else if (muti == 100 && _MultiLv > 900) _MultiLv = 1000;

        // 레벨 1 이상이면
        if (_MultiLv > 1)
        {
            _MultiResult *= _MultiLv;
        }
        else
        {
            _MultiResult *= 2d;
        }

        return Math.Truncate( _MultiResult);
    }

    /// <summary>
    /// 업그레이드 골드 소모 
    /// </summary>
    /// <param name="muti"> 곱해줄 배수 </param>
    /// <param name="lv"> 현재 레벨 thisLevel </param>
    /// <returns></returns>
    double GetMutipleUpgrade(int muti, int lv)
    {
        // 만렙 찍으면  MAX 
        if(thisLevel >= 1000)
        {
            thisLevel = 1000;
            return 0;
        }

        _MultiResult = ListModel.Instance.supList[_index].nextUpgradeNeed;
        // 다음 1레벨 / 다음 10레벨 / 다음 100레벨
        _MultiLv     = muti + lv;

        /// 만렙 예외 처리 1000레벨 고정
        if (_MultiLv > 1000) _MultiLv = 1000;
        //else if (muti == 10 && _MultiLv > 1000) _MultiLv = 1000;
        //else if (muti == 100 && _MultiLv > 1000) _MultiLv = 1000;

        //if (_MultiLv > 1)
        //{
        //    _MultiResult *= Math.Pow(1.12d, (_MultiLv - 1));
        //}

        /// 골드 소모량
        if (_MultiLv > 1)
        {
            double tmpMuti = 0;
            for (int i = thisLevel + 1; i < thisLevel + muti + 1; i++)
            {
                /// TODO : 변경전 tmpMuti += _MultiResult * (Math.Pow(1.12d, i));
                //tmpMuti += _MultiResult * (1 + 0.15 * i);
                tmpMuti += _MultiResult * (Math.Pow(1.12d, i));
            }
            _MultiResult = tmpMuti;
        }

        return Math.Truncate(_MultiResult);
    }

    /// <summary>
    /// 업그레이드 배수 만큼 포문 돌려서 적정 주가 계산
    /// </summary>
    public void RefreshMutiple()
    {
        //for (int i = 0; i < sm.upgrageMutiple; i++)
        //{
        //    mutiple = i + 1;
        //    // 돈 안 충분하다.
        //    if (!IsPurchaseable())
        //    {
        //        if (mutiple == 1) break;
        //        mutiple -= 1;
        //        break;
        //    }
        //}
        mutiple = sm.upgrageMutiple;

        /// 만렙 예외 처리 1000레벨 고정
        if (mutiple == 10 && thisLevel >= 991) mutiple = (1000 - thisLevel);
        else if (mutiple == 100 && thisLevel >= 901) mutiple = (1000 - thisLevel);

        /// <다음_업그레이드비용> 배수 곱하기 해서 텍스트 뿌려주기.
        UpgradeBox.text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Soozip_Powerup_Gold * GetMutipleUpgrade(mutiple, thisLevel));
        /// <획득골드량> 배수 곱하기 해서 텍스트 뿌려주기.
        EarnGoldBox.text = T_PLUS + PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Soozip_Gold_Earned * GetMutipleEarnGold(mutiple, thisLevel)) + " (" + mutiple + ")";
    }

    /// <summary>
    /// 박스 SetActive(true)일때 호출
    /// 변경 값 갱신 해줌
    /// 1. 아우터 글로우 해제
    /// 2. 해당 코루틴 일때만 재생
    /// 3. 배수 적용시 지닌 화폐만큼만 뻥튀기 해서 (int) 수치 붙이기
    /// </summary>
    public void BoxInfoUpdate(int cnt)
    {
        /// 인덱스 설정 -> 이 스크립트 전체
        _index = cnt;

        mutiple = sm.upgrageMutiple;

        /// 만렙 예외 처리 1000레벨 고정
        if (mutiple == 10 && thisLevel >= 991) mutiple = (1000 - thisLevel);
        else if (mutiple == 100 && thisLevel >= 901) mutiple = (1000 - thisLevel);

        // 코루틴 타이머 관련 갱신
        StopAllCoroutines();
        c_time = null;

        // 코루틴 도는 중에는 타이머 텍스트 표시 갱신 무시
        if (sm.currentTimes[_index] == -1) 
            TimerBox.text = SiBunCho((int)sm.MaxTime(_index));

        // 글로우 숨김 델리게이트
        sm.chain += HideGrowEffect;
        HideGrowEffect();

        spriteBox.sprite = icons[_index];

        NameBox.text = ListModel.Instance.supList[_index].supporterName;
        thisLevel = int.Parse(ListModel.Instance.supList[_index].supporterLevel);
        LevelBox.text = T_LV + thisLevel;

        /// <다음_업그레이드비용> 배수 곱하기 해서 텍스트 뿌려주기.
        UpgradeBox.text = PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Soozip_Powerup_Gold * GetMutipleUpgrade(mutiple, thisLevel));

        /// <획득골드량> 배수 곱하기 해서 텍스트 뿌려주기.
        EarnGoldBox.text = T_PLUS + PlayerPrefsManager.instance.DoubleToStringNumber(PlayerInventory.Soozip_Gold_Earned * GetMutipleEarnGold(mutiple , thisLevel)) + " (" + mutiple + ")";

        /// 만렙 찍으면 맥스버튼 활성화.
        ///             /// 만렙 찍으면 맥스버튼 활성화.
        if (thisLevel >= 1000)
        {
            MaxButton.SetActive(true);
        }
        else
        {
            MaxButton.SetActive(false);
        }

        // 상태가 활성화 아니라면 회색 이미지 true
        if (ListModel.Instance.supList[_index].isEnable != "TRUE")
        {
            GrayImage.SetActive(true);
            slider.value = 0;

            if (_index == 0) Clicked_LvUP();
        }
        else
        {
            GrayImage.SetActive(false);
           /// 레벨 0 이면 게이지 코루틴 멈추고 리턴
            if (thisLevel == 0)
            {
                StopAllCoroutines();
                c_time = null;
                slider.value = 0;
                return;
            }

            // 영구 타이머 미작동시 한번 실행
            ///  골드획득 영구 코루틴 작동 (0~29)
            sm.DieHardCoTimer(_index);
            // 단일 타이머 미작동시 한번 실행
            if (c_time == null && gameObject.activeSelf && int.Parse(name) == _index)
            {
                /// 슬라이더 코루틴
                c_time = StartCoroutine(TimerStart());
            }

        }

        // 첫 업데이트
        if (!isInit) isInit = true;
    }





    /// <summary>
    /// 시/분/초 로 나눠서 표기
    /// </summary>
    /// <param name="cnt"></param>
    /// <returns></returns>
    string SiBunCho(int cnt)
    {
        bun = cnt / 60;
        si = bun / 60;
        cho = cnt % 60;
        bun = bun % 60;

        timeStr = string.Format("{0:D2}:{1:D2}:{2:D2}", si, bun, cho);

        return timeStr;
    }


    /// <summary>
    /// 레벨업 버튼 클릭
    /// </summary>
    public void Clicked_LvUP()
    {
        if (MaxButton.activeSelf) return;

        /// 회색 음영일때 호출되면 음영만 벗겨냄
        if (ListModel.Instance.supList[_index].isEnable != "TRUE")
        {
            // 잠금 해제
            ListModel.Instance.Supporter_Unlock(_index);
        }
        else
        {
            // 글로우 모두 숨김
            sm.chain();
            // 이 오브젝트 글로우 표기
            glowEffect[0].SetActive(true);
            glowEffect[1].SetActive(true);


            if (thisLevel == 0 && !IsPurchaseable()) return;

            // 영구 타이머 미작동시 한번 실행
            if (sm.currentTimes[_index] == -1)
            {
                /// <다음_수집_해금 > 첫번째 이후에는, 이전 수집이 레벨 1 이상이 되면 잠금 해제. (한번 호출)
                sm.UnlockNextSoozip(_index + 1);
            }

            //  음영 벗겨졌고 회색 버튼이면 클릭 안되게.
            /// 돈 없으면 강화 안되게 막는 역할 || 맥스버튼이면
            if (DisableImage.sprite == BtnSprite[0] || MaxButton.activeSelf) return;

            /// 멀티플 적용 받고 골드 소모
            PlayerInventory.Money_Gold -= PlayerInventory.Soozip_Powerup_Gold * GetMutipleUpgrade(mutiple, thisLevel);
            /// 음영 없고 골드로 강화. +1 +10 +100
            thisLevel = (int)_MultiLv;
            /// 레벨업 맞음
            ListModel.Instance.Supporter_LvUP(_index, thisLevel);
            /// 새 깃털 수집 Lv.5 달성
            if (_index == 0 && PlayerPrefsManager.currentTutoIndex == 3) ListModel.Instance.TUTO_Update(3);
            /// 새 깃털 수집 Lv.20 달성
            if (_index == 0 && PlayerPrefsManager.currentTutoIndex == 5) ListModel.Instance.TUTO_Update(5);
            /// 하이에나 두개골 수집 레벨업 1회 진행
            if (_index == 1 && PlayerPrefsManager.currentTutoIndex == 12) ListModel.Instance.TUTO_Update(12, mutiple);
            /// 하이에나 두개골 수집 Lv. 10 달성
            if (_index == 1 && PlayerPrefsManager.currentTutoIndex == 19) ListModel.Instance.TUTO_Update(19);
            /// 새 부리 수집 Lv. 10 달성
            if (_index == 2 && PlayerPrefsManager.currentTutoIndex == 28) ListModel.Instance.TUTO_Update(28);
            /// 곰 발톱 수집 Lv. 10 달성
            if (_index == 3 && PlayerPrefsManager.currentTutoIndex == 42) ListModel.Instance.TUTO_Update(42);
            /// 사마귀 다리 수집 Lv.10 달성
            if (_index == 4 && PlayerPrefsManager.currentTutoIndex == 47) ListModel.Instance.TUTO_Update(47);

            /// 만렙 찍으면 맥스버튼 활성화.
            if (thisLevel >= 1000) MaxButton.SetActive(true);

            ///  수집 강화 카운터 1 올리기
            ListModel.Instance.DAYlist_Update(1);
            ///  업적  완료 카운트
            ListModel.Instance.ALLlist_Update(11, mutiple);
        }

        // 값 갱신 -> 레벨업은 이쪽에서 처리
        BoxInfoUpdate(_index);
        sm.RefleshAllitem();

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

    /// <summary>
    /// 서포트 이미지 슬라이더 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator TimerStart()
    {
        slider.value = 0;
        yield return null;

        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (ListModel.Instance.supList[_index].isEnable == "TRUE")
            {
                //sm.currentTimes[_index] += Time.deltaTime;

                slider.value = sm.currentTimes[_index] / sm.MaxTime(_index);
                TimerBox.text = SiBunCho(Mathf.CeilToInt(sm.MaxTime(_index) - sm.currentTimes[_index]));

                if (sm.currentTimes[_index] >= sm.MaxTime(_index))
                {
                    //sm.currentTimes[_index] = 0;
                    slider.value = 0;
                    TimerBox.text = SiBunCho(0);
                    
                    yield return new WaitForFixedUpdate();
                }
            }
            else
            {
                DisableImage.sprite = BtnSprite[0]; //  OFF 이미지
            }

        }

    }

    private void Update()
    {
        if (ListModel.Instance.supList[_index].isEnable == "TRUE")
        {
            /// TODO : 버튼 활성화 / 비활성화 전환.
            ChangeBtnColor();
        }
        else
        {
            if(DisableImage.sprite != BtnSprite[0]) DisableImage.sprite = BtnSprite[0]; //  OFF 이미지
        }
    }

    /// <summary>
    /// 살 수 있는 골드가 모였는지 체크
    /// </summary>
    bool IsPurchaseable()
    {
        if (PlayerInventory.Money_Gold >= PlayerInventory.Soozip_Powerup_Gold * GetMutipleUpgrade(mutiple, thisLevel)) return true;
        else return false;
    }


    /// <summary>
    /// 버튼 색 노랑 <-> 회색 체인지
    /// </summary>
    void ChangeBtnColor()
    {
        if(IsPurchaseable()) DisableImage.sprite = BtnSprite[1]; //  On 이미지\
        else if (DisableImage.sprite != BtnSprite[0]) DisableImage.sprite = BtnSprite[0]; //  OFF 이미지
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
