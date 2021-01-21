using CodeStage.AntiCheat.Storage;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeLoading : MonoBehaviour
{
    public BannerAdPanelController bac;
    public TutoManager tm;
    public OfflineManager om;
    public RuneManager rm;
    public DailyManager dm;
    public Image loadingBar;

    Image selfimg;
    float currentTime = 0f;
    float lastTime = 3f;
    float alpha;
    // Start is called before the first frame update

    void Start()
    {
        /// 타이틀 이미지, 로딩바 뒷쪽 활성화
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        /// 뒷 배경 활성화
        selfimg = GetComponent<Image>();
        selfimg.enabled = true;
        loadingBar.fillAmount = 0;

        StartCoroutine(Loading());
    }
    /// <summary>
    /// 테스트 버튼에 붙이자 -> json 리셋후 종료
    /// </summary>
    void TEST_RESTE_JSON()
    {
        ListModel.Instance.supList.Clear();
        ListModel.Instance.charatorList.Clear();
        ListModel.Instance.invisibleheartList.Clear();
        ListModel.Instance.invisibleruneList.Clear();
        ListModel.Instance.weaponList.Clear();
        ListModel.Instance.petList.Clear();
        ListModel.Instance.shopList.Clear();
        ListModel.Instance.shopListSPEC.Clear();
        ListModel.Instance.shopListNOR.Clear();
        ListModel.Instance.mineCraft.Clear();
        //
        CodeStage.AntiCheat.Storage.ObscuredPrefs.DeleteAll();
        PlayerPrefs.DeleteAll();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }


    IEnumerator Loading()
    {
        yield return null;

        /// Dotween 테스트
        StartManager.instance.headChatTxt.text = "";
        StartManager.instance.headChatTxt.DOText(StartManager.instance.NOTICE, 1f);

        //
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime / (lastTime * 2f);
            loadingBar.fillAmount = Mathf.SmoothStep(0, 0.6f, currentTime);
            yield return null;
        }
        //
        while (!PlayerPrefsManager.isLoadingComp)
        {
            yield return new WaitForFixedUpdate();
        }

        SystemPopUp.instance.StopLoopLoading();

        /// 버전 체크
        while (!PlayerPrefsManager.isMissingFive)
        {
            yield return new WaitForFixedUpdate();
        }


        /// 첫 실행시 메인 브금 재생
        AudioManager.instance.PlayAudio("Main", "BGM");
        StartManager.instance.headChatTxt.text = "";
        StartManager.instance.headChatTxt.DOText(StartManager.instance.NOTICE2, 1f);

        currentTime = 0;
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            loadingBar.fillAmount = Mathf.SmoothStep(0.6f, 1f, currentTime);
            yield return null;
        }


        while (!PlayerPrefsManager.isNickNameComp)
        {
            yield return new WaitForFixedUpdate();
        }


        while (!PlayerPrefsManager.isJObjectLoad)
        {
            yield return new WaitForFixedUpdate();
        }

        /// 무료 구매 몽땅하면 레드닷 꺼줌.
        var tmpppmt = ListModel.Instance.mvpDataList[0];

        /// 일간
        if (tmpppmt.daily_10 !=0)
        {
            RedDotManager.instance.RedDot[9].SetActive(false);
            RedDotManager.instance.RedDot[10].SetActive(false);
        }
        else
        {
            RedDotManager.instance.RedDot[9].SetActive(true);
            RedDotManager.instance.RedDot[10].SetActive(true);
        }
        /// 주간
        if (tmpppmt.weekend_14 != 0)
        {
            RedDotManager.instance.RedDot[11].SetActive(false);
            RedDotManager.instance.RedDot[12].SetActive(false);
        }
        else
        {
            RedDotManager.instance.RedDot[11].SetActive(true);
            RedDotManager.instance.RedDot[12].SetActive(true);
        }
        /// 월간
        if (tmpppmt.mouth_18 != 0)
        {
            RedDotManager.instance.RedDot[13].SetActive(false);
            RedDotManager.instance.RedDot[14].SetActive(false);
        }
        else
        {
            RedDotManager.instance.RedDot[13].SetActive(true);
            RedDotManager.instance.RedDot[14].SetActive(true);
        }
        /// 최상단
        if (tmpppmt.daily_10 != 0 && tmpppmt.weekend_14 != 0 && tmpppmt.mouth_18 != 0)
        {
            /// 패키지 레드닷  꺼줌
            RedDotManager.instance.RedDot[8].SetActive(false);
        }
        else
        {
            /// 패키지 레드닷  꺼줌
            RedDotManager.instance.RedDot[8].SetActive(true);
        }


        /// 튜토리얼 새로고침
        tm.InitTutorial();

        /// 타이틀 이미지, 로딩바 뒷쪽 꺼져
        StartManager.instance.headChatTxt.text = "";
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

        /// json 로딩 끝나면 착용한 룬 이미지 표기하기
        rm.InitShowIconFive();
        /// 아마존 결정 게이지 표기
        PlayerInventory.Money_AmazonCoin += 0;
        PlayerInventory.AmazonStoneCount += 0;

        currentTime = 0;
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            alpha = Mathf.SmoothStep(1, 0, currentTime);
            selfimg.color = new Color(1f, 1f, 1f, alpha);
            loadingBar.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }


        ///// 데이터 불러오기 재실행이면 우편함 날려줌
        //if (ObscuredPrefs.GetInt("isSeverDataLoad") != 0)
        //{
        //    GameObject.Find("NanooManager").GetComponent<NanooManager>().PostboxDelete();
        //    GameObject.Find("NanooManager").GetComponent<NanooManager>().PostboxRedDot();
        //    Debug.LogError("우편함 날리기");
        //}

        /// 페이크 로딩창 끄기.
        gameObject.SetActive(false);
        /// 광고제거 했으면 배너 제거 + 속도 1.1배
        bac.Banner525Hide();
        /// 출석체크 동작
        dm.CheckDailyInit();
        /// 오프라인 보상
        om.OfflineInit();
        /// 접속하기 업적
        ListModel.Instance.DAYlist_Update(6);
    }



}
