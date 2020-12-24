using Lean.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
///  유물 (Heart) 아이템 박스에서만 붙어있을 스크립트.
/// </summary>
public class RuntItem : MonoBehaviour
{
    public RuneManager sm;
    [Header("- 아이콘")]
    public Image spriteBox;
    [Header("- 정보 표기 부분")]
    public GameObject[] MotherCard;
    public Text[] NameBox;
    public Text[] DescBox;                                  // 4개 박스
    [Header("- 버튼 색 바꾸기")]
    public Image DisableImage;
    public Sprite[] BtnSprite;

    int _index = 0;
    string _rank = "NULL";

    /// <summary>
    /// 박스 SetActive(true)일때 호출
    /// 변경 값 갱신 해줌
    /// 1. 아우터 글로우 해제
    /// 2. 이 함수는 룬매니저에서 랜덤 룬 생성후에 호출 된다.
    /// </summary>
    public void BoxInfoUpdate(int cnt)
    {
        if (ListModel.Instance.runeList.Count <= cnt) return;

        /// 디스크립션 비활성화로 새로고침
        for (int i = 0; i < MotherCard.Length; i++)
        {
            MotherCard[i].SetActive(false);
        }

        /// 인덱스 설정 -> 이 스크립트 전체
        _index = cnt;

        /// 랭크 판단
        _rank = ListModel.Instance.runeList[_index].rank;

        /// 개당 텍스트박스랑 이미지 세팅
        switch (_rank)
        {
            case "B":
                spriteBox.sprite = int.Parse(ListModel.Instance.runeList[_index].imgIndex) < 10 ?
                        sm.B_con[int.Parse(ListModel.Instance.runeList[_index].imgIndex)-5] : sm.Etc_con;

                NameBox[3].text = ListModel.Instance.runeList[_index].desc_1;
                DescBox[3].text = ListModel.Instance.runeList[_index].main_1.ToString("N1") + "%";

                MotherCard[3].SetActive(true);
                break;

            case "A":
                spriteBox.sprite = sm.A_cons[int.Parse(ListModel.Instance.runeList[_index].imgIndex)];

                NameBox[3].text = ListModel.Instance.runeList[_index].desc_1;
                DescBox[3].text = ListModel.Instance.runeList[_index].main_1.ToString("N1") + "%";

                MotherCard[3].SetActive(true);
                break;

            case "S":
                spriteBox.sprite = sm.S_cons[int.Parse(ListModel.Instance.runeList[_index].imgIndex)];

                NameBox[0].text = ListModel.Instance.runeList[_index].desc_1;
                DescBox[0].text = ListModel.Instance.runeList[_index].main_1.ToString("N1") + "%";
                NameBox[2].text = ListModel.Instance.runeList[_index].desc_2;
                DescBox[2].text = ListModel.Instance.runeList[_index].main_2.ToString("N1") + "%";

                MotherCard[0].SetActive(true);
                MotherCard[2].SetActive(true);
                break;

            case "L":
                spriteBox.sprite = sm.L_cons[int.Parse(ListModel.Instance.runeList[_index].imgIndex)];

                NameBox[0].text = ListModel.Instance.runeList[_index].desc_1;
                DescBox[0].text = ListModel.Instance.runeList[_index].main_1.ToString("N1") + "%";
                NameBox[1].text = ListModel.Instance.runeList[_index].desc_2;
                DescBox[1].text = ListModel.Instance.runeList[_index].main_2.ToString("N1") + "%";
                NameBox[2].text = ListModel.Instance.runeList[_index].desc_3;
                DescBox[2].text = ListModel.Instance.runeList[_index].sub_1.ToString("N1") + "%";

                MotherCard[0].SetActive(true);
                MotherCard[1].SetActive(true);
                MotherCard[2].SetActive(true);
                break;

            case "R":
                spriteBox.sprite = sm.R_cons[int.Parse(ListModel.Instance.runeList[_index].imgIndex)];

                NameBox[1].text = ListModel.Instance.runeList[_index].desc_1;
                DescBox[1].text = ListModel.Instance.runeList[_index].main_1.ToString("N1") + "%";
                NameBox[3].text = ListModel.Instance.runeList[_index].desc_2;
                DescBox[3].text = ListModel.Instance.runeList[_index].main_2.ToString("N1") + "%";
                NameBox[4].text = ListModel.Instance.runeList[_index].desc_3;
                DescBox[4].text = ListModel.Instance.runeList[_index].sub_1.ToString("N1") + "%";
                NameBox[5].text = ListModel.Instance.runeList[_index].desc_4;
                DescBox[5].text = ListModel.Instance.runeList[_index].sub_2.ToString("N1") + "%";

                MotherCard[1].SetActive(true);
                MotherCard[3].SetActive(true);
                MotherCard[4].SetActive(true);
                MotherCard[5].SetActive(true);
                break;

            default: return;
        }
        /// 외부에서도 호출 + 새로 리스트 생성될 때도 호출
        ChangeInvenFusion();
    }

    /// <summary>
    /// 인피니티 뷰 새로고침 될때 호출
    /// </summary>
    public void ChangeInvenFusion()
    {
        if (ListModel.Instance.runeList.Count <= _index) return;

        if (PlayerPrefsManager.isRuneFussionTab) /// ----------------------------- 조합 ---------------------------
        {
            /// <조합> 룬 미들 뷰 이미지 초기화.
            if (PlayerPrefsManager.isRuneInit) sm.ShowFusionIcon();
            else
            {
                /// 미들 창 비워주기
                for (int i = 0; i < sm.IconPannel.childCount; i++)
                {
                    sm.IconPannel.GetChild(i).GetChild(1).gameObject.SetActive(false);
                }
            }

            /// 장착중이다 = 회색 이미지 표시중. -> 얘는 퓨전에 못 집어넣음.
            /// 장착중이라면 새로고침 해도 장착중 버튼 유지
            if (ListModel.Instance.runeList[_index].isEquip == "TRUE") ChangeButtonColorText("착용중");
            else if (ListModel.Instance.runeList[_index].isEquip == "FUSION") ChangeButtonColorText("해제");
            else ChangeButtonColorText("선택");
        }
        else                                                                    /// ----------------------------- 보관 ---------------------------
        {
            /// 룬 미들 뷰 이미지 초기화.
            if (PlayerPrefsManager.isRuneInit) sm.ShowIconFive();
            else InitRune();
            /// 퓨전 항목 리스트 초기화
            sm.fussionRuneList.Clear();
            sm.fussionSprList.Clear();
            sm.fusionIndex.Clear();
            sm.ResetGatchaCount();
            /// <조합> 에 쓰인 재료라면 장착해제 = FALSE 로 혼내준다
            for (int i = 0; i < sm.fusionIndex.Count; i++)
            {
                ListModel.Instance.Rune_ResetFromFussion(sm.fusionIndex[i], false);
            }

            /// 장착중이라면 새로고침 해도 장착중 버튼 유지
            if (ListModel.Instance.runeList[_index].isEquip == "TRUE") ChangeButtonColorText("해제");
            else if (ListModel.Instance.runeList[_index].isEquip == "FUSION")
            {
                ListModel.Instance.Rune_ResetFromFussion(_index, false);
                ChangeButtonColorText("착용");
            }
            else ChangeButtonColorText("착용");


        }
    }

    /// <summary>
    /// 장착한 룬 리스트와 미들 뷰에 표기되는 이미지 초기화.
    /// </summary>
    public void InitRune()
    {
        if (PlayerPrefsManager.isRuneInit) return;
        Debug.LogWarning("룬 초기화에욧");
        /// 초기화 한번 했다.
        for (int i = 0; i < sm.IconPannel.childCount; i++)
        {
            sm.IconPannel.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
        /// 초기화 한번 했다.
        PlayerPrefsManager.isRuneInit = true;
        /// 룬이 하나도 장착되지 않았다면 리턴하고
        if (ListModel.Instance.equipRuneList.Count == 0) return;

        /// 장착된 룬이 존재한다면?
        /// 미들 뷰에 이미지 실제로 표기해주기
        sm.ShowIconFive();
    }


    /// <summary>
    /// 착용 / 해제 버튼 클릭
    /// 1. 보관함이냐? 조합창이냐? 판단
    /// 2. 보관함의 경우 - > 리스트에 isEquip = true 면 착용처리 + 탑뷰에 이미지 등록 해주고 적절한 버튼 처리
    /// 3. 보관함 - 보기 버튼 등록된 룬 표기.
    /// 4. 조합창의 경우 모든 등록 이미지 해제해주고 버튼도 해지
    /// 5. 조합창 조합 누르면 팝업 뜨고 퓨전 처리 (리스트 5개 요소 삭제후 1개 생성) + 어떤 등급 넣었냐에 따라 합성확륙 UP
    /// </summary>
    public void Clicked_Rune()
    {
        /// 초기화 안했으면 리턴.
        if (!PlayerPrefsManager.isRuneInit) return;

        if (PlayerPrefsManager.isRuneFussionTab) /// ----------------------------- 조합 ---------------------------
        {
            /// 장착중이 아니다 -> 노란 버튼 -> 퓨전에 사용될 수 있음.
            if (DisableImage.sprite != BtnSprite[0])
            {
                /// 예외 처리 (이미 5개가 꽉 찼다.)
                if (sm.fussionSprList.Count >= 5) return;

                ///버튼 "해제" 텍스트로 바꿔줌 / 이미지도
                ChangeButtonColorText("해제");
                /// <조합> isEquip 옵션 바꿔주기 "해제" 
                ListModel.Instance.Rune_ResetFromFussion(_index, true);
                /// <조합> 목록에 내부 리스트 복사해주기.
                sm.fussionRuneList.Add(ListModel.Instance.runeList[_index]);
                sm.fusionIndex.Add(_index);
                sm.SetGatchaCount(ListModel.Instance.runeList[_index].rank, true);
                /// <조합> 목록에 이미지 추가해주기.
                sm.fussionSprList.Add(spriteBox.sprite);
                /// <조합> 미들 뷰에 이미지 실제로 표기해주기
                sm.ShowFusionIcon();
            }
            else                                       /// 퓨전 재료로 쓰일건데 누르면 빼줄거
            {
                /// 예외 처리 (하나도 없다.)
                if (sm.fussionSprList.Count == 0) return;
                /// 예외 처리 (이미 장착중이면)
                if (ListModel.Instance.runeList[_index].isEquip == "TRUE") return;
                /// <조합>버튼 "해제" 텍스트로 바꿔줌 / 이미지도
                ChangeButtonColorText("선택");
                /// 장착 목록에 내부 리스트 제거
                sm.fussionRuneList.Remove(ListModel.Instance.runeList[_index]);
                sm.fusionIndex.Remove(_index);
                /// isEquip 옵션 바꿔주기 "선택" 
                ListModel.Instance.Rune_ResetFromFussion(_index, false);
                sm.SetGatchaCount(ListModel.Instance.runeList[_index].rank, false);
                /// 장착 목록에 이미지 제거
                sm.fussionSprList.Remove(spriteBox.sprite);
                /// <조합> 미들 뷰에 남은 이미지 실제로 표기해주기
                sm.ShowFusionIcon();
            }
        }
        else /// ----------------------------- 보관함 --------------------------------
        {
            /// 장착중이다 = 회색 이미지 표시중.
            if (DisableImage.sprite == BtnSprite[0])                /// 의도 했던 것
            {
                /// 예외 처리 (하나도 없다.)
                if (sm.saveSprList.Count == 0) return;

                /// 장착 목록에 내부 리스트 제거
                ListModel.Instance.equipRuneList.Remove(ListModel.Instance.runeList[_index]);
                /// 룬 제거시 스탯 새로 고침
                ListModel.Instance.SetEquipedRuneEffect();
                ///버튼 "해제" 텍스트로 바꿔줌 / 이미지도
                ChangeButtonColorText("착용");
                /// isEquip 옵션 바꿔주기 "안착용중" 
                ListModel.Instance.Rune_Equip(_index, false);
                /// 장착 목록에 이미지 제거
                sm.saveSprList.Remove(spriteBox.sprite);
                /// 미들 뷰에 남은 이미지 실제로 표기해주기
                sm.ShowIconFive();

            }
            else                         /// 해제중이다 = 파란 이미지 표시중.
            {
                /// 예외 처리 (이미 5개가 꽉 찼다.)
                if (sm.saveSprList.Count >= 5) return;

                ///버튼 "해제" 텍스트로 바꿔줌 / 이미지도
                ChangeButtonColorText("해제");
                /// isEquip 옵션 바꿔주기 "착용중" 
                ListModel.Instance.Rune_Equip(_index, true);
                /// 장착 목록에 내부 리스트 복사해주기.
                ListModel.Instance.equipRuneList.Add(ListModel.Instance.runeList[_index]);
                /// 장착 된 룬 만큼 스탯 올려주기
                ListModel.Instance.SetEquipedRuneEffect();
                /// 장착 목록에 이미지 추가해주기.
                sm.saveSprList.Add(spriteBox.sprite);
                /// 미들 뷰에 이미지 실제로 표기해주기
                sm.ShowIconFive();
                /// 룬 착용하기
                if (PlayerPrefsManager.currentTutoIndex == 33) ListModel.Instance.TUTO_Update(33);
            }

        }
        /// -----Clicked_Rune() ---- End
    }



    /// <summary>
    /// 룬 버튼 클릭시 착용/선택/해제 관리
    /// </summary>
    /// <param name="_txt">착용/선택/해제</param>
    void ChangeButtonColorText(string _txt)
    {
        var textBox = DisableImage.transform.GetChild(0);

        switch (_txt)
        {
            case "착용":
                textBox.GetComponent<Text>().text = LeanLocalization.GetTranslationText("Category_Rune_Equip");
                DisableImage.sprite = BtnSprite[1];
                break;
            case "선택":
                textBox.GetComponent<Text>().text = LeanLocalization.GetTranslationText("Category_Rune_Select");
                DisableImage.sprite = BtnSprite[1];
                break;
            case "해제":
                textBox.GetComponent<Text>().text = LeanLocalization.GetTranslationText("Category_Rune_UnEquip");
                DisableImage.sprite = BtnSprite[0];
                break;
            case "착용중":
                textBox.GetComponent<Text>().text = LeanLocalization.GetTranslationText("Category_Rune_isEquip");
                DisableImage.sprite = BtnSprite[0];
                break;

            default:
                textBox.GetComponent<Text>().text = LeanLocalization.GetTranslationText("Category_Rune_Equip");
                DisableImage.sprite = BtnSprite[1];
                break;
        }

    }


}
