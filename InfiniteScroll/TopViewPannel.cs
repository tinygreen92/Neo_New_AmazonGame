using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopViewPannel : MonoBehaviour
{
    [Header("- 수집 매니저")]
    public SupportManager sm;
    public RuneManager rm;
    public PetManager pm;
    public WeaponManager wm;
    [Header("- 내부 박스 붙어있는 부모 오브젝트")]
    public Transform Charactor_INFINITI;
    public Transform Weapon_INFINITI;
    public Transform Heart_INFINITI;
    public Transform SupportView;
    public Transform Pet_INFINITI;
    public Transform Rune_INFINITI;
    public ShopItemManager SIM;
    [Header("- +100 관련 뭘로 바꿀래")]
    public Image[] btnImgs;
    public Sprite EnableBtn;
    public Sprite DisableBtn;

    /// <summary>
    ///  펫 / 룬 카테고리 달아주기.
    /// </summary>
    /// <param name="_index"></param>
    public void SwichPetRune(int _index)
    {
        if (transform.parent.name == "05.PetView") SetPet(_index);
        //if (transform.parent.name == "06.RuneView") 

        switch (_index)
        {
            case 0:
                btnImgs[0].sprite = EnableBtn;
                btnImgs[1].sprite = DisableBtn;
                if(PlayerPrefsManager.isPetDiaTab) PlayerPrefsManager.isPetDiaTab = false;
                if(PlayerPrefsManager.isRuneFussionTab) PlayerPrefsManager.isRuneFussionTab = false;
                /// 룬매니저 탑 뷰에만 적용
                if(rm != null)
                {
                    /// 보관함 글자 고정
                    rm.middleBtnText.text = LeanLocalization.GetTranslationText("Category_Rune_DetailPage");
                    /// 글자 / 박스 색 새로고침
                    SetRuneItemMid();
                }

                break;

            case 1:
                btnImgs[0].sprite = DisableBtn;
                btnImgs[1].sprite = EnableBtn;
                if (!PlayerPrefsManager.isPetDiaTab) PlayerPrefsManager.isPetDiaTab = true;
                if (!PlayerPrefsManager.isRuneFussionTab) PlayerPrefsManager.isRuneFussionTab = true;
                /// 룬매니저 탑 뷰에만 적용
                if (rm != null)
                {
                    /// 조합 글자 고정
                    rm.middleBtnText.text = LeanLocalization.GetTranslationText("Category_Rune_Fussion");
                    /// 글자 / 박스 색 새로고침
                    SetRuneItemMid();
                }

                break;


            default:
                btnImgs[0].sprite = EnableBtn;
                btnImgs[1].sprite = DisableBtn;
                break;
        }
    }

    public void SetRuneItemMid()
    {
        if (ListModel.Instance.runeList.Count != 0)
        {
            /// 지금 활성화된 자식만큼 배수 적용해줌. -> 배수 적용된 뒤에 새로고침.
            for (int i = 1; i < Rune_INFINITI.childCount; i++)
            {
                Rune_INFINITI.GetChild(i).GetComponent<RuntItem>()
                    .ChangeInvenFusion();
            }
        }
    }





    /// <summary>
    ///  무기 카테고리 달아주기.
    /// </summary>
    /// <param name="_index"></param>
    public void SwichWeapon(int _index)
    {
        /// UI 새로고침
        ChageModeWeapon(_index);

        switch (_index)
        {
            case 0:
                btnImgs[0].sprite = EnableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = DisableBtn;
                btnImgs[3].sprite = DisableBtn;
                break;

            case 1:
                btnImgs[0].sprite = DisableBtn;
                btnImgs[1].sprite = EnableBtn;
                btnImgs[2].sprite = DisableBtn;
                btnImgs[3].sprite = DisableBtn;
                break;

            case 2:
                btnImgs[0].sprite = DisableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = EnableBtn;
                btnImgs[3].sprite = DisableBtn;
                break;

            case 3:
                btnImgs[0].sprite = DisableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = DisableBtn;
                btnImgs[3].sprite = EnableBtn;
                break;

            default:
                btnImgs[0].sprite = EnableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = DisableBtn;
                btnImgs[3].sprite = DisableBtn;
                break;
        }
    }

    /// <summary>
    /// 해당 버튼 누르면 활성화
    /// 1 / 10 / 100
    /// </summary>
    public void DismissGrayBtn(int _multiple)
    {
        // mutiple 설정 1배 10배 100배 -> 추가 기능 해당 범위 미만일때
        if (transform.parent.name == "01.CharatorView") SetCharaLevel(_multiple);
        if (transform.parent.name == "03.HeartView") SetHeartMuti(_multiple);
        if (transform.parent.name == "04.SupportView") SetUpgradeLevel(_multiple);


        switch (_multiple)
        {
            case 1:
                btnImgs[0].sprite = EnableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = DisableBtn;
                if (transform.parent.name == "07.STORE_SROTE")
                {
                    PlayerPrefsManager.storeIndex = _multiple;
                    SIM.ChangeSection();
                }

                break;

            case 10:
                btnImgs[0].sprite = DisableBtn;
                btnImgs[1].sprite = EnableBtn;
                btnImgs[2].sprite = DisableBtn;
                if (transform.parent.name == "07.STORE_SROTE")
                {
                    PlayerPrefsManager.storeIndex = _multiple;
                    SIM.ChangeSection();
                }

                break;

            case 100:
                btnImgs[0].sprite = DisableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = EnableBtn;
                if (transform.parent.name == "07.STORE_SROTE")
                {
                    PlayerPrefsManager.storeIndex = _multiple;
                    SIM.ChangeSection();
                }

                break;

            default:
                btnImgs[0].sprite = EnableBtn;
                btnImgs[1].sprite = DisableBtn;
                btnImgs[2].sprite = DisableBtn;
                break;
        }
    }

    /// <summary>
    /// 무기 세팅
    /// </summary>
    /// <param name="_id"></param>
    public void ChageModeWeapon(int _indx)
    {
        Weapon_INFINITI.GetComponent<WeaponManager>().ModeName = _indx;
        /// 모든 무기 박스 인포업데이트
        WeaponAllRefresh();
    }
    public void WeaponAllRefresh()
    {
        for (int i = 1; i < Weapon_INFINITI.childCount; i++)
        {
            Weapon_INFINITI.GetChild(i).GetComponent<WeaponItem>()
                .BoxInfoUpdate(int.Parse(Weapon_INFINITI.GetChild(i).name));
        }
    }

    /// <summary>
    /// 캐릭터 세팅
    /// </summary>
    /// <param name="_id"></param>
    public void SetCharaLevel(int _multiple)
    {
        // 배수 설정 -> 얘는 다시 탑 버튼 누를때 까지 고정
        PlayerPrefsManager.charaUpgrageMutiple = _multiple;

        /// 지금 활성화된 자식만큼 배수 적용해줌. -> 배수 적용된 뒤에 새로고침.
        for (int i = 1; i < Charactor_INFINITI.childCount; i++)
        {
            Charactor_INFINITI.GetChild(i).GetComponent<CharactorItem>()
                .BoxInfoUpdate(int.Parse(Charactor_INFINITI.GetChild(i).name));
        }
    }


    /// <summary>
    /// 수집 세팅
    /// </summary>
    /// <param name="_id"></param>
    public void SetUpgradeLevel(int _multiple)
    {
        // 배수 설정 -> 얘는 다시 탑 버튼 누를때 까지 고정
        sm.upgrageMutiple = _multiple;

        /// 지금 활성화된 자식만큼 배수 적용해줌. -> 배수 적용된 뒤에 새로고침.
        for (int i = 1; i < SupportView.childCount; i++)
        {
            SupportView.GetChild(i).GetComponent<SupportItem>()
                .BoxInfoUpdate(int.Parse(SupportView.GetChild(i).name));
        }
    }

    /// <summary>
    /// 유물 세팅
    /// </summary>
    /// <param name="_multiple"></param>
    public void SetHeartMuti(int _multiple)
    {
        // 배수 설정 -> 얘는 다시 탑 버튼 누를때 까지 고정
        PlayerPrefsManager.heartUpgrageMutiple = _multiple;
        /// 유물 버튼 설명 새로고침
        SetHeartRefresh();
    }

    /// <summary>
    /// 유물 업그레이드 하면 유물 새로고침
    /// </summary>
    public void SetHeartRefresh()
    {
        //Debug.LogWarning(" 유물 업그레이드 하면 유물 새로고침");
        /// 지금 활성화된 자식만큼 배수 적용해줌. -> 배수 적용된 뒤에 새로고침.
        for (int i = 2; i < Heart_INFINITI.childCount; i++)
        {
            Heart_INFINITI.GetChild(i).GetComponent<HeartItem>()
                .BoxInfoUpdate(int.Parse(Heart_INFINITI.GetChild(i).name));
        }
    }

    /// <summary>
    /// 펫 세팅
    /// </summary>
    /// <param name="_multiple"></param>
    public void SetPet(int _multiple)
    {
        // 배수 설정 -> 얘는 다시 탑 버튼 누를때 까지 고정
        pm.diaORleaf = _multiple;

        /// 지금 활성화된 자식만큼 배수 적용해줌. -> 배수 적용된 뒤에 새로고침.
        for (int i = 1; i < Pet_INFINITI.childCount; i++)
        {
            Pet_INFINITI.GetChild(i).GetComponent<PetItem>().DiaLeafPoke();
        }
    }


}
