using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NestedScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TopViewPannel[] tvp;
    public BotBtnBackImg bbb;
    [Header("- 광산때 전환하기 위해서 사용")]
    Transform Charactor_INFINITI;
    Transform Weapon_INFINITI;
    Transform Support_INFINITI;
    Transform Pet_INFINITI;
    Transform Rune_INFINITI;
    [Header("- ho_스크롤바 넣어줌")]
    public Scrollbar scrollbar;
    [Header("- 메인 스크롤뷰 컨텐츠")]
    public Transform contentTr;
    [Header("- 무한 스크롤 넣어줌")]
    public InfiniteScroll[] infiniteScrolls;

    int SIZE = 7; /// 스크롤할 패널 갯수
    float[] pos = new float[7]; /// 거리에 따라 패널 구분할 수치 배열
    //
    bool isDrag;        // 드래그 중이냐?
    float distance;     // 드래그 하는 거리
    float currentPos;   // 현재 패널 거리
    float targetPos;    // 목표 패널 거리
    int targetIndex;    // 클릭한 패널 인덱스


    void Start()
    {
        if (transform.parent.parent.parent.name == "2.Auto_Canvas") SIZE = 1;
        StartCoroutine(RealStart());
    }

    IEnumerator RealStart()
    {
        yield return new WaitForEndOfFrame();

        if (!StartManager.instance.isGraphicMode)
        {
            float origContSize = contentTr.GetComponent<RectTransform>().rect.width * (SIZE - 1);
            contentTr.GetComponent<RectTransform>().sizeDelta = new Vector2(origContSize, 0);
        }

        while (!PlayerPrefsManager.isLoadingComp)
        {
            yield return new WaitForFixedUpdate();
        }


        distance = 1f / (SIZE - 1);

        for (int i = 0; i < SIZE; i++)
        {
            pos[i] = distance * i;
        }

        for (int i = 0; i < infiniteScrolls.Length; i++)
        {
            infiniteScrolls[i].ListStart();
        }

        for (int i = 0; i < SIZE; i++)
        {
            contentTr.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
        /// 광산이랑 공유하기 때문에
        Charactor_INFINITI = infiniteScrolls[0].transform;

        if (transform.parent.parent.parent.name != "2.Auto_Canvas")
        {
            Weapon_INFINITI = infiniteScrolls[1].transform;
            Support_INFINITI = infiniteScrolls[3].transform;
            Pet_INFINITI = infiniteScrolls[4].transform;
            Rune_INFINITI = infiniteScrolls[5].transform;
            /// 초기 포커스 유지
            OnOffinvisiblePanel(0);
        }
        else
        {
            contentTr.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 오브젝트 싹 비우기
    /// </summary>
    public void RefreshForSHOP()
    {
        infiniteScrolls[6].RRefreshShop();
    }
    public void GnerSHOP()
    {
        infiniteScrolls[6].SetLonWizard();
    }

    /// <summary>
    /// 무기 뽑기하면 새로고침
    /// </summary>
    public void RefreshWeapon()
    {
        TabClick(1);
    }

    /// <summary>
    /// 유물 뽑기 버튼으로 호출 하기.
    /// </summary>
    public void RefreshInfiList()
    {
        TabClick(2);
        infiniteScrolls[2].SetLinsCntForHeart();
    }
    public void GnerForHeart()
    {
        infiniteScrolls[2].SetCompHeartLins();
    }

    /// <summary>
    /// 오브젝트 싹 비우기
    /// </summary>
    public void RefreshForRune()
    {
        TabClick(5);
        infiniteScrolls[5].SetLinsCntForRune();
    }

    /// <summary>
    /// 리스트만큼 옵브젝트 생성하기
    /// </summary>
    public void GnerForRune()
    {
        PlayerPrefsManager.isRuneInit = false;
        infiniteScrolls[5].SetCompRuneLins();
    }

    /// <summary>
    /// 절반 거리를 기준으로 가까운 위치를 반환
    /// </summary>
    /// <returns>(f) pos 배열</returns>
    float SetPos()
    {
        for (int i = 0; i < SIZE; i++)
        {
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                OnOffinvisiblePanel(i); // 해당 패널 빼고 비활성화
                return pos[i];
            }
        }

        // 예외 처리 -> 여기로 들어오는 경우는 버그다
        return pos[0];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentPos = SetPos();
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        targetPos = SetPos();

        //// 절반거리 넘지 않아도 스와이핑 빠르게 하면?
        //if (currentPos == targetPos)
        //{
        //    if (eventData.delta.x > 16 && currentPos - distance >= 0)
        //    {
        //        --targetIndex;
        //        targetPos = currentPos - distance;
        //    }
        //    else if (eventData.delta.x < -16 && currentPos + distance <= 1.01f)
        //    {
        //        ++targetIndex;
        //        targetPos = currentPos + distance;
        //    }
        //}

        // 목표가 수직스크롤이고, 좌우 이동했다면 콘텐츠 최상단으로
        for (int i = 0; i < SIZE; i++)
        {
            if (contentTr.GetChild(i).GetComponent<ScrollScript>() && currentPos != pos[i] && targetPos == pos[i])
            {
                contentTr.GetChild(i).GetChild(1).GetComponent<Scrollbar>().value = 1f;
                OnOffinvisiblePanel(i); // 해당 패널 빼고 비활성화
            }
        }
    }

    Vector3 btnTagetPos;
    Vector3 btnTagetScale;
    void Update()
    {
        if (!isDrag)
        {
            #region <Delete> 목표 버튼 위로 올라가고 텍스트 표기
            //// 목표 버튼 위로 올라가고 텍스트 표기
            //for (int i = 0; i < SIZE; i++)
            //{
            //    btnRect[i].GetChild(0).gameObject.SetActive(false);
            //    //
            //    btnTagetPos = Vector3.zero;
            //    btnTagetScale = new Vector3(0.75f, 0.75f, 0.75f);
            //    //
            //    if (i == targetIndex)
            //    {
            //        btnTagetPos.y = 35f;
            //        btnTagetScale = new Vector3(0.9f, 0.9f, 0.9f);
            //        btnRect[i].GetChild(0).gameObject.SetActive(true);
            //    }
            //    //
            //    btnRect[i].anchoredPosition3D = Vector3.Lerp(btnRect[i].anchoredPosition3D, btnTagetPos, 0.25f);
            //    btnRect[i].localScale = Vector3.Lerp(btnRect[i].localScale, btnTagetScale, 0.25f);
            //}
            #endregion

            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
        }
    }


    /// <summary>
    /// Bot 패널에서 버튼 눌러서 인덱스 바꿔줌
    /// </summary>
    /// <param name="n"></param>
    public void TabClick(int n)
    {
        targetIndex = n;
        OnOffinvisiblePanel(n);
        targetPos = pos[n];
        // 클릭 당한 페이지는 최상단으로 올려
        contentTr.GetChild(n).GetChild(1).GetComponent<Scrollbar>().value = 1f;
    }


    /// <summary>
    /// 현재 포커스가 있는 탭 말고 컨텐츠 비활성화
    /// </summary>
    void OnOffinvisiblePanel(int _index)
    {
        if (contentTr.GetChild(_index).GetChild(0).gameObject.activeSelf) return;
        for (int i = 0; i < SIZE; i++)
        {
            contentTr.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
        contentTr.GetChild(_index).GetChild(0).gameObject.SetActive(true);
        /// 백그라운드 색깔
        bbb.BBB_Changer(_index);
        /// 탭 전환할때 새로고침
        switch (_index)
        {
            /// 캐릭터
            case 0:
                /// 100 레벨업 패치
                tvp[0].DismissGrayBtn(1);
                break;
            /// 무기
            case 1:
                /// 100 레벨업 패치
                tvp[1].SwichWeapon(0);
                break;
            /// 유물
            case 2:
                /// 100 레벨업 패치
                tvp[2].DismissGrayBtn(1);
                break;
            /// 수집
            case 3:
                /// 100 레벨업 패치
                tvp[3].DismissGrayBtn(1);
                break;
            /// 펫
            case 4:
                /// 100 레벨업 패치
                tvp[4].SwichPetRune(0);
                break;
            /// 룬
            case 5:
                /// 100 레벨업 패치
                tvp[5].SwichPetRune(0);
                break;
            default:
                break;
        }

    }

























    /// <summary>
    /// 테스트 빌드에서 채팅창 껏다 켰다 해줌
    /// </summary>
    bool isChatDisp;
    public void TabClickChatting(bool b)
    {
        if (b) isChatDisp = true;

        isChatDisp = !isChatDisp;
        GameObject.Find("Chat_Panel").transform.GetChild(2).gameObject.SetActive(isChatDisp);
    }



}
