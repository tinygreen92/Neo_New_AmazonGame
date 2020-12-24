using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public NestedScrollManager nsm;
    public PopInventory pi;
    [Header("- 풀 버튼 내용물")]
    public GameObject fullBtnLayout;
    public GameObject fullBtnExit;
    public GameObject fullBtnExitText;
    [Header("- 미니팝업 내용물")]
    public Text megaAmount;
    public Image miniImg;
    public Text miniAmount;
    public Text miniDesc;
    [Header("- 상자 뽑기 이미지들")]
    public Sprite[] HeartSprs;
    [Header("- 획득 창 / 조합 창")]
    public Image GetRuneImg;
    public Text RankText;
    public Text[] DescTexts;
    public Transform BotImg;

    /// <summary>
    /// 상단 탭으로 변화하는 내부 컨텐츠 내용물 장착/강화/합성/초월 0 1 2 3
    /// </summary>
    public int ModeName;
    [Header("- 무기 이미지들")]
    public Sprite[] WeaponSprs;
    public Sprite[] blackSprs;

    public delegate void ChainFunc();       // 아웃라인 델리게이트
    public ChainFunc chain;                 // 체인 메서드
    /// <summary>
    /// 가중치 랜덤에서 우승한 놈
    /// </summary>
    float returnValue;

    /// <summary>
    /// 무기 갯수 증가 이후 새로 고침 해줘야 한다.
    /// 무기 합성 이후에도 새로고침 하자구
    /// </summary>
    public void RefreshWeapon()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<WeaponItem>()
                .BoxInfoUpdate(int.Parse(transform.GetChild(i).name));
        }
    }


    /// <summary>
    /// 인벤토리 창에서 쿠폰 아이콘 클릭할때 팝업 표기
    /// </summary>
    public void ShowCouponPop()
    {
        if (PlayerInventory.box_Coupon < 1) return;

        megaAmount.text = "x" + PlayerInventory.box_Coupon.ToString("N0");
        PopUpManager.instance.ShowPopUP(16);
    }


    string tmpBoxIndex = "";
    /// <summary>
    /// 인벤토리 창에서 각각의 상자 클릭할때 해당 상자 그림과 텍스트 표기
    /// </summary>
    /// <param name="_index"></param>
    public void ShowMiniBoxPop(int _index)
    {
        miniImg.sprite = HeartSprs[_index];
        switch (_index)
        {
            case 0:
                if (PlayerInventory.box_E < 1) return;
                miniAmount.text = "x" +PlayerInventory.box_E.ToString("N0");
                miniDesc.text = Lean.Localization.LeanLocalization.GetTranslationText("Weapon_Get_Box_E");
                tmpBoxIndex = "E";
                break;
            case 1:
                if (PlayerInventory.box_D < 1) return;
                miniAmount.text = "x" + PlayerInventory.box_D.ToString("N0");
                miniDesc.text = Lean.Localization.LeanLocalization.GetTranslationText("Weapon_Get_Box_D");
                tmpBoxIndex = "D";
                break;
            case 2:
                if (PlayerInventory.box_C < 1) return;
                miniAmount.text = "x" + PlayerInventory.box_C.ToString("N0");
                miniDesc.text = Lean.Localization.LeanLocalization.GetTranslationText("Weapon_Get_Box_C");
                tmpBoxIndex = "C";
                break;
            case 3:
                if (PlayerInventory.box_B < 1) return;
                miniAmount.text = "x" + PlayerInventory.box_B.ToString("N0");
                miniDesc.text = Lean.Localization.LeanLocalization.GetTranslationText("Weapon_Get_Box_B");
                tmpBoxIndex = "B";
                break;
            case 4:
                if (PlayerInventory.box_A < 1) return;
                miniAmount.text = "x" + PlayerInventory.box_A.ToString("N0");
                miniDesc.text = Lean.Localization.LeanLocalization.GetTranslationText("Weapon_Get_Box_A");
                tmpBoxIndex = "A";
                break;
            case 5:
                if (PlayerInventory.box_S < 1) return;
                miniAmount.text = "x" + PlayerInventory.box_S.ToString("N0");
                miniDesc.text = Lean.Localization.LeanLocalization.GetTranslationText("Weapon_Get_Box_S");
                tmpBoxIndex = "S";
                break;
            case 6:
                if (PlayerInventory.box_L < 1) return;
                miniAmount.text = "x" + PlayerInventory.box_L.ToString("N0");
                miniDesc.text = Lean.Localization.LeanLocalization.GetTranslationText("Weapon_Get_Box_L");
                tmpBoxIndex = "L";
                break;
            default:  break;
        }

        PopUpManager.instance.ShowPopUP(17);
    }
    /// <summary>
    /// 무기 박스 알파벳 넣기 -> 미니 팝업 버튼에 달아주기 
    /// </summary>
    public void Gatcha_Box()
    {
        isTooMuchOpen = false;
        _isWeapGatcha = true;
        GatCha_WeaponBox(tmpBoxIndex);
    }
    bool isTooMuchOpen;
    /// <summary>
    /// 10번 뽑기 미니 팝업에 연결
    /// </summary>
    public void Gatcha_Box_10X()
    {
        isTooMuchOpen = true;
        _isWeapGatcha = true;
        GatCha_WeaponBox(tmpBoxIndex);
    }
    public void Gatcha_Coupon_1X()
    {
        isTooMuchOpen = false;
        _isWeapGatcha = false;
        GatCha_WeaponBox("");
    }
    public void Gatcha_Coupon_10X()
    {
        isTooMuchOpen = true;
        _isWeapGatcha = false;
        GatCha_WeaponBox("");
    }

    private string localIndex;
    /// <summary>
    ///  버튼에 달린 공백이면 무기 티켓 뽑기
    /// </summary>
    public void GatCha_WeaponBox(string _index)
    {
        /// 박스 모두까기 횟수
        int tmpCnt = 0;
        //
        localIndex = _index;
        System.Random seedRnd = new System.Random();
        int startIndex = seedRnd.Next();
        /// 가중치 설정 합계가 100f 가 되도록.
        switch (_index)
        {
            case "E":
                float[] probs_E = new float[] { 10f, 9f, 8f, 7f, 6.5f, 6f, 5.5f, 5f, 4.5f, 4f, 3.55f, 3.35f, 3.15f, 2.95f, 2.75f, 2.55f, 2.35f, 2.15f, 1.95f, 1.75f, 1.55f, 1.35f, 1.15f, 1.05f, 0.95f, 0.75f, 0.55f, 0.35f, 0.15f, 0.1f, 0.05f };
                if (isTooMuchOpen)
                {
                    if (PlayerInventory.box_E < 1) return;
                    /// 모두 열기
                    tmpCnt = PlayerInventory.box_E;
                    for (int i = 0; i < tmpCnt; i++)
                    {
                        returnValue = PlayerPrefsManager.instance.GetRandom(probs_E, startIndex++);
                        switch (returnValue)
                        {
                            case 10f: tmpStringList.Add(0); break;
                            case 9f: tmpStringList.Add(1); break;
                            case 8f: tmpStringList.Add(2); break;
                            case 7f: tmpStringList.Add(3); break;
                            case 6.5f: tmpStringList.Add(4); break;
                            case 6f: tmpStringList.Add(5); break;
                            case 5.5f: tmpStringList.Add(6); break;
                            case 5f: tmpStringList.Add(7); break;
                            case 4.5f: tmpStringList.Add(8); break;
                            case 4f: tmpStringList.Add(9); break;
                            case 3.55f: tmpStringList.Add(10); break;
                            case 3.35f: tmpStringList.Add(11); break;
                            case 3.15f: tmpStringList.Add(12); break;
                            case 2.95f: tmpStringList.Add(13); break;
                            case 2.75f: tmpStringList.Add(14); break;
                            case 2.55f: tmpStringList.Add(15); break;
                            case 2.35f: tmpStringList.Add(16); break;
                            case 2.15f: tmpStringList.Add(17); break;
                            case 1.95f: tmpStringList.Add(18); break;
                            case 1.75f: tmpStringList.Add(19); break;
                            case 1.55f: tmpStringList.Add(20); break;
                            case 1.35f: tmpStringList.Add(21); break;
                            case 1.15f: tmpStringList.Add(22); break;
                            case 1.05f: tmpStringList.Add(23); break;
                            case 0.95f: tmpStringList.Add(24); break;
                            case 0.75f: tmpStringList.Add(25); break;
                            case 0.55f: tmpStringList.Add(26); break;
                            case 0.35f: tmpStringList.Add(27); break;
                            case 0.15f: tmpStringList.Add(28); break;
                            case 0.1f: tmpStringList.Add(29); break;
                            case 0.05f: tmpStringList.Add(30); break;
                            default: tmpStringList.Add(0); break;
                        }
                        /// 무기뽑기 카운터 1 올리기
                        ListModel.Instance.DAYlist_Update(3);
                        ///  업적  완료 카운트
                        ListModel.Instance.ALLlist_Update(12, 1);
                    }
                    /// 반복문 끝나고 인덱스 0 호출
                    OpenWeaponBox(_index, tmpStringList[0]);
                }
                else
                {
                    returnValue = PlayerPrefsManager.instance.GetRandom(probs_E, startIndex);
                    switch (returnValue)
                    {
                        case 10f: OpenWeaponBox(_index, 0); break;
                        case 9f: OpenWeaponBox(_index, 1); break;
                        case 8f: OpenWeaponBox(_index, 2); break;
                        case 7f: OpenWeaponBox(_index, 3); break;
                        case 6.5f: OpenWeaponBox(_index, 4); break;
                        case 6f: OpenWeaponBox(_index, 5); break;
                        case 5.5f: OpenWeaponBox(_index, 6); break;
                        case 5f: OpenWeaponBox(_index, 7); break;
                        case 4.5f: OpenWeaponBox(_index, 8); break;
                        case 4f: OpenWeaponBox(_index, 9); break;
                        case 3.55f: OpenWeaponBox(_index, 10); break;
                        case 3.35f: OpenWeaponBox(_index, 11); break;
                        case 3.15f: OpenWeaponBox(_index, 12); break;
                        case 2.95f: OpenWeaponBox(_index, 13); break;
                        case 2.75f: OpenWeaponBox(_index, 14); break;
                        case 2.55f: OpenWeaponBox(_index, 15); break;
                        case 2.35f: OpenWeaponBox(_index, 16); break;
                        case 2.15f: OpenWeaponBox(_index, 17); break;
                        case 1.95f: OpenWeaponBox(_index, 18); break;
                        case 1.75f: OpenWeaponBox(_index, 19); break;
                        case 1.55f: OpenWeaponBox(_index, 20); break;
                        case 1.35f: OpenWeaponBox(_index, 21); break;
                        case 1.15f: OpenWeaponBox(_index, 22); break;
                        case 1.05f: OpenWeaponBox(_index, 23); break;
                        case 0.95f: OpenWeaponBox(_index, 24); break;
                        case 0.75f: OpenWeaponBox(_index, 25); break;
                        case 0.55f: OpenWeaponBox(_index, 26); break;
                        case 0.35f: OpenWeaponBox(_index, 27); break;
                        case 0.15f: OpenWeaponBox(_index, 28); break;
                        case 0.1f: OpenWeaponBox(_index, 29); break;
                        case 0.05f: OpenWeaponBox(_index, 30); break;
                        default: OpenWeaponBox(_index, 0); break;
                    }
                    /// 무기뽑기 카운터 1 올리기
                    ListModel.Instance.DAYlist_Update(3);
                    ///  업적  완료 카운트
                    ListModel.Instance.ALLlist_Update(12, 1);
                }

                break;

            case "D":
                float[] probs_D = new float[] { 14f, 12.5f, 11f, 9.5f, 8f, 7.55f, 6.85f, 5.15f, 3.95f, 2.75f, 2.55f, 2.35f, 2.15f, 1.95f, 1.75f, 1.55f, 1.35f, 1.15f, 1.05f, 0.95f, 0.75f, 0.55f, 0.35f, 0.15f, 0.1f, 0.05f };
                if (isTooMuchOpen)
                {
                    if (PlayerInventory.box_D < 1) return;
                    /// 모두 열기
                    tmpCnt = PlayerInventory.box_D;
                    for (int i = 0; i < tmpCnt; i++)
                    {
                        returnValue = PlayerPrefsManager.instance.GetRandom(probs_D, startIndex++);
                        switch (returnValue)
                        {
                            case 14f: tmpStringList.Add(5); break;
                            case 12.5f: tmpStringList.Add(6); break;
                            case 11f: tmpStringList.Add(7); break;
                            case 9.5f: tmpStringList.Add(8); break;
                            case 8f: tmpStringList.Add(9); break;
                            case 7.55f: tmpStringList.Add(10); break;
                            case 6.85f: tmpStringList.Add(11); break;
                            case 5.15f: tmpStringList.Add(12); break;
                            case 3.95f: tmpStringList.Add(13); break;
                            case 2.75f: tmpStringList.Add(14); break;
                            case 2.55f: tmpStringList.Add(15); break;
                            case 2.35f: tmpStringList.Add(16); break;
                            case 2.15f: tmpStringList.Add(17); break;
                            case 1.95f: tmpStringList.Add(18); break;
                            case 1.75f: tmpStringList.Add(19); break;
                            case 1.55f: tmpStringList.Add(20); break;
                            case 1.35f: tmpStringList.Add(21); break;
                            case 1.15f: tmpStringList.Add(22); break;
                            case 1.05f: tmpStringList.Add(23); break;
                            case 0.95f: tmpStringList.Add(24); break;
                            case 0.75f: tmpStringList.Add(25); break;
                            case 0.55f: tmpStringList.Add(26); break;
                            case 0.35f: tmpStringList.Add(27); break;
                            case 0.15f: tmpStringList.Add(28); break;
                            case 0.1f: tmpStringList.Add(29); break;
                            case 0.05f: tmpStringList.Add(30); break;
                            default: tmpStringList.Add(5); break;
                        }
                        /// 무기뽑기 카운터 1 올리기
                        ListModel.Instance.DAYlist_Update(3);
                        ///  업적  완료 카운트
                        ListModel.Instance.ALLlist_Update(12, 1);
                    }

                    /// 반복문 끝나고 인덱스 0 호출
                    OpenWeaponBox(_index, tmpStringList[0]);
                }
                else
                {
                    returnValue = PlayerPrefsManager.instance.GetRandom(probs_D, startIndex);
                    Debug.LogError("뽑힌거 : " + returnValue.ToString("F2"));
                    switch (returnValue)
                    {
                        case 14f: OpenWeaponBox(_index, 5); break;
                        case 12.5f: OpenWeaponBox(_index, 6); break;
                        case 11f: OpenWeaponBox(_index, 7); break;
                        case 9.5f: OpenWeaponBox(_index, 8); break;
                        case 8f: OpenWeaponBox(_index, 9); break;
                        case 7.55f: OpenWeaponBox(_index, 10); break;
                        case 6.85f: OpenWeaponBox(_index, 11); break;
                        case 5.15f: OpenWeaponBox(_index, 12); break;
                        case 3.95f: OpenWeaponBox(_index, 13); break;
                        case 2.75f: OpenWeaponBox(_index, 14); break;
                        case 2.55f: OpenWeaponBox(_index, 15); break;
                        case 2.35f: OpenWeaponBox(_index, 16); break;
                        case 2.15f: OpenWeaponBox(_index, 17); break;
                        case 1.95f: OpenWeaponBox(_index, 18); break;
                        case 1.75f: OpenWeaponBox(_index, 19); break;
                        case 1.55f: OpenWeaponBox(_index, 20); break;
                        case 1.35f: OpenWeaponBox(_index, 21); break;
                        case 1.15f: OpenWeaponBox(_index, 22); break;
                        case 1.05f: OpenWeaponBox(_index, 23); break;
                        case 0.95f: OpenWeaponBox(_index, 24); break;
                        case 0.75f: OpenWeaponBox(_index, 25); break;
                        case 0.55f: OpenWeaponBox(_index, 26); break;
                        case 0.35f: OpenWeaponBox(_index, 27); break;
                        case 0.15f: OpenWeaponBox(_index, 28); break;
                        case 0.1f: OpenWeaponBox(_index, 29); break;
                        case 0.05f: OpenWeaponBox(_index, 30); break;
                        default: OpenWeaponBox(_index, 5); break;
                    }
                    /// 무기뽑기 카운터 1 올리기
                    ListModel.Instance.DAYlist_Update(3);
                    ///  업적  완료 카운트
                    ListModel.Instance.ALLlist_Update(12, 1);
                }

                break;

            case "C":
                float[] probs_C = new float[] { 17.55f, 15.85f, 13.15f, 10.95f, 8.75f, 7.55f, 6.35f, 5.15f, 3.95f, 2.75f, 1.55f, 1.35f, 1.15f, 1.05f, 0.95f, 0.75f, 0.55f, 0.35f, 0.15f, 0.1f, 0.05f };
                if (isTooMuchOpen)
                {
                    if (PlayerInventory.box_C < 1) return;
                    /// 모두 열기
                    tmpCnt = PlayerInventory.box_C;
                    for (int i = 0; i < tmpCnt; i++)
                    {
                        returnValue = PlayerPrefsManager.instance.GetRandom(probs_C, startIndex++);
                        switch (returnValue)
                        {
                            case 17.55f: tmpStringList.Add(10); break;
                            case 15.85f: tmpStringList.Add(11); break;
                            case 13.15f: tmpStringList.Add(12); break;
                            case 10.95f: tmpStringList.Add(13); break;
                            case 8.75f: tmpStringList.Add(14); break;
                            case 7.55f: tmpStringList.Add(15); break;
                            case 6.35f: tmpStringList.Add(16); break;
                            case 5.15f: tmpStringList.Add(17); break;
                            case 3.95f: tmpStringList.Add(18); break;
                            case 2.75f: tmpStringList.Add(19); break;
                            case 1.55f: tmpStringList.Add(20); break;
                            case 1.35f: tmpStringList.Add(21); break;
                            case 1.15f: tmpStringList.Add(22); break;
                            case 1.05f: tmpStringList.Add(23); break;
                            case 0.95f: tmpStringList.Add(24); break;
                            case 0.75f: tmpStringList.Add(25); break;
                            case 0.55f: tmpStringList.Add(26); break;
                            case 0.35f: tmpStringList.Add(27); break;
                            case 0.15f: tmpStringList.Add(28); break;
                            case 0.1f: tmpStringList.Add(29); break;
                            case 0.05f: tmpStringList.Add(30); break;
                            default: tmpStringList.Add(10); break;
                        }
                        /// 무기뽑기 카운터 1 올리기
                        ListModel.Instance.DAYlist_Update(3);
                        ///  업적  완료 카운트
                        ListModel.Instance.ALLlist_Update(12, 1);
                    }

                    /// 반복문 끝나고 인덱스 0 호출
                    OpenWeaponBox(_index, tmpStringList[0]);
                }
                else
                {
                    returnValue = PlayerPrefsManager.instance.GetRandom(probs_C, startIndex);
                    Debug.LogError("뽑힌거 : " + returnValue.ToString("F2"));
                    switch (returnValue)
                    {
                        case 17.55f: OpenWeaponBox(_index, 10); break;
                        case 15.85f: OpenWeaponBox(_index, 11); break;
                        case 13.15f: OpenWeaponBox(_index, 12); break;
                        case 10.95f: OpenWeaponBox(_index, 13); break;
                        case 8.75f: OpenWeaponBox(_index, 14); break;
                        case 7.55f: OpenWeaponBox(_index, 15); break;
                        case 6.35f: OpenWeaponBox(_index, 16); break;
                        case 5.15f: OpenWeaponBox(_index, 17); break;
                        case 3.95f: OpenWeaponBox(_index, 18); break;
                        case 2.75f: OpenWeaponBox(_index, 19); break;
                        case 1.55f: OpenWeaponBox(_index, 20); break;
                        case 1.35f: OpenWeaponBox(_index, 21); break;
                        case 1.15f: OpenWeaponBox(_index, 22); break;
                        case 1.05f: OpenWeaponBox(_index, 23); break;
                        case 0.95f: OpenWeaponBox(_index, 24); break;
                        case 0.75f: OpenWeaponBox(_index, 25); break;
                        case 0.55f: OpenWeaponBox(_index, 26); break;
                        case 0.35f: OpenWeaponBox(_index, 27); break;
                        case 0.15f: OpenWeaponBox(_index, 28); break;
                        case 0.1f: OpenWeaponBox(_index, 29); break;
                        case 0.05f: OpenWeaponBox(_index, 30); break;
                        default: OpenWeaponBox(_index, 10); break;
                    }
                    /// 무기뽑기 카운터 1 올리기
                    ListModel.Instance.DAYlist_Update(3);
                    ///  업적  완료 카운트
                    ListModel.Instance.ALLlist_Update(12, 1);
                }

                break;

            case "B":
                float[] probs_B = new float[] { 18.55f, 16.35f, 14.15f, 11.95f, 9.75f, 7.55f, 6.35f, 5.15f, 4.05f, 2.95f, 1.75f, 0.8f, 0.35f, 0.15f, 0.1f, 0.05f };
                if (isTooMuchOpen)
                {
                    if (PlayerInventory.box_B < 1) return;
                    /// 모두 열기
                    tmpCnt = PlayerInventory.box_B;
                    for (int i = 0; i < tmpCnt; i++)
                    {
                        returnValue = PlayerPrefsManager.instance.GetRandom(probs_B, startIndex++);
                        switch (returnValue)
                        {
                            case 18.55f: tmpStringList.Add(15); break;
                            case 16.35f: tmpStringList.Add(16); break;
                            case 14.15f: tmpStringList.Add(17); break;
                            case 11.95f: tmpStringList.Add(18); break;
                            case 9.75f: tmpStringList.Add(19); break;
                            case 7.55f: tmpStringList.Add(20); break;
                            case 6.35f: tmpStringList.Add(21); break;
                            case 5.15f: tmpStringList.Add(22); break;
                            case 4.05f: tmpStringList.Add(23); break;
                            case 2.95f: tmpStringList.Add(24); break;
                            case 1.75f: tmpStringList.Add(25); break;
                            case 0.8f: tmpStringList.Add(26); break;
                            case 0.35f: tmpStringList.Add(27); break;
                            case 0.15f: tmpStringList.Add(28); break;
                            case 0.1f: tmpStringList.Add(29); break;
                            case 0.05f: tmpStringList.Add(30); break;
                            default: tmpStringList.Add(15); break;
                        }
                        /// 무기뽑기 카운터 1 올리기
                        ListModel.Instance.DAYlist_Update(3);
                        ///  업적  완료 카운트
                        ListModel.Instance.ALLlist_Update(12, 1);
                    }
                    /// 반복문 끝나고 인덱스 0 호출
                    OpenWeaponBox(_index, tmpStringList[0]);
                }
                else
                {
                    returnValue = PlayerPrefsManager.instance.GetRandom(probs_B, startIndex);
                    Debug.LogError("뽑힌거 : " + returnValue.ToString("F2"));
                    switch (returnValue)
                    {
                        case 18.55f: OpenWeaponBox(_index, 15); break;
                        case 16.35f: OpenWeaponBox(_index, 16); break;
                        case 14.15f: OpenWeaponBox(_index, 17); break;
                        case 11.95f: OpenWeaponBox(_index, 18); break;
                        case 9.75f: OpenWeaponBox(_index, 19); break;
                        case 7.55f: OpenWeaponBox(_index, 20); break;
                        case 6.35f: OpenWeaponBox(_index, 21); break;
                        case 5.15f: OpenWeaponBox(_index, 22); break;
                        case 4.05f: OpenWeaponBox(_index, 23); break;
                        case 2.95f: OpenWeaponBox(_index, 24); break;
                        case 1.75f: OpenWeaponBox(_index, 25); break;
                        case 0.8f: OpenWeaponBox(_index, 26); break;
                        case 0.35f: OpenWeaponBox(_index, 27); break;
                        case 0.15f: OpenWeaponBox(_index, 28); break;
                        case 0.1f: OpenWeaponBox(_index, 29); break;
                        case 0.05f: OpenWeaponBox(_index, 30); break;
                        default: OpenWeaponBox(_index, 15); break;
                    }
                    /// 무기뽑기 카운터 1 올리기
                    ListModel.Instance.DAYlist_Update(3);
                    ///  업적  완료 카운트
                    ListModel.Instance.ALLlist_Update(12, 1);
                }

                break;

            case "A":
                float[] probs_A = new float[] { 21.3f, 19.35f, 17.15f, 15.05f, 12.95f, 5.75f, 4.55f, 3.35f, 0.3f, 0.15f, 0.1f };
                if (isTooMuchOpen)
                {
                    if (PlayerInventory.box_A < 1) return;
                    /// 모두 열기
                    tmpCnt = PlayerInventory.box_A;
                    for (int i = 0; i < tmpCnt; i++)
                    {
                        returnValue = PlayerPrefsManager.instance.GetRandom(probs_A, startIndex++);
                        switch (returnValue)
                        {
                            case 21.3f: tmpStringList.Add(20); break;
                            case 19.35f: tmpStringList.Add(21); break;
                            case 17.15f: tmpStringList.Add(22); break;
                            case 15.05f: tmpStringList.Add(23); break;
                            case 12.95f: tmpStringList.Add(24); break;
                            case 5.75f: tmpStringList.Add(25); break;
                            case 4.55f: tmpStringList.Add(26); break;
                            case 3.35f: tmpStringList.Add(27); break;
                            case 0.3f: tmpStringList.Add(28); break;
                            case 0.15f: tmpStringList.Add(29); break;
                            case 0.1f: tmpStringList.Add(30); break;
                            default: tmpStringList.Add(20); break;
                        }
                        /// 무기뽑기 카운터 1 올리기
                        ListModel.Instance.DAYlist_Update(3);
                        ///  업적  완료 카운트
                        ListModel.Instance.ALLlist_Update(12, 1);
                    }


                    /// 반복문 끝나고 인덱스 0 호출
                    OpenWeaponBox(_index, tmpStringList[0]);
                }
                else
                {
                    returnValue = PlayerPrefsManager.instance.GetRandom(probs_A, startIndex);
                    Debug.LogError("뽑힌거 : " + returnValue.ToString("F2"));
                    switch (returnValue)
                    {
                        case 21.3f: OpenWeaponBox(_index, 20); break;
                        case 19.35f: OpenWeaponBox(_index, 21); break;
                        case 17.15f: OpenWeaponBox(_index, 22); break;
                        case 15.05f: OpenWeaponBox(_index, 23); break;
                        case 12.95f: OpenWeaponBox(_index, 24); break;
                        case 5.75f: OpenWeaponBox(_index, 25); break;
                        case 4.55f: OpenWeaponBox(_index, 26); break;
                        case 3.35f: OpenWeaponBox(_index, 27); break;
                        case 0.3f: OpenWeaponBox(_index, 28); break;
                        case 0.15f: OpenWeaponBox(_index, 29); break;
                        case 0.1f: OpenWeaponBox(_index, 30); break;
                        default: OpenWeaponBox(_index, 20); break;
                    }
                    /// 무기뽑기 카운터 1 올리기
                    ListModel.Instance.DAYlist_Update(3);
                    ///  업적  완료 카운트
                    ListModel.Instance.ALLlist_Update(12, 1);
                }

                break;

            case "S":
                float[] probs_S = new float[] { 34.6f, 32.55f, 30.35f, 1.65f, 0.65f, 0.2f };
                if (isTooMuchOpen)
                {
                    if (PlayerInventory.box_S < 1) return;
                    /// 모두 열기
                    tmpCnt = PlayerInventory.box_S;
                    for (int i = 0; i < tmpCnt; i++)
                    {
                        returnValue = PlayerPrefsManager.instance.GetRandom(probs_S, startIndex++);
                        switch (returnValue)
                        {
                            case 34.6f: tmpStringList.Add(25); break;
                            case 32.55f: tmpStringList.Add(26); break;
                            case 30.35f: tmpStringList.Add(27); break;
                            case 1.65f: tmpStringList.Add(28); break;
                            case 0.65f: tmpStringList.Add(29); break;
                            case 0.2f: tmpStringList.Add(30); break;
                            default: tmpStringList.Add(25); break;
                        }
                        /// 무기뽑기 카운터 1 올리기
                        ListModel.Instance.DAYlist_Update(3);
                        ///  업적  완료 카운트
                        ListModel.Instance.ALLlist_Update(12, 1);
                    }

                    /// 반복문 끝나고 인덱스 0 호출
                    OpenWeaponBox(_index, tmpStringList[0]);
                }
                else
                {
                    returnValue = PlayerPrefsManager.instance.GetRandom(probs_S, startIndex);
                    Debug.LogError("뽑힌거 : " + returnValue.ToString("F2"));
                    switch (returnValue)
                    {
                        case 34.6f: OpenWeaponBox(_index, 25); break;
                        case 32.55f: OpenWeaponBox(_index, 26); break;
                        case 30.35f: OpenWeaponBox(_index, 27); break;
                        case 1.65f: OpenWeaponBox(_index, 28); break;
                        case 0.65f: OpenWeaponBox(_index, 29); break;
                        case 0.2f: OpenWeaponBox(_index, 30); break;
                        default: OpenWeaponBox(_index, 25); break;
                    }
                    /// 무기뽑기 카운터 1 올리기
                    ListModel.Instance.DAYlist_Update(3);
                    ///  업적  완료 카운트
                    ListModel.Instance.ALLlist_Update(12, 1);
                }

                break;

            case "L":
                float[] probs_L = new float[] { 60f, 39f, 1f };
                if (isTooMuchOpen)
                {
                    if (PlayerInventory.box_L < 1) return;
                    /// 모두 열기
                    tmpCnt = PlayerInventory.box_L;
                    for (int i = 0; i < tmpCnt; i++)
                    {
                        returnValue = PlayerPrefsManager.instance.GetRandom(probs_L, startIndex++);
                        switch (returnValue)
                        {
                            case 60f: tmpStringList.Add(28); break;
                            case 39f: tmpStringList.Add(29); break;
                            case 1f: tmpStringList.Add(30); break;
                            default: tmpStringList.Add(28); break;
                        }
                        /// 무기뽑기 카운터 1 올리기
                        ListModel.Instance.DAYlist_Update(3);
                        ///  업적  완료 카운트
                        ListModel.Instance.ALLlist_Update(12, 1);
                    }

                    /// 반복문 끝나고 인덱스 0 호출
                    OpenWeaponBox(_index, tmpStringList[0]);
                }
                else
                {
                    returnValue = PlayerPrefsManager.instance.GetRandom(probs_L, startIndex);
                    switch (returnValue)
                    {
                        case 60f: OpenWeaponBox(_index, 28); break;
                        case 39f: OpenWeaponBox(_index, 29); break;
                        case 1f: OpenWeaponBox(_index, 30); break;
                        default: OpenWeaponBox(_index, 28); break;
                    }
                    /// 무기뽑기 카운터 1 올리기
                    ListModel.Instance.DAYlist_Update(3);
                    ///  업적  완료 카운트
                    ListModel.Instance.ALLlist_Update(12, 1);
                }
                break;


            default:
                /// 10회 뽑기다
                if (isTooMuchOpen)
                {
                    /// 소모권 남아있나 체크
                    if (PlayerInventory.box_Coupon > 0)
                    {
                        tmpCnt = PlayerInventory.box_Coupon;
                        PlayerInventory.SetBoxsCount("weapon_coupon", -tmpCnt);
                        megaAmount.text = "x0";
                    }
                    else return;

                    float[] probs = new float[] { 37f, 27f, 19f, 12f, 3f, 1.5f, 0.5f };
                    for (int i = 0; i < tmpCnt; i++)
                    {
                        returnValue = PlayerPrefsManager.instance.GetRandom(probs, startIndex++);
                        switch (returnValue)
                        {
                            case 37f: tmpList.Add("E"); break;
                            case 27f: tmpList.Add("D"); break;
                            case 19f: tmpList.Add("C"); break;
                            case 12f: tmpList.Add("B"); break;
                            case 3f: tmpList.Add("A"); break;
                            case 1.5f: tmpList.Add("S"); break;
                            case 0.5f: tmpList.Add("L"); break;
                            default: break;
                        }
                    }
                    /// 첫번째 결과 표시
                    OpenWeaponCoupon(tmpList[0]);
                }
                /// 1회 뽑기다
                else
                {
                    /// 소모권 남아있나 체크
                    if (PlayerInventory.box_Coupon > 0)
                    {
                        PlayerInventory.SetBoxsCount("weapon_coupon", -1);
                        megaAmount.text = "x" + PlayerInventory.box_Coupon.ToString("N0");
                    }
                    else return;
                    /// 공백이면 무기 뽑기권 사용 ->
                    float[] probs = new float[] { 40f, 25f, 15f, 10f, 5f, 4f, 1f };
                    returnValue = PlayerPrefsManager.instance.GetRandom(probs, startIndex);
                    switch (returnValue)
                    {
                        case 40f: OpenWeaponCoupon("E"); break;
                        case 25f: OpenWeaponCoupon("D"); break;
                        case 15f: OpenWeaponCoupon("C"); break;
                        case 10f: OpenWeaponCoupon("B"); break;
                        case 5f: OpenWeaponCoupon("A"); break;
                        case 4f: OpenWeaponCoupon("S"); break;
                        case 1f: OpenWeaponCoupon("L"); break;
                        default: break;
                    }
                }
                /// 무기 쿠폰 뽑기는 리턴 맞음
                return;
        }

        /// 새로고침
        RefreshWeapon();
    }

    /// <summary>
    /// 10회 뽑기시 임시 랜덤 값 보관 리스트
    /// </summary>
    List<string> tmpList = new List<string>();
    List<int> tmpStringList = new List<int>();

    /// <summary>
    /// 뽑힌 정보 바탕으로 큰 화면 무기뽑기 화면 내용채우고 팝업
    /// </summary>
    /// <param name="_index"></param>
    void OpenWeaponCoupon(string _index)
    {
        /// 상자 갯수 증가
        PlayerInventory.SetBoxsCount(_index, 1);
        /// 튜토업적 체크
        if (PlayerPrefsManager.currentTutoIndex == 34) ListModel.Instance.TUTO_Update(34);
        else if (PlayerPrefsManager.currentTutoIndex == 39) ListModel.Instance.TUTO_Update(39);
        else if (PlayerPrefsManager.currentTutoIndex == 43) ListModel.Instance.TUTO_Update(43);
        else if (PlayerPrefsManager.currentTutoIndex == 49) ListModel.Instance.TUTO_Update(49);
        /// 뽑기 화면 세팅
        for (int i = 0; i < DescTexts.Length; i++)
        {
            DescTexts[i].gameObject.SetActive(false);
        }
        // "X 등급 무기상자"
        RankText.text = _index + Lean.Localization.LeanLocalization.GetTranslationText("Weapon_Text_Box");
        GetRuneImg.sprite = HeartSprs[GetAlphaNum(_index)];
        DescTexts[0].text = Lean.Localization.LeanLocalization.GetTranslationText("Weapon_Get_Box_" + _index);
        /// 해당 라인 표시
        DescTexts[0].gameObject.SetActive(true);
        /// 봇 이미지 이동
        BotImg.SetSiblingIndex(2);

        /// 확인 / 다음 버튼 할당 
        if (isTooMuchOpen)
        {
            fullBtnLayout.SetActive(true); fullBtnExit.SetActive(false); fullBtnExitText.SetActive(false);
        }
        else
        {
            fullBtnLayout.SetActive(false); fullBtnExit.SetActive(true); fullBtnExitText.SetActive(true);
        }
        ///뽑기 팝업 호출
        PopUpManager.instance.ShowPopUP(18);
        // 새로 고침
        pi.ExtraRefresh();
    }

    private bool _isWeapGatcha;
    /// <summary>
    /// 쿠폰 10개 연속 뽑기시에 <확인> = false / <다음> = true
    /// </summary>
    public void AcceptCoupon_10X(bool _isNext)
    {
        if (_isWeapGatcha)
        {
            if (_isNext)
            {
                /// 리스트 남아있으면 첫 요소 삭제
                tmpStringList.RemoveAt(0);
                /// 남은 횟수가 한개도 없다
                if (tmpStringList.Count < 1)
                {
                    PopUpManager.instance.HidePopUP(18);
                }
                /// 남은 횟수가 있다.
                else
                {
                    OpenWeaponBox(localIndex, tmpStringList[0]);
                }
            }
            else
            {
                /// index 0 은 호출할때 사용 했으니 i = 1 부터 시작
                for (int i = 1; i < tmpStringList.Count; i++)
                {
                    OpenWeaponBox(localIndex, tmpStringList[i]);
                }
                /// 임시 리스트 비워주고
                tmpStringList.Clear();
                ///뽑기 팝업  setFalse
                PopUpManager.instance.HidePopUP(18);
            }
            /// 알림 창의 이미지 설명이 0개면?
            switch (localIndex)
            {
                case "E": if (PlayerInventory.box_E < 1) PopUpManager.instance.HidePopUP(17); break;
                case "D": if (PlayerInventory.box_D < 1) PopUpManager.instance.HidePopUP(17); break;
                case "C": if (PlayerInventory.box_C < 1) PopUpManager.instance.HidePopUP(17); break;
                case "B": if (PlayerInventory.box_B < 1) PopUpManager.instance.HidePopUP(17); break;
                case "A": if (PlayerInventory.box_A < 1) PopUpManager.instance.HidePopUP(17); break;
                case "S": if (PlayerInventory.box_S < 1) PopUpManager.instance.HidePopUP(17); break;
                case "L": if (PlayerInventory.box_L < 1) PopUpManager.instance.HidePopUP(17); break;
            }
        }
        else
        {
            if (_isNext)
            {
                /// 리스트 남아있으면 작업.
                tmpList.RemoveAt(0);
                if (tmpList.Count <= 0) PopUpManager.instance.HidePopUP(18);
                else OpenWeaponCoupon(tmpList[0]);
            }
            else
            {
                /// index 0 은 호출할때 사용 했으니 i = 1 부터 시작
                for (int i = 1; i < tmpList.Count; i++)
                {
                    OpenWeaponCoupon(tmpList[i]);
                }
                /// 임시 리스트 비워주고
                tmpList.Clear();
                ///뽑기 팝업  setFalse
                PopUpManager.instance.HidePopUP(18);
            }
        }


    }

    void OpenWeaponBox(string _index, int _weaInt)
    {
        /// 인벤토리 박스 갯수 소모 (0개 이하라면 리턴)
        switch (_index)
        {
            case "E":
                if (PlayerInventory.box_E > 0) PlayerInventory.SetBoxsCount(_index, -1);
                else return;
                miniAmount.text = "x" + PlayerInventory.box_E.ToString("N0");    
                break;
            case "D":
                if (PlayerInventory.box_D > 0) PlayerInventory.SetBoxsCount(_index, -1);
                else return;
                miniAmount.text = "x" + PlayerInventory.box_D.ToString("N0"); 
                break;
            case "C":
                if (PlayerInventory.box_C > 0) PlayerInventory.SetBoxsCount(_index, -1);
                else return;
                miniAmount.text = "x" + PlayerInventory.box_C.ToString("N0");   
                break;
            case "B":
                if (PlayerInventory.box_B > 0) PlayerInventory.SetBoxsCount(_index, -1);
                else return;
                miniAmount.text = "x" + PlayerInventory.box_B.ToString("N0");  
                break;
            case "A":
                if (PlayerInventory.box_A > 0) PlayerInventory.SetBoxsCount(_index, -1);
                else return;
                miniAmount.text = "x" + PlayerInventory.box_A.ToString("N0");  
                break;
            case "S":
                if (PlayerInventory.box_S > 0) PlayerInventory.SetBoxsCount(_index, -1);
                else return;
                miniAmount.text = "x" + PlayerInventory.box_S.ToString("N0");  
                break;
            case "L":
                if (PlayerInventory.box_L > 0) PlayerInventory.SetBoxsCount(_index, -1);
                else return;
                miniAmount.text = "x" + PlayerInventory.box_L.ToString("N0");   
                break;
            default: break;
        }

        /// 무기 리스트에 추가
        ListModel.Instance.Weapon_Add(_weaInt);
        /// 무기 뽑기 1회 진행
        if (PlayerPrefsManager.currentTutoIndex == 6) ListModel.Instance.TUTO_Update(6);

        /// 뽑기 화면 세팅
        for (int i = 0; i < DescTexts.Length; i++)
        {
            DescTexts[i].gameObject.SetActive(false);
        }
        // "X 등급 무기상자"
        RankText.text = ListModel.Instance.weaponList[_weaInt].headRank + ListModel.Instance.weaponList[_weaInt].tailRank + Lean.Localization.LeanLocalization.GetTranslationText("Weapon_Text_Weapon");
        GetRuneImg.sprite = WeaponSprs[_weaInt];
        //
        DescTexts[0].text = Lean.Localization.LeanLocalization.GetTranslationText("Weapon_Desc_Head") + " " + (ListModel.Instance.weaponList[_weaInt].startPower).ToString("F2") + "%";
        DescTexts[1].text = Lean.Localization.LeanLocalization.GetTranslationText("Weapon_Desc_Tail") + " " + (ListModel.Instance.weaponList[_weaInt].startPower * 0.1f).ToString("F2") + "%";
        /// 해당 라인 표시
        DescTexts[0].gameObject.SetActive(true);
        DescTexts[1].gameObject.SetActive(true);
        /// 봇 이미지 이동
        BotImg.SetSiblingIndex(3);
        /// 10개 뽑기인지 확인
        if (isTooMuchOpen)
        {
            fullBtnLayout.SetActive(true); fullBtnExit.SetActive(false); fullBtnExitText.SetActive(false);
        }
        else
        {
            fullBtnLayout.SetActive(false); fullBtnExit.SetActive(true); fullBtnExitText.SetActive(true);
        }
        ///뽑기 팝업 호출
        PopUpManager.instance.ShowPopUP(18);

        /// 인벤토리 새로고침
        pi.ExtraRefresh();
        ///무기창 새로고침
        nsm.RefreshWeapon();
        /// 무기 새로고침
        RefreshWeapon();
    }


    private int GetAlphaNum(string _index)
    {
        switch (_index)
        {
            case "E": return 0;
            case "D": return 1;
            case "C": return 2;
            case "B": return 3;
            case "A": return 4;
            case "S": return 5;
            case "L": return 6;
            default: return 0;
        }
    }

}
