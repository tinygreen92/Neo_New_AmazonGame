using EasyMobile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyMoblieManager : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        /// 이지모바일 로그인
        StartCoroutine(initEM());

        /// 모펍 sdk init
        //if (!MoPub.IsSdkInitialized) MoPub.InitializeSdk(MopubAdRewardedId[0]);
        //Advertising.GrantDataPrivacyConsent(AdNetwork.MoPub);
        //MoPub.LoadRewardedVideoPluginsForAdUnits(MopubAdRewardedId);
        ///// 모펍 초기화 되었다면 비디오 하나 불러오기
        //Advertising.LoadRewardedAd();
    }

    IEnumerator initEM()
    {
        /// 이지모바일  init
        RuntimeManager.Init();

        while (!RuntimeManager.IsInitialized())
        {
            yield return null;
        }

        // Grants the vendor-level consent for AdMob.
        Advertising.GrantDataPrivacyConsent(AdNetwork.AdMob);
        Advertising.GrantDataPrivacyConsent(AdNetwork.MoPub);
        // Revokes the vendor-level consent of AdMob.
        Advertising.RevokeDataPrivacyConsent(AdNetwork.AdMob);
        Advertising.RevokeDataPrivacyConsent(AdNetwork.MoPub);
    }

    public void ShowBanner()
    {
        Advertising.ShowBannerAd(BannerAdNetwork.AdMob, BannerAdPosition.Bottom, BannerAdSize.SmartBanner);
        SystemPopUp.instance.LoopLoadingImg();
        Invoke(nameof(InvoHideLoop), 3f);
    }
    void InvoHideLoop()
    {
        SystemPopUp.instance.StopLoopLoading();
    }

    public void HideBanner()
    {
        Advertising.HideBannerAd();
    }

    public void DestroyBanner()
    {
        Advertising.DestroyBannerAd();
    }

}
