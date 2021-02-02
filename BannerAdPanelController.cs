using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BannerAdPanelController : MonoBehaviour
{
    public GameObject SuperPannel;
    public Animator playerAnim;
    public EasyMoblieManager em;
    [Header("- 위로 들어올릴 패널 Bottom")]
    public RectTransform[] bottomPanel;
    [Header("- 위로 들어올릴 높이 값")]
    public int tagetY;

    [HideInInspector]
    public bool isBannerUP;                // 배너 공간 올라가는 중

    private bool isSuperUser;

    Vector2 targetPos;

    private void Awake()
    {
        targetPos = new Vector2(0, tagetY);
    }

    /// <summary>
    /// 배너 외부에서도 호출할거셈
    /// </summary>
    public static bool isOn;

    /// <summary>
    /// 1.BannerPanel 에서
    /// SendMessage 로 호출 중
    /// </summary>
    private void BannerOnoff()
    {
        /// 배너가 내려와 있을때 클릭하면 올라가고 속도 증가
        if (!isOn && !isBannerUP)
        {
            //isBannerUP = true;
            isOn = true;
            /// 배너 올려
            StartCoroutine(coBannerUP());
            em.ShowBanner();
            /// 속도 10% 증가.
            Time.timeScale = 1.1f;
        }
        /// 배너가 올라가 있을때 클릭하면 내려오고 속도 정상
        else if (isOn && isBannerUP)
        {
            isBannerUP = false;
            isOn = false;
            /// 배너 내려
            StartCoroutine(coBannerDOWN());
            em.HideBanner();
            /// 속도 원래대로 복귀
            Time.timeScale = 1.0f;
        }

    }
    
    IEnumerator coBannerUP()
    {
        yield return null;
        isBannerUP = false;
        /// 배너 올라가는 스위치
        while (!isBannerUP)
        {
            yield return null;
            for (int i = 0; i < bottomPanel.Length; i++)
            {
                bottomPanel[i].offsetMin = Vector2.Lerp(bottomPanel[i].offsetMin, targetPos, 0.1f);
                bottomPanel[i].offsetMax = Vector2.Lerp(bottomPanel[i].offsetMax, -targetPos * 0.5f, 0.1f);
            }
            /// 언제 멈추니
            if (bottomPanel[0].offsetMin.y >= tagetY - 1f)
            {
                for (int i = 0; i < bottomPanel.Length; i++)
                {
                    bottomPanel[i].offsetMin = targetPos;
                    bottomPanel[i].offsetMax = -targetPos * 0.5f;
                }
                /// 클릭불가 해제
                isBannerUP = true;
            }
        }
    }
    IEnumerator coBannerDOWN()
    {
        yield return null;
        isBannerUP = true;
        /// 배너 내려가는 스위치
        while (isBannerUP)
        {
            yield return null;

            for (int i = 0; i < bottomPanel.Length; i++)
            {
                bottomPanel[i].offsetMin = Vector2.Lerp(bottomPanel[i].offsetMin, Vector2.zero, 0.1f);
                bottomPanel[i].offsetMax = Vector2.Lerp(bottomPanel[i].offsetMax, Vector2.zero, 0.1f);
            }
            /// 언제 멈추니
            if (bottomPanel[0].offsetMin.y <= 1f)
            {
                for (int i = 0; i < bottomPanel.Length; i++)
                {
                    bottomPanel[i].offsetMin = Vector2.zero;
                    bottomPanel[i].offsetMax = Vector2.zero;
                }
                if (PlayerInventory.isSuperUser != 0)
                    isSuperUser = true;

                /// 클릭불가 해제
                isBannerUP = false;
            }
        }

    }



    /// <summary>
    /// 광고제거 구매했으면 배너 영구 제거
    /// </summary>
    public void Banner525Hide()
    {
        Invoke(nameof(Invo), 0.5f);
    }
    void Invo()
    {
        /// 광고제거 구매했니?
        if (PlayerInventory.isSuperUser != 0)
        {
            /// 배너 클릭 못하게 덮어
            SuperPannel.SetActive(true);
            /// 배너 내려
            StartCoroutine(coBannerDOWN());
            isOn = false;
            em.DestroyBanner();
            Time.timeScale = 1.1f;
        }
    }

    //void Update()
    //{
    //    if (isSuperUser) return;

    //    /// 배너 올라가는 스위치
    //    if (isBannerUP)
    //    {
    //        for (int i = 0; i < bottomPanel.Length; i++)
    //        {
    //            bottomPanel[i].offsetMin = Vector2.Lerp(bottomPanel[i].offsetMin, targetPos, 0.1f);
    //            bottomPanel[i].offsetMax = Vector2.Lerp(bottomPanel[i].offsetMax, -targetPos*0.5f, 0.1f);
    //        }

    //        if (bottomPanel[0].offsetMin.y >= tagetY - 1f)
    //        {
    //            for (int i = 0; i < bottomPanel.Length; i++)
    //            {
    //                bottomPanel[i].offsetMin = targetPos;
    //                bottomPanel[i].offsetMax = -targetPos*0.5f;
    //            }

    //            isBannerUP = false;
    //        }
    //    }

    //    /// 배너 내려가는 스위치
    //    if (isBannerDown)
    //    {
    //        for (int i = 0; i < bottomPanel.Length; i++)
    //        {
    //            bottomPanel[i].offsetMin = Vector2.Lerp(bottomPanel[i].offsetMin, Vector2.zero, 0.1f);
    //            bottomPanel[i].offsetMax = Vector2.Lerp(bottomPanel[i].offsetMax, Vector2.zero, 0.1f);
    //        }

    //        if (bottomPanel[0].offsetMin.y <= 1f)
    //        {
    //            for (int i = 0; i < bottomPanel.Length; i++)
    //            {
    //                bottomPanel[i].offsetMin = Vector2.zero;
    //                bottomPanel[i].offsetMax = Vector2.zero;
    //            }

    //            isBannerDown = false;
    //            if (PlayerInventory.isSuperUser != 0)
    //            {
    //                isSuperUser = true;
    //            }
    //        }
    //    }
    //}



}
