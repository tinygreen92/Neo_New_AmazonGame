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
    [HideInInspector]
    public bool isBannerDown;                // 배너 공간 내려가는 중

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
    /// congif - 3번째 - 배너 SendMessage 로 호출 중
    /// </summary>
    private void BannerOnoff()
    {
        if (!isOn && !isBannerDown)
        {
            isBannerUP = true;
            isOn = !isOn;
            em.ShowBanner();
            /// 속도 10% 증가.
            Time.timeScale = 1.1f;
        }
        else if (isOn && !isBannerUP)
        {
            isBannerDown = true;
            isOn = !isOn;
            em.HideBanner();
            /// 속도 원래대로 복귀
            Time.timeScale = 1.0f;
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
            SuperPannel.SetActive(true);

            isBannerDown = true;
            isOn = !isOn;
            em.DestroyBanner();
            Time.timeScale = 1.1f;
        }
    }

    ///// <summary>
    ///// http://cheongbok.blogspot.com/2018/07/dp-pixel.html
    ///// </summary>
    ///// <param name="fFixedResoulutionHeight">내가 고정한 해상도 높이 :: 1980</param>
    ///// <param name="fdpHeight">바꾸고자 하는 dp (애드몹 배너 320x50일 때 50)</param>
    ///// <returns></returns>
    //public float DPToPixel(float fFixedResoulutionHeight, float fdpHeight)
    //{
    //    float fNowDpi = (Screen.dpi * fFixedResoulutionHeight) / Screen.height;
    //    float scale = fNowDpi / 160;
    //    float pixel = fdpHeight * scale;

    //    return pixel;
    //}

    void Update()
    {
        if (isSuperUser) return;

        /// 배너 올라가는 스위치
        if (isBannerUP)
        {
            for (int i = 0; i < bottomPanel.Length; i++)
            {
                bottomPanel[i].offsetMin = Vector2.Lerp(bottomPanel[i].offsetMin, targetPos, 0.1f);
                bottomPanel[i].offsetMax = Vector2.Lerp(bottomPanel[i].offsetMax, -targetPos*0.5f, 0.1f);
            }

            if (bottomPanel[0].offsetMin.y >= tagetY - 1f)
            {
                for (int i = 0; i < bottomPanel.Length; i++)
                {
                    bottomPanel[i].offsetMin = targetPos;
                    bottomPanel[i].offsetMax = -targetPos*0.5f;
                }

                isBannerUP = false;
            }
        }

        /// 배너 내려가는 스위치
        if (isBannerDown)
        {
            for (int i = 0; i < bottomPanel.Length; i++)
            {
                bottomPanel[i].offsetMin = Vector2.Lerp(bottomPanel[i].offsetMin, Vector2.zero, 0.1f);
                bottomPanel[i].offsetMax = Vector2.Lerp(bottomPanel[i].offsetMax, Vector2.zero, 0.1f);
            }

            if (bottomPanel[0].offsetMin.y <= 1f)
            {
                for (int i = 0; i < bottomPanel.Length; i++)
                {
                    bottomPanel[i].offsetMin = Vector2.zero;
                    bottomPanel[i].offsetMax = Vector2.zero;
                }

                isBannerDown = false;
                if (PlayerInventory.isSuperUser != 0)
                {
                    isSuperUser = true;
                }
            }
        }
    }



}
