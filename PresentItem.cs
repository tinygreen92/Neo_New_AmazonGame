using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresentItem : MonoBehaviour
{
    public Image IconImg;
    public Text IconText;
    public Text DescText;

    string tmpHead = "";
    string tmpTail = "";
    string tmp_uid = "";

    /// <summary>
    /// 선물 매니저에서 호출해서 내용물 채워서 초기화 해줄 메서드
    /// </summary>
    public void SetPostContent(string _code, string _amount, string _uid, string _message)
    {
        IconText.text = "x" + _amount;
        tmp_uid = _uid;

        if (_message == "null" || _message == null || _message == string.Empty)
        {
            switch (_code)
            {
                case "weapon_coupon": tmpHead = "무기상자 뽑기권 "; tmpTail = " 개."; break;
                case "reinforce_box": tmpHead = "강화석 묶음 "; tmpTail = " 개."; break;
                case "leaf_box": tmpHead = "나뭇잎 묶음 "; tmpTail = " 개."; break;
                //
                case "E_box": tmpHead = "E 등급 무기 상자 "; tmpTail = " 개."; break;
                case "D_box": tmpHead = "D 등급 무기 상자 "; tmpTail = " 개."; break;
                case "C_box": tmpHead = "C 등급 무기 상자 "; tmpTail = " 개."; break;
                case "B_box": tmpHead = "B 등급 무기 상자 "; tmpTail = " 개."; break;
                case "A_box": tmpHead = "A 등급 무기 상자 "; tmpTail = " 개."; break;
                case "S_box": tmpHead = "S 등급 무기 상자 "; tmpTail = " 개."; break;
                case "L_box": tmpHead = "L 등급 무기 상자 "; tmpTail = " 개."; break;
                //
                case "pvp": tmpHead = "결투장 입장권 "; tmpTail = " 개."; break;
                case "cave": tmpHead = "숨겨진 늪지 입장권 "; tmpTail = " 개."; break;
                ///
                case "crystal": tmpHead = "아마존 포션 "; tmpTail = " 개."; break;
                ///
                case "stone": tmpHead = "아마존 결정 "; tmpTail = " 개."; break;
                case "reinforce": tmpHead = "강화석 "; tmpTail = " 개."; break;
                case "gold": tmpHead = "골드 "; tmpTail = " 개."; break;
                case "leaf": tmpHead = "나뭇잎 "; tmpTail = " 개."; break;
                case "diamond": tmpHead = "다이아몬드 "; tmpTail = " 개."; break;
                case "cave_clear": tmpHead = "숨겨진 늪지 소탕권 "; tmpTail = " 개."; break;
                case "elixr": tmpHead = "엘릭서 "; tmpTail = " 개."; break;
                ///
                case "S_leaf_box": tmpHead = "아마존 포션 "; tmpTail = " 개."; break;
                ///
                case "S_reinforce_box": tmpHead = "대박 강화석 묶음 "; tmpTail = " 개."; break;
                case "mining": tmpHead = "수정동굴 채굴권 "; tmpTail = " 개."; break;
                case "amber": tmpHead = "호박석 "; tmpTail = " 개."; break;
                // 미구현
                case "Crazy_dia": tmpHead = "대박 다이아  "; tmpTail = " 개."; break;
                case "Crazy_elixr": tmpHead = "대박 엘릭서  "; tmpTail = " 개."; break;


                default: break;
            }

            DescText.text = tmpHead + _amount + tmpTail;
        }
        else  // 메세지 내용이 있다면?
        {
            DescText.text = _message;
        }
    }

    /// <summary>
    /// 버튼에 달아 주는 거
    /// </summary>
    public void ClickedPresentBtn()
    {
        var playNANOO = GameObject.Find("NanooManager").GetComponent<NanooManager>();
        playNANOO.PostboxItemUse(tmp_uid);
        RedDotManager.instance.RedDot[3].SetActive(true);
        /// 트랜스폼 제거 해주고
        Lean.Pool.LeanPool.Despawn(transform);
        ///
        playNANOO.PostboxRedDot();
    }

}
