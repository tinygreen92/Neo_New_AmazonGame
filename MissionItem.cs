using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class MissionItem : MonoBehaviour
{
    
    public QuestManager qm;

    public Slider sd;

    public Text rewordText;
    public Text titleText;
    public Text descText;
    [Header("-버튼들")]
    public Image thisBtn;
    public GameObject maxBtn;
    public GameObject giftGetPop;
    public Text giftGetText;
    [Header("- 글로우 이펙트")]
    public GameObject[] glowEffect;

    private static string maxStr = " / ";
    private static string joy = "x";

    private int index;


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
        /// 업데이트
        UpdateMission();
    }

    /// <summary>
    /// 미션 아이템 새로고침
    /// </summary>
    public void UpdateMission()
    {
        /// 글로우 이펙트 
        qm.chain += HideGrowEffect;
        HideGrowEffect();

        index = int.Parse(name);

        ///                                                              top. 평생 미션일때 
        if (qm.isALLquest)
        {
            sd.wholeNumbers = false;
            /// 슬라이더 설정
            sd.maxValue = 1f;

            if (double.Parse(ListModel.Instance.missionALLlist[index].maxValue) 
                <= double.Parse(ListModel.Instance.missionALLlist[index].curentValue))
            {
                thisBtn.sprite = qm.BtnSprite[1];
                sd.value = 1f;
                RedDotManager.instance.RedDot[5].SetActive(true);
                /// 평생 미션 0 모두 받기 파랑 1
                qm.isAceptEnable = true;
                qm.dayAllBtnImg[1].sprite = qm.allBtnSprs[1];
                /// 보상 수령가능 아이템 최상단으로 재설정
                if (!qm.isAllaceptLoop) transform.SetAsFirstSibling();
            }
            else
            {
                thisBtn.sprite = qm.BtnSprite[0];
                sd.value = (float)(double.Parse(ListModel.Instance.missionALLlist[index].curentValue) / double.Parse(ListModel.Instance.missionALLlist[index].maxValue));
            }


            rewordText.text = joy + ListModel.Instance.missionALLlist[index].reword;
            if (index == 3 || index == 4 || index == 5)
            {
                descText.text = PlayerPrefsManager.instance.DoubleToStringNumber(double.Parse(ListModel.Instance.missionALLlist[index].curentValue))
                    + maxStr +
                    PlayerPrefsManager.instance.DoubleToStringNumber(double.Parse(ListModel.Instance.missionALLlist[index].maxValue));
            }
            else
            {
                descText.text = ListModel.Instance.missionALLlist[index].curentValue + maxStr + ListModel.Instance.missionALLlist[index].maxValue;
            }

            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            {
                if (index == 2)
                {
                    titleText.text = Lean.Localization.LeanLocalization.GetTranslationText("HotFix_Potion_Desc");
                }
                else
                {
                    titleText.text = ListModel.Instance.missionALLlist[index].korDesc;
                }
            }
            else
            {
                if (index == 2)
                {
                    titleText.text = Lean.Localization.LeanLocalization.GetTranslationText("HotFix_Potion_Desc");
                }
                else
                {
                    titleText.text = ListModel.Instance.missionALLlist[index].korDesc;
                }
            }
        }









        ///                                                           bot. 일일 미션일때
        else
        {
            sd.wholeNumbers = true;


            /// 일퀘 완료했을때 예외처리 -> 그레이 씌워줌
            if (ListModel.Instance.missionDAYlist[index].curentValue == "-1")
            {
                maxBtn.SetActive(true);
                transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                /// 완료된 아이템 최하단으로 재설정
                if (!qm.isAllaceptLoop) transform.SetAsLastSibling();
                /// 슬라이더 설정
                sd.maxValue = int.Parse(ListModel.Instance.missionDAYlist[index].maxValue);
                sd.value = int.Parse(ListModel.Instance.missionDAYlist[index].maxValue);
                rewordText.text = joy + ListModel.Instance.missionDAYlist[index].reword;
                descText.text = ListModel.Instance.missionDAYlist[index].maxValue + maxStr + ListModel.Instance.missionDAYlist[index].maxValue;
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                {
                    titleText.text = ListModel.Instance.missionDAYlist[index].korDesc;
                }
                else
                {
                    titleText.text = ListModel.Instance.missionDAYlist[index].engDesc;
                }
                return;
            }
            else
            {
                maxBtn.SetActive(false);
                transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                /// 슬라이더 설정
                sd.maxValue = int.Parse(ListModel.Instance.missionDAYlist[index].maxValue);
                sd.value = int.Parse(ListModel.Instance.missionDAYlist[index].curentValue);
                rewordText.text = joy + ListModel.Instance.missionDAYlist[index].reword;
                descText.text = ListModel.Instance.missionDAYlist[index].curentValue + maxStr + ListModel.Instance.missionDAYlist[index].maxValue;
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                {
                    titleText.text = ListModel.Instance.missionDAYlist[index].korDesc;
                }
                else
                {
                    titleText.text = ListModel.Instance.missionDAYlist[index].engDesc;
                }
            }

            /// 완료한 퀘 있니?
            if (ListModel.Instance.missionDAYlist[index].maxValue == ListModel.Instance.missionDAYlist[index].curentValue)
            {
                thisBtn.sprite = qm.BtnSprite[1];
                RedDotManager.instance.RedDot[5].SetActive(true);
                /// 일일 미션 0 모두 받기 파랑 1
                qm.isAceptEnable = true;
                qm.dayAllBtnImg[0].sprite = qm.allBtnSprs[1];
                /// 보상 수령가능 아이템 최상단으로 재설정
                if (!qm.isAllaceptLoop) transform.SetAsFirstSibling();
            }
            else
            {
                thisBtn.sprite = qm.BtnSprite[0];
            }

        }
    }

    /// <summary>
    ///  [Day 버튼에_달아주기] 퀘스트 완료하면 퀘스트 json 완료 + 보상 획득
    /// </summary>
    public void ClickedDayAceptBtn()
    {
        /// MAX 버튼이면 리턴
        if (maxBtn.activeSelf) return;
        // 글로우 모두 숨김
        qm.chain();
        // 이 오브젝트 글로우 표기
        glowEffect[0].SetActive(true);
        glowEffect[1].SetActive(true);
        /// 회색 버튼이면 리턴
        if (thisBtn.sprite == qm.BtnSprite[0]) return;
        /// 일퀘 리스트 초기화 해주시고
        ListModel.Instance.DAYlist_Update(index, -1);
        /// 보상 팝업 
        //giftGetText.text = joy+ListModel.Instance.missionDAYlist[index].reword;
        //giftGetPop.SetActive(true);
        PlayerInventory.Money_Dia +=int.Parse(ListModel.Instance.missionDAYlist[index].reword);
        /// 맥스버튼
        maxBtn.SetActive(true);
        /// 그레이 패널 활성화
        transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        /// 일퀘 미션 모두 완료 한 칸 올려
        ListModel.Instance.DAYlist_Update(0);
        /// 레드닷 끄기
        RedDotManager.instance.RedDot[5].SetActive(false);
        /// 일일 미션 0 모두 받기 회색  0
        qm.dayAllBtnImg[0].sprite = qm.allBtnSprs[0];
        /// 새로고침
        qm.QMc5Update();
        /// 스트링[] 몽땅 저장
        PlayerPrefsManager.instance.TEST_SaveJson();
    }

    /// <summary>
    ///  [All 버튼에_달아주기] 퀘스트 완료하면 퀘스트 json 완료 + 보상 획득
    /// </summary>
    public void ClickedAllAceptBtn()
    {
        // 글로우 모두 숨김
        qm.chain();
        // 이 오브젝트 글로우 표기
        glowEffect[0].SetActive(true);
        glowEffect[1].SetActive(true);
        /// 회색 버튼이면 리턴
        if (thisBtn.sprite == qm.BtnSprite[0]) return;

        /// Max 확장해주시고 (내부에서 curentValue 빼줌)
        ListModel.Instance.ALLlist_Max_Update(index);
        /// 보상 지급
        PlayerInventory.Money_Dia += int.Parse(ListModel.Instance.missionALLlist[index].reword);
        //giftGetText.text = joy + ListModel.Instance.missionALLlist[index].reword;
        //giftGetPop.SetActive(true);
        /// 레드닷 끄기
        RedDotManager.instance.RedDot[5].SetActive(false);
        /// 평생 미션 1 모두 받기 회색  0
        qm.dayAllBtnImg[1].sprite = qm.allBtnSprs[0];
        /// 새로고침
        qm.QMc17Update();
        /// 스트링[] 몽땅 저장
        PlayerPrefsManager.instance.TEST_SaveJson();
    }


}
