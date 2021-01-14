using EasyMobile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyMoblieManager : MonoBehaviour
{
    //public string[] MopubAdRewardedId;

    void Awake()
    {
        /// 이지모바일  init
        if (!RuntimeManager.IsInitialized()) RuntimeManager.Init();
        /// 모펍 sdk init
        //if (!MoPub.IsSdkInitialized) MoPub.InitializeSdk(MopubAdRewardedId[0]);
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Advertising.GrantDataPrivacyConsent(AdNetwork.MoPub);
        //Advertising.GrantDataPrivacyConsent(AdNetwork.MoPub);

        //MoPub.LoadRewardedVideoPluginsForAdUnits(MopubAdRewardedId);
        ///// 모펍 초기화 되었다면 비디오 하나 불러오기
        //Advertising.LoadRewardedAd();
    }

    public void ShowBanner()
    {
        Advertising.ShowBannerAd(BannerAdNetwork.MoPub, BannerAdPosition.Bottom, BannerAdSize.SmartBanner);
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
