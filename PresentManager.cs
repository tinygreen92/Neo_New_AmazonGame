using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresentManager : MonoBehaviour
{
    [Header("- 선물함 모두 받기 버튼")]
    public Image AllBtn;
    public Sprite[] AllBtnSprs;
    [Header("- 선물함 내용물")]
    public Transform ItemCart;
    public Transform ParentTrans;
    [Header("- 아이콘 이미지 뭉치")]
    public Sprite[] IconSprs;

    private void OnDisable()
    {
        while (ParentTrans.childCount != 0)
        {
            Lean.Pool.LeanPool.Despawn(ParentTrans.GetChild(0));
        }
        /// 모두 받기 회색
        AllBtn.sprite = AllBtnSprs[0];
    }

    public void AddPresent(string _code, string _amount, string _uid, string _message)
    {
        //프리팹에서 박스 생성
        Transform initBox = Lean.Pool.LeanPool.Spawn(ItemCart);
        initBox.SetParent(ParentTrans); // 스크롤뷰 안쪽에 생성.
        initBox.localPosition = Vector3.zero; // 뒤틀리는거 방지
        initBox.localScale = new Vector3(1, 1, 1);
        
        switch (_code)
        {
            case "weapon_coupon": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[0]; break;
            case "reinforce_box": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[1]; break;
            case "leaf_box": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[2]; break;
            case "E_box": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[3]; break;
            case "D_box": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[4]; break;
            case "C_box": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[5]; break;
            case "B_box": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[6]; break;
            case "A_box": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[7]; break;
            case "S_box": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[8]; break;
            case "L_box": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[9]; break;
            case "pvp": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[10]; break;
            case "cave": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[11]; break;
           
            /// --------- 아마존 포션
            
            case "crystal": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[12]; break;
            

            
            case "stone": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[13]; break;
            case "reinforce": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[14]; break;
            case "gold": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[15]; break;
            case "leaf": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[16]; break;
            case "diamond": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[17]; break;
            case "cave_clear": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[18]; break;
            case "elixr": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[19]; break;
            //
            case "S_leaf_box": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[12]; break;
            case "S_reinforce_box": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[21]; break;
            case "mining": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[22]; break;
            case "amber": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[23]; break;
            //
            case "Crazy_dia": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[24]; break;
            case "Crazy_elixr": initBox.GetComponent<PresentItem>().IconImg.sprite = IconSprs[25]; break;


            default: break; 
        }

        initBox.GetComponent<PresentItem>().SetPostContent(_code, _amount, _uid, _message);
    }


    /// <summary>
    /// 우편함 모두 받기
    /// </summary>
    public void AceptAllItem()
    {
        /// 우편함 비어 있으면 리턴
        if (ParentTrans.childCount <1)
        {
            return;
        }

        SystemPopUp.instance.LoopLoadingImg();
        Invoke(nameof(InvoStopLoop), 0.3f);

        for (; ; )
        {
            ParentTrans.GetChild(0).GetComponent<PresentItem>().ClickedPresentBtn();
            if (ParentTrans.childCount < 1) break;
        }

        /// 모두 받기 회색
        AllBtn.sprite = AllBtnSprs[0];

        /// 로컬 저장
        PlayerPrefsManager.instance.TEST_SaveJson();
    }

    public void RedotAssemble()
    {
        if (ParentTrans.childCount < 1)
        {
            AllBtn.sprite = AllBtnSprs[0];
        }
        else
        {
            AllBtn.sprite = AllBtnSprs[1];
        }
    }

    /// <summary>
    /// 버튼색 파란색? 회색?
    /// </summary>
    /// <param name="isBlue"></param>
    public void SetBtnBlueAllcept(bool isBlue)
    {
        if (isBlue)
        {
            AllBtn.sprite = AllBtnSprs[1];
        }
        else
        {
            AllBtn.sprite = AllBtnSprs[0];
        }
    }


    void InvoStopLoop()
    {
        SystemPopUp.instance.StopLoopLoading();
    }

}
