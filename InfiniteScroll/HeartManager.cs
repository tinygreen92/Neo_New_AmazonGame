using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public NestedScrollManager NSM;
    [Header("- 유물 뽑기 이미지들")]
    public Sprite[] HeartSprs;
    [Header("- 11. 팝업  획득 창 ")]
    public Image GetHeartImg;
    public Text TitleText;
    public Text DescTexts;

    public delegate void ChainFunc();       // 아웃라인 델리게이트
    public ChainFunc chain;                 // 체인 메서드


    /// <summary>
    /// HeartItem에서 불러와서  0 번 인덱스 호출하면
    /// 1. 다이아몬드 여부 확인해서 서버에서 소모해주고
    /// 2. 서버처리 완료되었다면 , 로컬에서도 소모해주고
    /// 3. 유물 스크롤뷰 추가, 다이아몬드 소모, 로딩바 꺼주기.
    /// 4. 팝업 생성해주고 종료.
    /// </summary>
    public void GatChaHerat()
    {
        /// 다이아몬드 재화 처리 + 플레이팹 접속
        CalDiamondWithPlayfab();
        /// 로딩 뺑글이 종료
        Invoke(nameof(TESTLOOOOOOP), 0.5f);
        // 유물 안 뽑힌거 하나 집어서 인벤토리로 넣어줌.
        int random = Random.Range(0, ListModel.Instance.invisibleheartList.Count);

        /// 인덱스 요소, 보이는 리스트에 복사
        ListModel.Instance.Heart_Unlock(random);
        /// 플레이어 인벤토리에 인덱스 추가
         var tmpStruct = ListModel.Instance.invisibleheartList[random];
        PlayerInventory.heartIndexs[int.Parse(tmpStruct.imgIndex) - 1] = ListModel.Instance.heartList.Count;
        /// 해당 요소, 보관함에서 삭제 
        ListModel.Instance.invisibleheartList.RemoveAt(random);

        /// 팝업에 내용물 채우기
        GetHeartImg.sprite = HeartSprs[int.Parse(tmpStruct.imgIndex)];
        TitleText.text = tmpStruct.heartName;
        DescTexts.text = tmpStruct.descHead + " " + tmpStruct.descTail;

        ///팝업 호출
        PopUpManager.instance.ShowPopUP(11);

        /// 스크롤 뷰에 유물 하나 추가하기.
        NSM.RefreshInfiList();
        /// 오브젝트 재생성
        NSM.GnerForHeart();
    }

    /// <summary>
    /// 일정 시간 뒤에 뺑뻉이 꺼주기.
    /// </summary>
    public void TESTLOOOOOOP()
    {
        SystemPopUp.instance.StopLoopLoading();
    }

    /// <summary>
    /// 플레이팹에 접속하여 다이아몬드 빼오기
    /// </summary>
    private void CalDiamondWithPlayfab()
    {
        /// TODO : 플레이팹 접속하기전 로딩 뺑뺑이 호출 StopLoopLoading
        SystemPopUp.instance.LoopLoadingImg();
        PlayerInventory.Money_Dia -= 300;
        /// 유물 뽑기 1회 진행
        if (PlayerPrefsManager.currentTutoIndex == 20) ListModel.Instance.TUTO_Update(20);
        if (PlayerPrefsManager.currentTutoIndex == 45) ListModel.Instance.TUTO_Update(45);
    }
}
