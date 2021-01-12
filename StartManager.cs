using EasyMobile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartManager : MonoBehaviour
{
    [TextArea]
    public string NOTICE;
    [TextArea]
    public string NOTICE2;
    [Header("- 디버그 공지사항용")]
    public Text headChatTxt;
    [Header("- 개발자용 빌드일 때 체크")]
    public bool isDeerveloperMode;
    [Header("- 디버그 모드시 체크")]
    public bool isDebugMode;
    [Header("- 미드 화면 자동 늘어남 체크")]
    public bool isGraphicMode;
    public GameObject[] AllPopUP;

    public static StartManager instance;
    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 59;

        /// ------------------------------------------------------------------ ///
        if (isDebugMode) return;
        // 로그 비활성화
        Debug.unityLogger.logEnabled = false;
        // 화면 꺼짐 방지
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
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
                        Application.Quit();
                };
        }

#endif
    }
}