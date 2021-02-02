using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleManager : MonoBehaviour
{
    public GameObject DragHandle;
    [Header("방치모드시 챗 꺼주고")]
    public PhotonChatManager pcm;
    public EasyMoblieManager em;
    [Header("방치모드시 숨겨줄 캔버스")]
    public Transform[] IdleForCanvas;
    [Header("대기중 위치 / 가리는 위치")]
    public Transform DisabledIdle_pos;
    public Transform AbledIdle_pos;
    // 방치 모드 슬라이드 해줄 속도인데 5 아니면 오류 나더라 왜지?
    private float rate = 5f;

    //bool isAnimPlaying; // 코루틴ing 체크

    Coroutine moveRoutine;

    public void IdleMode_On()
    {
        /// 배너 레이아웃이 올라온 상태가 아니면 광고 표시
        if (!BannerAdPanelController.isOn)
        {
            /// 슈퍼유저가 아니면 광고 표시
            if (PlayerInventory.isSuperUser != 0)
            {
                /// 슈퍼 유저다 
            }
            else
            {
                /// 일반 유저면 배너 올릴때 배너 광고 표시
                em.ShowBanner();
                Invoke(nameof(InvoHandleOn), 3f);
            }
        }
        /// 스르륵 내려온다
        gameObject.SetActive(true);
        moveRoutine = StartCoroutine(Progress());
        /// 포톤 접속종료
        pcm.ExDisconnect();
    }

    void InvoHandleOn()
    {
        DragHandle.SetActive(true);
    }

    public void IdleMode_Off()
    {
        moveRoutine = StartCoroutine(ProgressReverse());
        Application.targetFrameRate = 59;
        
        for (int i = 0; i < IdleForCanvas.Length; i++)
        {
            IdleForCanvas[i].gameObject.SetActive(true);
        }
        /// 포톤 재접속
        pcm.ExReconnect();
    }

    /// <summary>
    /// 방치 모드 화면 가리기
    /// </summary>
    /// <returns></returns>
    private IEnumerator Progress()
    {
        //isAnimPlaying = true;

        float progress = 0.0f;

        while (progress <= 1f)
        {
            yield return new WaitForFixedUpdate();

            var moving = Mathf.Lerp(DisabledIdle_pos.position.y, AbledIdle_pos.position.y, progress);

            transform.position = new Vector2(AbledIdle_pos.position.x, moving);

            progress += rate * Time.deltaTime;
        }

        for (int i = 0; i < IdleForCanvas.Length; i++)
        {
            IdleForCanvas[i].gameObject.SetActive(false);
        }

        Application.targetFrameRate = 29;
        PlayerPrefsManager.isIdleModeOn = true;
    }


    /// <summary>
    /// 방치모드 해제
    /// </summary>
    /// <returns></returns>
    private IEnumerator ProgressReverse()
    {
        //isAnimPlaying = true;

        float progress = 0.0f;

        while (progress <= 1f)
        {
            yield return new WaitForFixedUpdate();

            var moving = Mathf.Lerp(AbledIdle_pos.position.y, DisabledIdle_pos.position.y, progress);

            transform.position = new Vector2(AbledIdle_pos.position.x, moving);

            progress += rate * Time.deltaTime;
        }

        // 배너가 위로 올라오지 않았으면
        if (!BannerAdPanelController.isOn)
        {
            em.HideBanner();
        }

        PlayerPrefsManager.isIdleModeOn = false;
        DragHandle.SetActive(false);
        gameObject.SetActive(false);
    }
}








