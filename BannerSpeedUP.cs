using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerSpeedUP : MonoBehaviour
{

    public GameObject[] OnOffObject;


    /// <summary>
    /// 배너 누를때 활성화/비활성화 문구 텍스트 바꿔줌
    /// </summary>
    public void ClickedBanner()
    {
        /// 광고제거 구매했니?
        if (PlayerInventory.isSuperUser != 0)
        {
            return;
        }

        if (BannerAdPanelController.isOn)
        {
            OnOffObject[0].SetActive(false);
            OnOffObject[1].SetActive(true);
            OnOffObject[2].SetActive(false);
            OnOffObject[3].SetActive(true);
            gameObject.SetActive(true);
        }
        else
        {
            OnOffObject[0].SetActive(true);
            OnOffObject[1].SetActive(false);
            OnOffObject[2].SetActive(true);
            OnOffObject[3].SetActive(false);
            gameObject.SetActive(true);

        }
    }
}
