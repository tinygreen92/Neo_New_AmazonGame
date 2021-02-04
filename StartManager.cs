using EasyMobile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartManager : MonoBehaviour
{
    public GameObject DevelText;
    public GameObject TestBtn;
    [TextArea]
    public string NOTICE;
    [TextArea]
    public string NOTICE2;
    [Header("- 디버그 공지사항용")]
    public Text headChatTxt;
    [Header("- 개발자용 빌드일 때 체크")]
    public bool isDeerveloperMode;
    [Header("- PC에서  실행시 체크")]
    public bool isDebugMode;
    [Header("- 미드 화면 자동 늘어남 체크")]
    public bool isGraphicMode;
    public GameObject[] AllPopUP;

    public static StartManager instance;
    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 45;
        Application.runInBackground = true;

        // 화면 꺼짐 방지
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        /// ------------------------------------------------------------------ ///

        /// 개발자 모드만 켜서 빌드했냐? 로그 보는 용도.
        if (isDeerveloperMode)
        {
            DevelText.SetActive(true);
            TestBtn.SetActive(true);
            return;
        }


        // 로그 비황성화
        Debug.unityLogger.logEnabled = false;
    }



    private void Start()
    {
        /// 팝업 꺼주기
        for (int i = 0; i < AllPopUP.Length; i++)
        {
            AllPopUP[i].SetActive(false);
        }
    }



    void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // Ask if user wants to exit
            NativeUI.AlertPopup alert = NativeUI.ShowTwoButtonAlert("게임 종료",
                                            "아마존 탈출하기 : 방치형 RPG를 종료하시겠습니까?",
                                            "종료",
                                            "취소");

            if (alert != null)
                alert.OnComplete += delegate (int button)
                {
                    if (button == 0)
                    {
                        /// 종료하시겠습니까 ? 종료 누르면 발동
                        PlayerPrefsManager.instance.isResetAferSave = true;
                        PlayerPrefsManager.instance.TEST_SaveJson();
                        /// 소리 꺼주고 로딩 돌리고 1초뒤에 꺼
                        AudioManager.instance.AllMute();
                        SystemPopUp.instance.LoopLoadingImg();
                        Invoke(nameof(InvoQuit), 1f);
                    }

                };
        }

#endif
    }
    void InvoQuit()
    {
        Application.Quit();
    }
}