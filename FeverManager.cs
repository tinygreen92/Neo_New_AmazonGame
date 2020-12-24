using CodeStage.AntiCheat.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverManager : MonoBehaviour
{
    [Header("- 28.캐릭터 정보 팝업")]
    public Image[] ME_Img;
    public Sprite[] ME_Sprite;
    public GameObject[] MiddleEarth;


    [Header("- 캐릭터 정보")]
    public Text lvText;
    public Text nameText;

    [Header("- 디버그 모드 활성화.")]
    public GameObject[] TEST_Btn;
    [Header("- 피버 이제 안쓰는거")]
    public Transform FeverParent;
    public GameObject testFeverSpr;
    public static int state_Monster_KillCount;                          // 죽인 몬스터 수
    public static bool isFeverTime;


    private void Awake()
    {
        state_Monster_KillCount = ObscuredPrefs.GetInt("state_Monster_KillCount");
        FeverParent.GetChild(0).GetComponent<Image>().fillAmount = (state_Monster_KillCount / PlayerInventory.Kill_Cost);
    }


    /// <summary>
    /// 버튼 눌려서 팝업 호출
    /// </summary>
    public void ClickedCharactorInfo()
    {
        Clicked_Info_TopBtn(0);
        PopUpManager.instance.ShowPopUP(28);
    }


    /// <summary>
    /// 캐릭터 인포 탑 버튼 3가지
    /// </summary>
    public void Clicked_Info_TopBtn(int _Index)
    {
        MiddleEarth[0].SetActive(false);
        MiddleEarth[1].SetActive(false);
        MiddleEarth[2].SetActive(false);
        /// 선택 섹션 활성화
        MiddleEarth[_Index].SetActive(true);
        switch (_Index)
        {
            /// 캐릭터 정보
            case 0: 
                ME_Img[0].sprite = ME_Sprite[1];
                ME_Img[1].sprite = ME_Sprite[0];
                ME_Img[2].sprite = ME_Sprite[0];
                break;
            /// 재화 정보
            case 1:
                ME_Img[0].sprite = ME_Sprite[0];
                ME_Img[1].sprite = ME_Sprite[1];
                ME_Img[2].sprite = ME_Sprite[0];
                break;
            /// 기타 정보
            case 2:
                ME_Img[0].sprite = ME_Sprite[0];
                ME_Img[1].sprite = ME_Sprite[0];
                ME_Img[2].sprite = ME_Sprite[1];
                break;
        }
    }


    /// <summary>
    /// 설정에 숨겨져있는 디버그 모드 활성화.
    /// </summary>
    public void TEST_Mode_Activate()
    {
        if (!StartManager.instance.isDeerveloperMode) return;
        TEST_Btn[0].SetActive(true);
        TEST_Btn[1].SetActive(true);
    }

    public int State_Monster_KillCount
    {
        get
        { 
            if (state_Monster_KillCount > 0) return state_Monster_KillCount; 
            else return 0; 
        }
        set
        {
            /// 피버 지속중엔 카운트 안 쌓임
            if (isFeverTime) return;
            ///
            state_Monster_KillCount = value;

            if (state_Monster_KillCount >= PlayerInventory.Kill_Cost)
            {
                StartCoroutine(ItsFeverTime());
            }
            else
            {
                Debug.LogWarning("누적 state_Monster_KillCount" + state_Monster_KillCount);
                FeverParent.GetChild(0).GetComponent<Image>().fillAmount = (state_Monster_KillCount / PlayerInventory.Kill_Cost);
                ObscuredPrefs.SetInt("state_Monster_KillCount", state_Monster_KillCount);
                ///  킬 카운터 1 올리기
                ListModel.Instance.DAYlist_Update(5);
                ///  업적 카운트 올리기
                ListModel.Instance.ALLlist_Update(6,1);
            }
        }
    }

    IEnumerator ItsFeverTime()
    {
        testFeverSpr.SetActive(true);
        /// 킬카운터 초기화 하고 피버 타임 활성화.
        state_Monster_KillCount = 0;
        FeverParent.GetChild(0).GetComponent<Image>().fillAmount = 1;
        ObscuredPrefs.SetInt("state_Monster_KillCount", state_Monster_KillCount);

        isFeverTime = true;
        //
        float currntTime = 0;
        yield return null;

        while (true)
        {
            yield return new WaitForFixedUpdate();
            currntTime += Time.deltaTime;
            //FeverParent.GetChild(0).GetComponent<Image>().fillAmount = (currntTime / maxTime);

            if (currntTime >= PlayerInventory.Fever_Time)
            {
                testFeverSpr.SetActive(false);

                FeverParent.GetChild(0).GetComponent<Image>().fillAmount = 0;
                /// 피버 끝
                isFeverTime = false;
                break;
            }
        }
    }


    public void TestFeverCharge()
    {
        State_Monster_KillCount += 100;
    }




}
