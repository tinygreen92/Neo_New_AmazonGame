using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public Image CanvasImg;
    [Space]
    public Sprite[] animSpr;
    [Space]
    public Image topImg;
    public Image midImg;
    public Image botImg;


    private void Awake()
    {
        ResetColor();
    }

    void ResetColor()
    {
        topImg.color = new Color(1, 1, 1, 0);
        midImg.color = new Color(1, 1, 1, 0);
        botImg.color = new Color(1, 1, 1, 0);
    }


    void AllFadeOut()
    {
        topImg.DOFade(0, 0.3f).SetEase(Ease.InOutQuad);
        midImg.DOFade(0, 0.3f).SetEase(Ease.InOutQuad);
        botImg.DOFade(0, 0.3f).SetEase(Ease.InOutQuad);
    }
    void ChangeImage2()
    {
        topImg.sprite = animSpr[3];
        midImg.sprite = animSpr[4];
        botImg.sprite = animSpr[5];
    }
    void ChangeImage3()
    {
        topImg.sprite = animSpr[6];
        midImg.sprite = animSpr[7];
        botImg.sprite = animSpr[8];
    }

    void EndOfSoldier()
    {
        PlayerPrefsManager.isNickNameComp = true;
        Invoke(nameof(InvoSetFalse), 0.2f);
    }

    void InvoSetFalse()
    {
        CanvasImg.gameObject.SetActive(false);
    }

    /// <summary>
    /// 최초 접속 1회만 / 스킵버튼 없음 
    /// 최초 로딩 끝난 후 → 인트로 → 닉네임 설정 팝업 순서
    /// 순서대로 1 → 2 → 3 ... 해당 자리에 페이드인으로 띄우면 됨
    /// </summary>
    public void StartIntro()
    {
        CanvasImg.gameObject.SetActive(true);
        Sequence seq = DOTween.Sequence();
        // Create a new Sequence.
        /// 1 페이지
        seq.Append(topImg.DOFade(1, 1).SetEase(Ease.InOutQuad));
        seq.Append(midImg.DOFade(1, 1).SetEase(Ease.InOutQuad));
        seq.Append(botImg.DOFade(1, 1).SetEase(Ease.InOutQuad));
        //
        seq.AppendInterval(1);
        seq.AppendCallback(AllFadeOut);
        seq.AppendInterval(1);
        seq.AppendCallback(ChangeImage2);
        
        /// 2 페이지
        seq.Append(topImg.DOFade(1, 1).SetEase(Ease.InOutQuad));
        seq.Append(midImg.DOFade(1, 1).SetEase(Ease.InOutQuad));
        seq.Append(botImg.DOFade(1, 1).SetEase(Ease.InOutQuad));
        //
        seq.AppendInterval(1);
        seq.AppendCallback(AllFadeOut);
        seq.AppendInterval(1);
        seq.AppendCallback(ChangeImage3);
        
        /// 3 페이지
        seq.Append(topImg.DOFade(1, 1).SetEase(Ease.InOutQuad));
        seq.Append(midImg.DOFade(1, 1).SetEase(Ease.InOutQuad));
        seq.Append(botImg.DOFade(1, 1).SetEase(Ease.InOutQuad));
        //
        seq.AppendInterval(1);
        seq.AppendCallback(AllFadeOut);
        seq.AppendInterval(1);
        //
        seq.Play().OnComplete(EndOfSoldier);
    }



}
